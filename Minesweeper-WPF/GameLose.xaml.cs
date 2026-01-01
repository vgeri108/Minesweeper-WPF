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
            Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Close();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            BoardManager.Init();
            Close();

            // update timer text immediately if main window active
            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.UpdateTimerText();
            }
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            BoardManager.replayGame = true;
            BoardManager.Init();
            Close();

            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.UpdateTimerText();
            }
        }
    }
}
