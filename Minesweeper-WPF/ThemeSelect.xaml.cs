using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
using System.IO;
using System.Text.Json;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for ThemeSelect.xaml
    /// </summary>
    public partial class ThemeSelect : Window
    {
        private string StartTheme;

        public ThemeSelect()
        {
            InitializeComponent();
            StartTheme = Configuration.CurrentTheme;
            Load();
        }

        private void Load()
        {
            SelectedBoardIcon.Source = new BitmapImage(Appearance.Images.Board);
            SelectedMineIcon.Source = new BitmapImage(Appearance.Images.Mines);
            SelectedNumberIcon.Source = new BitmapImage(Appearance.Images.Numbers);
            SelectedBackgroundIcon.Source = new BitmapImage(Appearance.Images.Background);

            BoardSelected.Text = Appearance.Images.ImageNames["ThemeName"];
            MinesSelected.Text = Appearance.Images.ImageNames["ThemeName"];
            NumbersSelected.Text = Appearance.Images.ImageNames["ThemeName"];
            BackgroundSelected.Text = Appearance.Images.ImageNames["ThemeName"];

            ThemeName.Text = "Téma neve: " + Appearance.Images.ImageNames["ThemeName"];
            ThemeCreator.Text = "Téma készítője: " + Appearance.Images.ImageNames["Creator"];
            ThemeDescription.Text = "Téma leírása: " + Appearance.Images.ImageNames["Description"];

            ThemeList.Children.Clear();
            foreach (string item in ThemeFinder.GetThemeList())
            {
                Image img = new Image
                {
                    Source = new BitmapImage(Appearance.Images.ResolveThemeUri(item, ThemeFinder.GetThemeImageFromJson(item))),
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(0, 0, 0, 5),
                };

                TextBlock text = new TextBlock
                {
                    Text = ThemeFinder.GetThemeNameFromJson(item),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                StackPanel panel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                panel.Children.Add(img);
                panel.Children.Add(text);

                Button btn = new Button
                {
                    Content = panel,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    Background = null,
                    BorderThickness = new Thickness(0),
                    Tag = item
                };
                btn.Click += ThemeButton_Click;

                ThemeList.Children.Add(btn);
            }
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag is string themeName)
            {
                Configuration.CurrentTheme = themeName;
                JsonManager.Theme.Load();
                Load();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            JsonManager.Settings.Save();
            JsonManager.Style.Save();
            Close();
        }

        private void Canel_Click(object sender, RoutedEventArgs e)
        {
            Configuration.CurrentTheme = StartTheme;
            JsonManager.Settings.Save();
            JsonManager.Theme.Load();
            Close();
        }

        //private void Board_Click(object sender, RoutedEventArgs e)
        //{
        //    AdvancedThemeSelect advancedThemeSelect = new AdvancedThemeSelect("Board");
        //    ShowInTaskbar = false;
        //    advancedThemeSelect.ShowDialog();
        //    ShowInTaskbar = true;
        //}

        //private void Mines_Click(object sender, RoutedEventArgs e)
        //{
        //    AdvancedThemeSelect advancedThemeSelect = new AdvancedThemeSelect("Mines");
        //    ShowInTaskbar = false;
        //    advancedThemeSelect.ShowDialog();
        //    ShowInTaskbar = true;
        //}

        //private void Numbers_Click(object sender, RoutedEventArgs e)
        //{
        //    AdvancedThemeSelect advancedThemeSelect = new AdvancedThemeSelect("Numbers");
        //    ShowInTaskbar = false;
        //    advancedThemeSelect.ShowDialog();
        //    ShowInTaskbar = true;
        //}

        //private void Background_Click(object sender, RoutedEventArgs e)
        //{
        //    AdvancedThemeSelect advancedThemeSelect = new AdvancedThemeSelect("Background");
        //    ShowInTaskbar = false;
        //    advancedThemeSelect.ShowDialog();
        //    ShowInTaskbar = true;
        //}
    }
}
