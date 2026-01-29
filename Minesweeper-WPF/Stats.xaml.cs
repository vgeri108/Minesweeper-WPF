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
                if (item == "9_9_10")
                {
                    Difficulties.Items.Add("Kezdő");
                    if (item == Statistics.currentMode) Difficulties.SelectedItem = "Kezdő";
                }
                else if (item == "16_16_40")
                {
                    Difficulties.Items.Add("Középhaladó");
                    if (item == Statistics.currentMode) Difficulties.SelectedItem = "Középhaladó";
                }
                else if (item == "16_30_99")
                {
                    Difficulties.Items.Add("Haladó");
                    if (item == Statistics.currentMode) Difficulties.SelectedItem = "Haladó";
                }
                else
                {
                    Difficulties.Items.Add(item);
                    if (item == Statistics.currentMode) Difficulties.SelectedItem = item;
                }
            }
        }

        private void Difficulties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Difficulties.SelectedItem == null)
                return;

            if (Difficulties.SelectedItem.ToString() == "Kezdő")
            {
                SelectedDifficulty = "9_9_10";
            }
            else if (Difficulties.SelectedItem.ToString() == "Középhaladó")
            {
                SelectedDifficulty = "16_16_40";
            }
            else if (Difficulties.SelectedItem.ToString() == "Haladó")
            {
                SelectedDifficulty = "16_30_99";
            }
            else SelectedDifficulty = Difficulties.SelectedItem.ToString();

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
            ShowInTaskbar = false;
            ResetStats resetStats = new ResetStats(Difficulties.SelectedItem.ToString(), SelectedDifficulty);
            resetStats.Owner = this;
            resetStats.ShowDialog();
            ShowInTaskbar = true;
            FillList();
        }
    }
}
