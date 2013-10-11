Framework "4.0"

properties {
    $build_config = "Release"
    $pack_dir = ".\pack"
}

task default -depends build
task build -depends build-all
task test -depends build-all, test-all
task pack -depends pack-all
task push -depends push-all


task test-all {
    $mstest = Get-ChildItem -Recurse -Force 'C:\Program Files (x86)\Microsoft Visual Studio *\Common7\IDE\MSTest.exe'
    $mstest = $mstest.FullName
    $test_dlls = Get-ChildItem -Recurse ".\Highway\Test\**\bin\release\*Tests.dll" |
        ?{ $_.Directory.Parent.Parent.Name -eq ($_.Name.replace(".dll","")) }
    exec { 
        $test_dlls | % { & "$mstest" /testcontainer:$($_.FullName) }
    }
}

task build-all {
    rebuild .\Highway\Highway.sln
}

task pack-all -depends nuget-clean{
    create-packs
}

task push-all -depends nuget-clean {
    create-packs
    Get-ChildItem -Path .\pack\*.nupkg |
        %{ push-nuget $_; mv $_ .\nuget\ }
    rm .\pack -Recurse -Force
}

task nuget-clean {
    if (Test-Path $pack_dir) {
        Remove-item $pack_dir -Recurse -Force
    }
    if (PathDoesNotExist $pack_dir) {
        New-Item -ItemType Directory -Path $pack_dir | Out-Null
    }
}

function rebuild([string]$slnPath) { 
    exec { msbuild $slnPath /t:rebuild /v:q /clp:ErrorsOnly /nologo /p:Configuration=$build_config }
}

function create-packs {
	pack-nuget .\Highway\src\Highway.Data\Highway.Data.csproj
	pack-nuget .\Highway\src\Highway.Data.EntityFramework\Highway.Data.EntityFramework.csproj
	pack-nuget .\Highway\src\Highway.Test.MSTest\Highway.Test.MSTest.csproj
    pack-nuget .\Highway\src\Highway.Data.RavenDB\Highway.Data.RavenDB.csproj
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
