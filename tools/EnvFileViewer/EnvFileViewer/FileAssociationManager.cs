using Microsoft.Win32;

namespace EnvFileViewer;

public static class FileAssociationManager
{
    private const string Extension = ".bin";
    private const string ProgId = "binfile";
    private const string Verb = "Preview";

    /// <summary>
    /// Registers the "Preview" context menu action for *.bin files.
    /// </summary>
    /// <param name="applicationPath">Full path to your application executable.</param>
    public static void RegisterPreviewAction(string applicationPath)
    {
        try
        {
            // 1. Ensure .bin points to our ProgID (binfile)
            using (RegistryKey extKey = Registry.ClassesRoot.CreateSubKey(Extension))
            {
                extKey.SetValue("", ProgId);
            }

            // 2. Create the shell verb structure: binfile\shell\Preview\command
            string verbKeyPath = $@"{ProgId}\shell\{Verb}";
            using (RegistryKey verbKey = Registry.ClassesRoot.CreateSubKey(verbKeyPath))
            {
                verbKey.SetValue("", Verb); // The text shown in the context menu
            }

            using (RegistryKey commandKey = Registry.ClassesRoot.CreateSubKey($@"{verbKeyPath}\command"))
            {
                // Format: "C:\Path\To\App.exe" "%1"
                commandKey.SetValue("", $"\"{applicationPath}\" \"%1\"");
            }

            Console.WriteLine("Successfully registered 'Preview' action.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Error: You must run this application as an Administrator.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during registration: {ex.Message}");
        }
    }

    /// <summary>
    /// De-registers the "Preview" context menu action.
    /// </summary>
    public static void UnregisterPreviewAction()
    {
        try
        {
            string verbKeyPath = $@"{ProgId}\shell\{Verb}";

            // Delete the Preview key and all its subkeys (like 'command')
            Registry.ClassesRoot.DeleteSubKeyTree(verbKeyPath, throwOnMissingSubKey: false);

            Console.WriteLine("Successfully de-registered 'Preview' action.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Error: You must run this application as an Administrator.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during de-registration: {ex.Message}");
        }
    }
}