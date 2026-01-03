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
    /// Interaction logic for NewGame_MenuDialog.xaml
    /// </summary>
    public partial class NewGame_MenuDialog : Window
    {
        public NewGame_MenuDialog()
        {
            InitializeComponent();
        }

        private void StartNew_Click(object sender, RoutedEventArgs e)
        {
            Time.ResetTimer();
            Time.UpdateTimerText();
            BoardManager.Init();
            Close();
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            BoardManager.replayGame = true;
            BoardManager.Init();
            Close();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
