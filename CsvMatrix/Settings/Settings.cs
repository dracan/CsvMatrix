using System.Windows.Forms;

namespace CsvMatrix.Settings
{
    class Settings
    {
        public static void LoadWindowSettings(Form form)
        {
            if(Properties.Settings.Default.WindowPositionSaved)
            {
                form.Left = Properties.Settings.Default.WindowX;
                form.Top = Properties.Settings.Default.WindowY;
                form.Width = Properties.Settings.Default.WindowWidth;
                form.Height = Properties.Settings.Default.WindowHeight;
                form.WindowState = Properties.Settings.Default.WindowMaximised
                                       ? FormWindowState.Maximized
                                       : FormWindowState.Normal;
            }
        }

        public static void SaveWindowSettings(Form form)
        {
            if(form.WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.WindowX = form.RestoreBounds.Left;
                Properties.Settings.Default.WindowY = form.RestoreBounds.Top;
                Properties.Settings.Default.WindowWidth = form.RestoreBounds.Width;
                Properties.Settings.Default.WindowHeight = form.RestoreBounds.Height;
                Properties.Settings.Default.WindowMaximised = true;
            }
            else
            {
                Properties.Settings.Default.WindowX = form.Left;
                Properties.Settings.Default.WindowY = form.Top;
                Properties.Settings.Default.WindowWidth = form.Width;
                Properties.Settings.Default.WindowHeight = form.Height;
                Properties.Settings.Default.WindowMaximised = false;
            }

            Properties.Settings.Default.WindowPositionSaved = true;
            Properties.Settings.Default.Save();
        }
    }
}
