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
            string themesPath = Path.Combine("Assets", "Themes");

            List<string> themeNames =
                Directory.GetDirectories(themesPath)
                         .Where(dir => File.Exists(Path.Combine(dir, "ThemeConfig.json")))
                         .Select(Path.GetFileName)
                         .ToList();
            return themeNames;
        }
    }
}
