using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Mr.Zurkon
{
    public static class Util
    {
        /// <summary>
        /// Gets the product name of the assembly according to the <see cref="AssemblyProductAttribute"/>.
        /// </summary>
        /// <returns>The product name.</returns>
        public static string GetAssemblyProductName()
        {
            return Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyProductAttribute>().FirstOrDefault().Product;
        }


        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        /// <returns>The assembly version.</returns>
        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        public static Task<MessageDialogResult> ShowMessageBox(string message, string title = "", MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            return firstMetroWindow.ShowMessageAsync(title, message, style, settings);
        }


        public static Task<MessageDialogResult> ShowMessageBox(DependencyObject dependencyObject, string message, string title = "", MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            Window window = Window.GetWindow(dependencyObject);
            MetroWindow metroWindow = window as MetroWindow;
            return metroWindow.ShowMessageAsync(title, message, style, settings);
        }
    }
}
