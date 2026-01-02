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

                //public Dictionary<string, string> UpdateConfig { get; set; } = new();
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
    }
}
