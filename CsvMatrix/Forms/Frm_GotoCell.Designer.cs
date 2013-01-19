namespace CsvMatrix.Forms
{
    partial class Frm_GotoCell
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_ColumnIndex = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_RowIndex = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ColumnIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_RowIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Column Index:";
            // 
            // numericUpDown_ColumnIndex
            // 
            this.numericUpDown_ColumnIndex.Location = new System.Drawing.Point(95, 12);
            this.numericUpDown_ColumnIndex.Name = "numericUpDown_ColumnIndex";
            this.numericUpDown_ColumnIndex.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_ColumnIndex.TabIndex = 0;
            // 
            // numericUpDown_RowIndex
            // 
            this.numericUpDown_RowIndex.Location = new System.Drawing.Point(95, 38);
            this.numericUpDown_RowIndex.Name = "numericUpDown_RowIndex";
            this.numericUpDown_RowIndex.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_RowIndex.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Row Index:";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(140, 64);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 3;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(59, 64);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // Frm_GotoCell
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(222, 91);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown_RowIndex);
            this.Controls.Add(this.numericUpDown_ColumnIndex);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_GotoCell";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Go To Cell";
            this.Load += new System.EventHandler(this.Frm_GotoCell_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ColumnIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_RowIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_ColumnIndex;
        private System.Windows.Forms.NumericUpDown numericUpDown_RowIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
    }
}