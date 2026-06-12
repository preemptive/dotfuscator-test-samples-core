#!/usr/bin/env pwsh
[CmdletBinding()]
param(
    [string]$RootPath = (Join-Path (Get-Location).Path "MauiNativeCppDiag"),
    [int]$MaxRetries = 5
)

$ErrorActionPreference = 'Stop'

function Remove-DirectoryWithRetry {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    for ($attempt = 1; $attempt -le $MaxRetries; $attempt++) {
        try {
            if (Test-Path -LiteralPath $Path) {
                Remove-Item -LiteralPath $Path -Recurse -Force
            }
            return
        }
        catch {
            if ($attempt -eq $MaxRetries) {
                throw
            }

            Start-Sleep -Seconds $attempt
        }
    }
}

$targetFolders = Get-ChildItem -LiteralPath $RootPath -Directory -Recurse -Force |
    Where-Object { $_.Name -in @('bin', 'obj') } |
    Sort-Object FullName -Descending

foreach ($folder in $targetFolders) {
    Remove-DirectoryWithRetry -Path $folder.FullName
}