using BoltUtils.PAK;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mr.Zurkon.UserControls
{
    /// <summary>
    /// Interaction logic for PAKToolUC.xaml
    /// </summary>
    public partial class PAKToolUC : UserControl
    {
        public PAKToolUC()
        {
            InitializeComponent();
        }


        private void btn_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PAK Archives (*.pak)|*.pak|" + "All types (*.*)|*.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                PAK pak = PakReader.ReadPAK(filePath);
                PakContentUC pakContent = new PakContentUC(pak);
                grid_Content.Children.Clear();
                grid_Content.Children.Add(pakContent);
            }
        }
    }
}
