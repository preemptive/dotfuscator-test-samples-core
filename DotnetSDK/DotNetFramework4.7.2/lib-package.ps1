[CmdletBinding()]
Param(
    [string]$Version,
    [string]$NuGetSource
)

$scriptDirectory = $PSScriptRoot
Write-Host "The script directory is: $scriptDirectory"
if (-Not $NuGetSource) {
    $NuGetSource = $scriptDirectory + "\.packages"
}

if (-Not $Version) {
    Write-Host -ForegroundColor Red "Please specify the version of the package"
    Write-Host -ForegroundColor DarkYellow "USAGE: lib-package.ps1 -version <version>"
    Write-Host -ForegroundColor DarkYellow "See below the existing packages"
    nuget list -source $NuGetSource
    exit -1
}

nuget restore .\Samples.Core.Lib\Samples.Core.Lib.csproj

$msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if (-not (Test-Path $msbuild)) {
    $msbuild = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
}
if (-not (Test-Path $msbuild)) {
    Write-Host -ForegroundColor Red "MSBuild.exe not found. Please install Build Tools for Visual Studio."
    exit 1
}

& $msbuild .\Samples.Core.Lib\Samples.Core.Lib.csproj /t:Build /p:Configuration=Release

Write-Host -ForegroundColor Green "Packing and publishing package"
nuget pack .\Samples.Core.Lib\Samples.Core.Lib.csproj -Properties Configuration=Release -OutputDirectory $NuGetSource -Version $Version
dotnet nuget push "$NuGetSource\nupkgs\PreEmptive.Dotfuscator.Samples.Core.Lib.$Version.nupkg" -s $NuGetSource