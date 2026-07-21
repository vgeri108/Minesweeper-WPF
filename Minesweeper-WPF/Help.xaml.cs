using System;
using System.Collections.Generic;
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
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
            BoardShowcase.Source = new BitmapImage(Appearance.Images.HelpWindowBoard);
        }

        private async void JatekBeallitasModositas_Click(object sender, RoutedEventArgs e)
        {
            Inditas.IsExpanded = false;
            Mentes.IsExpanded = false;
            JatekBeallitasok.IsExpanded = true;
            Megjelenes.IsExpanded = false;

            JatekBeallitasok.BringIntoView();

            await HighlightExpander(JatekBeallitasok);
        }

        private async Task HighlightExpander(Expander expander)
        {
            Brush eredetiSzin = expander.Foreground;
            FontWeight eredetiBetu = expander.FontWeight;

            for (int i = 0; i < 3; i++)
            {
                expander.Foreground = Brushes.OrangeRed;
                expander.FontWeight = FontWeights.ExtraBold;

                await Task.Delay(200);

                expander.Foreground = eredetiSzin;
                expander.FontWeight = eredetiBetu;

                await Task.Delay(200);
            }
        }

        private void EgyediTema_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ez a súgótéma még nem érhető el.", "Súgó", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
