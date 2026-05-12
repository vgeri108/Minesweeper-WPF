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
    /// Interaction logic for SaveGame_CloseDialog.xaml
    /// </summary>
    public partial class SaveGame_CloseDialog : Window
    {
        public bool IsCanceled { get; set; }
        public SaveGame_CloseDialog()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            JsonManager.Game.Save();
            Close();
        }

        private void DontSave_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            JsonManager.Game.DeleteSave();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = true;
            Close();
        }
    }
}
