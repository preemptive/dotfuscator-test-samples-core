# PreEmptive Solutions Dotfuscator Test Samples
## 🚀 Overview
This repository houses a collection of test sample projects designed to demonstrate various C# application types and common language constructs. These samples serve as a robust test bed for Dotfuscator, providing diverse scenarios to validate its obfuscation, protection, and hardening capabilities across different Microsoft .NET platforms.

## 👥 Audience
This repository is primarily intended for the technical team responsible for developing, testing, and maintaining Dotfuscator. The samples provide practical, code-based scenarios to aid in product validation, feature development, and understanding Dotfuscator's behavior across various application types.

## 🌿 Versioning Strategy
To effectively support Dotfuscator's non-regression testing across different .NET framework versions, this repository utilizes a **folder-based versioning strategy within the `main` branch**, allowing for clear separation and targeted backporting.

**`main` Branch Structure:**
- The main branch contains top-level directories, each dedicated to a specific .NET framework version (e.g., dotnet8.0/, dotnet6.0/).
- Each of these directories holds a complete solution (.sln) and its associated projects, all configured specifically for that particular .NET version.
- For example:
```
/
├── dotnet8.0/
│   └── PreEmptive.Dotfuscator.TestSamples.sln (configured for .NET 8)
│       ├── PreEmptive.Dotfuscator.TestSamples.Console/
│       └── ... (other .NET 8 projects)
├── dotnet6.0/
│   └── PreEmptive.Dotfuscator.TestSamples.sln (configured for .NET 6)
│       ├── PreEmptive.Dotfuscator.TestSamples.Console/
│       └── ... (other .NET 6 projects)
└── README.md
```

**Latest Supported Version:**
- The dotnetX.Y/ directory corresponding to the latest supported .NET version (e.g., dotnet8.0/) serves as the primary development area. All new development, feature enhancements, and initial fixes for cutting-edge samples are introduced in the projects within this directory.

**Backporting Changes:**
- When an enhancement, new sample syntax, or bug fix (such as adding an inheritance example after identifying an issue) is implemented within the projects of the latest supported .NET version directory, it can be manuallybackported to the projects in relevant older .NET version directories (e.g., copying the updated class/file from dotnet8.0/ to dotnet6.0/).
- This ensures that critical test scenarios and improvements are propagated to all necessary historical versions, maintaining a robust suite for comprehensive non-regression testing, all within the main branch's history..

## ✨ Key Features
**Microsoft Stack Focus**: All projects are built using the Microsoft .NET ecosystem and C#.

**Diverse Frameworks**: The solution includes projects targeting a variety of .NET application models:
- .NET API
- WPF (Windows Presentation Foundation)
- MAUI (Multi-platform App UI)
- Console App
- Windows Forms

**Comprehensive C# Constructs**: Each project is intentionally designed to incorporate a range of C# language features and application patterns, including but not limited to:
- Object-Oriented Programming (OOP) Concepts: Abstract classes, virtual methods, interfaces, inheritance, polymorphism.
- Resource Handling: Usage of file resources, embedded resources, and other asset types.

## 📦 Project Structure & Naming Convention
The solution is organized to clearly identify the target platform for each sample.

**Naming Convention:** All projects adhere to the following naming convention: `PreEmptive.Dotfuscator.TestSamples.{TargetPlatform}`
Examples:
- PreEmptive.Dotfuscator.TestSamples.Console
- PreEmptive.Dotfuscator.TestSamples.Api
- PreEmptive.Dotfuscator.TestSamples.Maui
- PreEmptive.Dotfuscator.TestSamples.WPF
- PreEmptive.Dotfuscator.TestSamples.WinForms

## 🛠️ Installation Guidelines
To get started with these test samples, follow these simple steps:

1. Clone the Repository:
```Bash
git clone https://github.com/preemptive/dotfuscator-test-samples-core.git
```

2. Install .NET SDK: Ensure you have the .NET SDK installed for the Target Framework. You can download it from the official Microsoft .NET website: https://dotnet.microsoft.com/download

3. Build the Solution: Navigate to the root of the cloned repository and build the entire solution using the .NET CLI:

```Bash
dotnet build
```
This command will restore any necessary NuGet packages and compile all projects within the solution.
