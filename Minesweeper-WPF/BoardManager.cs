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
using System.Windows.Input;
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

        public static bool gameover = false;
        static string gameover_type = "-";

        // egyszerű kép cache a nagy pályákhoz
        private static readonly Dictionary<string, BitmapImage> bitmapCache = new Dictionary<string, BitmapImage>(StringComparer.OrdinalIgnoreCase);
        private static readonly object bitmapCacheLock = new object();

        // Button pool — újrafelhasználjuk a Button és Image objektumokat nagy pályákhoz
        private static readonly List<Button> buttonPool = new List<Button>();

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
                Sounds.Start.Play();
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
            int width = Data.visible.GetLength(0);
            int height = Data.visible.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Data.visible[x, y] = "false";
                }
            }

            Data.flagCount = 0;

            gameBoard.Rows = Data.meretM;
            gameBoard.Columns = Data.meretSZ;

            // Pool mérete (sorok*oszlopok)
            int maxX = Data.akna.GetLength(0);
            int maxY = Data.akna.GetLength(1);
            int required = maxX * maxY;

            // Ha a pool nem megfelelő, újrageneráljuk egyszer — ez minimalizálja a folyamatos objektumlétrehozást
            if (buttonPool.Count != required)
            {
                buttonPool.Clear();
                gameBoard.Children.Clear();

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                for (int y = 0; y < maxY; y++)
                {
                    for (int x = 0; x < maxX; x++)
                    {
                        Button btn = new Button
                        {
                            Tag = new Point(x, y),
                            FontWeight = FontWeights.Bold,
                            FontSize = 15,
                            Margin = new Thickness(0),
                            Padding = new Thickness(0),
                        };

                        // kezdő kép (cover)
                        string imagePath = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme, Data.coverTexture[x, y]);
                        Uri imageUri = new Uri(imagePath, UriKind.Absolute);
                        BitmapImage bmp = GetCachedBitmap(imageUri);

                        Image img = new Image
                        {
                            Source = bmp,
                            Stretch = System.Windows.Media.Stretch.UniformToFill,
                        };

                        btn.Content = img;
                        btn.Click += Cell_Click;
                        btn.MouseRightButtonUp += Cell_RightClick;
                        btn.PreviewMouseLeftButtonDown += Cell_PreviewMouseLeftButtonDown;

                        buttonPool.Add(btn);
                        gameBoard.Children.Add(btn);
                    }
                }
            }
            else
            {
                // Frissítjük a Tag-eket és a kezdő képeket, ha pool maradt ugyanakkora (pl. csak új játék, de felbontás ugyanaz)
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                int idx = 0;
                for (int y = 0; y < maxY; y++)
                {
                    for (int x = 0; x < maxX; x++)
                    {
                        var btn = buttonPool[idx++];
                        btn.Tag = new Point(x, y);

                        string imagePath = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme, Data.coverTexture[x, y]);
                        var bmp = GetCachedBitmap(new Uri(imagePath, UriKind.Absolute));
                        if (btn.Content is Image img) img.Source = bmp;
                        else btn.Content = new Image { Source = bmp, Stretch = System.Windows.Media.Stretch.UniformToFill };
                    }
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
            int maxX = Data.akna.GetLength(0);
            int maxY = Data.akna.GetLength(1);

            for (int tries = 0; tries < 1000 && !siker; tries++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
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
                        x = r.Next(0, maxX);
                        y = r.Next(0, maxY);
                    } while ((Data.akna[x, y] != semmi) || (x == select_x && y == select_y));
                    Data.akna[x, y] = minemark;
                }

                int count = 0;
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
                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
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
            // Nem töröljük a gameBoard.Children-t minden alkalommal — újrafelhasználjuk a poolt.
            gameBoard.Rows = Data.meretM;
            gameBoard.Columns = Data.meretSZ;

            int maxX = Data.akna.GetLength(0);
            int maxY = Data.akna.GetLength(1);
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            int CellIndex = 0;
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
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
                    }
                    else
                    {
                        switch (Data.visible[x, y])
                        {
                            case "false":
                                string imagePath = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme, Data.coverTexture[x, y]);
                                CellImage = new Uri(imagePath, UriKind.Absolute);
                                break;
                            case "flag": CellImage = Appearance.Images.zaszlozott; break;
                            case "question": CellImage = Appearance.Images.kerdojel; break;
                            default:
                                string defaultPath = Path.Combine(baseDir, "Assets", "Themes", Configuration.CurrentTheme, Data.coverTexture[x, y]);
                                CellImage = new Uri(defaultPath, UriKind.Absolute);
                                break;
                        }
                    }

                    if (CellIndex < buttonPool.Count)
                    {
                        var btn = buttonPool[CellIndex++];
                        btn.Tag = new Point(x, y);
                        if (btn.Content is Image img)
                        {
                            img.Source = GetCachedBitmap(CellImage);
                        }
                        else
                        {
                            btn.Content = new Image { Source = GetCachedBitmap(CellImage), Stretch = System.Windows.Media.Stretch.UniformToFill };
                        }
                    }
                    else
                    {
                        AddButton(CellImage, x, y);
                        CellIndex++;
                    }
                }
            }
        }

        // Preview handler a dupla kattintásra (easy mining)
        private static void Cell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;

            if (sender is not Button btn) return;
            if (btn.Tag is not Point pos) return;

            int x = (int)pos.X;
            int y = (int)pos.Y;

            // csak akkor működik, ha a mező látható és szám
            if (Data.visible == null || Data.akna == null) return;
            if (x < 0 || x >= Data.akna.GetLength(0) || y < 0 || y >= Data.akna.GetLength(1)) return;
            if (Data.visible[x, y] != "true") return;

            // jelleg ellenőrzés: csak számokra
            string cell = Data.akna[x, y];
            if (!int.TryParse(cell, out int requiredFlags) || requiredFlags <= 0) return;

            // elvégezzük az easy-mine műveletet
            EasyMine(x, y);

            // frissítjük a megjelenítést és állapotot
            Draw();
            NyeresEllenorzes();
            if (gameover)
            {
                ShowGameOverDialog();
            }

            // fogyasszuk el az eseményt, hogy a Click ne fusson kétszer
            e.Handled = true;
        }

        private static void EasyMine(int x, int y)
        {
            int maxX = Data.akna.GetLength(0);
            int maxY = Data.akna.GetLength(1);

            // megszámoljuk a környező zászlókat
            int flagCount = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int nx = x + dx, ny = y + dy;
                    if (nx < 0 || nx >= maxX || ny < 0 || ny >= maxY) continue;
                    if (Data.visible[nx, ny] == "flag") flagCount++;
                }
            }

            if (!int.TryParse(Data.akna[x, y], out int requiredFlags) || requiredFlags != flagCount) return;

            // ha megegyezik, felnyitjuk a környező nem-zászló / nem-kérdőjeles mezőket
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int nx = x + dx, ny = y + dy;
                    if (nx < 0 || nx >= maxX || ny < 0 || ny >= maxY) continue;

                    // kihagyjuk a zászlókat és kérdőjeleket és már láthatókat
                    if (Data.visible[nx, ny] == "flag" || Data.visible[nx, ny] == "question" || Data.visible[nx, ny] == "true") continue;

                    // ha akna — játék vége
                    if (Data.akna[nx, ny] == minemark)
                    {
                        gameover = true;
                        gameover_type = "akna";
                        Time.StopTimer();
                    }
                    else if (Data.akna[nx, ny] == semmi)
                    {
                        // ha üres, teljes felfedés szükséges (rekurzív viselkedés megőrzése)
                        Felfedes(nx, ny);
                    }
                    else
                    {
                        // szám: csak jelöljük láthatónak
                        Data.visible[nx, ny] = "true";
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
                    int maxX = Data.akna.GetLength(0);
                    int maxY = Data.visible.GetLength(1);
                    if (x < 0 || x >= maxX || y < 0 || y >= maxY) return;

                    if (newGame)
                    {
                        Statistics.GenerateStatsIfNotExists();
                        Statistics.PlayedGames[Statistics.currentMode]++;
                        JsonManager.Stats.Save();
                        if (!LoadedGame && firstClick)
                        {
                            Sounds.Click.Play();
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
                    else
                    {
                        Sounds.EveryClick.Play();
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
                    int maxX = Data.akna.GetLength(0);
                    int maxY = Data.akna.GetLength(1);
                    if (x < 0 || x >= maxX || y < 0 || y >= maxY) return;

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
                        Sounds.Flag.Play();
                    }
                    else if (Data.visible[x, y] == "flag")
                    {
                        if (Configuration.EnableQuestionMarks)
                            Data.visible[x, y] = "question";
                        else Data.visible[x, y] = "false";
                        
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
            // Ez a visszaesés addolási út, ha poolból valamiért nem tudunk dolgozni.
            Button btn = new Button
            {
                Tag = new Point(x, y),
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                Margin = new Thickness(0),
                Padding = new Thickness(0),
            };
            
            BitmapImage bitmap = GetCachedBitmap(CellImage);

            Image img = new Image
            {
                Source = bitmap,
                Stretch = System.Windows.Media.Stretch.UniformToFill,
            };

            btn.Content = img;
            btn.Click += Cell_Click;
            btn.MouseRightButtonUp += Cell_RightClick;
            btn.PreviewMouseLeftButtonDown += Cell_PreviewMouseLeftButtonDown;

            gameBoard.Children.Add(btn);
        }

        private static BitmapImage GetCachedBitmap(Uri uri)
        {
            if (uri == null) uri = Appearance.Images.error;
            string key = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();

            lock (bitmapCacheLock)
            {
                if (bitmapCache.TryGetValue(key, out var cached)) return cached;

                try
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.UriSource = uri;
                    bmp.EndInit();
                    bmp.Freeze();
                    bitmapCache[key] = bmp;
                    return bmp;
                }
                catch
                {
                    // ha hiba történik, próbáljuk meg az error képet használni (szintén cache-elve)
                    var errUri = Appearance.Images.error;
                    string errKey = errUri.IsAbsoluteUri ? errUri.AbsoluteUri : errUri.ToString();
                    if (bitmapCache.TryGetValue(errKey, out var errCached)) return errCached;

                    try
                    {
                        var errBmp = new BitmapImage();
                        errBmp.BeginInit();
                        errBmp.CacheOption = BitmapCacheOption.OnLoad;
                        errBmp.UriSource = errUri;
                        errBmp.EndInit();
                        errBmp.Freeze();
                        bitmapCache[errKey] = errBmp;
                        return errBmp;
                    }
                    catch
                    {
                        // végső fallback: új üres BitmapImage (ritkán fordul elő)
                        var empty = new BitmapImage();
                        return empty;
                    }
                }
            }
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
            int maxX = Data.akna.GetLength(0);
            int maxY = Data.akna.GetLength(1);

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
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
            int maxX = Data.coverTexture.GetLength(0);
            int maxY = Data.coverTexture.GetLength(1);

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (Appearance.Images.CoverTextureList.Count > 0)
                    {
                        Data.coverTexture[x, y] = Appearance.Images.CoverTextureList[r.Next(0, Appearance.Images.CoverTextureList.Count)];
                    }
                    else
                    {
                        Data.coverTexture[x, y] = Appearance.Images.ImageNames["fedes"];
                    }
                }
            }
        }
    }
}
