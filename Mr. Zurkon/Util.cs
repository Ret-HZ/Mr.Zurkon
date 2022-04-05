using System;

namespace Mr.Zurkon
{
    public static class Util
    {
        public static string GetAppTitle()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            string name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            string title = String.Format("{0} {1}", name, version);
            return title;
        }
    }
}
