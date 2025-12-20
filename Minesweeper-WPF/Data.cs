using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper_WPF
{
    class Data
    {
        public static int meretM = 9;
        public static int meretSZ = 9;
        public static int aknakszama = 10;
        public static string[,] akna = new string[meretM, meretSZ];
        public static string[,] visible = new string[meretM, meretSZ];
        
        public static int flagCount = 0;
        public static int flagCorrect = 0;
    }
    class Configuration
    {
        public static string CurrentTheme = "Screenshot";
    }
    class UIinteract
    {
        
    }
}
