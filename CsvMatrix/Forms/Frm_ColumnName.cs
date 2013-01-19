using System;
using System.Windows.Forms;

namespace CsvMatrix.Forms
{
    public partial class Frm_ColumnName : Form
    {
        public string ColumnName
        {
            get { return textBox_ColumnName.Text; }
        }

        public Frm_ColumnName()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
