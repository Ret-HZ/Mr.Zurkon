using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Mr.Zurkon.UserControls;
using Mr.Zurkon.Windows;
using System.Windows;

namespace Mr.Zurkon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = $"{Util.GetAssemblyProductName()} {Util.GetAssemblyVersion()}";
            tab_Tools_PAKTool.Content = new PAKToolUC();
            tab_Tools_Relocalizer.Content = new RelocalizerUC();
        }


        private void MenuOpenAboutMrZurkon(object sender, RoutedEventArgs e)
        {
            About_MrZurkon aboutWindow = new About_MrZurkon();
            aboutWindow.Show();
        }


        private void MenuOpenAboutPAKTool(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("About PAKTool", "PAKTool can open, preview, edit and save .PAK files.\n\n" +
                "PAKs usually contain GIM images/textures, although they can also contain models (PS2).");
        }


        private void MenuOpenAboutRelocalizer(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("About Relocalizer", "Relocalizer can open, preview, edit and save Localization Data (LOC_DATA) files.\n\n" +
                "LOC_DATA files contain every text string used in the game, with all their localizations to different supported languages.");
        }
    }
}
