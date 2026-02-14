using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minesweeper_WPF
{
    internal class ThemeFinder
    {
        public static List<string> GetThemeList()
        {
            string themesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Themes");
            
            if (!Directory.Exists(themesPath))
            {
                return new List<string>();
            }

            List<string> themeNames =
                Directory.GetDirectories(themesPath)
                         .Where(dir => File.Exists(Path.Combine(dir, "ThemeConfig.json")))
                         .Select(Path.GetFileName)
                         .ToList();
            return themeNames;
        }
        public static string GetThemeNameFromJson(string themeFolder)
        {
            if (string.IsNullOrWhiteSpace(themeFolder))
                return themeFolder ?? string.Empty;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, "Assets", "Themes", themeFolder, "ThemeConfig.json");

            if (!File.Exists(path))
                return themeFolder;

            try
            {
                string json = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("ImageNames", out JsonElement imageNames))
                {
                    if (imageNames.TryGetProperty("ThemeName", out JsonElement nameElement))
                    {
                        return nameElement.GetString() ?? themeFolder;
                    }
                }
            }
            catch
            {

            }

            return themeFolder;
        }

        public static string GetThemeImageFromJson(string themeFolder)
        {
            if (string.IsNullOrWhiteSpace(themeFolder))
                return "pack.png";

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, "Assets", "Themes", themeFolder, "ThemeConfig.json");

            if (!File.Exists(path))
                return "pack.png";

            try
            {
                string json = File.ReadAllText(path);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("ImageNames", out JsonElement imageNames))
                {
                    if (imageNames.TryGetProperty("ThemeImage", out JsonElement imageElement))
                    {
                        return imageElement.GetString() ?? "pack.png";
                    }
                }
            }
            catch
            {
                // ignore parse errors and fall back
            }

            return "pack.png";
        }
    }
}
