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
    Write-Host -ForegroundColor Red "Please specifcy the version to publish"
    Write-Host -ForegroundColor Cyan -NoNewLine "USAGE:"
    Write-Host "lib-package.ps1 -version <version>"
    Write-Host -ForegroundColor Yellow "Existing packages list below"
    nuget list -source $NuGetSource
    exit -1
}

dotnet build .\Samples.Core.Lib\Samples.Core.Lib.csproj -c Release
Write-Host -ForegroundColor Yellow "Packing and publishing package"
dotnet pack .\Samples.Core.Lib\Samples.Core.Lib.csproj -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -c Release /p:Version=$version
dotnet nuget push .\Samples.Core.Lib\bin\release\PreEmptive.Dotfuscator.Samples.Core.Lib.$Version.nupkg -s $NuGetSource