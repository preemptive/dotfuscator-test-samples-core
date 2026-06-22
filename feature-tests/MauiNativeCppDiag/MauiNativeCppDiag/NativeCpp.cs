using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MauiNativeCppDiag
{
#pragma warning disable CA1060
    public static class NativeCPP
#pragma warning restore CA1060
    {
        private const string envFileName = "environment0f617e02.bin";
        static bool isMaui = Type.GetType("Microsoft.Maui.Storage.FileSystem, Microsoft.Maui.Essentials") != null;
        static NativeCPP()
        {
            if (IsAndroid())
            {
                TryPreloadAndroidLibrary();
                return;
            }

            if (IsIOS() || IsMacCatalyst())
            {
                TryPreloadIOSLibrary();
                return;
            }

            try
            {
                string rid = GetCurrentRuntimeIdentifier();
                string runtimesPath = GetRuntimesPath();

                if (string.IsNullOrEmpty(runtimesPath))
                {
                    throw new FileNotFoundException(
                        $"Native library runtime directory not found. " +
                        $"Expected structure: <dir>/runtimes/{rid}/native/ " +
                        $"Searched: assembly dir '{typeof(NativeCPP).Assembly.Location}', " +
                        $"AppContext.BaseDirectory '{AppContext.BaseDirectory}'");
                }

                string libPath = Path.Combine(runtimesPath, GetPlatformLibraryFileName());

                if (!File.Exists(libPath))
                {
                    throw new FileNotFoundException(
                        $"Native library file not found. Expected: {libPath} " +
                        $"OS: {RuntimeInformation.OSDescription}, Architecture: {RuntimeInformation.ProcessArchitecture}",
                        libPath);
                }

                IntPtr libraryHandle = TryPreload(libPath);

                if (libraryHandle != IntPtr.Zero)
                    SetupDllImportResolver(libraryHandle);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    SetDllDirectory(runtimesPath);
            }
            catch (PlatformNotSupportedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to initialize native library for V2 String Encryption. " +
                    $"OS: {RuntimeInformation.OSDescription}, Architecture: {RuntimeInformation.ProcessArchitecture}, " +
                    $"BaseDirectory: {AppContext.BaseDirectory}",
                    ex);
            }
        }

        /// <summary>
        /// Gets the path to the native library in the standard .NET RID structure.
        /// Returns null if the RID structure doesn't exist in any of the searched locations.
        /// Throws PlatformNotSupportedException if platform is not supported.
        /// </summary>
        private static string GetRuntimesPath()
        {
            string rid = GetCurrentRuntimeIdentifier();

            string assemblyLocation = typeof(NativeCPP).Assembly.Location;
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                string assemblyDir = Path.GetDirectoryName(assemblyLocation);
                if (!string.IsNullOrEmpty(assemblyDir))
                {
                    string runtimesPath = Path.Combine(assemblyDir, "runtimes", rid, "native");
                    if (Directory.Exists(runtimesPath))
                        return runtimesPath;
                }
            }

            string fallbackPath = Path.Combine(AppContext.BaseDirectory, "runtimes", rid, "native");
            if (Directory.Exists(fallbackPath))
                return fallbackPath;

            return null;
        }

        /// <summary>
        /// Gets the current Runtime Identifier (RID) based on OS and architecture.
        /// Throws PlatformNotSupportedException for unsupported OS or architecture combinations.
        /// </summary>
        private static string GetCurrentRuntimeIdentifier()
        {
            Architecture arch = RuntimeInformation.ProcessArchitecture;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (arch)
                {
                    case Architecture.X64:
                        return "win-x64";
                    case Architecture.X86:
                        return "win-x86";
                    default:
                        throw new PlatformNotSupportedException(
                            $"Unsupported Windows architecture: {arch}");
                }
            }
            if (IsAndroid())
            {
                switch (arch)
                {
                    case Architecture.Arm64:
                        return "android-arm64";
                    case Architecture.X64:
                        return "android-x64";
                    case Architecture.X86:
                        return "android-x86";
                    default:
                        throw new PlatformNotSupportedException(
                            $"Unsupported Android architecture: {arch}");
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                switch (arch)
                {
                    case Architecture.X64:
                        return "linux-x64";
                    case Architecture.Arm64:
                        return "linux-arm64";
                    default:
                        throw new PlatformNotSupportedException(
                            $"Unsupported Linux architecture: {arch}");
                }
            }
            if (IsIOS())
            {
                if (IsIOSSimulator())
                {
                    switch (arch)
                    {
                        case Architecture.Arm64:
                            return "iossimulator-arm64";
                        case Architecture.X64:
                            return "iossimulator-x64";
                        default:
                            throw new PlatformNotSupportedException(
                                $"Unsupported iOS Simulator architecture: {arch}");
                    }
                }
                else
                {
                    switch (arch)
                    {
                        case Architecture.Arm64:
                            return "ios-arm64";
                        default:
                            throw new PlatformNotSupportedException(
                                $"Unsupported iOS device architecture: {arch}");
                    }
                }
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (IsMacCatalyst())
                {
                    switch (arch)
                    {
                        case Architecture.Arm64:
                            return "maccatalyst-arm64";
                        case Architecture.X64:
                            return "maccatalyst-x64";
                        default:
                            throw new PlatformNotSupportedException(
                                $"Unsupported Mac Catalyst architecture: {arch}");
                    }
                }
                switch (arch)
                {
                    case Architecture.X64:
                        return "osx-x64";
                    case Architecture.Arm64:
                        return "osx-arm64";
                    default:
                        throw new PlatformNotSupportedException(
                            $"Unsupported macOS architecture: {arch}");
                }
            }

            throw new PlatformNotSupportedException(
                $"Unsupported operating system: {RuntimeInformation.OSDescription} (Architecture: {arch})");
        }

        private static byte[] envData;
        private static GCHandle envDataHandle;

        private static void InitEnvData()
        {
            if (envData == null)
            {
                if (isMaui)
                {
                    ReadEnvDataMaui();
                }
                else
                {
                    ReadEnvData();
                }

                // Pin envData so it won't be moved by GC
                envDataHandle = GCHandle.Alloc(envData, GCHandleType.Pinned);
            }
        }

        private static void ReadEnvDataMaui()
        {
            try
            {
                // Get the FileSystem type from the MAUI assembly
                var fileSystemType = Type.GetType("Microsoft.Maui.Storage.FileSystem, Microsoft.Maui.Essentials");
                if (fileSystemType == null)
                    throw new InvalidOperationException("FileSystem type not found. MAUI assembly may not be loaded.");

                // Get the OpenAppPackageFileAsync method
                var method = fileSystemType.GetMethod("OpenAppPackageFileAsync",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                    null,
                    new[] { typeof(string) },
                    null);

                if (method == null)
                    throw new InvalidOperationException("OpenAppPackageFileAsync method not found.");

                // Invoke the method and get the Task
                var task = method.Invoke(null, new object[] { envFileName }) as System.Threading.Tasks.Task;
                if (task == null)
                    throw new InvalidOperationException("OpenAppPackageFileAsync returned null.");

                // Wait for the task to complete
                task.Wait();

                // Get the Result property from the Task<Stream>
                var resultProperty = task.GetType().GetProperty("Result");
                var stream = resultProperty?.GetValue(task) as System.IO.Stream;

                if (stream == null)
                    throw new InvalidOperationException("Failed to get Stream from task result.");

                envData = new byte[20480];
                stream.Read(envData, 0, 20480);
                stream.Close();
                stream.Dispose();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to read environment data using MAUI FileSystem.", ex);
            }
        }

        private static void ReadEnvData()
        {
            try
            {
                var assemblyFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                envData = File.ReadAllBytes($"{assemblyFolder}/{envFileName}");
            }
            catch
            {
                envData = new byte[20480]; // Fallback to empty data if file read fails
            }
        }

        public static byte[] GetEnvData()
        {
            InitEnvData();
            return envData;
        }

        public static IntPtr GetEnvDataPtr()
        {
            InitEnvData();
            return envDataHandle.AddrOfPinnedObject();
        }

        /// <summary>
        /// Computes the key using the native enc_create_key.
        /// CRITICAL: This method must be as simple as possible to avoid .NET Framework JIT limitations.
        /// Even validation code can trigger JIT errors with jagged arrays!
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static byte[] ComputeKey(byte[][] parts)
        {
#if NET472 || NET48
            return ComputeKeyframework(parts);
#else
            return ComputeKeyModern(parts);
#endif
        }

#if !NET472 && !NET48
        /// <summary>
        /// Modern .NET implementation using GCHandle for memory pinning.
        /// </summary>
        private static byte[] ComputeKeyModern(byte[][] parts)
        {
            GCHandle[] handles = new GCHandle[parts.Length];
            IntPtr[] ptrs = new IntPtr[parts.Length];
            int[] lens = new int[parts.Length];
            GCHandle ptrsHandle = default;
            GCHandle lensHandle = default;

            try
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == null)
                        throw new ArgumentException("Invalid key parts.", nameof(parts));
                    handles[i] = GCHandle.Alloc(parts[i], GCHandleType.Pinned);
                    ptrs[i] = handles[i].AddrOfPinnedObject();
                    lens[i] = parts[i].Length;
                }

                ptrsHandle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
                lensHandle = GCHandle.Alloc(lens, GCHandleType.Pinned);

                byte[] outHash = new byte[32];
                int rc = __eck(
                    GetEnvDataPtr(),
                    ptrsHandle.AddrOfPinnedObject(),
                    lensHandle.AddrOfPinnedObject(),
                    parts.Length,
                    outHash);

                if (rc != 0)
                    throw new InvalidOperationException($"__eck failed (rc={rc}).");

                return outHash;
            }
            finally
            {
                if (lensHandle.IsAllocated)
                    lensHandle.Free();
                if (ptrsHandle.IsAllocated)
                    ptrsHandle.Free();

                for (int i = 0; i < handles.Length; i++)
                {
                    if (handles[i].IsAllocated)
                        handles[i].Free();
                }
            }
        }
#endif

#if NET472 || NET48
        /// <summary>
        /// .NET Framework implementation using unsafe pointers.
        /// This is the SIMPLEST possible implementation to avoid JIT limitations.
        /// Split into multiple tiny methods, each doing one simple thing.
        /// </summary>
        private static unsafe byte[] ComputeKeyframework(byte[][] parts)
        {
            int count = parts.Length;
            for (int i = 0; i < count; i++)
            {
                if (parts[i] == null)
                    throw new ArgumentException("Invalid key parts.");
            }

            byte** ptrsArray = (byte**)Marshal.AllocHGlobal(IntPtr.Size * count);
            int* lensArray = (int*)Marshal.AllocHGlobal(sizeof(int) * count);

            CopyPartsToUnmanaged(parts, ptrsArray, lensArray, count);

            byte[] result = CallNativeAndCleanup(ptrsArray, lensArray, count);

            return result;
        }

        /// <summary>
        /// Separate method: Copy managed arrays to unmanaged memory.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static unsafe void CopyPartsToUnmanaged(byte[][] parts, byte** ptrsArray, int* lensArray, int count)
        {
            for (int i = 0; i < count; i++)
            {
                byte[] part = parts[i];
                int len = part.Length;

                byte* buffer = (byte*)Marshal.AllocHGlobal(len);

                fixed (byte* src = part)
                {
                    for (int j = 0; j < len; j++)
                    {
                        buffer[j] = src[j];
                    }
                }

                ptrsArray[i] = buffer;
                lensArray[i] = len;
            }
        }

        /// <summary>
        /// Separate method: Call native function and cleanup.
        /// CRITICAL: Must free all allocated memory.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static unsafe byte[] CallNativeAndCleanup(byte** ptrsArray, int* lensArray, int count)
        {
            byte[] outHash = new byte[32];
            int rc = 0;

            try
            {
                rc = __eck(GetEnvDataPtr(), (IntPtr)ptrsArray, (IntPtr)lensArray, count, outHash);
            }
            finally
            {
                for (int i = 0; i < count; i++)
                {
                    byte* dataPtr = ptrsArray[i];
                    if (dataPtr != null)
                    {
                        Marshal.FreeHGlobal((IntPtr)dataPtr);
                    }
                }

                if (ptrsArray != null)
                    Marshal.FreeHGlobal((IntPtr)ptrsArray);
                if (lensArray != null)
                    Marshal.FreeHGlobal((IntPtr)lensArray);
            }

            if (rc != 0)
                throw new InvalidOperationException($"__eck failed (rc={rc}).");

            return outHash;
        }
#endif

        private static string GetPlatformLibraryFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "Runtime.Extensions.dll";

            if (IsIOS() || IsMacCatalyst())
                return "Runtime.Extensions.dylib";

            if (IsAndroid())
                return "Runtime.Extensions.so";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "Runtime.Extensions.so";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "Runtime.Extensions.dylib";

            throw new PlatformNotSupportedException(
                $"Cannot determine native library filename for OS: {RuntimeInformation.OSDescription}");
        }

        /// <summary>
        /// Detects if running on Android platform.
        /// Android reports as Linux, so we need additional checks.
        /// </summary>
        private static bool IsAndroid()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsAndroid();
#else
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return false;

            try
            {
                var monoAndroidType = Type.GetType("Android.Runtime.JNIEnv, Mono.Android");
                if (monoAndroidType != null)
                    return true;

                var androidEnvType = Type.GetType("Android.OS.Build, Mono.Android");
                if (androidEnvType != null)
                    return true;

                string osDesc = RuntimeInformation.OSDescription.ToLowerInvariant();
                if (osDesc.Contains("android"))
                    return true;
            }
            catch {}

            return false;
#endif
        }

        /// <summary>
        /// Detects if running on iOS platform (device or simulator).
        /// iOS does not have OSPlatform.iOS, so we check for Xamarin.iOS or MAUI iOS types.
        /// </summary>
        private static bool IsIOS()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsIOS();
#else
            try
            {
                var uiDeviceType = Type.GetType("UIKit.UIDevice, Xamarin.iOS")
                                ?? Type.GetType("UIKit.UIDevice, Microsoft.iOS");
                if (uiDeviceType != null)
                    return true;

                var nsObjectType = Type.GetType("Foundation.NSObject, Microsoft.iOS");
                if (nsObjectType != null)
                    return true;

                var objcRuntimeType = Type.GetType("ObjCRuntime.Runtime, Xamarin.iOS");
                if (objcRuntimeType != null)
                    return true;
            }
            catch {}

            return false;
#endif
        }

        /// <summary>
        /// Detects if running on iOS Simulator vs physical device.
        /// </summary>
        private static bool IsIOSSimulator()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsIOS() && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SIMULATOR_DEVICE_NAME"));
#else
            try
            {
                var uiDeviceType = Type.GetType("UIKit.UIDevice, Xamarin.iOS") ??
                                   Type.GetType("UIKit.UIDevice, Microsoft.iOS");

                if (uiDeviceType != null)
                {
                    var currentProperty = uiDeviceType.GetProperty("CurrentDevice");
                    if (currentProperty != null)
                    {
                        var currentDevice = currentProperty.GetValue(null);
                        if (currentDevice != null)
                        {
                            var modelProperty = currentDevice.GetType().GetProperty("Model");
                            if (modelProperty != null)
                            {
                                var model = modelProperty.GetValue(currentDevice) as string;
                                if (model != null && model.Contains("Simulator"))
                                    return true;
                            }
                        }
                    }
                }

                var simDeviceName = Environment.GetEnvironmentVariable("SIMULATOR_DEVICE_NAME");
                if (!string.IsNullOrEmpty(simDeviceName))
                    return true;
            }
            catch {}

            return false;
#endif
        }

        /// <summary>
        /// Detects if running as Mac Catalyst (iOS app on macOS).
        /// </summary>
        private static bool IsMacCatalyst()
        {
#if NET6_0_OR_GREATER
            return OperatingSystem.IsMacCatalyst();
#else
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return false;

            try
            {
                var uiDeviceType = Type.GetType("UIKit.UIDevice, Microsoft.iOS") ??
                                   Type.GetType("UIKit.UIDevice, Xamarin.iOS");

                if (uiDeviceType != null)
                    return true;

                var idiom = Environment.GetEnvironmentVariable("DOTNET_TARGET_PLATFORM_IDIOM");
                if (idiom == "maccatalyst")
                    return true;
            }
            catch {}

            return false;
#endif
        }

        /// <summary>
        /// On iOS / macCatalyst the dylib is in the flat app-bundle root.  Load it by full path
        /// so that SetDllImportResolver gets a valid handle and DllImport("Runtime.Extensions")
        /// resolves to the correct native library regardless of dlopen name-matching behaviour.
        /// </summary>
        private static void TryPreloadIOSLibrary()
        {
#if NET5_0_OR_GREATER
            try
            {
                var candidateDirs = new[]
                {
                    AppContext.BaseDirectory,
                    Path.GetDirectoryName(typeof(NativeCPP).Assembly.Location),
                };

                foreach (var dir in candidateDirs)
                {
                    if (string.IsNullOrEmpty(dir))
                        continue;

                    string libPath = Path.Combine(dir, "libRuntime.Extensions.dylib");
                    IntPtr handle = TryPreload(libPath);
                    if (handle != IntPtr.Zero)
                    {
                        SetupDllImportResolver(handle);
                        return;
                    }
                }
            }
            catch { }
#endif
        }

        /// <summary>
        /// On Android the native library is packaged in the APK as "libRuntime.Extensions.so".
        /// Preload it and cache the handle so the (post-obfuscation) enc_create_key resolves the
        /// export from loadedLibraryHandle instead of relying on a non-probing NativeLibrary.TryLoad
        /// of the bare name "Runtime.Extensions", which fails because dlopen needs the lib/.so form.
        /// </summary>
        private static void TryPreloadAndroidLibrary()
        {
#if NET5_0_OR_GREATER
            try
            {
                // 1) Probing overload — same resolution the original [DllImport] used
                //    (applies the "lib" prefix and ".so" suffix and searches the app lib dir).
                if (NativeLibrary.TryLoad(
                        "Runtime.Extensions",
                        typeof(NativeCPP).Assembly,
                        DllImportSearchPath.SafeDirectories | DllImportSearchPath.AssemblyDirectory,
                        out IntPtr handle)
                    && handle != IntPtr.Zero)
                {
                    SetupDllImportResolver(handle);
                    return;
                }

                // 2) Fallback: explicit platform names via the simple loader (dlopen by soname).
                string[] candidates = { "libRuntime.Extensions.so", "Runtime.Extensions.so", "Runtime.Extensions" };
                foreach (string name in candidates)
                {
                    if (NativeLibrary.TryLoad(name, out handle) && handle != IntPtr.Zero)
                    {
                        SetupDllImportResolver(handle);
                        return;
                    }
                }
            }
            catch { }
#endif
        }

        private static IntPtr TryPreload(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
                return IntPtr.Zero;

#if NET5_0_OR_GREATER
            try
            {
                IntPtr handle = NativeLibrary.Load(fullPath);
                return handle;
            }
            catch
            {
                return IntPtr.Zero;
            }
#else
            return IntPtr.Zero;
#endif
        }

#if NET5_0_OR_GREATER
        private static IntPtr loadedLibraryHandle;
#endif

        private static void SetupDllImportResolver(IntPtr libraryHandle)
        {

#if NET5_0_OR_GREATER

            loadedLibraryHandle = libraryHandle;

            DllImportResolver resolver = new DllImportResolver(ResolveDllImport);
            NativeLibrary.SetDllImportResolver(typeof(NativeCPP).Assembly, resolver);
#endif
        }

#if NET5_0_OR_GREATER
        private static IntPtr ResolveDllImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == "Runtime.Extensions")
                return loadedLibraryHandle;

            return IntPtr.Zero;
        }
#endif

#pragma warning disable CA2101

        [DllImport("kernel32.dll",
            EntryPoint = "SetDllDirectoryW",
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = true)]
        private static extern bool SetDllDirectory(
            [MarshalAs(UnmanagedType.LPWStr)] string lpPathName);
#pragma warning restore CA2101

#if NET472 || NET48
        [DllImport("Runtime.Extensions.dll", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern int enc_create_key(
            IntPtr parts, IntPtr lengths, int count, [Out] byte[] outHash32);

        [DllImport("Runtime.Extensions.dll", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "__eck")]
        private static extern int __eck(
            IntPtr a, IntPtr b, IntPtr c, int d, [Out] byte[] e);
#else
        // .NET 5+ (and .NET Core): omit extension for cross‑platform resolution
        [DllImport("Runtime.Extensions", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static extern int enc_create_key(
            IntPtr parts, IntPtr lengths, int count, [Out] byte[] outHash32);

        [DllImport("Runtime.Extensions", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "__eck")]
        private static extern int __eck(
            IntPtr a, IntPtr b, IntPtr c, int d, [Out] byte[] e);
#endif
    }
}
