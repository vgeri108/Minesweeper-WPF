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
        public static string[,] coverTexture;

        public static int flagCount = 0;

        public static int LastMeretSZ = 9;
        public static int LastMeretM = 9;
        public static int LastAknakszama = 10;

        public static bool ApplyOnNextGame = false; //ha true akkor az alábbi beállítások fogják felülírni a feljebbi értékeket
        public static int NextMeretM = 9;
        public static int NextMeretSZ = 9;
        public static int NextAknakszama = 10;
        public static string NextDifficulty = "Easy";

        public static void ResizeBoard()
        {
            akna = new string[meretSZ, meretM];
            visible = new string[meretSZ, meretM];
            coverTexture = new string[meretSZ, meretM];
            Statistics.currentMode = $"{meretSZ}_{meretM}_{aknakszama}";
            if (!Statistics.PlayedGames.ContainsKey(Statistics.currentMode))
            {
                Statistics.Modes.Add(Statistics.currentMode);
                Statistics.PlayedGames.Add(Statistics.currentMode, 0);
                Statistics.WinnedGames.Add(Statistics.currentMode, 0);
                Statistics.BestTimes.Add(Statistics.currentMode, 999);
            }
        }
    }
    class Configuration
    {
        public static string CurrentTheme = "Default";

        public static bool Animations = true;
        public static bool Sounds = true;
        public static bool Tips = true;
        public static bool AlwaysContinueSavedGame = false;
        public static bool AlwaysSaveGameOnExit = false;
        public static bool EnableQuestionMarks = true;
        public static bool AutomaticUpdateSearch = true;
    }
    public class Version
    {
        public static bool FirstStart = true;
        public static string Game = "Beta 1.8";
        public static string Json = Game;
        public static string GithubTag = "vB1.8";
    }
    public class Statistics
    {
        public static string currentMode = "9_9_10";
        public static List<string> Modes = new List<string>();
        public static Dictionary<string, int> PlayedGames = new Dictionary<string, int>();
        public static Dictionary<string, int> WinnedGames = new Dictionary<string, int>();
        public static Dictionary<string, List<int>> Times = new Dictionary<string, List<int>>();
        public static Dictionary<string, List<string>> Dates = new Dictionary<string, List<string>>();
        public static Dictionary<string, int> BestTimes = new Dictionary<string, int>();
        public static Dictionary<string, int> WinStreak = new Dictionary<string, int>();
        public static Dictionary<string, int> LongestWinStreak = new Dictionary<string, int>();
        public static Dictionary<string, int> LoseStreak = new Dictionary<string, int>();
        public static Dictionary<string, int> LongestLoseStreak = new Dictionary<string, int>();
        public static Dictionary<string, int> CurrentStreak = new Dictionary<string, int>();
        public static Dictionary<string, bool> IsLastGameWinned = new Dictionary<string, bool>();

        public static void GenerateStatsIfNotExists()
        {
            if (!Modes.Contains(currentMode)) Modes.Add(currentMode);
            if (!PlayedGames.ContainsKey(currentMode)) PlayedGames.Add(currentMode, 0);
            if (!WinnedGames.ContainsKey(currentMode)) WinnedGames.Add(currentMode, 0);
            if (!Times.ContainsKey(currentMode)) Times.Add(currentMode, new List<int>(){-1});
            if (!Dates.ContainsKey(currentMode)) Dates.Add(currentMode, new List<string>(){"Nincs adat."});
            if (!BestTimes.ContainsKey(currentMode)) BestTimes.Add(currentMode, 999);
            if (!IsLastGameWinned.ContainsKey(currentMode)) IsLastGameWinned.Add(currentMode, false);
            if (!WinStreak.ContainsKey(currentMode)) WinStreak.Add(currentMode, 0);
            if (!LongestWinStreak.ContainsKey(currentMode)) LongestWinStreak.Add(currentMode, 0);
            if (!LoseStreak.ContainsKey(currentMode)) LoseStreak.Add(currentMode, 0);
            if (!LongestLoseStreak.ContainsKey(currentMode)) LongestLoseStreak.Add(currentMode, 0);
            if (!CurrentStreak.ContainsKey(currentMode)) CurrentStreak.Add(currentMode, 0);
            JsonManager.Stats.Save();
        }

        public static void SortTimes()
        {
            if (!Times.ContainsKey(currentMode)) return;
            var timesList = Times[currentMode];
            if (timesList == null || timesList.Count <= 1) return;

            if (!Dates.ContainsKey(currentMode) || Dates[currentMode].Count != timesList.Count)
            {
                // Ensure Dates has the same length to avoid indexing errors
                Dates[currentMode] = new List<string>(Enumerable.Repeat("Nincs adat.", timesList.Count));
            }

            int n = timesList.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (timesList[j] > timesList[j + 1])
                    {
                        int tmp = timesList[j];
                        timesList[j] = timesList[j + 1];
                        timesList[j + 1] = tmp;

                        string tmpText = Dates[currentMode][j];
                        Dates[currentMode][j] = Dates[currentMode][j + 1];
                        Dates[currentMode][j + 1] = tmpText;
                    }
                }
            }

            Times[currentMode] = timesList;
            TimeListSizeTo5();
            JsonManager.Stats.Save();
        }

        private static void TimeListSizeTo5()
        {
            if (!Times.ContainsKey(currentMode)) return;
            
            List<int> Times5 = new List<int>();
            List<string> Dates5 = new List<string>();
            int Max = Times[currentMode].Count < 5 ? Times[currentMode].Count : 5;
            for (int i = 0; i < Max; i++)
            {
                Times5.Add(Times[currentMode][i]);
                Dates5.Add(Dates[currentMode][i]);
            }
            Times[currentMode] = Times5;
            Dates[currentMode] = Dates5;
            JsonManager.Stats.Save();
        }
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

        public static void StartTimer(int Elapsed = 0)
        {
            ElapsedSeconds = Elapsed;
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
