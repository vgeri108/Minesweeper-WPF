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
        private static string Theme = Configuration.CurrentTheme;
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
            public static Uri error = new Uri($"Assets/Images/GameBoard/{Theme}/Covered.png", UriKind.Relative);
            public static Uri fedes = new Uri($"Assets/Images/GameBoard/{Theme}/Covered.png", UriKind.Relative);
            public static Uri zaszlozott = new Uri($"Assets/Images/GameBoard/{Theme}/Flagged.png", UriKind.Relative);
            public static Uri kerdojel = new Uri($"Assets/Images/GameBoard/{Theme}/Question.png", UriKind.Relative);
            public static Uri semmi = new Uri($"Assets/Images/GameBoard/{Theme}/Empty.png", UriKind.Relative);
            public static Uri akna = new Uri($"Assets/Images/GameBoard/{Theme}/Mine.png", UriKind.Relative);
            public static Uri aknaNyomva = new Uri($"Assets/Images/GameBoard/{Theme}/MineClicked.png", UriKind.Relative);
            public static Uri aknaNemNyomott = new Uri($"Assets/Images/GameBoard/{Theme}/MineNotClicked.png", UriKind.Relative);
            public static Uri _1 = new Uri($"Assets/Images/GameBoard/{Theme}/1.png", UriKind.Relative);
            public static Uri _2 = new Uri($"Assets/Images/GameBoard/{Theme}/2.png", UriKind.Relative);
            public static Uri _3 = new Uri($"Assets/Images/GameBoard/{Theme}/3.png", UriKind.Relative);
            public static Uri _4 = new Uri($"Assets/Images/GameBoard/{Theme}/4.png", UriKind.Relative);
            public static Uri _5 = new Uri($"Assets/Images/GameBoard/{Theme}/5.png", UriKind.Relative);
            public static Uri _6 = new Uri($"Assets/Images/GameBoard/{Theme}/6.png", UriKind.Relative);
            public static Uri _7 = new Uri($"Assets/Images/GameBoard/{Theme}/7.png", UriKind.Relative);
            public static Uri _8 = new Uri($"Assets/Images/GameBoard/{Theme}/8.png", UriKind.Relative);
        }
    }
}
