Framework "4.0"

properties {
    $build_config = "Release"
    $pack_dir = ".\pack"
    $build_archive = ".\buildarchive\"
    $version_number = "4.2.0.0"
    if ($Env:BUILD_NUMBER -ne $null) {
        $version_number += "-$Env:BUILD_NUMBER"
    }
}

##########################################################################################
# Task Aliases
##########################################################################################

task default -depends build
task build -depends build-all
task test -depends build-all, test-all, pack-ci
task pack -depends pack-all
task push -depends push-all


##########################################################################################
# Tasks
##########################################################################################

task test-all -depends clean_buildarchive, Clean-TestResults {
    $mstest = Get-ChildItem -Recurse -Force 'C:\Program Files (x86)\Microsoft Visual Studio *\Common7\IDE\MSTest.exe'
    $mstest = $mstest.FullName
    $test_dlls = Get-ChildItem -Recurse ".\Highway\Test\**\bin\release\*Tests.dll" |
        ?{ $_.Directory.Parent.Parent.Name -eq ($_.Name.replace(".dll","")) }
    
    $test_dlls | % { exec { & "$mstest" /testcontainer:$($_.FullName) } }
} -postaction {
    if (Test-IsCI) {
        cp .\TestResults\*.trx $build_archive -Verbose
    }
}

task build-all -depends Update-Version {
    rebuild .\Highway\Highway.sln
}

task pack-ci -depends clean-buildarchive, pack-all -precondition Test-IsCI {
    dir -Path .\pack\*.nupkg | % { 
        cp $_ $build_archive
    }
    rm $pack_dir -Recurse -Force   
}

task pack-all -depends Test-PackageDoesNotExist, clean-nuget {
	pack-nuget .\Highway\src\Highway.Data\Highway.Data.csproj
	pack-nuget .\Highway\src\Highway.Data.EntityFramework\Highway.Data.EntityFramework.csproj
	pack-nuget .\Highway\src\Highway.Test.MSTest\Highway.Test.MSTest.csproj
    pack-nuget .\Highway\src\Highway.Data.RavenDB\Highway.Data.RavenDB.csproj
}

task push-all -depends clean-nuget, pack-all {
    dir -Path .\pack\*.nupkg | % { 
        push-nuget $_
        mv $_ .\nuget\ 
    }
    rm $pack_dir -Recurse -Force
}

task clean_buildarchive {
    Reset-Directory $build_archive
}

task clean-nuget {
    Reset-Directory $pack_dir
}

task clean-testresults {
    Reset-Directory .\TestResults
}

task Update-Version {
    $solution_info = Get-Content .\Highway\SolutionInfo.cs
    $solution_info = $solution_info -replace 'Version\(".+"\)', "Version(`"$version_number`")"
    Set-Content -Path .\Highway\SolutionInfo.cs -Value $solution_info
    if (Test-ModifiedInGIT .\Highway\SolutionInfo.cs) {
        Write-Warning "SolutionInfo.cs changed, most likely updating to a new version"
    }
}

##########################################################################################
# Functions
##########################################################################################

function Test-IsCI {
    $Env:TEAMCITY_VERSION -ne $null
}

function Test-PackageDoesNotExist() {
    (ls ".\nuget\*$version_number.nupkg" | Measure-Object).Count -gt 0
}

function Test-ModifiedInGIT($path) {
    $status_result = & git status $path --porcelain
    $status_result -ne $null
}

function Reset-Directory($path) {
    if (Test-Path $path) {
        Remove-item $path -Recurse -Force
    }
    if (PathDoesNotExist $path) {
        New-Item -ItemType Directory -Path $path | Out-Null
    }
}

function rebuild([string]$slnPath) { 
    Set-Content Env:\EnableNuGetPackageRestore -Value true
    .\Highway\.nuget\nuget.exe restore $slnPath
    exec { msbuild $slnPath /t:rebuild /v:q /clp:ErrorsOnly /nologo /p:Configuration=$build_config }
}

function pack-nuget($prj) {
    exec { 
        & .\Highway\.nuget\nuget.exe pack $prj -o pack -prop configuration=$build_config
    }
}

function push-nuget($prj) {
    exec { 
        & .\Highway\.nuget\nuget.exe push $prj
    }
}

function PathDoesNotExist($path) {
    (Test-Path $path) -eq $false
}
