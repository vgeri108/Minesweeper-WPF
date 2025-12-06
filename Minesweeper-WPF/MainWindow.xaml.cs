using Minesweeper_WPF;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoardManager generator;
        public MainWindow()
        {
            InitializeComponent();
            generator = new BoardManager(GameBoard);
            BoardManager.InitFist();
            BoardManager.Generate(0,0);
            BoardManager.Draw();
        }
    }
}