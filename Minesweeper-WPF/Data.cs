using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

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

        public static int LastMeretM = 9;
        public static int LastMeretSZ = 9;
        public static int LastAknakszama = 10;
    }
    public class Version
    {
        public static bool FirstStart = true;
        public static string Game = "Developer 0.0.3";
        public static string Json = Game;
    }
    public class Time
    {
        // Simple elapsed seconds counter for the game timer
        public static int ElapsedSeconds = 0;

        // Shared DispatcherTimer stored here per request
        public static DispatcherTimer Timer;

        // Raised when timer is reset so UI can update immediately
        public static event EventHandler? Reset;

        static Time()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += (s, e) => { ElapsedSeconds++; };
        }

        public static void StartTimer()
        {
            ElapsedSeconds = 0;
            Timer.Start();
            Reset?.Invoke(null, EventArgs.Empty);
        }

        public static void StopTimer()
        {
            Timer.Stop();
        }

        public static void ResetTimer()
        {
            Timer.Stop();
            ElapsedSeconds = 0;
            Reset?.Invoke(null, EventArgs.Empty);
        }
    }
}
