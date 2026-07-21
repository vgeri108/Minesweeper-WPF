using Minesweeper_WPF;
using System.Diagnostics;
using System.IO;
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

        public MainWindow()
        {
            InitializeComponent();

            generator = new BoardManager(GameBoard);

            Update.ApplyChanges(JsonManager.Ver.Load());
            JsonManager.Ver.Save();

            JsonManager.Settings.Load();
            JsonManager.Stats.Load();
            JsonManager.Theme.Load();
            JsonManager.Style.Load();

            ApplyTheme();

            BoardManager.Init();

            if (File.Exists("LastSave.mine"))
            {
                if (!Configuration.AlwaysContinueSavedGame)
                {
                    Show();
                    LoadGame_OpenDialog loadGame_OpenDialog = new LoadGame_OpenDialog();
                    loadGame_OpenDialog.Owner = this;
                    loadGame_OpenDialog.ShowDialog();
                }
                else
                {
                    Progress progress = new Progress("Betöltés", "A mentés betöltése folyamatban van...");
                    progress.Show();
                    JsonManager.Game.Load();
                    progress.Close();
                }
            }

            if (Configuration.AutomaticUpdateSearch)
            {
                if (Update.IsNewAvailable())
                {
                    Show();
                    NewInUpdate newInUpdate = new NewInUpdate();
                    newInUpdate.Owner = this;
                    newInUpdate.ShowDialog();
                }
            }
            Show();
            Time.Timer.Tick += DataTimer_Tick;
            Time.Reset += (s, e) => UpdateTimerText();

            UpdateTimerText();

            if (Version.FirstStart)
            {
                FirstStartDifficulty firstGame = new FirstStartDifficulty();
                firstGame.ShowDialog();
                string message =
                    "Ha rákattint egy négyzetre, meglátja, hány virág van körülötte. " +
                    "Ha sikerül úgy felfednie minden négyzetet, hogy közben nem lép virágra, " +
                    "megnyerte a játékot.";
                Tips tips = new Tips("Játékszabályok", message);
                tips.Show();
                tips.Owner = this;
                Version.FirstStart = false;
                JsonManager.Settings.Save();
                Statistics.GenerateStatsIfNotExists();
                JsonManager.Stats.Save();
                JsonManager.Theme.Save();
                JsonManager.Style.Save();
            }
        }

        private void ApplyTheme()
        {
            ClockImage.Source = new BitmapImage(Appearance.Images.Clock);
            FlowerImage.Source = new BitmapImage(Appearance.Images.Flower);
            BackgroundImage.ImageSource = new BitmapImage(Appearance.Images.Hatter);

            MineCounterBox.Background = HexToBrush(Appearance.Images.ImageNames["TextBoxBackgroundColor"]);
            MineCounter.Foreground = HexToBrush(Appearance.Images.ImageNames["TextBoxTextColor"]);
            TimerBox.Background = HexToBrush(Appearance.Images.ImageNames["TextBoxBackgroundColor"]);
            Timer.Foreground = HexToBrush(Appearance.Images.ImageNames["TextBoxTextColor"]);
        }

        public static System.Windows.Media.Brush HexToBrush(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return System.Windows.Media.Brushes.Transparent;

            hex = hex.Trim();

            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            try
            {
                byte a = 255;
                byte r, g, b;

                if (hex.Length == 6) // RRGGBB
                {
                    r = Convert.ToByte(hex.Substring(0, 2), 16);
                    g = Convert.ToByte(hex.Substring(2, 2), 16);
                    b = Convert.ToByte(hex.Substring(4, 2), 16);
                }
                else if (hex.Length == 8) // AARRGGBB
                {
                    a = Convert.ToByte(hex.Substring(0, 2), 16);
                    r = Convert.ToByte(hex.Substring(2, 2), 16);
                    g = Convert.ToByte(hex.Substring(4, 2), 16);
                    b = Convert.ToByte(hex.Substring(6, 2), 16);
                }
                else
                {
                    return System.Windows.Media.Brushes.Transparent;
                }

                var color = System.Windows.Media.Color.FromArgb(a, r, g, b);
                var brush = new System.Windows.Media.SolidColorBrush(color);
                if (brush.CanFreeze) brush.Freeze();
                return brush;
            }
            catch
            {
                return System.Windows.Media.Brushes.Transparent;
            }
        }

        public static System.Windows.Media.Color HexToColor(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return Colors.Transparent;

            hex = hex.Trim().TrimStart('#');

            byte a = 255;
            byte r, g, b;

            if (hex.Length == 6)
            {
                r = Convert.ToByte(hex.Substring(0, 2), 16);
                g = Convert.ToByte(hex.Substring(2, 2), 16);
                b = Convert.ToByte(hex.Substring(4, 2), 16);
            }
            else if (hex.Length == 8)
            {
                a = Convert.ToByte(hex.Substring(0, 2), 16);
                r = Convert.ToByte(hex.Substring(2, 2), 16);
                g = Convert.ToByte(hex.Substring(4, 2), 16);
                b = Convert.ToByte(hex.Substring(6, 2), 16);
            }
            else
            {
                return Colors.Transparent;
            }

            return Color.FromArgb(a, r, g, b);
        }

        public static bool AllCellsAreHidden()
        {
            foreach (string item in Data.visible)
            {
                if (item != "false") return false;
            }
            return true;
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
            MineCounter.Text = count.ToString();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (AllCellsAreHidden())
            {
                Time.ResetTimer();
                UpdateTimerText();
                BoardManager.Init();
            }
            else
            {
                Time.StopTimer();
                NewGame_MenuDialog newGame_MenuDialog = new NewGame_MenuDialog();
                newGame_MenuDialog.Owner = this;
                newGame_MenuDialog.ShowDialog();
                UpdateTimerText();
            }
        }
        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            bool Timer = Time.Timer.IsEnabled;
            Time.StopTimer();
            Stats stats = new Stats();
            stats.Owner = this;
            stats.ShowDialog();
            if (Timer && !AllCellsAreHidden())
            {
                Time.StartTimer(Time.ElapsedSeconds);
            }
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            bool loadedGame = BoardManager.LoadedGame;
            Time.StopTimer();
            BoardManager.LoadedGame = false;
            Settings settings = new Settings();
            settings.Owner = this;
            settings.ShowDialog();
            if ((settings.IsCanceled || settings.ContinueTimer) && !AllCellsAreHidden())
            {
                Time.StartTimer(Time.ElapsedSeconds);
            }
            else
            { 
                Time.ResetTimer();
                UpdateTimerText();
            }
        }
        private void Appearance_Click(object sender, RoutedEventArgs e)
        {
            bool Timer = Time.Timer.IsEnabled;

            Time.StopTimer();
            ThemeSelect themeSelect = new ThemeSelect();
            themeSelect.Owner = this;
            themeSelect.ShowDialog();
            ShowInTaskbar= true;
            ApplyTheme();
            BoardManager.BoardApplyTheme();

            if (Timer && !AllCellsAreHidden())
            {
                Time.StartTimer(Time.ElapsedSeconds);
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            bool Timer = Time.Timer.IsEnabled;
            Time.StopTimer();
            Help help = new Help();
            help.Owner = this;
            help.ShowDialog();
            if (Timer && !AllCellsAreHidden())
            {
                Time.StartTimer(Time.ElapsedSeconds);
            }
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            bool Timer = Time.Timer.IsEnabled;
            Time.StopTimer();
            about About = new about();
            About.Owner = this;
            About.ShowDialog();
            if (Timer && !AllCellsAreHidden())
            {
                Time.StartTimer(Time.ElapsedSeconds);
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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!AllCellsAreHidden() && !BoardManager.gameover)
            {
                if (!Configuration.AlwaysSaveGameOnExit)
                {
                    Time.StopTimer();
                    SaveGame_CloseDialog saveGame_CloseDialog = new SaveGame_CloseDialog();
                    saveGame_CloseDialog.Owner = this;
                    saveGame_CloseDialog.ShowDialog();
                    if (saveGame_CloseDialog.IsCanceled) e.Cancel = true;
                    Time.StartTimer(Time.ElapsedSeconds);
                }
                else
                {
                    JsonManager.Game.Save();
                }
            }
        }
        private void AppSettings_Click(object sender, RoutedEventArgs e)
        {
            bool Timer = Time.Timer.IsEnabled;
            Time.StopTimer();

            AppSettings appSettings = new AppSettings();
            appSettings.Owner = this;
            appSettings.ShowDialog();

            if (Timer && !AllCellsAreHidden())
            {
                Time.StartTimer(Time.ElapsedSeconds);
            }
        }

        private async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress("Frissítések keresése", "A frissítések keresése folyamatban van...");
            progress.Show();

            bool isNewAvailable = false;

            await Task.Run(() =>
            {
                isNewAvailable = Update.IsNewAvailable();
            });

            progress.Close();

            if (isNewAvailable)
            {
                Show();
                NewInUpdate newInUpdate = new NewInUpdate();
                newInUpdate.Owner = this;
                newInUpdate.ShowDialog();
            }
            else
            {
                MessageBox.Show("A legfrissebb verzió van telepítve.", "Frissítés", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}