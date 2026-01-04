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
    /// Interaction logic for NewGame_SettingsDialog.xaml
    /// </summary>
    public partial class NewGame_SettingsDialog : Window
    {
        public string Selected { get; private set; } = "Cancel";
        public NewGame_SettingsDialog()
        {
            InitializeComponent();
        }

        private void StartNew_Click(object sender, RoutedEventArgs e)
        {
            Selected = "StartNew";
            Close();
        }

        private void OnNextGame_Click(object sender, RoutedEventArgs e)
        {
            Selected = "OnNextGame";
            Close();
        }
    }
}
