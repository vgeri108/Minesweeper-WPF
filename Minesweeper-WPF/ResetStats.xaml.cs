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
    /// Interaction logic for ResetStats.xaml
    /// </summary>
    public partial class ResetStats : Window
    {
        private string selectName;
        private string selectKey;
        public ResetStats(string selectName, string selectKey)
        {
            InitializeComponent();
            this.selectName = selectName;
            this.selectKey = selectKey;
            Selected.Content = "Csak itt: " + selectName;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            Statistics.Modes = new List<string>();
            Statistics.PlayedGames = new Dictionary<string, int>();
            Statistics.WinnedGames = new Dictionary<string, int>();
            Statistics.Times = new Dictionary<string, List<int>>();
            Statistics.Dates = new Dictionary<string, List<string>>();
            Statistics.BestTimes = new Dictionary<string, int>();
            Statistics.WinStreak = new Dictionary<string, int>();
            Statistics.LongestWinStreak = new Dictionary<string, int>();
            Statistics.LoseStreak = new Dictionary<string, int>();
            Statistics.LongestLoseStreak = new Dictionary<string, int>();
            Statistics.CurrentStreak = new Dictionary<string, int>();
            Statistics.IsLastGameWinned = new Dictionary<string, bool>();
            Statistics.GenerateStatsIfNotExists();
            JsonManager.Stats.Save();
            Close();
        }

        private void Selected_Click(object sender, RoutedEventArgs e)
        {
            Statistics.Modes.Remove(selectKey);
            Statistics.PlayedGames.Remove(selectKey);
            Statistics.WinnedGames.Remove(selectKey);
            Statistics.Times.Remove(selectKey);
            Statistics.Dates.Remove(selectKey);
            Statistics.BestTimes.Remove(selectKey);
            Statistics.WinStreak.Remove(selectKey);
            Statistics.LongestWinStreak.Remove(selectKey);
            Statistics.LoseStreak.Remove(selectKey);
            Statistics.LongestLoseStreak.Remove(selectKey);
            Statistics.CurrentStreak.Remove(selectKey);
            Statistics.IsLastGameWinned.Remove(selectKey);
            Statistics.GenerateStatsIfNotExists();
            JsonManager.Stats.Save();
            Close();
        }
    }
}
