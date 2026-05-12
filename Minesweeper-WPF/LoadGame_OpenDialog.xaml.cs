using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for LoadGame_OpenDialog.xaml
    /// </summary>
    public partial class LoadGame_OpenDialog : Window
    {
        public LoadGame_OpenDialog()
        {
            InitializeComponent();
            AlwaysContinue.IsChecked = Configuration.AlwaysContinueSavedGame;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Progress progress = new Progress("Betöltés", "A mentés betöltése folyamatban van...");
            progress.Show();
            JsonManager.Game.Load();
            progress.Close();
            Close();
        }

        private void DontContinue_Click(object sender, RoutedEventArgs e)
        {

            JsonManager.Game.DeleteSave();
            
            Close();
        }

        private void AlwaysContinue_Checked(object sender, RoutedEventArgs e)
        {
            Configuration.AlwaysContinueSavedGame = true;
            JsonManager.Settings.Save();
        }
        private void AlwaysContinue_Unchecked(object sender, RoutedEventArgs e)
        {
            Configuration.AlwaysContinueSavedGame = false;
            JsonManager.Settings.Save();
        }
    }
}
