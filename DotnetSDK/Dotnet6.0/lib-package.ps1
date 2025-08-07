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
    Write-Host -ForegroundColor Red "Please specifcy the version of the package"
    Write-Host -ForegroundColor DarkYellow "USAGE: lib-package.ps1 -version <version>"
    Write-Host -ForegroundColor DarkYellow "See below the existing packages"
    nuget list -source $NuGetSource
    exit -1
}

dotnet build .\Samples.Core.Lib\Samples.Core.Lib.csproj -c Release
Write-Host -ForegroundColor Green "Packing and publishing package"
dotnet pack .\Samples.Core.Lib\Samples.Core.Lib.csproj -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -c Release /p:Version=$version
dotnet nuget push .\Samples.Core.Lib\bin\release\PreEmptive.Dotfuscator.Samples.Core.Lib.$Version.nupkg -s $NuGetSource