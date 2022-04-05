using MahApps.Metro.Controls;
using Microsoft.Win32;
using Mr.Zurkon.UserControls;
using PAKLib;
using System;
using System.IO;
using System.Windows;
using Yarhl.IO;

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
        }


        private void OpenLOC_DATA(string path)
        {
            throw new NotImplementedException();
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
            using (var stream = DataStreamFactory.FromFile(path, FileOpenMode.Read))
            {
                var reader = new DataReader(stream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };


                string magic = reader.ReadString(4);
                switch (magic)
                {
                    case "PAKK": OpenPAK(path); break;
                    case "LOC_": OpenLOC_DATA(path); break;
                    default: MessageBox.Show("Format not supported", "Error", MessageBoxButton.OK, MessageBoxImage.Error); break;
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
                "PAK Archives (*.pak*)|*.pak*",
                "LOCALDAT Localization Data (*.bin*)|*.bin*"
                );
        }

        private void MenuOpenFileDialogPAK(object sender, RoutedEventArgs e)
        {
            OpenFileDialogGeneric("PAK Archives (*.pak*)|*.pak*");
        }

        private void MenuOpenFileDialogLOCALDAT(object sender, RoutedEventArgs e)
        {
            OpenFileDialogGeneric("LOCALDAT Localization Data (*.bin*)|*.bin*");
        }
    }
}
