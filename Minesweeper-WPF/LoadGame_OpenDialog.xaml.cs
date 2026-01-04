using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for LoadGame_OpenDialog.xaml
    /// </summary>
    public partial class LoadGame_OpenDialog : Window
    {
        public LoadGame_OpenDialog()
        {
            InitializeComponent();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            JsonManager.Game.Load();
            Close();
        }

        private void DontContinue_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                File.Delete("LastSave.mine");
            }
            catch (Exception ex) { }
            
            Close();
        }
    }
}
