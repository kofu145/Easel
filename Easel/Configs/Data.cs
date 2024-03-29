using System.IO;
using Easel.Math;
using Easel.Utilities;

namespace Easel.Configs;

public static class Data
{
    public static string AppBaseDir = "Data";

    public static string ConfigFile = "Config.cfg";

    public static EaselConfig LoadedConfig;

    public static bool LoadConfig<T>() where T : EaselConfig
    {
        LoadedConfig = GetConfigFromFile<T>(Path.Combine(AppBaseDir, ConfigFile));
        if (LoadedConfig == null)
            return false;

        return true;
    }

    public static bool LoadConfig<T>(ref GameSettings settings) where T : EaselConfig
    {
        if (!LoadConfig<T>())
        {
            settings.Size = new Size(1280, 720);
            settings.VSync = true;

            return false;
        }

        settings.Size = LoadedConfig.DisplayConfig.Size;
        settings.VSync = LoadedConfig.DisplayConfig.VSync;
        // TODO: GameSettings doesn't have a "start in fullscreen" option??

        return true;
    }

    public static void SaveConfig()
    {
        SaveConfigFromFile(Path.Combine(AppBaseDir, ConfigFile), LoadedConfig);
    }

    public static void SaveConfigFromFile(string path, EaselConfig config)
    {
        Logging.Log("Saving config file \"" + ConfigFile + "\".");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, XmlSerializer.Serialize(config));
    }

    public static T GetConfigFromFile<T>(string path) where T : EaselConfig
    {
        Logging.Log("Loading config file \"" + ConfigFile + "\".");
        if (!File.Exists(path))
            return null;
        return XmlSerializer.Deserialize<T>(File.ReadAllText(path));
    }
}