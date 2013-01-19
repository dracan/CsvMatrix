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
        private bool _modified;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateMenuStates();

            Settings.Settings.LoadWindowSettings(this);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            var ofd = new OpenFileDialog
                          {
                              Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*",
                              FilterIndex = 0,
                              RestoreDirectory = true
                          };

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                var frmCsvProperties = new Frm_CsvProperties(false);

                int numColumns;

                string suspectedDelimiter = CsvFile.DetermineDelimiter(ofd.FileName, out numColumns);

                frmCsvProperties.CsvProperties.Delimiter = suspectedDelimiter;
                frmCsvProperties.CsvProperties.NumColumns = numColumns;

                if(frmCsvProperties.ShowDialog() == DialogResult.OK)
                {
                    _currentCsv = new CsvFile(frmCsvProperties.CsvProperties);
                    _currentCsv.Load(ofd.FileName);

                    dataGridView_Main.DataSource = _currentCsv.DataSource;
                    _modified = false;

                    UpdateMenuStates();
                    UpdateStatusBar();

                    _currentFilename = ofd.FileName;
                }
            }
        }

        private bool CheckForChangesBeforeClosing()
        {
            if(_currentCsv != null && (_currentCsv.HasChanges || _modified))
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
                _modified = false;

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
                _currentCsv.Save(_currentFilename, (from DataGridViewColumn c in dataGridView_Main.Columns
                                                    orderby c.DisplayIndex
                                                    select c.Index).ToList());

                _modified = false;
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
                _currentCsv.Save(saveFileDialog.FileName, (from DataGridViewColumn c in dataGridView_Main.Columns
                                                           orderby c.DisplayIndex
                                                           select c.Index).ToList());

                _currentFilename = saveFileDialog.FileName;

                _modified = false;
            }
        }

        private void UpdateMenuStates()
        {
            if(_currentCsv == null)
            {
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                propertiesToolStripMenuItem.Enabled = false;
                closeToolStripMenuItem.Enabled = false;
            }
            else
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                propertiesToolStripMenuItem.Enabled = true;
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

            Settings.Settings.SaveWindowSettings(this);
        }

        private void dataGridView_Main_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateStatusBar();
        }

        private void dataGridView_Main_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateStatusBar();
        }

        private void dataGridView_Main_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            _modified = true;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentCsv.UpdateProperties();

            var frmProperties = new Frm_CsvProperties(true) { CsvProperties = _currentCsv.Properties };

            if(frmProperties.ShowDialog() == DialogResult.OK)
            {
                _currentCsv.Properties = frmProperties.CsvProperties;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewCsvFile();
        }

        private void NewCsvFile()
        {
            var frmProperties = new Frm_CsvProperties(false, new CsvProperties {NumColumns = 10, NumRows = 10});

            if(frmProperties.ShowDialog() == DialogResult.OK)
            {
                _currentCsv = new CsvFile(frmProperties.CsvProperties);

                _currentCsv.CreateNew(frmProperties.CsvProperties.NumColumns, frmProperties.CsvProperties.NumRows);

                dataGridView_Main.DataSource = _currentCsv.DataSource;
                _modified = false;

                UpdateMenuStates();
                UpdateStatusBar();

                _currentFilename = null;
            }
        }
    }
}
