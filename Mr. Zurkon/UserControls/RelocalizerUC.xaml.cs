using BoltUtils.LOC_DATA;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mr.Zurkon.UserControls
{
    /// <summary>
    /// Interaction logic for RelocalizerUC.xaml
    /// </summary>
    public partial class RelocalizerUC : UserControl
    {
        EncodingVariant SelectedEncodingVariant = EncodingVariant.LatinExtended;


        public RelocalizerUC()
        {
            InitializeComponent();
            combobox_Encoding.ItemsSource = Enum.GetValues(typeof(EncodingVariant));
            combobox_Encoding.SelectedItem = SelectedEncodingVariant;
        }


        private async void btn_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LOCALDAT Localization Data (*.bin)|*.bin|" + "All types (*.*)|*.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var metroWindow = (Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow);
                var controller = await metroWindow.ShowProgressAsync("Opening LOC_DATA", "Please wait...");
                controller.SetIndeterminate();

                try
                {
                    await Task.Run(() =>
                    {
                        string filePath = openFileDialog.FileName;
                        LOC_DATA locData = Loc_DataReader.ReadLOC_DATA(filePath, SelectedEncodingVariant);
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            lbl_Version.Content = (BoltUtils.LOC_DATA.Version)locData.Version;
                            Loc_DataContentUC locDataContent = new Loc_DataContentUC(locData, SelectedEncodingVariant);
                            grid_Content.Children.Clear();
                            grid_Content.Children.Add(locDataContent);
                        });
                    });
                }
                catch (Exception ex)
                {
                    await Util.ShowMessageBox($"{ex.Message}", "Error");
                }
                finally
                {
                    await controller.CloseAsync();
                }
            }
        }


        private void combobox_Encoding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_Encoding.SelectedItem is EncodingVariant selected)
            {
                SelectedEncodingVariant = selected;
            }
        }
    }
}
