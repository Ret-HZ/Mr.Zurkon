using LOC_DATALib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            datagrid_entries.ItemsSource = localisation.Entries;
        }


        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;
            
            if(dataGridCellTarget.Column.DisplayIndex == 1) //Text
            {
                //TODO
                MessageBox.Show(dataGridCellTarget.Content.ToString());
                TextBox tb = (TextBox)dataGridCellTarget.Content;
                tb.Text = "test12345";
                dataGridCellTarget.IsEditing = false;

                this.IsEnabled = false;
            }
        }
    }
}
