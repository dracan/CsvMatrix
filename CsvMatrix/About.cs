using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace CsvMatrix
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void About_Load(object sender, System.EventArgs e)
        {
            var link = new LinkLabel.Link
                       {
                           LinkData = "https://github.com/dracan/CsvMatrix"
                       };

            linkLabel_GithubUrl.Links.Add(link);

            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            label_VersionNumber.Text = string.Format("Version {0}.{1}.{2}",
                                                     fileVersionInfo.FileMajorPart,
                                                     fileVersionInfo.FileMinorPart,
                                                     fileVersionInfo.FileBuildPart);
        }

        private void linkLabel_GithubUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }
    }
}
