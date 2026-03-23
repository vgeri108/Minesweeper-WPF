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
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public bool IsCanceled { get; private set; } = true;
        public bool NeedNewGame { get; private set; } = false;
        public bool ContinueTimer { get; private set; } = false;
        private int MaxMines;
        public Settings()
        {
            InitializeComponent();

            //Animations.IsChecked = Configuration.Animations;
            Sounds.IsChecked = Configuration.Sounds;
            Tips.IsChecked = Configuration.Tips;
            ContinueSaved.IsChecked = Configuration.AlwaysContinueSavedGame;
            SaveExit.IsChecked = Configuration.AlwaysSaveGameOnExit;
            QuestionMarks.IsChecked = Configuration.EnableQuestionMarks;
            UpdateCheck.IsChecked = Configuration.AutomaticUpdateSearch;
            tbHeight.Text = Data.LastMeretM.ToString();
            tbWidth.Text = Data.LastMeretSZ.ToString();
            tbMines.Text = Data.LastAknakszama.ToString();

            if (Data.Difficulty == "Easy")
            {
                Easy.IsChecked = true;
            }
            else if (Data.Difficulty == "Intermediate")
            {
                Intermediate.IsChecked = true;
            }
            else if (Data.Difficulty == "Advanced")
            {
                Advanced.IsChecked = true;
            }else if (Data.Difficulty == "Custom")
            {
                rbCustom.IsChecked = true;
            }
            CalcMaxMines();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            if (rbCustom.IsChecked == false)
            {
                if (Easy.IsChecked == true)
                {
                    Data.NextMeretM = 9;
                    Data.NextMeretSZ = 9;
                    Data.NextAknakszama = 10;
                    Data.NextDifficulty = "Easy";
                    if (Data.Difficulty != "Easy") NeedNewGame = true;
                }
                if (Intermediate.IsChecked == true)
                {
                    Data.NextMeretM = 16;
                    Data.NextMeretSZ = 16;
                    Data.NextAknakszama = 40;
                    Data.NextDifficulty = "Intermediate";
                    if (Data.Difficulty != "Intermediate") NeedNewGame = true;
                }
                if (Advanced.IsChecked == true)
                {
                    Data.NextMeretM = 16;
                    Data.NextMeretSZ = 30;
                    Data.NextAknakszama = 99;
                    Data.NextDifficulty = "Advanced";
                    if (Data.Difficulty != "Advanced") NeedNewGame = true;
                }

                try
                {
                    Data.LastMeretM = int.Parse(tbHeight.Text);
                    Data.LastMeretSZ = int.Parse(tbWidth.Text);
                    Data.LastAknakszama = int.Parse(tbMines.Text);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    if (Data.Difficulty != "Custom") NeedNewGame = true;
                    Data.NextDifficulty = "Custom";
                    Data.LastMeretM = int.Parse(tbHeight.Text);
                    Data.LastMeretSZ = int.Parse(tbWidth.Text);
                    Data.LastAknakszama = int.Parse(tbMines.Text);

                    if (Data.meretM != Data.LastMeretM || Data.meretSZ != Data.LastMeretSZ || Data.aknakszama != Data.LastAknakszama) NeedNewGame = true;

                    Data.NextMeretM = int.Parse(tbHeight.Text);
                    Data.NextMeretSZ = int.Parse(tbWidth.Text);
                    Data.NextAknakszama = int.Parse(tbMines.Text);
                } catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Hiba",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (BoardManager.replayGame) BoardManager.replayGame = false;

            //Configuration.Animations = (bool)Animations.IsChecked;
            Configuration.Sounds = (bool)Sounds.IsChecked;
            Configuration.Tips = (bool)Tips.IsChecked;
            Configuration.AlwaysContinueSavedGame = (bool)ContinueSaved.IsChecked;
            Configuration.AlwaysSaveGameOnExit = (bool)SaveExit.IsChecked;
            Configuration.EnableQuestionMarks = (bool)QuestionMarks.IsChecked;
            Configuration.AutomaticUpdateSearch = (bool)UpdateCheck.IsChecked;

            if (NeedNewGame && !MainWindow.AllCellsAreHidden())
            {
                NewGame_SettingsDialog newGame_SettingsDialog = new NewGame_SettingsDialog();
                if (!(System.Windows.Application.Current?.MainWindow is MainWindow mw)) return;
                newGame_SettingsDialog.Owner = mw;
                newGame_SettingsDialog.ShowDialog();

                if (newGame_SettingsDialog.Selected == "Cancel")
                {
                    ContinueTimer = true;
                }
                if (newGame_SettingsDialog.Selected == "OnNextGame")
                {
                    Data.ApplyOnNextGame = true;
                    JsonManager.Settings.Save();
                    ContinueTimer = true;
                    Close();
                }
                if (newGame_SettingsDialog.Selected == "StartNew")
                {
                    Data.meretM = Data.NextMeretM;
                    Data.meretSZ = Data.NextMeretSZ;
                    Data.aknakszama = Data.NextAknakszama;
                    Data.Difficulty = Data.NextDifficulty;
                    JsonManager.Settings.Save();
                    BoardManager.Init();
                    Close();
                }
            }
            else
            {
                Data.ApplyOnNextGame = true;
                BoardManager.Init();
                Close();
            }
        }
        private void tbHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            bool Valid = int.TryParse(tbHeight.Text, out value);
            if ((!Valid || tbHeight.Text.Contains(' ')) && tbHeight.Text.Length > 0)
            {
                tbHeight.Text = tbHeight.Text.Remove(tbHeight.Text.Length - 1);
                tbHeight.CaretIndex = tbHeight.Text.Length;
            }
        }

        private void tbWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            bool Valid = int.TryParse(tbWidth.Text, out value);
            if ((!Valid || tbWidth.Text.Contains(' ')) && tbWidth.Text.Length > 0)
            {
                tbWidth.Text = tbWidth.Text.Remove(tbWidth.Text.Length - 1);
                tbWidth.CaretIndex = tbWidth.Text.Length;
            }
        }

        private void tbMines_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            bool Valid = int.TryParse(tbMines.Text, out value);
            if ((!Valid || tbMines.Text.Contains(' ')) && tbMines.Text.Length > 0)
            {
                tbMines.Text = tbMines.Text.Remove(tbHeight.Text.Length - 1);
                tbMines.CaretIndex = tbHeight.Text.Length;
            }
        }
        private void tbHeight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbHeight.Text.Length == 0) tbHeight.Text = "9";
            if (Convert.ToInt32(tbHeight.Text) < 9) tbHeight.Text = "9";
            if (Convert.ToInt32(tbHeight.Text) > 24) tbHeight.Text = "24";
            CalcMaxMines();
        }
        private void tbWidth_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbWidth.Text.Length == 0) tbWidth.Text = "9";
            if (Convert.ToInt32(tbWidth.Text) < 9) tbWidth.Text = "9";
            if (Convert.ToInt32(tbWidth.Text) > 30) tbWidth.Text = "30";
            CalcMaxMines();
        }
        private void tbMines_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbMines.Text.Length == 0) tbMines.Text = "10";
            if (Convert.ToInt32(tbMines.Text) < 9) tbMines.Text = "10";
            if (Convert.ToInt32(tbMines.Text) > MaxMines) tbMines.Text = MaxMines.ToString();
        }
        private void CalcMaxMines()
        {
            MaxMines = (int)Math.Round((Convert.ToInt32(tbHeight.Text) * Convert.ToInt32(tbWidth.Text)) * 0.80);
            MineCountText.Text = $"Aknák (10-{MaxMines})";
        }
    }
}
