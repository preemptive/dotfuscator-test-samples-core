## ShelfLifeSink Samples

### Overview

This project provides a sample usage of the following sink types from  **ShelfLife Check**: **ExpirationNotificationSink** and **WarningNotificationSink**.

The sinks are using: 
- `bool` data type
    - It means warning/expired, depending on the selected sink type
- `string` data type:
    - Has `(string warningDate, string expirationDate)` signature

Sink supports:
- **static** objects
    - On any accessible obj from current assembly or referenced assemblies
 - **instance** objects
    - On the same class
    - Uses `this` object to call the sink from the same class instance

**Note:** To keep the samples concise and avoid duplication, only instance sinks are shown. If your testing requires static sinks, simply add the static keyword to the declarations and rebuild the project.

Sink object types:
- **Method**
- **Field**
- **Property**
- **MethodArgument**
    - Custom `delegate` type
    - `Action<bool>`
- **Delegate**
    - Custom `delegate` type
    - `Action<bool>`

### Build

The project has 3 build configurations: **Debug**, **Release**, **ReleaseDotfuscated**

To test the sinks without using Dotfuscator in debug mode, you can select **ReleaseDotfuscated** configuration and build using dotfuscator msbuild integration

To run Dotfuscator from Visual Studio, you should build the project using **Release** configuration, then obfuscate it manually using `dot.xml` configuration.
After the obfuscation, you need to manually copy `ShelfLifeSink.runtimeconfig.json` from `bin\Release\net10.0` to `Dotfuscated` folder in order to run the application using `dotnet ShelfLifeSink.dll`.
