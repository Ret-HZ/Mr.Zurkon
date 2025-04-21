using BoltUtils.LOC_DATA;
using BoltUtils.PAK;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Mr.Zurkon.UserControls;
using Mr.Zurkon.Windows;
using System;
using System.IO;
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
            this.Title = Util.GetAppTitle();
        }


        private void OpenPAK(string path)
        {
            PAK pak = PakReader.ReadPAK(path);
            PakContentUC pakcontent = new PakContentUC(pak);
            tab_tools_pak.Content = pakcontent;
            tabcontrol_tools.SelectedIndex = 0;
        }


        private void OpenLOC_DATA(string path)
        {
            LOC_DATA locdata = Loc_DataReader.ReadLOC_DATA(path);
            Loc_DataContentUC locdatacontent = new Loc_DataContentUC(locdata);
            tab_tools_locdata.Content = locdatacontent;
            tabcontrol_tools.SelectedIndex = 1;
        }


        /// <summary>
        /// Opens an OpenFileDialog with the specified filters.
        /// </summary>
        /// <param name="filters">The filters to use in the file dialog.</param>
        private void OpenFileDialogGeneric(params string[] filters)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string dialogFilter = "";
            foreach (string filter in filters)
            {
                dialogFilter += filter;
                if (filter != filters[filters.Length - 1]) dialogFilter += "|";
            }
            openFileDialog.Filter = dialogFilter;
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                // Open file 
                string filePath = openFileDialog.FileName;
                string extension = Path.GetExtension(filePath);

                switch (extension)
                {
                    case ".PAK": OpenPAK(filePath); break;
                    default: CheckMagic(filePath); break;
                }
            }
        }


        /// <summary>
        /// Checks the magic of the file to determine the format.
        /// </summary>
        /// <param name="path"></param>
        private void CheckMagic(string path)
        {
            using (Stream stream = new MemoryStream(File.ReadAllBytes(path)))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int magicBytes = reader.ReadInt32();
                switch (magicBytes)
                {
                    case 1263223120: // PAKK
                        {
                            OpenPAK(path);
                            break;
                        }

                    case 1598246732: // LOC_
                        {
                            OpenLOC_DATA(path);
                            break;
                        }

                    default:
                        {
                            MessageBox.Show("Format not supported", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                }
            }
        }


        private void grid_main_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) CheckMagic(file);
        }


        private void MenuOpenFileDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialogGeneric(
                "All files (*.*)|*.*", 
                "PAK Archives (*.pak)|*.pak",
                "LOCALDAT Localization Data (*.bin)|*.bin"
                );
        }

        private void MenuOpenFileDialogPAK(object sender, RoutedEventArgs e)
        {
            OpenFileDialogGeneric("PAK Archives (*.pak)|*.pak");
        }

        private void MenuOpenFileDialogLOCALDAT(object sender, RoutedEventArgs e)
        {
            OpenFileDialogGeneric("LOCALDAT Localization Data (*.bin)|*.bin");
        }

        private void MenuOpenAboutMrZurkon(object sender, RoutedEventArgs e)
        {
            About_MrZurkon amz = new About_MrZurkon();
            amz.Show();
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
