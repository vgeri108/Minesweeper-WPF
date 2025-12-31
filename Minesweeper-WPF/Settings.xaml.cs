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
        public Settings()
        {
            InitializeComponent();

            Animations.IsChecked = Configuration.Animations;
            Sounds.IsChecked = Configuration.Sounds;
            Tips.IsChecked = Configuration.Tips;
            ContinueSaved.IsChecked = Configuration.AlwaysContinueSavedGame;
            SaveExit.IsChecked = Configuration.AlwaysSaveGameOnExit;
            QuestionMarks.IsChecked = Configuration.EnableQuestionMarks;

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
                tbHeight.Text = Data.meretM.ToString();
                tbWidth.Text = Data.meretSZ.ToString();
                tbMines.Text = Data.aknakszama.ToString();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (rbCustom.IsChecked == false)
            {
                if (Easy.IsChecked == true)
                {
                    Data.meretM = 9;
                    Data.meretSZ = 9;
                    Data.aknakszama = 10;
                    Data.Difficulty = "Easy";
                }
                if (Intermediate.IsChecked == true)
                {
                    Data.meretM = 16;
                    Data.meretSZ = 16;
                    Data.aknakszama = 40;
                    Data.Difficulty = "Intermediate";
                }
                if (Advanced.IsChecked == true)
                {
                    Data.meretM = 16;
                    Data.meretSZ = 30;
                    Data.aknakszama = 99;
                    Data.Difficulty = "Advanced";
                }

                Configuration.Animations = (bool)Animations.IsChecked;
                Configuration.Sounds = (bool)Sounds.IsChecked;
                Configuration.Tips = (bool)Tips.IsChecked;
                Configuration.AlwaysContinueSavedGame = (bool)ContinueSaved.IsChecked;
                Configuration.AlwaysSaveGameOnExit = (bool)SaveExit.IsChecked;
                Configuration.EnableQuestionMarks = (bool)QuestionMarks.IsChecked;

                JsonManager.Settings.Save();
            }
            else
            {
                try
                {
                    Data.meretM = int.Parse(tbHeight.Text);
                    Data.meretSZ = int.Parse(tbWidth.Text);
                    Data.aknakszama = int.Parse(tbMines.Text);
                    Data.Difficulty = "Custom";
                } catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Hiba",MessageBoxButton.OK, MessageBoxImage.Error); //le kell tírtani hogy pontot, vesszűt betűt,stb... ne engedjen beírni
                }
            }

            BoardManager.Init();
            Close();
        }
    }
}
