Framework "4.0"
Import-Module .\DatabaseDeploy.psm1 -DisableNameChecking

properties {
    $msbuildexe = Get-Item "C:\Program Files (x86)\MSBuild\12.0\bin\amd64\msbuild.exe" -ErrorAction SilentlyContinue
    if ($msbuildexe -eq $null) {
        $msbuildexe = Get-Item "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" -ErrorAction SilentlyContinue
        Assert ($msbuildexe -ne $null) "Unable to locate MSBuild.exe"
    }

    $build_config = "Release"
    $pack_dir = ".\pack"
    $build_archive = ".\buildarchive\"
    $version_number = "5.1.3.0"
    $nuget_version_number = $version_number
    if ($Env:BUILD_NUMBER -ne $null) {
        $nuget_version_number += "-$Env:BUILD_NUMBER"
    }
}

##########################################################################################
# Task Aliases
##########################################################################################

task default -depends build
task build -depends build-all
task test -depends build-all, DeployDb, test-all, pack-ci
task pack -depends pack-all
task push -depends push-all


##########################################################################################
# Tasks
##########################################################################################
task test-all -depends clean-buildarchive {
    $mstest = Get-ChildItem -Recurse -Force 'C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe'
    $mstest = $mstest.FullName
    $test_dlls = Get-ChildItem -Recurse ".\Highway\Test\**\bin\$build_config\*Tests.dll" |
        ?{ $_.Directory.Parent.Parent.Name -eq ($_.Name.replace(".dll","")) }
    
    $test_dlls | % { 
        try {
            exec { & "$mstest" /testcontainer:$($_.FullName) } 
        } finally {
            Move-Item .\TestResults\*.trx $build_archive -Verbose
        }
    }
} -postaction {
    if (Test-IsCI) {
        cp .\TestResults\*.trx $build_archive -Verbose
    }
}

task DeployDb {
    $pac = get-item ".\Highway\test\Highway.Data.Tests.Db\bin\$build_config\Highway.Data.Tests.Db.dacpac"
    $pacxml = get-item ".\Highway\test\Highway.Data.Tests.Db\Highway.Data.Tests.Db.publish.xml"
    Publish-DACPAC -DACPAC  $pac.FullName `
    -PublishProfile $pacxml.FullName `
    -ConnectionString "Server=.;Integrated Security=SSPI;" -Database "Highway.Test"
}

task build-all -depends Update-Version {
    rebuild .\Highway\Highway.sln
}

task pack-ci -depends clean-buildarchive, pack-all -precondition { Test-IsCI } {
    dir -Path .\pack\*.nupkg | % { 
        cp $_ $build_archive
    } 
}

task pack-all -depends clean-nuget {
	pack-nuget .\Highway\src\Highway.Data\Highway.Data.csproj
	pack-nuget .\Highway\src\Highway.Data.EntityFramework\Highway.Data.EntityFramework.csproj
}

task push-all -depends clean-nuget, pack-all {
    dir -Path .\pack\*.nupkg | % { 
        push-nuget $_
        mv $_ .\nuget\ 
    }
    rm $pack_dir -Recurse -Force
}

task clean-buildarchive {
    Reset-Directory $build_archive
}

task clean-nuget {
    Reset-Directory $pack_dir
}

task clean-testresults {
    Reset-Directory .\TestResults
}

task Update-Version -depends Update-Version-SolutionInfo, Update-Version-EFNuSpec

task Update-Version-SolutionInfo {
    $solution_info = Get-Content .\Highway\SolutionInfo.cs
    $solution_info = $solution_info -replace 'Version\(".+"\)', "Version(`"$version_number`")"
    $solution_info = $solution_info -replace 'AssemblyFileVersion\(".+"\)', "AssemblyFileVersion(`"$nuget_version_number`")"
    $solution_info = $solution_info -replace 'AssemblyInformationalVersion\(".+"\)', "AssemblyInformationalVersion(`"$nuget_version_number`")"
    Set-Content -Path .\Highway\SolutionInfo.cs -Value $solution_info
    if (Test-ModifiedInGIT .\Highway\SolutionInfo.cs) {
        Write-Warning "SolutionInfo.cs changed, most likely updating to a new version"
    }
}

task Update-Version-EFNuSpec {
    $path = "Highway\src\Highway.Data.EntityFramework\Highway.Data.EntityFramework.nuspec"
    $content = Get-Content $path
    $content = $content -replace 'id="Highway.Data" version=".+"', "id=`"Highway.Data`" version=`"$version_number`""
    Set-Content -Path $path -Value $content
    if (Test-ModifiedInGIT $path) {
        Write-Warning "$path changed, most likely updating to a new version"
    }
}

##########################################################################################
# Functions
##########################################################################################

function Test-IsCI {
    $Env:TEAMCITY_VERSION -ne $null
}

function Test-ModifiedInGIT($path) {
    if (Test-IsCI -eq $false) { return $false }
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
    exec { & $msbuildexe $slnPath /t:rebuild /v:q /clp:ErrorsOnly /nologo /p:Configuration=$build_config }
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
