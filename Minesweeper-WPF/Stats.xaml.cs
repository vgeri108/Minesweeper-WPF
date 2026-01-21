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
        private static string SelectedDifficulty = "9_9_10";
        public Stats()
        {
            InitializeComponent();

            Statistics.GenerateStatsIfNotExists();

            foreach (string item in Statistics.Modes)
            {
                Difficulties.Items.Add(item);
            }

            Title = $"Aknakereső statisztikája - {Environment.UserName}";
            PlayedGames.Text = "Lejátszott játékok: " + Statistics.PlayedGames[SelectedDifficulty].ToString();
            WinnedGames.Text = "Megnyert játékok: " + Statistics.WinnedGames[SelectedDifficulty].ToString();
            WinPercentage.Text = "Nyerési arány: " + Math.Round(((Double)Statistics.WinnedGames[SelectedDifficulty] / Statistics.PlayedGames[SelectedDifficulty]) * 100).ToString() + "%";
            LongestWinStreak.Text = "Leghosszabb győzelemsorozat: " + Statistics.LongestWinStreak[SelectedDifficulty].ToString();
            LongestLoseStreak.Text = "Leghosszabb vereségsorozat: " + Statistics.LongestLoseStreak[SelectedDifficulty].ToString();
            CurrentStreak.Text = "Jelenlegi sorozat: " + Statistics.CurrentStreak[SelectedDifficulty].ToString();
        }

        private void Difficulties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Difficulties.SelectedItem == null)
                return;

            SelectedDifficulty = Difficulties.SelectedItem.ToString();
        }
    }
}
