using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crews.Utility.RotatoChip
{
    public class Configuration
    {
        public List<Shortcut> Shortcuts { get; set; }

        public bool StartWithWindows { get; set; }
        public bool ShowSettingsOnStart { get; set; }

        public static Configuration Load()
        {
            if (!File.Exists(ConfigurationPath))
            {
                return NewConfiguration();
            }
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigurationPath));
        }

        public void Save()
        {
            if (StartWithWindows)
            {
                AddRegistryKeyIfNone();
            }
            else
            {
                RemoveRegistryKeyIfExists();
            }
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigurationPath));
            File.WriteAllText(ConfigurationPath, JsonConvert.SerializeObject(this));
        }

        private void AddRegistryKeyIfNone()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_STARTUP_SUBKEY, true);
            key.SetValue("Rotato Chip", Process.GetCurrentProcess().MainModule.FileName);
        }

        private void RemoveRegistryKeyIfExists()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_STARTUP_SUBKEY, true);
            try
            {
                key.DeleteValue("Rotato Chip");
            }
            catch { }
        }

        private static Configuration NewConfiguration()
        {
            Configuration configuration = new() { Shortcuts = new(), ShowSettingsOnStart = true };
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigurationPath));
            File.WriteAllText(ConfigurationPath, JsonConvert.SerializeObject(configuration));
            return configuration;
        }

        private const string REGISTRY_STARTUP_SUBKEY = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        private static readonly string ConfigurationPath =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar +
            $"Crews{Path.DirectorySeparatorChar}Utility{Path.DirectorySeparatorChar}" +
            $"RotatoChip{Path.DirectorySeparatorChar}Configuration.json";
    }
}
