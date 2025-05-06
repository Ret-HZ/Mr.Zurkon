using BoltUtils.LOC_DATA;
using Mr.Zurkon.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Mr.Zurkon.UserControls
{
    /// <summary>
    /// Interaction logic for Loc_DataLocalisationContentUC.xaml
    /// </summary>
    public partial class Loc_DataLocalisationContentUC : UserControl
    {
        Localisation localisation;


        public Loc_DataLocalisationContentUC(Localisation localisation)
        {
            InitializeComponent();
            this.localisation = localisation;
            InitEntries();
        }


        public void InitEntries()
        {
            datagrid_Entries.ItemsSource = localisation.Entries;
        }


        private void DataGridCell_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }


        private void datagrid_Entries_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            object rowItem = e.Row.Item;
            int rowIndex = dataGrid.Items.IndexOf(rowItem);

            LocEntry entry = localisation.GetEntry(rowIndex);
            Loc_DataTextEditor txtedit = new Loc_DataTextEditor(entry.Text);
            this.IsEnabled = false;
            Nullable<bool> result = txtedit.ShowDialog();
            if (result == true)
            {
                entry.Text = txtedit.EditedString;
            }
            this.IsEnabled = true;
            e.Cancel = true;

            CollectionViewSource.GetDefaultView(datagrid_Entries.ItemsSource)?.Refresh();
        }
    }
}
