using System;
using System.Windows.Forms;
using CsvMatrix.Common;

namespace CsvMatrix
{
    public partial class MainForm : Form
    {
        private CsvFile _currentCsv;
        private string _currentFilename;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateMenuStates();
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

                UpdateMenuStates();

                _currentFilename = ofd.FileName;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_currentFilename == null)
            {
                SaveAs();
            }
            else
            {
                _currentCsv.Save(_currentFilename);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog
                                     {
                                        Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*",
                                        FilterIndex = 0,
                                        RestoreDirectory = true
                                     };

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _currentCsv.Save(saveFileDialog.FileName);
                _currentFilename = saveFileDialog.FileName;
            }
        }

        private void UpdateMenuStates()
        {
            if(_currentCsv == null)
            {
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
            }
            else
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
        }
    }
}
