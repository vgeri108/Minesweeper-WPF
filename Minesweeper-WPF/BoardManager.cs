using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        public static bool replayGame = false;
        public static bool LoadedGame = false;

        private static string semmi = Appearance.Characters.semmi;
        private static string minemark = Appearance.Characters.akna;

        static bool gameover = false;
        static string gameover_type = "-";


        public static void Init()
        {
            if (!LoadedGame)
            {
                if (Data.ApplyOnNextGame)
                {
                    Data.ApplyOnNextGame = false;
                    Data.meretM = Data.NextMeretM;
                    Data.meretSZ = Data.NextMeretSZ;
                    Data.aknakszama = Data.NextAknakszama;
                    Data.Difficulty = Data.NextDifficulty;
                    JsonManager.Settings.Save();
                }

                Time.ResetTimer();
                if (!replayGame) Data.ResizeBoard();
                newGame = true; //az időmérő nullázódik ha true és új generálás
                                //replayGame ---- ha true akkor az időmérő nullázódik, de nem lesz új pálya
                gameover = false;
                gameover_type = "-";
                InitGenerate();
            }
            else
            {
                newGame = false;
                Draw();
                int LoadedTime = Time.ElapsedSeconds;
                Time.StopTimer();
                Time.StartTimer(LoadedTime);
            }
        }
        private static void InitGenerate()
        {
            for (int y = 0; y < Data.meretM; y++)
            {
                for (int x = 0; x < Data.meretSZ; x++)
                {
                    Data.visible[y, x] = "false";
                }
            }

            Data.flagCount = 0;

            gameBoard.Children.Clear();
            gameBoard.Rows = Data.meretM;
            gameBoard.Columns = Data.meretSZ;

            for (int y = 0; y < Data.meretM; y++)
            {
                for (int x = 0; x < Data.meretSZ; x++)
                {
                    AddButton(Appearance.Images.fedes, x, y);
                }
            }

            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.MineCounterUpdate(Data.aknakszama - Data.flagCount);
            }
        }
        public static string[,] Generate(int select_x, int select_y)
        {
            bool vanUres;
            bool siker = false;
            Random random = new Random();
            for (int tries = 0; tries < 1000 && !siker; tries++)
            {
                for (int i = 0; i < Data.akna.GetLength(0); i++)
                {
                    for (int j = 0; j < Data.akna.GetLength(1); j++)
                    {
                        Data.akna[i, j] = semmi;
                        Data.visible[i, j] = "false";
                    }
                }
                for (int i = 0; i < Data.aknakszama; i++)
                {
                    int x, y;
                    do
                    {
                        x = random.Next(0, Data.meretSZ);
                        y = random.Next(0, Data.meretM);
                    } while ((Data.akna[y, x] != semmi) || (x == select_x && y == select_y));
                    Data.akna[y, x] = minemark;
                }

                int count = 0;

                for (int x = 0; x < Data.meretSZ; x++)
                {
                    for (int y = 0; y < Data.meretM; y++)
                    {
                        if (Data.akna[y, x] != minemark)
                        {
                            count = 0;
                            if (y - 1 >= 0) // fel (up)
                            {
                                if (Data.akna[y - 1, x] == minemark) count++;
                            }
                            if (x - 1 >= 0) // balra (left)
                            {
                                if (Data.akna[y, x - 1] == minemark) count++;
                            }
                            if ((y - 1 >= 0) && (x - 1 >= 0)) // balra fel (up-left)
                            {
                                if (Data.akna[y - 1, x - 1] == minemark) count++;
                            }
                            if (y + 1 < Data.meretM) // le (down)
                            {
                                if (Data.akna[y + 1, x] == minemark) count++;
                            }
                            if ((y - 1 >= 0) && (x + 1 < Data.meretSZ)) // jobbra fel (up-right)
                            {
                                if (Data.akna[y - 1, x + 1] == minemark) count++;
                            }
                            if (x + 1 < Data.meretSZ) // jobbra (right)
                            {
                                if (Data.akna[y, x + 1] == minemark) count++;
                            }
                            if ((y + 1 < Data.meretM) && (x - 1 >= 0)) // balra le (down-left)
                            {
                                if (Data.akna[y + 1, x - 1] == minemark) count++;
                            }
                            if ((y + 1 < Data.meretM) && (x + 1 < Data.meretSZ)) // jobbra le (down-right)
                            {
                                if (Data.akna[y + 1, x + 1] == minemark) count++;
                            }
                            if (count == 0)
                            {
                                Data.akna[y, x] = semmi;
                            }
                            else
                            {
                                Data.akna[y, x] = Convert.ToString(count);
                            }
                        }
                    }
                }

                vanUres = false;
                for (int x = 0; x < Data.meretSZ; x++)
                {
                    for (int y = 0; y < Data.meretM; y++)
                    {
                        if (Data.akna[y, x] == semmi)
                        {
                            vanUres = true;
                            break;
                        }
                    }
                    if (vanUres) break;
                }

                if (Data.akna[select_y, select_x] == semmi)
                {
                    siker = true;
                }
                else if (!vanUres && Data.akna[select_y, select_x] != minemark)
                {
                    siker = true;
                }
            }

            return Data.akna;
        }
        public static void Draw()
        {
            NyeresEllenorzes();
            gameBoard.Children.Clear();
            gameBoard.Rows = Data.meretM;
            gameBoard.Columns = Data.meretSZ;
            for (int y = 0; y < Data.meretM; y++)
            {
                for (int x = 0; x < Data.meretSZ; x++)
                {
                    Uri CellImage = Appearance.Images.error; //ha nem lenne valami hiba miatt kép
                    string Cell = Data.akna[y, x];
                    if (Data.visible[y, x] == "true" || gameover)
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
                    }
                    else if (Data.visible[y, x] == "flag")
                    {
                        AddButton(Appearance.Images.zaszlozott, x, y);
                    }
                    else if (Data.visible[y, x] == "question")
                    {
                        AddButton(Appearance.Images.kerdojel, x, y);
                    }
                    else if (Data.visible[y, x] == "false")
                    {
                        AddButton(Appearance.Images.fedes, x, y);
                    }
                }
            }
        }
        private static void Felfedes(int x, int y)
        {
            if (x < 0 || x >= Data.meretSZ || y < 0 || y >= Data.meretM) return;
            if (Data.visible[y, x] == "true" || Data.visible[y, x] == "flag") return;
            Data.visible[y, x] = "true";
            if (Data.akna[y, x] == semmi)
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
        }
        private static void Cell_Click(object sender, RoutedEventArgs e)
        {
            if (!gameover)
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    Point pos = (Point)btn.Tag;
                    int x = (int)pos.X;
                    int y = (int)pos.Y;

                    if (Data.akna == null || Data.visible == null) return;
                    if (y < 0 || y >= Data.akna.GetLength(0) || x < 0 || x >= Data.akna.GetLength(1)) return;

                    if (newGame)
                    {
                        Statistics.GenerateStatsIfNotExists();
                        Statistics.PlayedGames[Statistics.currentMode]++;
                        JsonManager.Stats.Save();
                        if (!LoadedGame)
                        {
                            Time.StartTimer();
                        }
                        LoadedGame = false;
                        if (!replayGame)
                        {
                            Generate(x, y);
                        }
                        else
                        {
                            for (int _y = 0; _y < Data.meretM; _y++)
                            {
                                for (int _x = 0; _x < Data.meretSZ; _x++)
                                {
                                    Data.visible[_y, _x] = "false";
                                }
                            }
                        }
                        newGame = false;
                        replayGame = false;
                    }

                    if (Data.akna[y, x] == minemark)
                    {
                        gameover = true;
                        gameover_type = "akna";
                        Time.StopTimer();
                    }
                    Felfedes(x, y);
                    Draw();

                    if (gameover)
                    {
                        ShowGameOverDialog();
                    }
                }
            }
        }
        private static void Cell_RightClick(object sender, RoutedEventArgs e)
        {
            if (!gameover)
            {
                Button btn = sender as Button;
                if (btn != null)
                {
                    Point pos = (Point)btn.Tag;
                    int x = (int)pos.X;
                    int y = (int)pos.Y;

                    if (Data.akna == null || Data.visible == null) return;
                    if (y < 0 || y >= Data.akna.GetLength(0) || x < 0 || x >= Data.akna.GetLength(1)) return;

                    if (newGame)
                    {
                        Statistics.PlayedGames[Statistics.currentMode]++;
                        Statistics.GenerateStatsIfNotExists();
                        JsonManager.Stats.Save();
                        if (!LoadedGame) Time.StartTimer();
                    }
                    LoadedGame = false;
                    if (Data.visible[y, x] == "false")
                    {
                        Data.visible[y, x] = "flag";
                        Flag(y, x);
                    }
                    else if (Data.visible[y, x] == "flag")
                    {
                        Data.visible[y, x] = "question";
                        RemoveFlag(y, x);
                    }
                    else if (Data.visible[y, x] == "question")
                    {
                        Data.visible[y, x] = "false";
                    }
                    Draw();
                }
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
        private static void Flag(int y, int x)
        {
            Data.flagCount++;

            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.MineCounterUpdate(Data.aknakszama - Data.flagCount);
            }
        }
        private static void RemoveFlag(int y, int x)
        {
            Data.flagCount--;

            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.MineCounterUpdate(Data.aknakszama - Data.flagCount);
            }
        }
        private static void NyeresEllenorzes()
        {
            for (int y = 0; y < Data.akna.GetLength(0); y++)
            {
                for (int x = 0; x < Data.akna.GetLength(1); x++)
                {
                    if (Data.akna[y, x] != minemark && Data.visible[y, x] != "true")
                    {
                        return;
                    }
                }
            }

            gameover = true;
            gameover_type = "cleared";
            Time.StopTimer();
        }
        private static void ShowGameOverDialog()
        {
            if (!(System.Windows.Application.Current?.MainWindow is MainWindow mw)) return;

            mw.ShowInTaskbar = false;
            Window dialog;
            if (gameover_type == "akna")
            {
                dialog = new GameLose();
            }
            else
            {
                Statistics.Times[Statistics.currentMode].Add(Time.ElapsedSeconds);
                Statistics.Dates[Statistics.currentMode].Add(DateTime.Now.ToString("yyyy/MM/dd"));
                dialog = new GameWin();
            }

            Statistics.GenerateStatsIfNotExists();

            if (Statistics.IsLastGameWinned[Statistics.currentMode] && gameover_type != "akna")
            {
                Statistics.WinStreak[Statistics.currentMode]++;
                Statistics.CurrentStreak[Statistics.currentMode]++;
            }
            else if (!Statistics.IsLastGameWinned[Statistics.currentMode] && gameover_type != "akna")
            {
                Statistics.CurrentStreak[Statistics.currentMode] = 1;
                Statistics.LoseStreak[Statistics.currentMode] = 0;
                Statistics.WinStreak[Statistics.currentMode]++;
            }
            else if (Statistics.IsLastGameWinned[Statistics.currentMode] && gameover_type == "akna")
            {
                Statistics.CurrentStreak[Statistics.currentMode] = 1;
                Statistics.WinStreak[Statistics.currentMode] = 0;
                Statistics.LoseStreak[Statistics.currentMode]++;
            }
            else if (!Statistics.IsLastGameWinned[Statistics.currentMode] && gameover_type == "akna")
            {
                Statistics.LoseStreak[Statistics.currentMode]++;
                Statistics.CurrentStreak[Statistics.currentMode]++;
            }

            if (Statistics.WinStreak[Statistics.currentMode] > Statistics.LongestWinStreak[Statistics.currentMode]) Statistics.LongestWinStreak = Statistics.WinStreak;
            if (Statistics.LoseStreak[Statistics.currentMode] > Statistics.LongestLoseStreak[Statistics.currentMode]) Statistics.LongestLoseStreak = Statistics.LoseStreak;

            JsonManager.Stats.Save();
            dialog.Owner = mw;
            dialog.ShowInTaskbar = true;
            dialog.ShowDialog();
            mw.ShowInTaskbar = true;
            mw.UpdateTimerText();
        }

    }
}
