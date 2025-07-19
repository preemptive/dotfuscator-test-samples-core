# 🎯 NuGet Build and Obfuscation on Ubuntu

This project includes two console applications for demonstration and testing:

- 🖥️ `Samples.ConsoleApp`
- ⚙️ `Helper.ParallelExecution`

---

## 🚀 Prerequisites

Ensure you have the .NET SDK and Runtimes installed on Ubuntu.

### ✅ 1. Check Installed SDKs

```bash
dotnet --list-sdks
```

<sub>Sample output:</sub>

```
8.0.117 [/usr/lib/dotnet/sdk]
```

---

### ✅ 2. Check Installed Runtimes

```bash
dotnet --list-runtimes
```

<sub>Sample output:</sub>

```
Microsoft.AspNetCore.App 8.0.17 [/usr/lib/dotnet/shared/Microsoft.AspNetCore.App]
Microsoft.NETCore.App 8.0.17 [/usr/lib/dotnet/shared/Microsoft.NETCore.App]
```

---

### ✅ 3. Check Default SDK Version

```bash
dotnet --version
```

<sub>Sample output:</sub>

```
8.0.117
```

---

### ✅ 4. Check NuGet Version

```bash
dotnet nuget --version
```

<sub>Sample output:</sub>

```
NuGet Command Line 6.8.1.32767
```

---

## ⚙️ Environment Setup

### 🔧 5. Configure Environment Variables

```bash
export DOTFUSCATOR_HOME=/path/nugetInstalledDir/PreEmptive.Protection.Dotfuscator.Pro/tools/programdir/netcore
export DOTFUSCATOR_MSBUILDPATH=/path/nugetInstalledDir/PreEmptive.Protection.Dotfuscator.Pro/tools/msbuilddir
export DOTFUSCATOR_LICENSE=xxxxxxx:user@domain.com
```

Add these to your `.bashrc` or `.zshrc` to persist across sessions.

---

## 🏗️ Build Instructions

### 🧪 6. Build the Console App

```bash
dotnet build -c Release Samples.ConsoleApp.csproj
```

---

### 🔐 7. Obfuscate the Console App

```bash
dotfuscator.exe DotfuscatorConfig.xml -v
```

---

### 🚦 8. Run the Obfuscated Console App

```bash
dotnet run
```

---

## 🔁 Parallel Obfuscation Helper App

The `Helper.ParallelExecution` project uses multiple instances of Dotfuscator to concurrently obfuscate the ConsoleApp.

### 🧱 9. Build the ParallelExecution App

```bash
dotnet build -c Release Helper.ParallelExecution.csproj
```

---

### 🔐 10. Obfuscate the ParallelExecution App

```bash
dotfuscator.exe DotfuscatorConfig.xml -v
```

---

### 🚦 11. Run the Obfuscated ParallelExecution App

```bash
dotnet run
```

---

## 📝 Notes

- Ensure `dotfuscator.exe` is either in your `$PATH` or used with the full path.
- Both applications target **.NET 8.0**.
- Designed for CLI-based builds on **Ubuntu**.

---

> 💡 Tip: Use `htop` or `top` to monitor CPU usage while running ParallelExecution for better insight into concurrency.