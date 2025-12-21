using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper_WPF
{
    class Data
    {
        public static int meretM = 5;
        public static int meretSZ = 10;
        public static int aknakszama = 10;

        public static string[,] akna;
        public static string[,] visible;

        public static int flagCount = 0;
        public static int flagCorrect = 0;

        public static void ResizeBoard()
        {
            akna = new string[meretM, meretSZ];
            visible = new string[meretM, meretSZ];
        }
    }
    class Configuration
    {
        public static string CurrentTheme = "Screenshot";
    }
    class UIinteract
    {
        
    }
}
