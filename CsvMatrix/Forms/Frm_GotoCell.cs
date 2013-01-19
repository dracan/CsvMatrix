using System;
using System.Globalization;
using System.Windows.Forms;

namespace CsvMatrix.Forms
{
    public partial class Frm_GotoCell : Form
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }

        public Frm_GotoCell(int maxColumnIndex, int maxRowIndex)
        {
            InitializeComponent();

            numericUpDown_ColumnIndex.Maximum = maxColumnIndex;
            numericUpDown_RowIndex.Maximum = maxRowIndex;
        }

        private void Frm_GotoCell_Load(object sender, EventArgs e)
        {
            numericUpDown_ColumnIndex.Value = ColumnIndex;
            numericUpDown_RowIndex.Value = RowIndex;

            numericUpDown_ColumnIndex.Select(0, numericUpDown_ColumnIndex.Value.ToString(CultureInfo.InvariantCulture).Length);
            numericUpDown_RowIndex.Select(0, numericUpDown_ColumnIndex.Value.ToString(CultureInfo.InvariantCulture).Length);
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            ColumnIndex = (int)numericUpDown_ColumnIndex.Value;
            RowIndex = (int)numericUpDown_RowIndex.Value;

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
