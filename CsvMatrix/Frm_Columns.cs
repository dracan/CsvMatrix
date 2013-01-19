using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CsvMatrix
{
    public partial class Frm_Columns : Form
    {
        public IList<ColumnInfo> Columns { get; set; }

        public Frm_Columns()
        {
            InitializeComponent();
        }

        private void Frm_Columns_Load(object sender, EventArgs e)
        {
            dataGridView_Columns.DataSource = Columns;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if(Columns.Any(c => c.Deleted))
            {
                if(MessageBox.Show("You have flagged columns to be deleted. This cannot be undone. Are you sure you want to continue?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

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
