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

        private static bool VanInternet()
        {
            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 200);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public static bool IsNewAvailable()
        {
            if (VanInternet())
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string GH_TagList = client.GetStringAsync("https://raw.githubusercontent.com/vgeri108/Minesweeper-WPF/refs/heads/main/Minesweeper-WPF/VersionTags.txt").Result;
                        List<string> Tags = new List<string>(GH_TagList.Split('\n'));

                        if (Tags.IndexOf(Version.GithubTag) != 0)
                        {
                            for (int i = Tags.IndexOf(Version.GithubTag) -1; i >= 0; i--)
                            {
                                bool IsTagValid = true;
                                try
                                {
                                    string GH_TagDescriptionCheck = client.GetStringAsync($"https://raw.githubusercontent.com/vgeri108/Minesweeper-WPF/refs/tags/{Tags[i]}/Minesweeper-WPF/Version.txt").Result;
                                }
                                catch { IsTagValid = false; }

                                if (IsTagValid)
                                {
                                    NewTags.Insert(0, Tags[i]);
                                    string GH_TagDescription = client.GetStringAsync($"https://raw.githubusercontent.com/vgeri108/Minesweeper-WPF/refs/tags/{Tags[i]}/Minesweeper-WPF/Version.txt").Result;
                                    TagDescriptions.Insert(0,GH_TagDescription);
                                }
                            }
                            return true;
                        }
                        else return false;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }
            else return false;
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
                if (CompareVersion(oldVersion, new[] { 1, 8 }) < 0)
                {
                    Configuration.CurrentTheme = "Default";
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Assets", "Themes", "Frontvonal");
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }



                progress.Close();
                MessageBox.Show($"Az Aknakereső frissült a(z) {Version.GithubTag} verzióra.", "Frissítés", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
