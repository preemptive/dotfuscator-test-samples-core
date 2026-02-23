[CmdletBinding()]
Param(
    [string]$Version,
    [string]$NuGetSource
)

$scriptDirectory = $PSScriptRoot
Write-Host "The script directory is: $scriptDirectory"

if (-Not $NuGetSource) {
    $NuGetSource = Join-Path $scriptDirectory ".packages"
}

if (-Not $Version) {
    Write-Host -ForegroundColor Red "Please specifcy the version of the package"
    Write-Host -ForegroundColor DarkYellow "USAGE: lib-package.ps1 -version <version>"
    Write-Host -ForegroundColor DarkYellow "See below the existing packages"
    nuget list -source $NuGetSource
    exit -1
}

$projectPath = Join-Path $scriptDirectory "Samples.NetStandard.Lib\Samples.NetStandard.Lib.csproj"

# Build the NET Standard library in Release
 dotnet build $projectPath -c Release

Write-Host -ForegroundColor Green "Packing and publishing package"
 dotnet pack $projectPath -c Release -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg /p:Version=$Version

# Push to the local feed (default: .packages)
$packageFile = Join-Path $scriptDirectory "Samples.NetStandard.Lib\bin\Release\PreEmptive.Dotfuscator.Samples.NetStandard.Lib.$Version.nupkg"
 dotnet nuget push $packageFile -s $NuGetSource
