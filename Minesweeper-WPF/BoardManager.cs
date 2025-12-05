using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Minesweeper_WPF
{
    class BoardManager
    {
        private static UniformGrid gameBoard;
        public BoardManager(UniformGrid grid)
        {
            gameBoard = grid;
        }
        
        private string[,] board = Data.board;
        private string[,] visible = Data.visible;
        private static int meretM = Data.meretM;
        private static int meretSZ = Data.meretSZ;
        private int aknakszama = Data.aknakszama;
        private string semmi = Appearance.Characters.semmi;
        private string minemark = Appearance.Characters.akna;
        public static void InitFist()
        {
            gameBoard.Children.Clear();
            gameBoard.Rows = meretM;
            gameBoard.Columns = meretSZ;
            for (int x = 0; x < meretSZ; x++)
            {
                for (int y = 0; y < meretM; y++)
                {
                    Button btn = new Button
                    {
                        Tag = new Point(x, y),
                        FontWeight = FontWeights.Bold,
                        FontSize = 15,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                    };
                    btn.Content = new Image
                    {
                        Source = new BitmapImage(Appearance.Images.fedes),
                        Stretch = Stretch.Uniform,
                    };
                    //btn.Click += Cell_Click;
                    //btn.MouseRightButtonUp += Cell_RightClick;

                    gameBoard.Children.Add(btn);
                }
            }
        }
        //void Generate()
        //{
        //    Random random = new Random();
        //    for (int i = 0; i < board.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < board.GetLength(1); j++)
        //        {
        //            board[i, j] = semmi;
        //            visible[i, j] = "false";
        //        }
        //    }
        //    for (int i = 0; i < aknakszama; i++)
        //    {
        //        int x, y;
        //        do
        //        {
        //            x = random.Next(0, meretM);
        //            y = random.Next(0, meretSZ);
        //        } while ((board[x, y] != semmi) || (x == select_y && y == select_x));
        //        board[x, y] = minemark;
        //    }
        //}
        void Draw()
        {
            gameBoard.Children.Clear();
            gameBoard.Rows = meretM;
            gameBoard.Columns = meretSZ;
            for (int x = 0; x < meretSZ; x++)
            {
                for (int y = 0; y < meretM; y++)
                {
                    Button btn = new Button
                    {
                        Tag = new Point(x, y),
                        FontWeight = FontWeights.Bold,
                        FontSize = 15,
                        Margin = new Thickness(1)
                    };

                    Image img = new Image
                    {
                        Source = new BitmapImage(Appearance.Images._3),
                        Stretch = Stretch.Uniform,
                        Width = 24,
                        Height = 24
                    };

                    btn.Content = img;
                    //btn.Click += Cell_Click;
                    //btn.MouseRightButtonUp += Cell_RightClick;

                    gameBoard.Children.Add(btn);
                }
            }
        }
    }
}
