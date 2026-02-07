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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Minesweeper_WPF
{
    /// <summary>
    /// Interaction logic for AdvancedThemeSelect.xaml
    /// </summary>
    public partial class AdvancedThemeSelect : Window
    {
        private string StartTheme = Configuration.CurrentTheme;
        private Dictionary<string, string> StartThemeSet = Appearance.Images.ImageNames;
        private string SettingsType;
        public AdvancedThemeSelect(string SettingsType)
        {
            InitializeComponent();
            this.SettingsType = SettingsType;
            Load();
        }
        private void Load()
        {
            ThemeList.Children.Clear();
            foreach (string item in ThemeFinder.GetThemeList())
            {
                Image img = new Image
                {
                    Source = new BitmapImage(Appearance.Images.ResolveThemeUri(item, Appearance.Images.ImageNames[SettingsType])),
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(0, 0, 0, 5),
                };

                TextBlock text = new TextBlock
                {
                    Text = item,
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

                Dictionary<string, string> tmp = Appearance.Images.ImageNames;
                Configuration.CurrentTheme = themeName;
                JsonManager.Theme.Load();
                string ValueToKeep = Appearance.Images.ImageNames[SettingsType];
                Configuration.CurrentTheme = StartTheme;
                Appearance.Images.ImageNames = tmp;

                Appearance.Images.ImageNames[SettingsType] = ValueToKeep;
                Load();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            JsonManager.Style.Save();
            Close();
        }

        private void Canel_Click(object sender, RoutedEventArgs e)
        {
            Appearance.Images.ImageNames = StartThemeSet;
            Close();
        }
    }
}
