using BoltUtils.PAK;
using BoltUtils.PAK.GIM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Mr.Zurkon.UserControls
{
    /// <summary>
    /// Interaction logic for PakContentUC.xaml
    /// </summary>
    public partial class PakContentUC : UserControl
    {
        //TODO: Rewrite this mess. Code is not exactly pretty but it will do for now.
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
                        Filename = gimdata.Filename,
                        Author = gimdata.Username,
                        Timestamp = gimdata.Timestamp,
                        Origin = gimdata.Originator
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


        /// <summary>
        /// Opens an OpenFileDialog with the specified filters.
        /// </summary>
        /// <param name="filters">The filters to use in the file dialog.</param>
        private void OpenFileDialogGeneric(Action<string> action, params string[] filters)
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
                action(openFileDialog.FileName);
            }
        }


        private void SavePAK(string path)
        {
            PakWriter.WritePAK(path, PakFile);
        }


        private void SaveSelectedFile(string path)
        {
            if (datagrid.SelectedIndex != -1)
            {
                PakFile.ExportFile(datagrid.SelectedIndex, path);
            }
        }


        private void ReplaceSelectedFile(string path)
        {
            int index = datagrid.SelectedIndex;
            if (index != -1)
            {
                byte[] file = File.ReadAllBytes(path);
                PakFile.ReplaceFile(index, file);
                List<FileData> fd = (List<FileData>)datagrid.ItemsSource;
                fd[index].Format = PakFile.GetFileFormat(index).ToString();
                if (PakFile.GetFileFormat(index) == PakContentFormats.Format.GIM)
                {
                    GimData gimdata = PakFile.GetFileData()[index];
                    fd[index].Filename = gimdata.Filename;
                    fd[index].Author = gimdata.Username;
                    fd[index].Timestamp = gimdata.Timestamp;
                    fd[index].Origin = gimdata.Originator;
                }
            }
        }


        private void AddFile(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            PakFile.AddFile(file);
            List<FileData> fd = new List<FileData>((List<FileData>)datagrid.ItemsSource);
            int index = fd.Count;
            if (PakFile.GetFileFormat(index) == PakContentFormats.Format.GIM)
            {
                GimData gimdata = PakFile.GetFileData()[index];
                fd.Add(new FileData()
                {
                    Format = PakContentFormats.Extension[PakFile.GetFileFormat(index)],
                    Filename = gimdata.Filename,
                    Author = gimdata.Username,
                    Timestamp = gimdata.Timestamp,
                    Origin = gimdata.Originator
                }); ;
            }
            else
            {
                fd.Add(new FileData()
                {
                    Format = PakContentFormats.Extension[PakFile.GetFileFormat(index)]
                }); ;
            }
            datagrid.ItemsSource = fd;
        }


        private void btn_save_pak_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialogGeneric(PakFile.GetPAKName(),
                SavePAK,
                "PAK Archives (*.pak)|*.pak",
                "All files (*.*)|*.*");
        }


        private void btn_export_all_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult result = dialog.ShowDialog();
            if (result == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                for(int i=0; i<PakFile.GetPAKFileAmount(); i++)
                {
                    string path = String.Format("{0}/{1}", dialog.FileName, PakFile.GetFilenameForExport(i));
                    PakFile.ExportFile(i, path);
                }
            }
        }


        private void btn_export_selected_Click(object sender, RoutedEventArgs e)
        {
            int index = datagrid.SelectedIndex;
            if (index != -1)
            {
                SaveFileDialogGeneric(PakFile.GetFilenameForExport(index),
                    SaveSelectedFile,
                    "All files (*.*)|*.*");
            }
                
        }


        private void btn_replace_selected_Click(object sender, RoutedEventArgs e)
        {
            int index = datagrid.SelectedIndex;
            if (index != -1)
            {
                OpenFileDialogGeneric(
                ReplaceSelectedFile,
                "All files (*.*)|*.*");
            }
        }


        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialogGeneric(
            AddFile,
            "All files (*.*)|*.*");
        }
    }
}
