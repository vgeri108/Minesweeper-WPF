using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Minesweeper_WPF
{
    class Appearance
    {
        public class BoardColors
        {

        }
        public class Characters
        {
            public static string semmi = " ";
            public static string zaszlo = "!";
            public static string akna = "*";
        }
        public class Images
        {
            public static Dictionary<string, string> ImageNames = new Dictionary<string, string>()
            {
                { "error", "Images/!noTexture.png" },
                { "fedes", "Images/Covered.png" },
                { "zaszlozott", "Images/Flagged.png" },
                { "kerdojel", "Images/Question.png" },
                { "semmi", "Images/Empty.png" },
                { "akna", "Images/Mine.png" },
                { "aknaNyomva", "Images/MineClicked.png" },
                { "aknaNemNyomott", "Images/MineNotClicked.png" },
                { "1", "Images/1.png" },
                { "2", "Images/2.png" },
                { "3", "Images/3.png" },
                { "4", "Images/4.png" },
                { "5", "Images/5.png" },
                { "6", "Images/6.png" },
                { "7", "Images/7.png" },
                { "8", "Images/8.png" },

                { "Clock", "Images/GUI/clock.png" },
                { "Flower", "Images/GUI/flower.png" },
                { "Hatter", "Images/GUI/hatter.png" },
                { "win?", "Images/GUI/windows.png" },
                { "win7", "Images/GUI/windows7.png" },
                { "win8", "Images/GUI/windows8.png" },
                { "win10", "Images/GUI/windows10.png" },
                { "win11", "Images/GUI/windows11.png" },
            };

            // Try resolving the path to an existing file in several likely locations under the app output folder.
            // If a matching file is found, return an absolute Uri to that file (so BitmapImage can load reliably).
            // Otherwise return the original relative URI under Assets/Themes/{CurrentTheme}/ to preserve current behaviour.
            private static Uri ResolveUri(string relativePath)
            {
                // Candidate base paths (order matters)
                string themeRootCandidate = $"Assets/Themes/{Configuration.CurrentTheme}";
                string[] bases = new[]
                {
                    themeRootCandidate,                                           // preferred: Assets/Themes/{Theme}/<rel>
                    $"Assets/Images/GameBoard/{Configuration.CurrentTheme}",      // alternate layout
                    "Assets/Images",                                              // common layout
                    "Assets"                                                      // fallback
                };

                foreach (var b in bases)
                {
                    string candidate = Path.Combine(b, relativePath.Replace('/', Path.DirectorySeparatorChar));
                    string abs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, candidate);
                    try
                    {
                        if (File.Exists(abs))
                        {
                            // return absolute file Uri
                            return new Uri(abs, UriKind.Absolute);
                        }
                    }
                    catch
                    {
                        // ignore and continue
                    }
                }

                // no existing file found; return same relative path under theme root (keeps previous behaviour)
                string relUri = $"{themeRootCandidate}/{relativePath}".Replace('\\', '/');
                return new Uri(relUri, UriKind.Relative);
            }

            public static Uri error => ResolveUri(ImageNames["error"]);
            public static Uri fedes => ResolveUri(ImageNames["fedes"]);
            public static Uri zaszlozott => ResolveUri(ImageNames["zaszlozott"]);
            public static Uri kerdojel => ResolveUri(ImageNames["kerdojel"]);
            public static Uri semmi => ResolveUri(ImageNames["semmi"]);
            public static Uri akna => ResolveUri(ImageNames["akna"]);
            public static Uri aknaNyomva => ResolveUri(ImageNames["aknaNyomva"]);
            public static Uri aknaNemNyomott => ResolveUri(ImageNames["aknaNemNyomott"]);
            public static Uri _1 => ResolveUri(ImageNames["1"]);
            public static Uri _2 => ResolveUri(ImageNames["2"]);
            public static Uri _3 => ResolveUri(ImageNames["3"]);
            public static Uri _4 => ResolveUri(ImageNames["4"]);
            public static Uri _5 => ResolveUri(ImageNames["5"]);
            public static Uri _6 => ResolveUri(ImageNames["6"]);
            public static Uri _7 => ResolveUri(ImageNames["7"]);
            public static Uri _8 => ResolveUri(ImageNames["8"]);

            public static Uri Clock => ResolveUri(ImageNames["Clock"]);
            public static Uri Flower => ResolveUri(ImageNames["Flower"]);
            public static Uri Hatter => ResolveUri(ImageNames["Hatter"]);
            public static Uri windows => ResolveUri(ImageNames["win?"]);
            public static Uri windows7 => ResolveUri(ImageNames["win7"]);
            public static Uri windows8 => ResolveUri(ImageNames["win8"]);
            public static Uri windows10 => ResolveUri(ImageNames["win10"]);
            public static Uri windows11 => ResolveUri(ImageNames["win11"]);
        }
    }
}
