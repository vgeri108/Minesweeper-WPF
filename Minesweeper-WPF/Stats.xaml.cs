using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for Stats.xaml
    /// </summary>
    public partial class Stats : Window
    {
        private static string SelectedDifficulty = Statistics.currentMode;
        public Stats()
        {
            InitializeComponent();
            FillList();
        }
        private void FillList()
        {
            Statistics.GenerateStatsIfNotExists();
            JsonManager.Stats.Save();
            Difficulties.Items.Clear();

            foreach (string item in Statistics.Modes)
            {
                string displayName = Statistics.DisplayNames[item];

                Difficulties.Items.Add(displayName);
                if (item == Statistics.currentMode) Difficulties.SelectedItem = displayName;
            }
        }

        private void Difficulties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Difficulties.SelectedItem == null)
                return;

            string displayName = Difficulties.SelectedItem.ToString();

            foreach (string mode in Statistics.Modes)
            {
                if ((mode == "9_9_10" && displayName == "Kezdő") ||
                    (mode == "16_16_40" && displayName == "Középhaladó") ||
                    (mode == "16_30_99" && displayName == "Haladó") ||
                    (mode == displayName))
                {
                    SelectedDifficulty = mode;
                    break;
                }
            }

            Title = $"Aknakereső statisztikája - {Environment.UserName}";
            PlayedGames.Text = "Lejátszott játékok: " + Statistics.PlayedGames[SelectedDifficulty].ToString();
            WinnedGames.Text = "Megnyert játékok: " + Statistics.WinnedGames[SelectedDifficulty].ToString();
            WinPercentage.Text = "Nyerési arány: " + Math.Round(((Double)Statistics.WinnedGames[SelectedDifficulty] / Statistics.PlayedGames[SelectedDifficulty]) * 100).ToString() + "%";
            LongestWinStreak.Text = "Leghosszabb győzelemsorozat: " + Statistics.LongestWinStreak[SelectedDifficulty].ToString();
            LongestLoseStreak.Text = "Leghosszabb vereségsorozat: " + Statistics.LongestLoseStreak[SelectedDifficulty].ToString();
            CurrentStreak.Text = "Jelenlegi sorozat: " + Statistics.CurrentStreak[SelectedDifficulty].ToString();

            Times.Children.Clear();
            Dates.Children.Clear();

            Statistics.SortTimes();

            foreach (int time in Statistics.Times[SelectedDifficulty])
            {
                if (time == -1) return;
                TextBlock TimeText = new TextBlock
                {
                    Text = time.ToString(),
                };
                Times.Children.Add(TimeText);
            }
            foreach (string date in Statistics.Dates[SelectedDifficulty])
            {
                if (date == "Nincs adat.") return;
                TextBlock DateText = new TextBlock
                {
                    Text = date,
                };
                Dates.Children.Add(DateText);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetStats resetStats = new ResetStats(Difficulties.SelectedItem.ToString(), SelectedDifficulty);
            resetStats.Owner = this;
            resetStats.ShowDialog();
            FillList();
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            string displayName = Difficulties.SelectedItem?.ToString() ?? SelectedDifficulty;
            RenameMode renameMode = new RenameMode(displayName);
            renameMode.Owner = this;
            renameMode.ShowDialog();
            FillList();
        }
    }
}
