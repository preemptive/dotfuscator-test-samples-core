# NuGet Build and Obfuscation Steps on Ubuntu

This project includes two console applications for build and testing:

- `PreEmptive.Dotfuscator.TestSamples.ConsoleApp`
- `PreEmptive.Dotfuscator.TestSamples.Helper.ParallelExecution`

## Prerequisites

Ensure you have .NET SDK and Runtimes installed.

### 1. Check Installed SDKs

```bash
dotnet --list-sdks
```

Sample output:
```
8.0.117 [/usr/lib/dotnet/sdk]
```

### 2. Check Installed Runtimes

```bash
dotnet --list-runtimes
```

Sample output:
```
Microsoft.AspNetCore.App 8.0.17 [/usr/lib/dotnet/shared/Microsoft.AspNetCore.App]
Microsoft.NETCore.App 8.0.17 [/usr/lib/dotnet/shared/Microsoft.NETCore.App]
```

### 3. Check Default SDK Version

```bash
dotnet --version
```

Output:
```
8.0.117
```

### 4. Check NuGet Version

```bash
dotnet nuget --version
```

Output:
```
NuGet Command Line 6.8.1.32767
```

---

## Environment Setup Instructions

### 5. Configure Environment variables

	DOTFUSCATOR_HOME = /path/nugetInstalledDir/PreEmptive.Protection.Dotfuscator.Pro/tools/programdir/netcore
	DOTFUSCATOR_MSBUILDPATH = /path/nugetInstalledDir/PreEmptive.Protection.Dotfuscator.Pro/tools/msbuilddir
	DOTFUSCATOR_LICENSE = xxxxxxx:user@domain.com

## Build Instructions

### 5. Build the Console Application

```bash
dotnet build -c Release PreEmptive.Dotfuscator.TestSamples.ConsoleApp.csproj
```

### 6. Obfuscate the Console Application

```bash
dotfuscator.exe DotfuscatorConfig.xml -v
```

### 7. Run the Obfuscated Console Application

```bash
bin/Release/net8.0/PreEmptive.Dotfuscator.TestSamples.ConsoleApp
```

---

## Parallel Obfuscation Helper App

The `PreEmptive.Dotfuscator.TestSamples.Helper.ParallelExecution` project uses multiple instances of Dotfuscator to concurrently obfuscate the ConsoleApp.

### 8. Build the ParallelExecution App

```bash
dotnet build -c Release PreEmptive.Dotfuscator.TestSamples.Helper.ParallelExecution.csproj
```

### 9. Obfuscate the ParallelExecution App

```bash
dotfuscator.exe DotfuscatorConfig.xml -v
```

### 10. Run the Obfuscated ParallelExecution App

```bash
bin/Release/net8.0/PreEmptive.Dotfuscator.TestSamples.Helper.ParallelExecution
```

---

## Notes

- Ensure `dotfuscator.exe` is in your PATH or referenced via full path.
- Both applications target `.NET 8.0` and assume the environment is set up correctly for CLI-based builds on Ubuntu.