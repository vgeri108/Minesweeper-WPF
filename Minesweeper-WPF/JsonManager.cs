using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Minesweeper_WPF
{
    internal class JsonManager
    {
        private static string configPath = "config.json";
        private static string statsPath = "stats.json";
        private static string gamesPath = "LastSave.mine";
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        };
        public class Settings
        {
            private class SettingsData
            {
                public string JsonVersion { get; set; } = "WPF"; //Nincs betöltve
                public bool FirstProgramStart { get; set; } = true;
                public int MeretM { get; set; } = 9;
                public int MeretSZ { get; set; } = 9;
                public int Aknakszama { get; set; } = 10;
                public string Difficulty { get; set; } = "Easy";

                public bool Animations { get; set; } = true;
                public bool Sounds { get; set; } = true;
                public bool Tips { get; set; } = true;
                public bool AlwaysContinueSavedGame { get; set; } = false;
                public bool AlwaysSaveGameOnExit { get; set; } = false;
                public bool EnableQuestionMarks { get; set; } = true;

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
                    FirstProgramStart = Version.FirstStart,
                    MeretM = Data.meretM,
                    MeretSZ = Data.meretSZ,
                    Aknakszama = Data.aknakszama,

                    Difficulty = Data.Difficulty,
                    Animations = Configuration.Animations,
                    Sounds = Configuration.Sounds,
                    Tips = Configuration.Tips,
                    AlwaysContinueSavedGame = Configuration.AlwaysContinueSavedGame,
                    AlwaysSaveGameOnExit = Configuration.AlwaysSaveGameOnExit,
                    EnableQuestionMarks = Configuration.EnableQuestionMarks,

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
                Data.meretM = Settings.MeretM;
                Data.meretSZ = Settings.MeretSZ;
                Data.aknakszama = Settings.Aknakszama;
                Data.Difficulty = Settings.Difficulty;

                Configuration.Animations = Settings.Animations;
                Configuration.Sounds = Settings.Sounds;
                Configuration.Tips = Settings.Tips;
                Configuration.AlwaysContinueSavedGame = Settings.AlwaysContinueSavedGame;
                Configuration.AlwaysSaveGameOnExit = Settings.AlwaysSaveGameOnExit;
                Configuration.EnableQuestionMarks = Settings.EnableQuestionMarks;

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
                public Dictionary<string, string> PlayedGames { get; set; } = new();
                public Dictionary<string, string> WinnedGames { get; set; } = new();
                public Dictionary<string, string> BestTimes { get; set; } = new();
            }
            public static void Save()
            {
                var Stats = new StatsData
                {
                    JsonVersion = Version.Json,
                    PlayedGames = Statistics.PlayedGames.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()),
                    WinnedGames = Statistics.WinnedGames.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()),
                    BestTimes = Statistics.BestTimes.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()),
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

                foreach (var kv in Stats.PlayedGames)
                {
                    Statistics.PlayedGames[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.WinnedGames)
                {
                    Statistics.WinnedGames[kv.Key] = Convert.ToInt32(kv.Value);
                }
                foreach (var kv in Stats.BestTimes)
                {
                    Statistics.BestTimes[kv.Key] = Convert.ToInt32(kv.Value);
                }
            }
        }
        public class Game
        {
            private class GamesData
            {
                public string JsonVersion { get; set; } = "WPF"; //Nincs betöltve
                public int meretM { get; set; } = 9;
                public int meretSZ { get; set; } = 9;
                public int aknakszama { get; set; } = 10;
                public string Difficulty { get; set; } = "Easy";
                public int ElapsedSeconds { get; set; } = 1;
                public Dictionary<string, string> Akna { get; set; } = new();
                public Dictionary<string, string> Visible { get; set; } = new();
            }
            public static void Save()
            {
                var Games = new GamesData
                {
                    JsonVersion = Version.Json,
                    meretM = Data.meretM,
                    meretSZ = Data.meretSZ,
                    aknakszama = Data.aknakszama,
                    Difficulty = Data.Difficulty,
                    ElapsedSeconds = Time.ElapsedSeconds
                };

                for (int x = 0; x < Data.akna.GetLength(0); x++)
                {
                    for (int y = 0; y < Data.akna.GetLength(1); y++)
                    {
                        Games.Akna[$"{y},{x}"] = Data.akna[y, x];
                        Games.Visible[$"{y},{x}"] = Data.visible[y, x];
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

                for (int x = 0; x < Games.meretM; x++)
                {
                    for (int y = 0; y < Games.meretSZ; y++)
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
    }
}
