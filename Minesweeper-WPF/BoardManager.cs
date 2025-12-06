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
        
        private static string[,] akna = Data.akna;
        private static string[,] visible = Data.visible;
        private static int meretM = Data.meretM;
        private static int meretSZ = Data.meretSZ;
        private static int aknakszama = Data.aknakszama;
        private static string semmi = Appearance.Characters.semmi;
        private static string minemark = Appearance.Characters.akna;
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
                        Tag = new Point(x,y),
                        FontWeight = FontWeights.Bold,
                        FontSize = 15,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                        MinWidth = 24,
                        MinHeight = 24,
                    };
                    btn.Content = new Image
                    {
                        Source = new BitmapImage(Appearance.Images.fedes),
                        Stretch = Stretch.UniformToFill,
                    };
                    //btn.Click += Cell_Click;
                    //btn.MouseRightButtonUp += Cell_RightClick;

                    gameBoard.Children.Add(btn);
                }
            }
        }
        public static string[,] Generate(int select_x, int select_y)
        {
            Random random = new Random();
            for (int i = 0; i < akna.GetLength(0); i++)
            {
                for (int j = 0; j < akna.GetLength(1); j++)
                {
                    akna[i, j] = semmi;
                    visible[i, j] = "false";
                }
            }
            for (int i = 0; i < aknakszama; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(0, meretM);
                    y = random.Next(0, meretSZ);
                } while ((akna[x, y] != semmi) || (x == select_y && y == select_x));
                akna[x, y] = minemark;
            }



            int count = 0;
            for (int x = 0; x < akna.GetLength(0); x++)
            {
                for (int y = 0; y < akna.GetLength(1); y++)
                {
                    if (akna[x, y] != minemark)
                    {
                        count = 0;
                        if (x - 1 >= 0) //fel
                        {
                            if (akna[x - 1, y] == minemark) count++;
                        }
                        if (y - 1 >= 0) //balra
                        {
                            if (akna[x, y - 1] == minemark) count++;
                        }
                        if ((x - 1 >= 0) && (y - 1 >= 0)) //balra fel
                        {
                            if (akna[x - 1, y - 1] == minemark) count++;
                        }
                        if (x + 1 < meretM) //le
                        {
                            if (akna[x + 1, y] == minemark) count++;
                        }
                        if (((x - 1 >= 0) && (y + 1 < meretSZ))) //jobbra fel
                        {
                            if (akna[x - 1, y + 1] == minemark) count++;
                        }
                        if (y + 1 < meretSZ) //jobbra
                        {
                            if (akna[x, y + 1] == minemark) count++;
                        }
                        if ((y - 1 >= 0) && (x + 1 < meretM)) //balra le
                        {
                            if (akna[x + 1, y - 1] == minemark) count++;
                        }
                        if ((y + 1 < meretSZ) && (x + 1 < meretM)) //jobbra le
                        {
                            if (akna[x + 1, y + 1] == minemark) count++;
                        }
                        if (count == 0)
                        {
                            akna[x, y] = semmi;
                        }
                        else
                        {
                            akna[x, y] = Convert.ToString(count);
                        }
                    }
                }
            }
            return akna;
        }
        public static void Draw()
        {
            gameBoard.Children.Clear();
            gameBoard.Rows = meretM;
            gameBoard.Columns = meretSZ;
            Uri CellImage = Appearance.Images.kerdojel; //ha nem lenne valami hiba miatt kép
            for (int x = 0; x < meretSZ; x++)
            {
                for (int y = 0; y < meretM; y++)
                {
                    string Cell = akna[x, y];

                    if (Cell == "1")
                    {
                        CellImage = Appearance.Images._1;
                    }
                    else if (Cell == "2")
                    {
                        CellImage = Appearance.Images._2;
                    }
                    else if (Cell == "3")
                    {
                        CellImage = Appearance.Images._3;
                    }
                    else if (Cell == "4")
                    {
                        CellImage = Appearance.Images._4;
                    }
                    else if (Cell == "5")
                    {
                        CellImage = Appearance.Images._5;
                    }
                    else if (Cell == "6")
                    {
                        CellImage = Appearance.Images._6;
                    }
                    else if (Cell == "7")
                    {
                        CellImage = Appearance.Images._7;
                    }
                    else if (Cell == "8")
                    {
                        CellImage = Appearance.Images._8;
                    }
                    else if (Cell == Appearance.Characters.semmi)
                    {
                        CellImage = Appearance.Images.semmi;
                    }
                    else if (Cell == Appearance.Characters.zaszlo)
                    {
                        CellImage = Appearance.Images.zaszlozott;
                    }
                    else if (Cell == Appearance.Characters.akna)
                    {
                        CellImage = Appearance.Images.zaszlozott; //akna kép kell ide
                    }

                    Button btn = new Button
                    {
                        Tag = new Point(x, y),
                        FontWeight = FontWeights.Bold,
                        FontSize = 15,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                        MinWidth = 24,
                        MinHeight = 24,
                    };
                    Image img = new Image
                    {
                        Source = new BitmapImage(CellImage),
                        Stretch = Stretch.UniformToFill,
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
