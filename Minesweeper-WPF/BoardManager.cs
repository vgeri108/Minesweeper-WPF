using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private static readonly Random r = new Random();

        private static bool newGame = true;
        private static bool firstClick = true;
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
                newGame = true;
                firstClick = true;
                gameover_type = "-";
                RandomizeCover();
                InitGenerate();
            }
            else
            {
                newGame = false;
                firstClick = false;
                Draw();
                int LoadedTime = Time.ElapsedSeconds;
                Time.StopTimer();
                Time.StartTimer(LoadedTime);
            }
        }
        private static void InitGenerate()
        {
            for (int x = 0; x < Data.visible.GetLength(0); x++)
            {
                for (int y = 0; y < Data.visible.GetLength(1); y++)
                {
                    Data.visible[x, y] = "false";
                }
            }

            Data.flagCount = 0;

            gameBoard.Children.Clear();
            gameBoard.Rows = Data.meretM;
            gameBoard.Columns = Data.meretSZ;

            for (int y = 0; y < Data.akna.GetLength(1); y++)
            {
                for (int x = 0; x < Data.akna.GetLength(0); x++)
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string imagePath = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme, Data.coverTexture[x, y]);
                    Uri imageUri = new Uri(imagePath, UriKind.Absolute);
                    AddButton(imageUri, x, y);
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
            for (int tries = 0; tries < 1000 && !siker; tries++)
            {
                for (int x = 0; x < Data.akna.GetLength(0); x++)
                {
                    for (int y = 0; y < Data.akna.GetLength(1); y++)
                    {
                        Data.akna[x, y] = semmi;
                        if (Data.visible[x,y] != "flag" && Data.visible[x, y] != "question") Data.visible[x, y] = "false";
                    }
                }
                for (int i = 0; i < Data.aknakszama; i++)
                {
                    int x, y;
                    do
                    {
                        x = r.Next(0, Data.akna.GetLength(0));
                        y = r.Next(0, Data.akna.GetLength(1));
                    } while ((Data.akna[x, y] != semmi) || (x == select_x && y == select_y));
                    Data.akna[x, y] = minemark;
                }

                int count = 0;
                int maxX = Data.akna.GetLength(0);
                int maxY = Data.akna.GetLength(1);
                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        if (Data.akna[x, y] != minemark)
                        {
                            count = 0;

                            // fel
                            if (y - 1 >= 0 && Data.akna[x, y - 1] == minemark) count++;

                            // le
                            if (y + 1 < maxY && Data.akna[x, y + 1] == minemark) count++;

                            // bal
                            if (x - 1 >= 0 && Data.akna[x - 1, y] == minemark) count++;

                            // jobb
                            if (x + 1 < maxX && Data.akna[x + 1, y] == minemark) count++;

                            // bal-fel
                            if (x - 1 >= 0 && y - 1 >= 0 && Data.akna[x - 1, y - 1] == minemark) count++;

                            // jobb-fel
                            if (x + 1 < maxX && y - 1 >= 0 && Data.akna[x + 1, y - 1] == minemark) count++;

                            // bal-le
                            if (x - 1 >= 0 && y + 1 < maxY && Data.akna[x - 1, y + 1] == minemark) count++;

                            // jobb-le
                            if (x + 1 < maxX && y + 1 < maxY && Data.akna[x + 1, y + 1] == minemark) count++;

                            if (count == 0)
                            {
                                Data.akna[x, y] = semmi;
                            }
                            else
                            {
                                Data.akna[x, y] = Convert.ToString(count);
                            }
                        }
                    }
                }

                vanUres = false;
                for (int x = 0; x < Data.akna.GetLength(0); x++)
                {
                    for (int y = 0; y < Data.akna.GetLength(1); y++)
                    {
                        if (Data.akna[x, y] == semmi)
                        {
                            vanUres = true;
                            break;
                        }
                    }
                    if (vanUres) break;
                }

                if (Data.akna[select_x, select_y] == semmi)
                {
                    siker = true;
                }
                else if (!vanUres && Data.akna[select_x, select_y] != minemark)
                {
                    siker = true;
                }
            }

            return Data.akna;
        }
        public static void Draw()
        {
            gameBoard.Children.Clear();
            gameBoard.Rows = Data.meretM;
            gameBoard.Columns = Data.meretSZ;

            for (int y = 0; y < Data.akna.GetLength(1); y++)
            {
                for (int x = 0; x < Data.akna.GetLength(0); x++)
                {
                    Uri CellImage = Appearance.Images.error;
                    string Cell = Data.akna[x, y];

                    if (Data.visible[x, y] == "true" || gameover)
                    {
                        switch (Cell)
                        {
                            case "1": CellImage = Appearance.Images._1; break;
                            case "2": CellImage = Appearance.Images._2; break;
                            case "3": CellImage = Appearance.Images._3; break;
                            case "4": CellImage = Appearance.Images._4; break;
                            case "5": CellImage = Appearance.Images._5; break;
                            case "6": CellImage = Appearance.Images._6; break;
                            case "7": CellImage = Appearance.Images._7; break;
                            case "8": CellImage = Appearance.Images._8; break;
                            case var s when s == Appearance.Characters.semmi: CellImage = Appearance.Images.semmi; break;
                            case var s when s == Appearance.Characters.akna: CellImage = Appearance.Images.akna; break;
                        }
                        AddButton(CellImage, x, y);
                    }
                    else
                    {
                        switch (Data.visible[x, y])
                        {
                            case "false": 
                                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                                string imagePath = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme, Data.coverTexture[x,y]);
                                AddButton(new Uri(imagePath, UriKind.Absolute), x, y); 
                                break;
                            case "flag": AddButton(Appearance.Images.zaszlozott, x, y); break;
                            case "question": AddButton(Appearance.Images.kerdojel, x, y); break;
                        }
                    }
                }
            }
        }

        private static void Felfedes(int x, int y)
        {
            int maxX = Data.visible.GetLength(0);
            int maxY = Data.visible.GetLength(1);

            if (x < 0 || x >= maxX || y < 0 || y >= maxY) return;

            if (Data.visible[x, y] == "true" || Data.visible[x, y] == "flag") return;
            Data.visible[x, y] = "true";
            if (Data.akna[x, y] == semmi)
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
                    if (x < 0 || x >= Data.akna.GetLength(0) || y < 0 || y >= Data.akna.GetLength(1)) return;

                    if (newGame)
                    {
                        Statistics.GenerateStatsIfNotExists();
                        Statistics.PlayedGames[Statistics.currentMode]++;
                        JsonManager.Stats.Save();
                        if (!LoadedGame && firstClick)
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
                            for (int _x = 0; _x < Data.visible.GetLength(0); _x++)
                            {
                                for (int _y = 0; _y < Data.visible.GetLength(1); _y++)
                                {
                                    Data.visible[_x, _y] = "false";
                                }
                            }
                        }
                        newGame = false;
                        firstClick = false;
                        replayGame = false;
                    }

                    if (Data.akna[x, y] == minemark)
                    {
                        gameover = true;
                        gameover_type = "akna";
                        Time.StopTimer();
                    }
                    Felfedes(x, y);
                    Draw();
                    NyeresEllenorzes();
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
                    if (x < 0 || x >= Data.akna.GetLength(0) || y < 0 || y >= Data.akna.GetLength(1)) return;

                    if (firstClick)
                    {
                        Statistics.PlayedGames[Statistics.currentMode]++;
                        Statistics.GenerateStatsIfNotExists();
                        JsonManager.Stats.Save();
                        if (!LoadedGame) Time.StartTimer();
                        firstClick = false;
                    }
                    LoadedGame = false;
                    if (Data.visible[x, y] == "false")
                    {
                        Data.visible[x, y] = "flag";
                        Flag();
                    }
                    else if (Data.visible[x, y] == "flag")
                    {
                        Data.visible[x, y] = "question";
                        RemoveFlag();
                    }
                    else if (Data.visible[x, y] == "question")
                    {
                        Data.visible[x, y] = "false";
                    }
                    Draw();
                    NyeresEllenorzes();
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
            
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = CellImage;
            try
            {
                bitmap.EndInit();
            }
            catch
            {
                bitmap = new BitmapImage(Appearance.Images.error);
            }
            
            Image img = new Image
            {
                Source = bitmap,
                Stretch = Stretch.UniformToFill,
            };

            btn.Content = img;
            btn.Click += Cell_Click;
            btn.MouseRightButtonUp += Cell_RightClick;

            gameBoard.Children.Add(btn);
        }
        private static void Flag()
        {
            Data.flagCount++;

            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.MineCounterUpdate(Data.aknakszama - Data.flagCount);
            }
        }
        private static void RemoveFlag()
        {
            Data.flagCount--;

            if (System.Windows.Application.Current?.MainWindow is MainWindow mw)
            {
                mw.MineCounterUpdate(Data.aknakszama - Data.flagCount);
            }
        }
        private static void NyeresEllenorzes()
        {
            for (int x = 0; x < Data.akna.GetLength(0); x++)
            {
                for (int y = 0; y < Data.akna.GetLength(1); y++)
                {
                    if (Data.akna[x, y] != minemark && Data.visible[x, y] != "true")
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
                if (Statistics.Times[Statistics.currentMode].Contains(-1)) Statistics.Times[Statistics.currentMode].Remove(-1);
                if (Statistics.Dates[Statistics.currentMode].Contains("Nincs adat.")) Statistics.Dates[Statistics.currentMode].Remove("Nincs adat.");
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
        public static void BoardApplyTheme()
        {
            RandomizeCover();
            Draw();
        }
        private static void RandomizeCover()
        {
            for (int x = 0; x < Data.coverTexture.GetLength(0); x++)
            {
                for (int y = 0; y < Data.coverTexture.GetLength(1); y++)
                {
                    if (Appearance.Images.CoverTextureList.Count > 0)
                    {
                        Data.coverTexture[x, y] = Appearance.Images.CoverTextureList[r.Next(0, Appearance.Images.CoverTextureList.Count)];
                    }
                    else
                    {
                        Data.coverTexture[x, y] = Appearance.Images.ImageNames["fedes"];
                    }
                    Debug.Write(Data.coverTexture[x, y] + ",");
                }
                Debug.WriteLine("");
            }
        }
    }
}
