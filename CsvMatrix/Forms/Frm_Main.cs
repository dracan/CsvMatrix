using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using CsvMatrix.Common;
using System.Linq;

namespace CsvMatrix.Forms
{
    public partial class Frm_Main : Form
    {
        private CsvFile _currentCsv;
        private bool _modified;
        private int _rightClickColumnIndex;
        private string _currentFilename;

        private string CurrentFilename
        {
            get { return _currentFilename; }
            set
            {
                _currentFilename = value;
                Text = value == null ? "CsvMatrix" : "CsvMatrix - " + value;
            }
        }

        public Frm_Main()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateMenuStates();

            Settings.Settings.LoadWindowSettings(this);

            ProcessCommandLineArguments();
        }

        private void ProcessCommandLineArguments()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();

            var filenameToOpen = (from x in commandLineArgs
                                  where !x.StartsWith("-") && !x.StartsWith("/") && !x.Contains(".exe")
                                  select x).FirstOrDefault();

            if(!string.IsNullOrEmpty(filenameToOpen))
            {
                OpenFile(filenameToOpen);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CheckForChanges())
            {
                OpenFile();
            }
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
                OpenFile(ofd.FileName);
            }
        }

        private void OpenFile(string filename)
        {
            var frmCsvProperties = new Frm_CsvProperties(this, false);

            int numColumns;

            string suspectedDelimiter = CsvFile.DetermineDelimiter(filename, out numColumns);

            frmCsvProperties.CsvProperties.Delimiter = suspectedDelimiter;
            frmCsvProperties.CsvProperties.NumColumns = numColumns;

            if(frmCsvProperties.ShowDialog() == DialogResult.OK)
            {
                _currentCsv = new CsvFile(frmCsvProperties.CsvProperties);

                if(_currentCsv.Load(filename))
                {
                    if(_currentCsv.LoadErrors.Count > 0)
                    {
                        var frmErrors = new Frm_Errors("File has been loaded, but the following errors were found:", _currentCsv.LoadErrors);
                        frmErrors.ShowDialog();
                    }

                    dataGridView_Main.DataSource = _currentCsv.DataSource;
                    _modified = false;

                    UpdateMenuStates();
                    UpdateStatusBar();

                    CurrentFilename = filename;
                }
                else
                {
                    if(_currentCsv.LoadErrors.Count > 0)
                    {
                        var frmErrors = new Frm_Errors("File could not be loaded. The following errors were found:", _currentCsv.LoadErrors);
                        frmErrors.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Could not load file, as it appears to be invalid");
                    }
                }
            }
        }

        private bool CheckForChanges()
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
            if(CheckForChanges())
            {
                Close();
            }
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(CheckForChanges())
            {
                _currentCsv = null;
                CurrentFilename = null;
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
            if(CurrentFilename == null)
            {
                SaveAs();
            }
            else
            {
                _currentCsv.Save(CurrentFilename, (from DataGridViewColumn c in dataGridView_Main.Columns
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

                CurrentFilename = saveFileDialog.FileName;

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
                modifyColumnsToolStripMenuItem.Enabled = false;
                gotoCellToolStripMenuItem.Enabled = false;
            }
            else
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                propertiesToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = true;
                modifyColumnsToolStripMenuItem.Enabled = true;
                gotoCellToolStripMenuItem.Enabled = true;
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
            var aboutForm = new Frm_About();
            aboutForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!CheckForChanges())
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

            var frmProperties = new Frm_CsvProperties(this, true) { CsvProperties = _currentCsv.Properties };

            if(frmProperties.ShowDialog() == DialogResult.OK)
            {
                _currentCsv.Properties = frmProperties.CsvProperties;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CheckForChanges())
            {
                NewCsvFile();
            }
        }

        private void NewCsvFile()
        {
            var frmProperties = new Frm_CsvProperties(this, false, new CsvProperties { NumColumns = 10, NumRows = 10 });

            if(frmProperties.ShowDialog() == DialogResult.OK)
            {
                _currentCsv = new CsvFile(frmProperties.CsvProperties);

                _currentCsv.CreateNew(frmProperties.CsvProperties.NumColumns, frmProperties.CsvProperties.NumRows);

                dataGridView_Main.DataSource = _currentCsv.DataSource;
                _modified = false;

                UpdateMenuStates();
                UpdateStatusBar();

                CurrentFilename = null;
            }
        }

        private void modifyColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmColumns = new Frm_Columns
                             {
                                 Columns = (from DataColumn c in _currentCsv.DataSource.Columns
                                            select new ColumnInfo { ColumnName = c.ColumnName }).ToList()
                             };

            if(frmColumns.ShowDialog() == DialogResult.OK)
            {
                var columnsToDelete = new List<DataColumn>();

                var columnIndex = 0;

                foreach(DataColumn column in _currentCsv.DataSource.Columns)
                {
                    if(frmColumns.Columns[columnIndex].Deleted)
                    {
                        columnsToDelete.Add(column);
                    }
                    else
                    {
                        column.ColumnName = frmColumns.Columns[columnIndex].ColumnName;
                    }

                    columnIndex++;
                }

                foreach(var columnToDelete in columnsToDelete)
                {
                    _currentCsv.DataSource.Columns.Remove(columnToDelete);
                }
            }
        }

        private void dataGridView_Main_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip_RightClickColumnHeader.Show(Cursor.Position);

                _rightClickColumnIndex = e.ColumnIndex;
            }
        }

        private void beforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertColumn(_rightClickColumnIndex);
        }

        private void afterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertColumn(_rightClickColumnIndex + 1);
        }

        private void InsertColumn(int columnIndex)
        {
            var frmColumnName = new Frm_ColumnName();

            if(frmColumnName.ShowDialog() == DialogResult.OK)
            {
                _currentCsv.DataSource.Columns.Add(frmColumnName.ColumnName);

                dataGridView_Main.Columns[dataGridView_Main.Columns.Count - 1].DisplayIndex = columnIndex;
            }
        }

        private void deleteColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var columnToDelete = dataGridView_Main.Columns[_rightClickColumnIndex];

            if(MessageBox.Show(String.Format("Are you sure you want to delete column '{0}'? This cannot be undone!", columnToDelete.Name), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _currentCsv.DataSource.Columns.RemoveAt(columnToDelete.Index);
            }
        }

        private void gotoCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmGotoCell = new Frm_GotoCell(dataGridView_Main.Columns.Count - 1, dataGridView_Main.Rows.Count - 1);

            if(frmGotoCell.ShowDialog() == DialogResult.OK)
            {
                dataGridView_Main.CurrentCell = dataGridView_Main.Rows[frmGotoCell.RowIndex].Cells[frmGotoCell.ColumnIndex];
                dataGridView_Main.BeginEdit(true);
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView_Main.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            noneToolStripMenuItem.CheckState = CheckState.Checked;
            fillToolStripMenuItem.CheckState = CheckState.Unchecked;
            autoFitUsingAllCells_ToolStripMenuItem.CheckState = CheckState.Unchecked;
            autoFitUsingVisibleCells_ToolStripMenuItem.CheckState = CheckState.Unchecked;
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView_Main.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            noneToolStripMenuItem.CheckState = CheckState.Unchecked;
            fillToolStripMenuItem.CheckState = CheckState.Checked;
            autoFitUsingAllCells_ToolStripMenuItem.CheckState = CheckState.Unchecked;
            autoFitUsingVisibleCells_ToolStripMenuItem.CheckState = CheckState.Unchecked;
        }

        private void autofitUsingAllCells_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView_Main.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            noneToolStripMenuItem.CheckState = CheckState.Unchecked;
            fillToolStripMenuItem.CheckState = CheckState.Unchecked;
            autoFitUsingAllCells_ToolStripMenuItem.CheckState = CheckState.Checked;
            autoFitUsingVisibleCells_ToolStripMenuItem.CheckState = CheckState.Unchecked;
        }

        private void autofitUsingVisibleCells_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView_Main.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            noneToolStripMenuItem.CheckState = CheckState.Unchecked;
            fillToolStripMenuItem.CheckState = CheckState.Unchecked;
            autoFitUsingAllCells_ToolStripMenuItem.CheckState = CheckState.Unchecked;
            autoFitUsingVisibleCells_ToolStripMenuItem.CheckState = CheckState.Checked;
        }

        private void dataGridView_Main_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void dataGridView_Main_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if(files.Any())
            {
                if(CheckForChanges())
                {
                    OpenFile(files.First());
                }
            }
        }
    }
}
