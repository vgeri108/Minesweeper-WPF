using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : Window
    {
        public Progress(string title = "Folyamat", string message = "Kérjük várjon...")
        {
            InitializeComponent();
            Title = title;
            MessageBlock.Text = message;
            DataContext = this;
            this.Title = title;
        }
    }
}
