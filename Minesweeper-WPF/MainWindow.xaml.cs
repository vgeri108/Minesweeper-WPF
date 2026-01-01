using Minesweeper_WPF;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoardManager generator;
        private BoardManager timerEditor;

        public MainWindow()
        {
            InitializeComponent();
            generator = new BoardManager(GameBoard);


            JsonManager.Settings.Load();

            BoardManager.Init();

            if (Version.FirstStart)
            {
                FirstStartDifficulty firstGame = new FirstStartDifficulty();
                firstGame.ShowDialog();
                Version.FirstStart = false;
                JsonManager.Settings.Save();
            }

            // subscribe to Time.Timer to update UI each second
            Time.Timer.Tick += DataTimer_Tick;
            // subscribe to Reset so UI updates immediately when timer is reset
            Time.Reset += (s, e) => UpdateTimerText();

            UpdateTimerText();
        }

        public void DataTimer_Tick(object? sender, System.EventArgs e)
        {
            UpdateTimerText();
        }

        public void UpdateTimerText()
        {
            Timer.Text = Time.ElapsedSeconds.ToString();
        }

        public void MineCounterUpdate(int count)
        {
            MineCounter.Text =count.ToString();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Time.ResetTimer();
            UpdateTimerText();
            BoardManager.Init();
        }
        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false;
            Settings settings = new Settings();
            settings.ShowDialog();
            Time.ResetTimer();
            UpdateTimerText();
            Timer.Text = "0";
            ShowInTaskbar = true;
        }
        private void Appearance_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false;
            about About = new about();
            About.ShowDialog();
            ShowInTaskbar = true;
        }
        private void MoreGames_Click(object sender, RoutedEventArgs e)
        {
            //DEBUG MIATT VAN ITT

            GameLose gameLose = new GameLose();
            gameLose.ShowDialog();
        }
    }
}