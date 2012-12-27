using System.Diagnostics;
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
        }

        private void linkLabel_GithubUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }
    }
}
