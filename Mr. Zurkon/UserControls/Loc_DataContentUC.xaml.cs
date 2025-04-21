using BoltUtils.LOC_DATA;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Mr.Zurkon.UserControls
{
    /// <summary>
    /// Interaction logic for Loc_DataContentUC.xaml
    /// </summary>
    public partial class Loc_DataContentUC : UserControl
    {

        LOC_DATA locdata;

        public Loc_DataContentUC(LOC_DATA locdata)
        {
            InitializeComponent();
            this.locdata = locdata;
            InitTabs();
        }


        private void InitTabs()
        {
            for(int i=0; i<locdata.GetLocalisationAmount(); i++)
            {
                TabItem tab = new TabItem();
                tab.Header = string.Format("Loc {0}", i+1);
                tab.Content = new Loc_DataLocalisationContentUC(locdata.GetLocalisation(i));
                tabcontrol_localisations.Items.Add(tab);
            }
        }


        private void btn_save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string dialogFilter = "LOCALDAT Localization Data (*.bin)|*.bin|All files(*.*)| *.*";
            saveFileDialog.Filter = dialogFilter;
            saveFileDialog.FileName = "LOCALDAT.BIN";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    Loc_DataWriter.WriteLOC_DATA(saveFileDialog.FileName, locdata);
                    var metroWindow = (Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow);
                    metroWindow.ShowMessageAsync("", "LOC_DATA saved!");
                } catch (Exception ex)
                {
                    MessageBox.Show(String.Format("An error occurred:\n{0}\n{1}", ex.Message, ex.StackTrace), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void btn_exportcsv_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new OpenFolderDialog
            {
                Title = "Choose where to extract the contents",
            };

            if (openFolderDialog.ShowDialog() == true)
            {
                try
                {
                    for (int i = 0; i < locdata.GetLocalisationAmount(); i++)
                    {
                        string path = String.Format("{0}/Loc{1}.csv", openFolderDialog.FolderName, i+1);
                        string text = locdata.GetLocalisationDataAsCSV(i);
                        File.WriteAllText(path, text, Encoding.GetEncoding(28591));
                    }
                    var metroWindow = Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow;
                    metroWindow.ShowMessageAsync("", "LOC_DATA exported as CSV!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("An error occurred:\n{0}\n{1}", ex.Message, ex.StackTrace), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
