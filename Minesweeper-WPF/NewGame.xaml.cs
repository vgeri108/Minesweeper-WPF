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
    public partial class NewGame : Window
    {
        public NewGame()
        {
            InitializeComponent();
        }

        private void NewBeginner(object sender, RoutedEventArgs e)
        {
            Data.meretM = 9;
            Data.meretSZ = 9;
            Data.aknakszama = 10;
            Data.ResizeBoard();
            BoardManager.InitFirst();
            BoardManager.InitNew();
        }

        private void NewIntermediate(object sender, RoutedEventArgs e)
        {
            Data.meretM = 16;
            Data.meretSZ = 16;
            Data.aknakszama = 40;
            Data.ResizeBoard();
            BoardManager.InitFirst();
            BoardManager.InitNew();
        }

        private void NewAdvanced(object sender, RoutedEventArgs e)
        {
            Data.meretM = 16;
            Data.meretSZ = 30;
            Data.aknakszama = 99;
            Data.ResizeBoard();
            BoardManager.InitFirst();
            BoardManager.InitNew();
        }
    }
}
