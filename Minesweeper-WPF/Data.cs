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

        public static string Difficulty = "Easy";

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

        public static bool Animations = true;
        public static bool Sounds = true;
        public static bool Tips = true;
        public static bool AlwaysContinueSavedGame = false;
        public static bool AlwaysSaveGameOnExit = false;
        public static bool EnableQuestionMarks = true;
    }
    public class Version
    {
        public static bool FirstStart = true;
        public static string Game = "Developer 0.0.3";
        public static string Json = Game;
    }
}
