using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for GameWin.xaml
    /// </summary>
    public partial class GameWin : Window
    {
        public GameWin()
        {
            InitializeComponent();
            Statistics.WinnedGames[Statistics.currentMode]++;

            if (Time.ElapsedSeconds < Statistics.BestTimes[Statistics.currentMode]) Statistics.BestTimes[Statistics.currentMode] = Time.ElapsedSeconds;
            int Percent = (int)Math.Round(((Double)Statistics.WinnedGames[Statistics.currentMode] / Statistics.PlayedGames[Statistics.currentMode]) * 100);

            TimeText.Text = $"Idő: {Time.ElapsedSeconds} másodperc";
            BestTime.Text = $"Legjobb idő: {Statistics.BestTimes[Statistics.currentMode]} másodperc";
            Date.Text = DateTime.Now.ToString();
            PlayedGames.Text = $"Lejátszott játékok: {Statistics.PlayedGames[Statistics.currentMode]}";
            WinnedGames.Text = $"Megnyert játékok: {Statistics.WinnedGames[Statistics.currentMode]}";
            PercentageRating.Text = $"Százalékos értékelés: {Percent}%";

            JsonManager.Stats.Save();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            BoardManager.Init();
            Close();

            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.UpdateTimerText();
            }
        }

        private void MoreGames_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/vgeri108",
                UseShellExecute = true
            });
        }
    }
}
