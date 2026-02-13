using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Windows.Media;
using static Minesweeper_WPF.Appearance;
using static Minesweeper_WPF.JsonManager;

namespace Minesweeper_WPF
{
    internal class JsonManager
    {
        private static string configPath = "Config.json";
        private static string statsPath = "Stats.json";
        private static string gamesPath = "LastSave.mine";
        private static string stylePath = "Styles.json";
        private static string themesPath = "ThemeConfig.json";
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        public class Settings
        {
            private class SettingsData
            {
                public string JsonVersion { get; set; } = "WPF"; //Nincs betöltve
                public string SerializationTime { get; set; } = DateTime.Now.ToString();
                public bool FirstProgramStart { get; set; } = true;
                public string Theme { get; set; }

                public int MeretM { get; set; } = 9;
                public int MeretSZ { get; set; } = 9;
                public int Aknakszama { get; set; } = 10;
                public string Difficulty { get; set; } = "Easy";
                public string LastMode { get; set; } = "9_9_10";

                public bool Animations { get; set; } = true;
                public bool Sounds { get; set; } = true;
                public bool Tips { get; set; } = true;
                public bool AlwaysContinueSavedGame { get; set; } = false;
                public bool AlwaysSaveGameOnExit { get; set; } = false;
                public bool EnableQuestionMarks { get; set; } = true;
                public bool AutomaticUpdateSearch { get; set; } = true;

                public int CustomM { get; set; } = 9;
                public int CustomSZ { get; set; } = 9;
                public int CustomAknakszama { get; set; } = 10;

                public bool ApplyOnNextGame { get; set; } = false;
                public int NextMeretM { get; set; } = 9;
                public int NextMeretSZ { get; set; } = 9;
                public int NextAknakszama { get; set; } = 10;
                public string NextDifficulty { get; set; } = "Easy";
            }
            public static void Save()
            {
                var Settings = new SettingsData
                {
                    JsonVersion = Version.Json,
                    SerializationTime = DateTime.Now.ToString(),
                    FirstProgramStart = Version.FirstStart,
                    Theme = Configuration.CurrentTheme,

                    MeretM = Data.meretM,
                    MeretSZ = Data.meretSZ,
                    Aknakszama = Data.aknakszama,
                    Difficulty = Data.Difficulty,
                    LastMode = Statistics.currentMode,

                    Animations = Configuration.Animations,
                    Sounds = Configuration.Sounds,
                    Tips = Configuration.Tips,
                    AlwaysContinueSavedGame = Configuration.AlwaysContinueSavedGame,
                    AlwaysSaveGameOnExit = Configuration.AlwaysSaveGameOnExit,
                    EnableQuestionMarks = Configuration.EnableQuestionMarks,
                    AutomaticUpdateSearch = Configuration.AutomaticUpdateSearch,

                    CustomM = Data.LastMeretM,
                    CustomSZ = Data.LastMeretSZ,
                    CustomAknakszama = Data.LastAknakszama,

                    ApplyOnNextGame = Data.ApplyOnNextGame,
                    NextMeretM = Data.NextMeretM,
                    NextMeretSZ = Data.NextMeretSZ,
                    NextAknakszama = Data.NextAknakszama,
                    NextDifficulty = Data.NextDifficulty,
                };

                string json = JsonSerializer.Serialize(Settings, jsonOptions);
                File.WriteAllText(configPath, json);
            }
            public static void Load()
            {
                if (!File.Exists(configPath))
                    return;

                string json = File.ReadAllText(configPath);
                var Settings = JsonSerializer.Deserialize<SettingsData>(json, jsonOptions);

                if (Settings == null)
                    return;

                Version.FirstStart = Settings.FirstProgramStart;
                Configuration.CurrentTheme = Settings.Theme;

                Data.meretM = Settings.MeretM;
                Data.meretSZ = Settings.MeretSZ;
                Data.aknakszama = Settings.Aknakszama;
                Data.Difficulty = Settings.Difficulty;
                Statistics.currentMode = Settings.LastMode;

                Configuration.Animations = Settings.Animations;
                Configuration.Sounds = Settings.Sounds;
                Configuration.Tips = Settings.Tips;
                Configuration.AlwaysContinueSavedGame = Settings.AlwaysContinueSavedGame;
                Configuration.AlwaysSaveGameOnExit = Settings.AlwaysSaveGameOnExit;
                Configuration.EnableQuestionMarks = Settings.EnableQuestionMarks;
                Configuration.AutomaticUpdateSearch = Settings.AutomaticUpdateSearch;

                Data.LastMeretM = Settings.CustomM;
                Data.LastMeretSZ = Settings.CustomSZ;
                Data.LastAknakszama = Settings.CustomAknakszama;

                Data.ApplyOnNextGame = Settings.ApplyOnNextGame;
                Data.NextMeretM = Settings.NextMeretM;
                Data.NextMeretSZ = Settings.NextMeretSZ;
                Data.NextAknakszama = Settings.NextAknakszama;
                Data.NextDifficulty = Settings.NextDifficulty;
            }
        }
        public class Stats
        {
            private class StatsData
            {
                public string JsonVersion { get; set; } = "WPF"; //Nincs betöltve
                public string SerializationTime { get; set; } = DateTime.Now.ToString();
                public List<string> Modes { get; set; } = Statistics.Modes;
                public Dictionary<string, int> PlayedGames { get; set; } = new();
                public Dictionary<string, int> WinnedGames { get; set; } = new();
                public Dictionary<string, List<string>> Times { get; set; } = new();
                public Dictionary<string, List<string>> Dates { get; set; } = new();
                public Dictionary<string, int> BestTimes { get; set; } = new();
                public Dictionary<string, int> WinStreak { get; set; } = new();
                public Dictionary<string, int> LongestWinStreak { get; set; } = new();
                public Dictionary<string, int> LoseStreak { get; set; } = new();
                public Dictionary<string, int> LongestLoseStreak { get; set; } = new();
                public Dictionary<string, int> CurrentStreak { get; set; } = new();
                public Dictionary<string, bool> IsLastGameWinned { get; set; } = new();
            }
            public static void Save()
            {
                var Stats = new StatsData
                {
                    JsonVersion = Version.Json,
                    SerializationTime = DateTime.Now.ToString(),
                    Modes = Statistics.Modes,
                    PlayedGames = Statistics.PlayedGames.ToDictionary(kv => kv.Key, kv => kv.Value),
                    WinnedGames = Statistics.WinnedGames.ToDictionary(kv => kv.Key, kv => kv.Value),
                    Times = Statistics.Times.ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Select(i => i.ToString()).ToList()
                    ),
                    Dates = Statistics.Dates.ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Select(i => i.ToString()).ToList()
                    ),
                    BestTimes = Statistics.BestTimes.ToDictionary(kv => kv.Key, kv => kv.Value),
                    WinStreak = Statistics.WinStreak.ToDictionary(kv => kv.Key, kv => kv.Value),
                    LongestWinStreak = Statistics.LongestWinStreak.ToDictionary(kv => kv.Key, kv => kv.Value),
                    LoseStreak = Statistics.LoseStreak.ToDictionary(kv => kv.Key, kv => kv.Value),
                    LongestLoseStreak = Statistics.LongestLoseStreak.ToDictionary(kv => kv.Key, kv => kv.Value),
                    CurrentStreak = Statistics.CurrentStreak.ToDictionary(kv => kv.Key, kv => kv.Value),
                    IsLastGameWinned = Statistics.IsLastGameWinned.ToDictionary(kv => kv.Key, kv => kv.Value),
                };

                string json = JsonSerializer.Serialize(Stats, jsonOptions);
                File.WriteAllText(statsPath, json);
            }
            public static void Load()
            {
                if (!File.Exists(statsPath))
                    return;

                string json = File.ReadAllText(statsPath);
                var Stats = JsonSerializer.Deserialize<StatsData>(json, jsonOptions);

                if (Stats == null)
                    return;

                Statistics.Modes = Stats.Modes;

                foreach (var kv in Stats.PlayedGames)
                {
                    Statistics.PlayedGames[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.WinnedGames)
                {
                    Statistics.WinnedGames[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.Times)
                {
                    if (kv.Value == null)
                        Statistics.Times[kv.Key] = new List<int>();
                    else
                        Statistics.Times[kv.Key] = kv.Value.Select(s =>
                        {
                            int v;
                            return int.TryParse(s, out v) ? v : 0;
                        }).ToList();
                }
                foreach (var kv in Stats.Dates)
                {
                    Statistics.Dates[kv.Key] = kv.Value != null ? kv.Value.ToList() : new List<string>();
                }
                foreach (var kv in Stats.BestTimes)
                {
                    Statistics.BestTimes[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.WinStreak)
                {
                    Statistics.WinStreak[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.LongestWinStreak)
                {
                    Statistics.LongestWinStreak[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.LoseStreak)
                {
                    Statistics.LoseStreak[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.LongestLoseStreak)
                {
                    Statistics.LongestLoseStreak[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.CurrentStreak)
                {
                    Statistics.CurrentStreak[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.IsLastGameWinned)
                {
                    Statistics.IsLastGameWinned[kv.Key] = Convert.ToBoolean(kv.Value);
                }
            }
        }
        public class Game
        {
            private class GamesData
            {
                public string JsonVersion { get; set; } = "WPF"; //Nincs betöltve
                public string SerializationTime { get; set; } = DateTime.Now.ToString();
                public int meretM { get; set; } = 9;
                public int meretSZ { get; set; } = 9;
                public int aknakszama { get; set; } = 10;
                public string Difficulty { get; set; } = "Easy";
                public int ElapsedSeconds { get; set; } = 1;
                public Dictionary<string, string> Akna { get; set; } = new();
                public Dictionary<string, string> Visible { get; set; } = new();
                public Dictionary<string, string> CoverTexture { get; set; } = new();
                public string CoverTheme { get; set; }
            }
            public static void Save()
            {
                var Games = new GamesData
                {
                    JsonVersion = Version.Json,
                    SerializationTime = DateTime.Now.ToString(),
                    meretM = Data.meretM,
                    meretSZ = Data.meretSZ,
                    aknakszama = Data.aknakszama,
                    Difficulty = Data.Difficulty,
                    ElapsedSeconds = Time.ElapsedSeconds,
                    CoverTheme = Configuration.CurrentTheme,
                };

                for (int x = 0; x < Data.akna.GetLength(0); x++)
                {
                    for (int y = 0; y < Data.akna.GetLength(1); y++)
                    {
                        Games.Akna[$"{x},{y}"] = Data.akna[x, y];
                        Games.Visible[$"{x},{y}"] = Data.visible[x, y];
                        Games.CoverTexture[$"{x},{y}"] = Data.coverTexture[x, y].ToString();
                    }
                }

                string json = JsonSerializer.Serialize(Games, jsonOptions);
                File.WriteAllText(gamesPath, json);
            }
            public static void Load()
            {
                if (!File.Exists(gamesPath))
                    return;

                string json = File.ReadAllText(gamesPath);
                var Games = JsonSerializer.Deserialize<GamesData>(json, jsonOptions);

                if (Games == null)
                    return;

                Data.meretM = Games.meretM;
                Data.meretSZ = Games.meretSZ;
                Data.aknakszama = Games.aknakszama;
                Data.Difficulty = Games.Difficulty;
                Time.ElapsedSeconds = Games.ElapsedSeconds;

                BoardManager.LoadedGame = true;
                Data.ResizeBoard();

                foreach (var kv in Games.Akna)
                {
                    var p = kv.Key.Split(',');
                    int x = int.Parse(p[0]);
                    int y = int.Parse(p[1]);
                    Data.akna[x, y] = kv.Value;
                }

                foreach (var kv in Games.Visible)
                {
                    var p = kv.Key.Split(',');
                    int x = int.Parse(p[0]);
                    int y = int.Parse(p[1]);
                    Data.visible[x, y] = kv.Value;
                }

                if (Games.CoverTheme == Configuration.CurrentTheme)
                {
                    foreach (var kv in Games.CoverTexture)
                    {
                        var p = kv.Key.Split(',');
                        int x = int.Parse(p[0]);
                        int y = int.Parse(p[1]);
                        Data.coverTexture[x, y] = kv.Value;
                    }
                }
                else
                {
                    for (int x = 0; x < Data.coverTexture.GetLength(0); x++)
                    {
                        for (int y = 0; y < Data.coverTexture.GetLength(1); y++)
                        {
                            Data.coverTexture[x, y] = Appearance.Images.ImageNames["fedes"];
                        }
                    }
                }

                    for (int x = 0; x < Games.meretSZ; x++)
                    {
                        for (int y = 0; y < Games.meretM; y++)
                        {
                            if (Data.akna[x, y] == null)
                                Data.akna[x, y] = Appearance.Characters.semmi;

                            if (Data.visible[x, y] == null)
                                Data.visible[x, y] = "false";
                        }
                    }

                BoardManager.Init();
            }
        }
        public class Style
        {
            private class StyleData
            {
                public string JsonVersion { get; set; } = "WPF"; //Nincs betöltve
                public string SerializationTime { get; set; } = DateTime.Now.ToString();
                public Dictionary<string, string> ImageNames { get; set; } = new();
                public List<string> CoverTextures{ get; set; } = new();
            }
            public static void Save()
            {
                var styles = new StyleData
                {
                    JsonVersion = Version.Json,
                    SerializationTime = DateTime.Now.ToString(),
                    ImageNames = Appearance.Images.ImageNames.ToDictionary(kv => kv.Key, kv => kv.Value),
                    CoverTextures = Appearance.Images.CoverTextureList
                };

                string json = JsonSerializer.Serialize(styles, jsonOptions);
                File.WriteAllText(stylePath, json, System.Text.Encoding.UTF8);
            }

            public static void Load()
            {
                if (!File.Exists(stylePath))
                    return;

                string json = File.ReadAllText(stylePath, Encoding.UTF8);

                var styles = JsonSerializer.Deserialize<StyleData>(json, jsonOptions);
                if (styles == null)
                    return;

                foreach (var kv in styles.ImageNames)
                {
                    Appearance.Images.ImageNames[kv.Key] = kv.Value;
                }
                Appearance.Images.CoverTextureList = styles.CoverTextures;
            }
        }
        public class Theme
        {
            private class ThemeData
            {
                public string JsonVersion { get; set; } = "WPF"; //Nincs betöltve
                public string SerializationTime { get; set; } = DateTime.Now.ToString();
                public Dictionary<string, string> ImageNames { get; set; } = new();
                public List<string> CoverTextures { get; set; } = new();
            }
            public static void Save()
            {
                var Themes = new ThemeData
                {
                    JsonVersion = Version.Json,
                    SerializationTime = DateTime.Now.ToString(),

                    ImageNames = Appearance.Images.ImageNames.ToDictionary(kv => kv.Key, kv => kv.Value),
                    CoverTextures = Appearance.Images.CoverTextureList
                };

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string dir = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme);
                Directory.CreateDirectory(dir);
                string filePath = Path.Combine(dir, themesPath);
                File.WriteAllText(filePath, JsonSerializer.Serialize(Themes, jsonOptions), System.Text.Encoding.UTF8);
            }
            public static void Load()
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme, themesPath);
                if (!File.Exists(path))
                    return;

                string json = File.ReadAllText(path, System.Text.Encoding.UTF8);
                var Themes = JsonSerializer.Deserialize<ThemeData>(json, jsonOptions);

                if (Themes == null)
                    return;

                foreach (var kv in Themes.ImageNames)
                {
                    if (Appearance.Images.ImageNames.ContainsKey(kv.Key))
                        Appearance.Images.ImageNames[kv.Key] = kv.Value;
                    else
                        Appearance.Images.ImageNames.Add(kv.Key, kv.Value);
                }
                Appearance.Images.CoverTextureList = Themes.CoverTextures;
            }
        }
    }
}
