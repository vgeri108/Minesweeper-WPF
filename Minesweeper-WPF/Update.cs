using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Minesweeper_WPF
{
    internal class Update
    {
        public static List<string> NewTags = new List<string>();
        public static List<string> TagDescriptions = new List<string>();

        public static bool CheckFailed = false;

        public static async Task<bool> IsNewAvailable(bool IgnoreErrors = false)
        {
            CheckFailed = false;

            try
            {
                NewTags.Clear();
                TagDescriptions.Clear();

                using HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);

                string GH_TagList = await client.GetStringAsync(
                    "https://raw.githubusercontent.com/vgeri108/Minesweeper-WPF/refs/heads/main/Minesweeper-WPF/VersionTags.txt"
                );

                List<string> Tags = GH_TagList.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToList();

                int currentIndex = Tags.IndexOf(Version.GithubTag);

                if (currentIndex == 0) return false;

                for (int i = currentIndex - 1; i >= 0; i--)
                {
                    try
                    {
                        string description = await client.GetStringAsync($"https://raw.githubusercontent.com/vgeri108/Minesweeper-WPF/refs/tags/{Tags[i]}/Minesweeper-WPF/Version.txt");

                        NewTags.Insert(0, Tags[i]);
                        TagDescriptions.Insert(0, description);
                    }
                    catch
                    {
                        
                    }
                }

                return NewTags.Count > 0;
            }
            catch (Exception e)
            {
                if (IgnoreErrors) return false;

                CheckFailed = true;

                MessageBox.Show("A frissítések ellenőrzése sikertelen:\n\n" + e.Message +
                    "\n\nElőfordulhat, hogy nem csatlakozik az internethez.",
                    "Kapcsolódási hiba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation
                );

                return false;
            }
        }
        public static async Task Install()
        {
            string GH_SetupURL = "https://github.com/vgeri108/Minesweeper-WPF/raw/refs/heads/main/inno-setup/scripts/Output/minesweeper_setup.exe";
            string DownloadFilePath = Path.Combine(Path.GetTempPath(), "minesweeper_setup.exe");

            var window = new DownloadProgress();
            window.Show();

            try
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Installer");

                using var response = await client.GetAsync(
                    GH_SetupURL,
                    HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                long total = response.Content.Headers.ContentLength ?? 1;
                long read = 0;

                using var stream = await response.Content.ReadAsStreamAsync();

                using (var file = File.Create(DownloadFilePath))
                {
                    byte[] buffer = new byte[8192];
                    int bytes;

                    while ((bytes = await stream.ReadAsync(buffer)) > 0)
                    {
                        await file.WriteAsync(buffer, 0, bytes);
                        read += bytes;

                        double percent = read * 100d / total;
                        window.Dispatcher.Invoke(() =>
                            window.Progress.Value = percent);
                    }
                }

                window.Close();

                Process.Start(new ProcessStartInfo
                {
                    FileName = DownloadFilePath,
                    UseShellExecute = true,
                    Arguments = "/silent"
                });

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                window.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private static int CompareVersion(int[] oldVersion, int[] newVersion)
        {
            int length = Math.Max(oldVersion.Length, newVersion.Length);

            for (int i = 0; i < length; i++)
            {
                int oldPart = i < oldVersion.Length ? oldVersion[i] : 0;
                int newPart = i < newVersion.Length ? newVersion[i] : 0;

                if (oldPart < newPart) return -1;
                if (oldPart > newPart) return 1;
            }

            return 0;
        }

        public static void ApplyChanges(string version)
        {
            if (version != Version.GithubTag)
            {
                Progress progress = new Progress("Frissítés", "A frissítés konfigurálása folyamatban van...");
                progress.Show();
                int[] oldVersion = version.Substring(2).Split('.').Select(int.Parse).ToArray();


                //frissítés után lefuttatandó kódok


                //vB1.8
                if (CompareVersion(oldVersion, new[] { 1, 8, 1}) < 0)
                {
                    Configuration.CurrentTheme = "Default";
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Assets", "Themes", "Frontvonal");
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }


                JsonManager.Settings.Load();
                JsonManager.Theme.Load();
                JsonManager.Style.Save();
                progress.Close();
                MessageBox.Show($"Az Aknakereső frissült a(z) {Version.GithubTag} verzióra.", "Frissítés", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
