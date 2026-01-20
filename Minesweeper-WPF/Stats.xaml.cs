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
        public Stats()
        {
            InitializeComponent();
            Title = $"Aknakereső statisztikája - {Environment.UserName}";
            PlayedGames.Text = "Lejátszott játékok: " + Statistics.PlayedGames[Statistics.currentMode].ToString();
            WinnedGames.Text = "Megnyert játékok: " + Statistics.WinnedGames[Statistics.currentMode].ToString();
            WinPercentage.Text = "Nyerési arány: " + Math.Round(((Double)Statistics.WinnedGames[Statistics.currentMode] / Statistics.PlayedGames[Statistics.currentMode]) * 100).ToString() + "%";
            LongestWinStreak.Text = "Leghosszabb győzelemsorozat: " + Statistics.LongestWinStreak[Statistics.currentMode].ToString();
            LongestLoseStreak.Text = "Leghosszabb vereségsorozat: " + Statistics.LongestLoseStreak[Statistics.currentMode].ToString();
            CurrentStreak.Text = "Jelenlegi sorozat: " + Statistics.CurrentStreak[Statistics.currentMode].ToString();
        }

        private void Difficulties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
