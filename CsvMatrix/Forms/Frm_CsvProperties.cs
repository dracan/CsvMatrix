using System.Windows.Forms;
using CsvMatrix.Common;

namespace CsvMatrix.Forms
{
    public partial class Frm_CsvProperties : Form
    {
        public CsvProperties CsvProperties { get; set; }
        private readonly bool _isAlreadyLoaded;
        private Frm_Main _parentForm;

        public Frm_CsvProperties(Frm_Main parentForm, bool isAlreadyLoaded, CsvProperties properties = null)
        {
            InitializeComponent();

            _isAlreadyLoaded = isAlreadyLoaded;
            _parentForm = parentForm;

            if(properties == null)
            {
                CsvProperties = new CsvProperties();
            }
            else
            {
                CsvProperties = (CsvProperties)properties.Clone();
            }
        }

        private void Frm_CsvProperties_Load(object sender, System.EventArgs e)
        {
            // For some reason, the form's "StartPosition" property to CenterParent wasn't working when this OpenFile was called from the
            // drag and drop event handler (dataGridView_Main_DragDrop). Instead the CsvProperties form was appearing in the top, left of
            // the screen. So instead, manually calculate the centre parent position.
            Left = (_parentForm.Left + _parentForm.Width / 2) - (Width / 2);
            Top = (_parentForm.Top + _parentForm.Height / 2) - (Height / 2);

            // Setup defaults

            checkBox_HeaderRow.Checked = CsvProperties.HasHeaderRow;

            switch(CsvProperties.Delimiter)
            {
                case ",": radioButton_Comma.Checked = true; break;
                case "\t": radioButton_Tab.Checked = true; break;
                case ";": radioButton_Semicolon.Checked = true; break;
                case "|": radioButton_Pipe.Checked = true; break;
                default: radioButton_Other.Checked = true; break;
            }

            numericUpDown_NumRows.Value = CsvProperties.NumRows;
            numericUpDown_NumColumns.Value = CsvProperties.NumColumns;

            if(radioButton_Other.Checked)
            {
                textBox_Other.Enabled = radioButton_Other.Checked;
                textBox_Other.Text = CsvProperties.Delimiter;
            }

            // Disable textboxes if number of rows is 0. This happens when opening a CSV. We only want to be able to edit this
            // when viewing the properties of an already loaded CSV.
            numericUpDown_NumColumns.Enabled = (CsvProperties.NumRows > 0);
            numericUpDown_NumRows.Enabled = (CsvProperties.NumRows > 0);
        }

        private void button_Ok_Click(object sender, System.EventArgs e)
        {
            if(radioButton_Comma.Checked)
            {
                CsvProperties.Delimiter = ",";
            }
            else if(radioButton_Tab.Checked)
            {
                CsvProperties.Delimiter = "\t";
            }
            else if(radioButton_Semicolon.Checked)
            {
                CsvProperties.Delimiter = ";";
            }
            else if(radioButton_Pipe.Checked)
            {
                CsvProperties.Delimiter = "|";
            }
            else if(radioButton_Other.Checked)
            {
                CsvProperties.Delimiter = textBox_Other.Text;
            }

            CsvProperties.HasHeaderRow = checkBox_HeaderRow.Checked;

            var newRowCount = (int)numericUpDown_NumRows.Value;
            var newColumnCount = (int)numericUpDown_NumColumns.Value;

            var croppingColumns = newColumnCount < CsvProperties.NumColumns;
            var croppingRows = newRowCount < CsvProperties.NumRows;

            if(_isAlreadyLoaded && (croppingColumns || croppingRows))
            {
                string subMessage = "";

                if(croppingColumns)
                {
                    subMessage = "columns";

                    if(croppingRows)
                    {
                        subMessage += " and rows";
                    }
                }
                else
                {
                    subMessage += "rows";
                }

                if(MessageBox.Show(
                    "The number of " + subMessage + " have been reduced, this will cause loss of data! Are you sure you want to do this?",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            CsvProperties.HasHeaderRow = checkBox_HeaderRow.Checked;
            CsvProperties.NumColumns = newColumnCount;
            CsvProperties.NumRows = newRowCount;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button_Cancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void radioButton_Other_CheckedChanged(object sender, System.EventArgs e)
        {
            // Only enable the 'other' textbox if the 'other' radio button is checked
            textBox_Other.Enabled = radioButton_Other.Checked;
        }
    }
}
