﻿using MahApps.Metro.Controls;

namespace Mr.Zurkon.Windows
{
    /// <summary>
    /// Interaction logic for About_MrZurkon.xaml
    /// </summary>
    public partial class About_MrZurkon : MetroWindow
    {
        public About_MrZurkon()
        {
            InitializeComponent();
            txt_version.Text = $"v{Util.GetAssemblyVersion()}";
        }


        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", e.Uri.ToString());
        }
    }
}
