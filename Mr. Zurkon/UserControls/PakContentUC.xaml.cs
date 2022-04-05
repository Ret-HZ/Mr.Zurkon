using Microsoft.Win32;
using PAKLib;
using PAKLib.GIM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Mr.Zurkon.UserControls
{
    /// <summary>
    /// Interaction logic for PakContentUC.xaml
    /// </summary>
    public partial class PakContentUC : UserControl
    {

        PAK PakFile;

        public PakContentUC(PAK pakFile)
        {
            InitializeComponent();
            this.PakFile = pakFile;
            PopulateGrid(PakFile.GetFileFormats(), PakFile.GetFileData());
        }


        private void PopulateGrid(List<PakContentFormats.Format> formats, List<GimData> filedata)
        {
            List<FileData> data = new List<FileData>();

            for(int i=0; i<formats.Count; i++)
            {
                if (formats[i] == PakContentFormats.Format.GIM)
                {
                    GimData gimdata = filedata[i];
                    data.Add(new FileData()
                    {
                        Format = PakContentFormats.Extension[formats[i]],
                        Filename = gimdata.filename,
                        Author = gimdata.username,
                        Timestamp = gimdata.timestamp,
                        Origin = gimdata.originator
                    });;
                }

                else
                {
                    data.Add(new FileData()
                    {
                        Format = PakContentFormats.Extension[formats[i]]
                    }); ;
                }
            }

            datagrid.ItemsSource = data;
        }


        public class FileData
        {
            [ColumnName("Format")]
            public string Format { get; set; }
            [ColumnName("Filename")]
            public string Filename { get; set; }
            [ColumnName("Author")]
            public string Author { get; set; }
            [ColumnName("Timestamp")]
            public string Timestamp { get; set; }
            [ColumnName("Originator")]
            public string Origin { get; set; }
        }


        public class ColumnNameAttribute : Attribute
        {
            public ColumnNameAttribute(string Name) { this.Name = Name; }
            public string Name { get; set; }
        }


        private void datagrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var desc = e.PropertyDescriptor as PropertyDescriptor;
            var att = desc.Attributes[typeof(ColumnNameAttribute)] as ColumnNameAttribute;
            if (att != null)
            {
                e.Column.Header = att.Name;
            }
        }

        //TODO SAVE DIALOG FILENAME GETS TAKEN FROM THE PAK THEN PASSED TO THE SAVE FUNCTION
        private void SaveFileDialogGeneric(string filename, Action<string> action, params string[] filters)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string dialogFilter = "";
            foreach (string filter in filters)
            {
                dialogFilter += filter;
                if (filter != filters[filters.Length - 1]) dialogFilter += "|";
            }
            saveFileDialog.Filter = dialogFilter;
            saveFileDialog.FileName = filename;
            if (saveFileDialog.ShowDialog() == true)
            {
                action(saveFileDialog.FileName);
            }
        }


        private void SavePAK(string path)
        {
            PakWriter.WritePAK(path, PakFile);
        }


        private void btn_save_pak_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialogGeneric(PakFile.GetPAKName(),
                SavePAK,
                "PAK Archives (*.pak*)|*.pak*",
                "All files (*.*)|*.*");
        }
    }
}
