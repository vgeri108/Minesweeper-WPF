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

            string winName;

            if (v.Major == 6 && v.Minor == 1)
                winName = "Windows 7";
            else if (v.Major == 6 && v.Minor == 2)
                winName = "Windows 8";
            else if (v.Major == 6 && v.Minor == 3)
                winName = "Windows 8.1";
            else if (v.Major == 10 && v.Build < 22000)
                winName = "Windows 10";
            else if (v.Major == 10 && v.Build >= 22000)
                winName = "Windows 11";
            else
                winName = "Ismeretlen Windows";

            InfoText.Text =
$@"Microsoft Windows
Verzió:{" "+v.Major +"."+ v.Minor} (build: {v.Build})
© 2025 Microsoft Corporation. Minden jog fenntartva.
A {winName} {edition} operációs rendszert és felhasználói felületét védjegyek, továbbá oltalom alatt álló vagy bejegyzett szellemi tulajdonjogok védik az Egyesült Államokban, illetve más országokban vagy régiókban.

Aknakereső - Fejlesztő: Oberon Games és Microsoft Corporation, a Microsoft Corporation megbízásából.

A termék a Microsoft szoftverlicenc-szerződés hatálya alá esik. A termék használatára jogosult: 

{Environment.UserName}";
        }
        string GetWindowsEdition()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            return key?.GetValue("EditionID")?.ToString() ?? "Ismeretlen kiadás";
        }
    }
}
