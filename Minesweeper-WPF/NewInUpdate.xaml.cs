using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Minesweeper_WPF
{
    public partial class NewInUpdate : Window
    {
        Dictionary<string, string> ReleaseDetails = new Dictionary<string, string>();

        public NewInUpdate()
        {
            InitializeComponent();
            LoadTags();

            SearchOnStart.IsChecked = Configuration.AutomaticUpdateSearch;

            Topmost = true;
            Focus();
        }

        private void LoadTags()
        {
            List<string> tags = Update.NewTags;

            foreach (string tag in tags)
            {
                VersionList.Items.Add(tag);
            }
        }

        private void VersionList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (VersionList.SelectedItem == null)
                return;

            DetailsText.Text = Update.TagDescriptions[Update.NewTags.IndexOf(VersionList.SelectedItem.ToString())];
        }

        private void SearchOnStart_Checked(object sender, RoutedEventArgs e)
        {
            Configuration.AutomaticUpdateSearch = true;
            JsonManager.Settings.Save();
        }

        private void SearchOnStart_Unchecked(object sender, RoutedEventArgs e)
        {
            Configuration.AutomaticUpdateSearch = false;
            JsonManager.Settings.Save();
        }

        private async void Install_Click(object sender, RoutedEventArgs e)
        {
            Close();
            await Update.Install();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
