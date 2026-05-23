using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for about.xaml
    /// </summary>
    public partial class about : Window
    {
        public about()
        {
            InitializeComponent();
            FillInfo();
        }

        private void OK_click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FillInfo()
        {
            string user = Environment.UserName;
            string edition = GetWindowsEdition();
            var os = Environment.OSVersion;
            var v = os.Version;
            Uri winIcon = Appearance.Images.windows;
            string winName;

            if (v.Major == 6 && v.Minor == 1)
            {
                winName = "Windows 7";
                winIcon = Appearance.Images.windows7;
            }
            else if (v.Major == 6 && v.Minor == 2)
            {
                winName = "Windows 8";
                winIcon = Appearance.Images.windows8;
            }
            else if (v.Major == 6 && v.Minor == 3)
            {
                winName = "Windows 8.1";
                winIcon = Appearance.Images.windows8;
            }
            else if (v.Major == 10 && v.Build < 22000)
            {
                winName = "Windows 10";
                winIcon = Appearance.Images.windows10;
            }
            else if (v.Major == 10 && v.Build >= 22000)
            {
                winName = "Windows 11";
                winIcon = Appearance.Images.windows11;
            }
            else
            {
                winName = "Ismeretlen Windows";
                winIcon = Appearance.Images.windows;

            }

            Logo.Source = new BitmapImage(winIcon);

            InfoText.Text =
$@"Naquadah-Fusion
Windows: {v.Major +"."+ v.Minor} (build: {v.Build})
Aknakereső: {Version.Game} - GitHub tag: {Version.GithubTag}
© 2026 Naquadah-Fusion. Minden jog fenntartva.
Ez a szoftver nyílt forráskódú, szabadon felhasználható, módosítható és terjeszthető a MIT licenc feltételei szerint.

Aknakereső - Fejlesztő: Naquadah-Fusion

A termék a MIT licenc feltételei alá esik. A termék használatára jogosult: 

{Environment.UserName}";
        }
        string GetWindowsEdition()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            return key?.GetValue("EditionID")?.ToString() ?? "Ismeretlen kiadás";
        }
    }
}
