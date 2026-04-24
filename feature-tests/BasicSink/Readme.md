## BasicSink Samples

### Overview

This project provides a sample usage of all sink types supported by regular Dotfuscator RASP checks: **Debug Check**, **Tamper Check** and **Root Check**.
The sinks are using `bool` data type.

Note: the sample application has `Press any key to start...` section in the beginning to allow attaching a debugger and check the sink value with/without debugger attached.

Sink supports:
- **static** objects
    - On any accessible obj from current assembly or referenced assemblies
 - **instance** objects
    - On the same class
    - Uses `this` object to call the sink from the same class instance

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
After the obfuscation, you need to manually copy `BasicSink.runtimeconfig.json` from `bin\Release\net10.0` to `Dotfuscated` folder in order to run the application using `dotnet BasicSink.dll` or using dnSpyEx.

