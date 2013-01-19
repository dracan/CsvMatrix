using System.Collections.Generic;
using System.Windows.Forms;

namespace CsvMatrix.Forms
{
    public partial class Frm_Errors : Form
    {
        public Frm_Errors(string message, IEnumerable<string> errorList)
        {
            InitializeComponent();

            label_Message.Text = message;

            listBox_Errors.DataSource = errorList;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Escape)
            {
                Close();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
