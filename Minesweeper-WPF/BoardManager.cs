using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        private static bool newGame = true;

        private static string[,] akna = Data.akna;
        private static string[,] visible = Data.visible;
        private static int meretM = Data.meretM;
        private static int meretSZ = Data.meretSZ;
        private static int aknakszama = Data.aknakszama;
        private static string semmi = Appearance.Characters.semmi;
        private static string minemark = Appearance.Characters.akna;
        
        public static void InitNew()
        {
            newGame = true;
        }
        public static void InitFist()
        {
            gameBoard.Children.Clear();
            gameBoard.Rows = meretM;
            gameBoard.Columns = meretSZ;
            for (int x = 0; x < meretSZ; x++)
            {
                for (int y = 0; y < meretM; y++)
                {
                    AddButton(Appearance.Images.fedes, x, y);
                }
            }
        }
        public static string[,] Generate(int select_x, int select_y)
        {
            bool vanUres;
            bool siker = false;
            Random random = new Random();
            for (int tries = 0; tries < 1000 && !siker; tries++)
            {
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
                    } while ((akna[y, x] != semmi) || (x == select_x && y == select_y));
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

                vanUres = false;
                for (int x = 0; x < akna.GetLength(0); x++)
                {
                    for (int y = 0; y < akna.GetLength(1); y++)
                    {
                        if (akna[x, y] == semmi)
                        {
                            vanUres = true;
                            break;
                        }
                    }
                    if (vanUres) break;
                }

                if (akna[select_x, select_y] == semmi)
                {
                    siker = true;
                }
                else if (!vanUres && akna[select_x, select_y] != minemark)
                {
                    siker = true;
                }
            }

            return akna;
        }
        public static void Draw()
        {
            gameBoard.Children.Clear();
            gameBoard.Rows = meretM;
            gameBoard.Columns = meretSZ;
            Uri CellImage = Appearance.Images.error; //ha nem lenne valami hiba miatt kép
            for (int x = 0; x < meretSZ; x++)
            {
                for (int y = 0; y < meretM; y++)
                {
                    string Cell = akna[x, y];
                    if (visible[x, y] == "true")
                    {
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
                        else if (Cell == Appearance.Characters.akna)
                        {
                            CellImage = Appearance.Images.akna;
                        }
                        AddButton(CellImage, x, y);
                        
                    }else if (visible[x,y] == "flag")
                    {
                        AddButton(Appearance.Images.zaszlozott, x, y);
                    }
                    else if (visible[x, y] == "question")
                    {
                        AddButton(Appearance.Images.kerdojel, x, y);
                    }
                    else if (visible[x,y] == "false")
                    {
                        AddButton(Appearance.Images.fedes, x, y);
                    }
                }
            }
        }
        private static void Felfedes(int x, int y)
        {
            if (x < 0 || x >= meretM || y < 0 || y >= meretSZ) return;
            if (visible[x, y] == "true" || visible[x, y] == "flag") return; //|| visible[x,y] == "question"
            visible[x, y] = "true";
            if (akna[x, y] == semmi)
            {
                Felfedes(x - 1, y); //fel
                Felfedes(x + 1, y); //le
                Felfedes(x, y - 1); //bal
                Felfedes(x, y + 1); //jobb
                Felfedes(x - 1, y - 1); //bal-fel
                Felfedes(x - 1, y + 1); //jobb-fel
                Felfedes(x + 1, y - 1); //bal-le
                Felfedes(x + 1, y + 1); //jobb-le
            }
            if (akna[x, y] == minemark)
            {
                //gameover = true;
                //gameover_type = "akna";
            }
        }
        private static void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Point pos = (Point)btn.Tag;
                int x = (int)pos.X;
                int y = (int)pos.Y;

                if (newGame)
                {
                    Generate(x, y);
                    newGame = false;
                }

                Felfedes(x, y);
                Draw();
            }
        }
        private static void Cell_RightClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Point pos = (Point)btn.Tag;
                int x = (int)pos.X;
                int y = (int)pos.Y;

                if (visible[x,y] == "false")
                {
                    visible[x, y] = "flag";
                }
                else if (visible[x, y] == "flag")
                {
                    visible[x, y] = "question";
                }
                else if (visible[x, y] == "question")
                {
                    visible[x, y] = "false";
                }

                Draw();
            }
        }
        private static void AddButton(Uri CellImage, int x, int y)
        {
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
            btn.Click += Cell_Click;
            btn.MouseRightButtonUp += Cell_RightClick;

            gameBoard.Children.Add(btn);
        }
    }
}
