using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    }
}
