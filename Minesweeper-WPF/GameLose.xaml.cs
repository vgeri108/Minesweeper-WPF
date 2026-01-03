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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for GameLose.xaml
    /// </summary>
    public partial class GameLose : Window
    {
        public GameLose()
        {
            InitializeComponent();

            int Percent = (int)Math.Round(((Double)Statistics.WinnedGames[Statistics.currentMode] / Statistics.PlayedGames[Statistics.currentMode]) * 100);

            TimeText.Text = $"Idő: {Time.ElapsedSeconds} másodperc";
            BestTime.Text = $"Legjobb idő: {Statistics.BestTimes[Statistics.currentMode]} másodperc";
            Date.Text = "Dátum: " + DateTime.Now.ToString("yyyy/MM/dd");
            PlayedGames.Text = $"Lejátszott játékok: {Statistics.PlayedGames[Statistics.currentMode]}";
            WinnedGames.Text = $"Megnyert játékok: {Statistics.WinnedGames[Statistics.currentMode]}";
            PercentageRating.Text = $"Százalékos értékelés: {Percent}%";

            JsonManager.Stats.Save();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.ToString(),
                UseShellExecute = true
            });
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            BoardManager.Init();
            Close();
            Time.UpdateTimerText();
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            BoardManager.replayGame = true;
            BoardManager.Init();
            Close();
        }
    }
}
