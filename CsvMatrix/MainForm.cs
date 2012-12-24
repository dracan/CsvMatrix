using System;
using System.Windows.Forms;
using CsvMatrix.Common;

namespace CsvMatrix
{
    public partial class MainForm : Form
    {
        private CsvFile _currentCsv;

        public MainForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
                          {
                              Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*",
                              FilterIndex = 0,
                              RestoreDirectory = true
                          };

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                _currentCsv = new CsvFile(ofd.FileName);

                dataGridView_Main.DataSource = _currentCsv.DataSource;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
