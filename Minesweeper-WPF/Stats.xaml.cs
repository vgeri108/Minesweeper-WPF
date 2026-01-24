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

            Statistics.GenerateStatsIfNotExists();
            JsonManager.Stats.Save();

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

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
