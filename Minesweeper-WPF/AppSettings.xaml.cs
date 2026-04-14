using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for AppSettings.xaml
    /// </summary>
    public partial class AppSettings : Window
    {
        public AppSettings()
        {
            InitializeComponent();

            UpdateCheck.IsChecked = Configuration.AutomaticUpdateSearch;
            ContinueSaved.IsChecked = Configuration.AlwaysContinueSavedGame;
            SaveExit.IsChecked = Configuration.AlwaysSaveGameOnExit;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Configuration.AutomaticUpdateSearch = (bool)UpdateCheck.IsChecked;
            Configuration.AlwaysContinueSavedGame = (bool)ContinueSaved.IsChecked;
            Configuration.AlwaysSaveGameOnExit = (bool)SaveExit.IsChecked;
            JsonManager.Settings.Save();
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void CheckUpdates_Click(object sender, RoutedEventArgs e)
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
