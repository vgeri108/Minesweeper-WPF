using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
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
    /// Interaction logic for FirstStartDifficulty.xaml
    /// </summary>
    
    public partial class FirstStartDifficulty : Window
    {
        public FirstStartDifficulty()
        {
            InitializeComponent();
        }

        private void NewBeginner(object sender, RoutedEventArgs e)
        {
            Data.Difficulty = "Easy";
            Data.meretM = 9;
            Data.meretSZ = 9;
            Data.aknakszama = 10;
            BoardManager.Init();
            Close();
        }

        private void NewIntermediate(object sender, RoutedEventArgs e)
        {
            Data.Difficulty = "Intermediate";
            Data.meretM = 16;
            Data.meretSZ = 16;
            Data.aknakszama = 40;
            BoardManager.Init();
            Close();
        }

        private void NewAdvanced(object sender, RoutedEventArgs e)
        {
            Data.Difficulty = "Advanced";
            Data.meretM = 16;
            Data.meretSZ = 30;
            Data.aknakszama = 99;
            BoardManager.Init();
            Close();
        }
    }
}
