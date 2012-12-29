using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using CsvMatrix.Common;
using System.Linq;

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
                UpdateStatusBar();

                _currentFilename = ofd.FileName;
            }
        }

        private bool CheckForChangesBeforeClosing()
        {
            if(_currentCsv != null && _currentCsv.HasChanges)
            {
                switch(MessageBox.Show("Do you want to save your changes?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                    {
                        Save();
                        break;
                    }
                    case DialogResult.No:
                    {
                        return true;
                    }
                    case DialogResult.Cancel:
                    {
                        return false;
                    }
                    default:
                    {
                        throw new InvalidEnumArgumentException();
                    }
                }
            }

            return true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CheckForChangesBeforeClosing())
            {
                Close();
            }
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(CheckForChangesBeforeClosing())
            {
                _currentCsv = null;
                _currentFilename = null;
                dataGridView_Main.DataSource = null;

                UpdateMenuStates();
                UpdateStatusBar();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void Save()
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
                closeToolStripMenuItem.Enabled = false;
            }
            else
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = true;
            }
        }

        private void UpdateStatusBar()
        {
            statusStrip_Main.Items.Clear();

            if(_currentCsv != null)
            {
                statusStrip_Main.Items.Add("Row Count: " + (from DataRow r in _currentCsv.DataSource.Rows
                                                            where r.RowState != DataRowState.Deleted
                                                            select r).Count());

                statusStrip_Main.Items.Add("Column Count: " + _currentCsv.DataSource.Columns.Count);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new About();
            aboutForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!CheckForChangesBeforeClosing())
            {
                e.Cancel = true;
            }
        }

        private void dataGridView_Main_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateStatusBar();
        }

        private void dataGridView_Main_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateStatusBar();
        }
    }
}
