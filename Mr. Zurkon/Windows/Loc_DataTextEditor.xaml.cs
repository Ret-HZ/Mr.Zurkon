using MahApps.Metro.Controls;
using System;

namespace Mr.Zurkon.Windows
{
    /// <summary>
    /// Interaction logic for Loc_DataTextEditor.xaml
    /// </summary>
    public partial class Loc_DataTextEditor : MetroWindow
    {
        public string Text { get; set; }

        public string EditedString;


        public Loc_DataTextEditor(string text)
        {
            InitializeComponent();
            this.Title = "LOC Editor";
            DataContext = this;
            this.Text = String.Copy(text).Replace("\0", string.Empty);
        }


        private void btn_save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EditedString = String.Copy(txt_edit.Text);
            this.DialogResult = true;
            this.Close();
        }

        private void btn_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        private void InsertText (int position, string text)
        {
            txt_edit.Text = txt_edit.Text.Insert(position, text);
        }

        private void InsertText(string text)
        {
            txt_edit.Text = txt_edit.Text.Insert(txt_edit.CaretIndex, text);
        }


        private void InsertColorTags(string text)
        {
            int selectionStart = txt_edit.SelectionStart;
            InsertText(selectionStart + txt_edit.SelectionLength, "</text_color>");
            InsertText(selectionStart, text);
        }

        private void btn_button_x_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=x/>");
        }

        private void btn_button_square_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=square/>");
        }

        private void btn_button_triangle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=triangle/>");
        }

        private void btn_button_circle_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=circle/>");
        }

        private void btn_button_joystick_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=joystick/>");
        }

        private void btn_button_dpad_up_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=d-pad_up/>");
        }

        private void btn_button_dpad_down_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=d-pad_down/>");
        }

        private void btn_button_dpad_left_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=d-pad_left/>");
        }

        private void btn_button_dpad_right_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=d-pad_right/>");
        }

        private void btn_button_dpad_all_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=d-pad_all/>");
        }

        private void btn_button_volume_up_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=volume_up/>");
        }

        private void btn_button_volume_down_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=volume_down/>");
        }

        private void btn_button_l_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=l/>");
        }

        private void btn_button_r_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=r/>");
        }

        private void btn_button_select_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=select/>");
        }

        private void btn_button_start_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=start/>");
        }

        private void btn_button_home_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<button=home/>");
        }

        private void btn_symbol_ellipsis_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<symbol=ellipsis/>");
        }

        private void btn_symbol_registered_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertText("<symbol=registered/>");
        }

        private void btn_color_orange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=orange>");
        }

        private void btn_color_red_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=red>");
        }

        private void btn_color_magenta_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=magenta>");
        }

        private void btn_color_pink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=pink>");
        }

        private void btn_color_cyan_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=cyan>");
        }

        private void btn_color_salmon_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=salmon>");
        }

        private void btn_color_blue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=blue>");
        }

        private void btn_color_green_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=green>");
        }

        private void btn_color_purple_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=purple>");
        }

        private void btn_color_brown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=brown>");
        }

        private void btn_color_white_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=white>");
        }

        private void btn_color_yellow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertColorTags("<text_color=yellow>");
        }
    }
}
