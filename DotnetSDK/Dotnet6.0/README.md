
# PreEmptive Solutions Dotfuscator Test Samples

## 🚀 Overview

**Samples** is a collection of .NET 6.0 sample applications created to showcase various C# language constructs and project structures. These test samples are specifically crafted to validate the obfuscation, protection, and hardening capabilities of **Dotfuscator** across different Microsoft .NET platforms.

All sample applications target **.NET 6.0**.

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

### 2. `Samples.Helper.ParallelExecution`

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

---

## 🔧 Purpose

These projects serve as a robust testbed to:

- Evaluate Dotfuscator's performance under varied application structures
- Validate obfuscation and protection behaviors in real-world C# scenarios
- Test parallelism and concurrency in obfuscation workflows

---

## 🛠 Target Framework

- All projects are built using **.NET 6.0**

---

## 📦 Structure

```
Samples/
├── Samples.ConsoleApp/
└── Samples.Helper.ParallelExecution/
├── Samples.WinForms/
└── Samples.WPF/
```

---

## 📜 License

This repository is intended for test and demonstration purposes only. All usage must comply with your organization's license for **PreEmptive Dotfuscator**.
