using System.Windows.Forms;
using CsvMatrix.Common;

namespace CsvMatrix
{
    public partial class Frm_CsvProperties : Form
    {
        public CsvProperties CsvProperties { get; set; }

        public Frm_CsvProperties(CsvProperties properties = null)
        {
            InitializeComponent();

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
            // Setup defaults

            checkBox_HeaderRow.Checked = true;

            switch(CsvProperties.Delimiter)
            {
                case ",":   radioButton_Comma.Checked = true; break;
                case "\t":  radioButton_Tab.Checked = true; break;
                case ";":   radioButton_Semicolon.Checked = true; break;
                case "|":   radioButton_Pipe.Checked = true; break;
                default:    radioButton_Other.Checked = true; break;
            }
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
