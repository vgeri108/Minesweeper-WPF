using System;
using System.ComponentModel;
using System.Windows;

namespace Minesweeper_WPF {
    public partial class RenameMode {
        public RenameMode(string currentDisplay) {
            InitializeComponent();
            DataContext = this;
            CurrentLabel.Text = currentDisplay;
            InputBox.Text = currentDisplay;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!InputBox.Text.IsWhiteSpace())
            {
                if (!Statistics.DisplayNames.ContainsValue(InputBox.Text))
                {
                    string key = null;
                    foreach (var kvp in Statistics.DisplayNames)
                    {
                        if (kvp.Value == CurrentLabel.Text)
                        {
                            key = kvp.Key;
                            break;
                        }
                    }

                    if (key != null)
                    {
                        Statistics.DisplayNames[key] = InputBox.Text;
                        JsonManager.Stats.Save();
                    }
                }
                else
                {
                    MessageBox.Show("A megadott név már használatban van.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("A név nem lehet üres.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
