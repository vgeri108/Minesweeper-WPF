using System;
using System.Collections.Generic;
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
                { "error", "!noTexture.png" },
                { "fedes", "Covered.png" },
                { "zaszlozott", "Flagged.png" },
                { "kerdojel", "Question.png" },
                { "semmi", "Empty.png" },
                { "akna", "Mine.png" },
                { "aknaNyomva", "MineClicked.png" },
                { "aknaNemNyomott", "MineNotClicked.png" },
                { "1", "1.png" },
                { "2", "2.png" },
                { "3", "3.png" },
                { "4", "4.png" },
                { "5", "5.png" },
                { "6", "6.png" },
                { "7", "7.png" },
                { "8", "8.png" },
            };

            // Use properties so URIs are recalculated after theme or image name changes.
            public static Uri error => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["error"]}", UriKind.Relative);
            public static Uri fedes => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["fedes"]}", UriKind.Relative);
            public static Uri zaszlozott => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["zaszlozott"]}", UriKind.Relative);
            public static Uri kerdojel => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["kerdojel"]}", UriKind.Relative);
            public static Uri semmi => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["semmi"]}", UriKind.Relative);
            public static Uri akna => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["akna"]}", UriKind.Relative);
            public static Uri aknaNyomva => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["aknaNyomva"]}", UriKind.Relative);
            public static Uri aknaNemNyomott => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["aknaNemNyomott"]}", UriKind.Relative);
            public static Uri _1 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["1"]}", UriKind.Relative);
            public static Uri _2 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["2"]}", UriKind.Relative);
            public static Uri _3 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["3"]}", UriKind.Relative);
            public static Uri _4 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["4"]}", UriKind.Relative);
            public static Uri _5 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["5"]}", UriKind.Relative);
            public static Uri _6 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["6"]}", UriKind.Relative);
            public static Uri _7 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["7"]}", UriKind.Relative);
            public static Uri _8 => new Uri($"Assets/Images/GameBoard/{Configuration.CurrentTheme}/{ImageNames["8"]}", UriKind.Relative);
        }
    }
}
