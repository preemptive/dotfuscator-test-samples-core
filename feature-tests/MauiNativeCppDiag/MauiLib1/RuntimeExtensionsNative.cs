using System.Reflection;
using System.Runtime.InteropServices;

namespace MauiLib1;

public static class RuntimeExtensionsNative
{
    private const string LibName = "Runtime.Extensions";
    static bool isMaui = Type.GetType("Microsoft.Maui.Storage.FileSystem, Microsoft.Maui.Essentials") != null;

    [DllImport(LibName, EntryPoint = "enc_create_key")]
    private static extern int enc_create_key(
        IntPtr parts,
        IntPtr lengths,
        int count,
        [Out] byte[] outHash32);

    [DllImport(LibName, EntryPoint = "enc_create_key2")]
    private static extern int enc_create_key2(
        IntPtr env_data,
        IntPtr parts,
        IntPtr lengths,
        int count,
        [Out] byte[] outHash32);

    [DllImport(LibName, EntryPoint = "parse_env")]
    private static extern uint parse_env(
    IntPtr env_data,
    [Out] byte[] outData);

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
            var task = method.Invoke(null, new object[] { "environment0f617e02.bin" }) as System.Threading.Tasks.Task;
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
            envData = File.ReadAllBytes($"{assemblyFolder}/environment0f617e02.bin");
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
    /// Managed wrapper for enc_create_key. Pins each part, calls the native function,
    /// and returns the resulting 32-byte hash.
    /// </summary>
    /// <param name="parts">Array of byte arrays to use as key material.</param>
    /// <returns>32-byte hash output, or null if the native call fails.</returns>
    public static byte[]? CreateKey(byte[][] parts)
    {
        ArgumentNullException.ThrowIfNull(parts);

        int count = parts.Length;
        var outHash = new byte[32];

        // Pin each part and collect pointers + lengths
        var handles = new GCHandle[count];
        var partPtrs = new IntPtr[count];
        var lengths = new int[count];

        try
        {
            for (int i = 0; i < count; i++)
            {
                handles[i] = GCHandle.Alloc(parts[i], GCHandleType.Pinned);
                partPtrs[i] = handles[i].AddrOfPinnedObject();
                lengths[i] = parts[i].Length;
            }

            // Pin the pointer and length arrays before passing to native
            var partPtrsHandle = GCHandle.Alloc(partPtrs, GCHandleType.Pinned);
            var lengthsHandle = GCHandle.Alloc(lengths, GCHandleType.Pinned);

            try
            {
                int result = enc_create_key(
                    partPtrsHandle.AddrOfPinnedObject(),
                    lengthsHandle.AddrOfPinnedObject(),
                    count,
                    outHash);

                return result == 0 ? outHash : null;
            }
            finally
            {
                partPtrsHandle.Free();
                lengthsHandle.Free();
            }
        }
        finally
        {
            for (int i = 0; i < count; i++)
            {
                if (handles[i].IsAllocated)
                    handles[i].Free();
            }
        }
    }

    public static byte[]? CreateKey2(byte[][] parts)
    {
        ArgumentNullException.ThrowIfNull(parts);

        int count = parts.Length;
        var outHash = new byte[32];

        // Pin each part and collect pointers + lengths
        var handles = new GCHandle[count];
        var partPtrs = new IntPtr[count];
        var lengths = new int[count];

        try
        {
            for (int i = 0; i < count; i++)
            {
                handles[i] = GCHandle.Alloc(parts[i], GCHandleType.Pinned);
                partPtrs[i] = handles[i].AddrOfPinnedObject();
                lengths[i] = parts[i].Length;
            }

            // Pin the pointer and length arrays before passing to native
            var partPtrsHandle = GCHandle.Alloc(partPtrs, GCHandleType.Pinned);
            var lengthsHandle = GCHandle.Alloc(lengths, GCHandleType.Pinned);

            try
            {
                int result = enc_create_key2(
                    GetEnvDataPtr(),
                    partPtrsHandle.AddrOfPinnedObject(),
                    lengthsHandle.AddrOfPinnedObject(),
                    count,
                    outHash);

                return result == 0 ? outHash : null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                partPtrsHandle.Free();
                lengthsHandle.Free();
            }
        }
        finally
        {
            for (int i = 0; i < count; i++)
            {
                if (handles[i].IsAllocated)
                    handles[i].Free();
            }
        }
    }

    public static EnvFileData ParseEnv()
    {
        var outData = new byte[32];

        try
        {
            uint keySeed = parse_env(
                GetEnvDataPtr(),
                outData);

            return new EnvFileData
            {
                KeySeed = keySeed,
                Status = outData[0],
                ModuleCount = outData[1]
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public class EnvFileData
    {
        public uint KeySeed { get; set; }
        public byte Status { get; set; }
        public byte ModuleCount { get; set; }
    }
}