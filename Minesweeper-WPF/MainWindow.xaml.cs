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
            Data.ResizeBoard();
            BoardManager.InitFirst();
            BoardManager.InitNew();
            NewGame newGame = new NewGame();
            newGame.Show();

            //BoardManager.Generate(1,1);
            //BoardManager.Draw();
        }
        
        public void MineCounterUpdate(int count)
        {
            MineCounter.Text =count.ToString();
        }
        
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            BoardManager.InitFirst();
            BoardManager.InitNew();
            //BoardManager.Generate(0, 0);
            //BoardManager.Draw();
        }
        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            NewGame newGame = new NewGame();
            newGame.Show();
        }
        private void Appearance_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void MoreGames_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}