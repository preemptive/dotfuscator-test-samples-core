
# PreEmptive Solutions Dotfuscator Test Samples

## 🚀 Overview

**Samples** is a collection of .NET 9.0 sample applications created to showcase various C# language constructs and project structures. These test samples are specifically crafted to validate the obfuscation, protection, and hardening capabilities of **Dotfuscator** across different Microsoft .NET platforms.

All sample applications target **.NET 9.0**.

---

## 📁 Projects

### 1. `Samples.ConsoleApp`

- Demonstrates core C# constructs:
  - Abstract classes
  - Interfaces
  - Static, virtual, and override methods
- Includes embedded resource files
- Performs file read/write operations
- Fully obfuscated and protected using **Dotfuscator**

### 2. `Helper.ParallelExecution`

- References the `ConsoleApp` DLL
- Executes multiple concurrent Dotfuscator builds in parallel
- Designed to simulate high-concurrency test environments

### 3. `Samples.WinForms`

- Windows Form application which uses resources,Library of classes - Abstract,public, private, Interfaces , unused classes and methods
- Application implements these features
- Designed to simulate high-concurrency test environments

### 4. `Samples.WPF`

- Windows Presentation Foundation (WPF)  application which uses resources,Library of classes - Abstract,public, private, Interfaces , unused classes and methods
- Application implements these features
- Designed to simulate high-concurrency test environments

### 5. `Samples.MAUI`

A MAUI application is a cross-platform app built with .NET MAUI (Multi-platform App UI) — a Microsoft framework for creating native apps using C# and XAML for:

	📲Android
	🍎IOS  
	🖥Windows
	🧭MacOS 

	all from a single shared codebase.
	Sample application tries to cover concurrent file operations on MAUI application

	🧱 MAUI App Basics
	Uses XAML for UI layout (like WPF or Xamarin.Forms).
	Uses C# for app logic and platform-specific services.
	Runs natively on each platform using native UI controls (e.g., Android's Activity, Windows' Window, etc.).

	✅ What You Get with .NET MAUI:
	Feature	Description
	🔄 Single Codebase	One project targets all platforms.
	🎨 Native UI	Renders using native controls on each OS.
	🖼️ XAML + C# UI	Declarative XAML for UI, C# for backend logic.
	🔌 Dependency Injection	Built-in DI container like in ASP.NET Core.
	📦 NuGet Support	Reuse .NET libraries and third-party packages easily.
	🧩 Platform Customization	Add platform-specific code in Platforms/Android, Platforms/Windows, etc.


### 6. `NuGet`
This repository contains sample projects demonstrating how to integrate [PreEmptive Dotfuscator](https://www.preemptive.com/products/dotfuscator/) into a .NET 8 application using the NuGet package.
## 📦 Included Samples

| Project                            | Description                                      |
|------------------------------------|--------------------------------------------------|
| `Samples.ConsoleApp` | A basic console app obfuscated via Dotfuscator. |
| `DotfuscatorConfig.xml`            | Sample Dotfuscator configuration file.           |


## 🔧 Purpose

These projects serve as a robust testbed to:

- Evaluate Dotfuscator's performance under varied application structures
- Validate obfuscation and protection behaviors in real-world C# scenarios
- Test parallelism and concurrency in obfuscation workflows

---

## 🛠 Target Framework

- All projects are built using **.NET 8.0**

---

## 📦 Structure

```
Samples/
├── Samples.ConsoleApp/
└── Helper.ParallelExecution/
├── Samples.WinForms/
└── Samples.WPF/
```

---

## 📜 License

This repository is intended for test and demonstration purposes only. All usage must comply with your organization's license for **PreEmptive Dotfuscator**.
