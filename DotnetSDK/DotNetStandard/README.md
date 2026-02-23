# DotnetStandard (NetStandard + Net10)

This solution mirrors the Dotnet6.0 sample-style structure, but keeps the library minimal.

## Projects

- **Samples.NetStandard.Lib** (netstandard2.0)
  - Abstractions: `IStepResultProcessor`
  - Models: `StepMetadata`, `StepResult`
  - Services: `ConsoleOutputStepProcessor`

- **Samples.Net10.ConsoleApp** (net10.0)
  - References the NET Standard library and demonstrates `ConsoleOutputStepProcessor`.

## Build & Pack

From the repo root:

```powershell
.\lib-package.ps1 -Version 1.0.0
```

The package output is placed in `./.packages`.
