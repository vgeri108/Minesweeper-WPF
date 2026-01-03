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

        public static int LastMeretM = 9;
        public static int LastMeretSZ = 9;
        public static int LastAknakszama = 10;

        public static void ResizeBoard()
        {
            akna = new string[meretM, meretSZ];
            visible = new string[meretM, meretSZ];
            Statistics.currentMode = $"{meretM}_{meretSZ}_{aknakszama}";
            if (!Statistics.PlayedGames.ContainsKey(Statistics.currentMode))
            {
                Statistics.PlayedGames.Add(Statistics.currentMode, 0);
                Statistics.WinnedGames.Add(Statistics.currentMode, 0);
                Statistics.BestTimes.Add(Statistics.currentMode, 999);
            }
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
        public static string Game = "Alpha 0.1";
        public static string Json = Game;
    }
    public class Statistics
    {
        public static string currentMode = "9_9_10";
        public static Dictionary<string, int> PlayedGames = new Dictionary<String, int>();
        public static Dictionary<string, int> WinnedGames = new Dictionary<string, int>();
        public static Dictionary<string, int> BestTimes = new Dictionary<string, int>();
    }
    public class Time
    {
        public static int ElapsedSeconds = 0;
        public static DispatcherTimer Timer;
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
        public static void UpdateTimerText()
        {
            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.UpdateTimerText();
            }
        }
    }
}
