using LOC_DATALib;
using Mr.Zurkon.Windows;
using System;
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
                if (dataGridCellTarget.Content.GetType() == typeof(TextBlock)) // This happens as a result of double right click
                {
                    dataGridCellTarget.IsEditing = false;
                    return;
                }
                TextBox tb = (TextBox)dataGridCellTarget.Content;
                Loc_DataTextEditor txtedit = new Loc_DataTextEditor(tb.Text);
                this.IsEnabled = false;
                Nullable<bool> result = txtedit.ShowDialog();
                if (result == true)
                {
                    tb.Text = txtedit.EditedString;
                    System.Windows.Data.BindingExpression be = tb.GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }

                dataGridCellTarget.IsEditing = false;
                this.IsEnabled = true;

            }
        }
    }
}
