namespace CsvMatrix
{
    partial class Frm_CsvProperties
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_Other = new System.Windows.Forms.TextBox();
            this.radioButton_Other = new System.Windows.Forms.RadioButton();
            this.radioButton_Pipe = new System.Windows.Forms.RadioButton();
            this.radioButton_Semicolon = new System.Windows.Forms.RadioButton();
            this.radioButton_Tab = new System.Windows.Forms.RadioButton();
            this.radioButton_Comma = new System.Windows.Forms.RadioButton();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.checkBox_HeaderRow = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_Other);
            this.groupBox1.Controls.Add(this.radioButton_Other);
            this.groupBox1.Controls.Add(this.radioButton_Pipe);
            this.groupBox1.Controls.Add(this.radioButton_Semicolon);
            this.groupBox1.Controls.Add(this.radioButton_Tab);
            this.groupBox1.Controls.Add(this.radioButton_Comma);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 182);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Delimiter";
            // 
            // textBox_Other
            // 
            this.textBox_Other.Location = new System.Drawing.Point(34, 146);
            this.textBox_Other.Name = "textBox_Other";
            this.textBox_Other.Size = new System.Drawing.Size(100, 20);
            this.textBox_Other.TabIndex = 5;
            // 
            // radioButton_Other
            // 
            this.radioButton_Other.AutoSize = true;
            this.radioButton_Other.Location = new System.Drawing.Point(17, 119);
            this.radioButton_Other.Name = "radioButton_Other";
            this.radioButton_Other.Size = new System.Drawing.Size(51, 17);
            this.radioButton_Other.TabIndex = 4;
            this.radioButton_Other.TabStop = true;
            this.radioButton_Other.Text = "Other";
            this.radioButton_Other.UseVisualStyleBackColor = true;
            this.radioButton_Other.CheckedChanged += new System.EventHandler(this.radioButton_Other_CheckedChanged);
            // 
            // radioButton_Pipe
            // 
            this.radioButton_Pipe.AutoSize = true;
            this.radioButton_Pipe.Location = new System.Drawing.Point(17, 94);
            this.radioButton_Pipe.Name = "radioButton_Pipe";
            this.radioButton_Pipe.Size = new System.Drawing.Size(46, 17);
            this.radioButton_Pipe.TabIndex = 3;
            this.radioButton_Pipe.TabStop = true;
            this.radioButton_Pipe.Text = "Pipe";
            this.radioButton_Pipe.UseVisualStyleBackColor = true;
            // 
            // radioButton_Semicolon
            // 
            this.radioButton_Semicolon.AutoSize = true;
            this.radioButton_Semicolon.Location = new System.Drawing.Point(17, 71);
            this.radioButton_Semicolon.Name = "radioButton_Semicolon";
            this.radioButton_Semicolon.Size = new System.Drawing.Size(74, 17);
            this.radioButton_Semicolon.TabIndex = 2;
            this.radioButton_Semicolon.TabStop = true;
            this.radioButton_Semicolon.Text = "Semicolon";
            this.radioButton_Semicolon.UseVisualStyleBackColor = true;
            // 
            // radioButton_Tab
            // 
            this.radioButton_Tab.AutoSize = true;
            this.radioButton_Tab.Location = new System.Drawing.Point(17, 48);
            this.radioButton_Tab.Name = "radioButton_Tab";
            this.radioButton_Tab.Size = new System.Drawing.Size(44, 17);
            this.radioButton_Tab.TabIndex = 1;
            this.radioButton_Tab.TabStop = true;
            this.radioButton_Tab.Text = "Tab";
            this.radioButton_Tab.UseVisualStyleBackColor = true;
            // 
            // radioButton_Comma
            // 
            this.radioButton_Comma.AutoSize = true;
            this.radioButton_Comma.Location = new System.Drawing.Point(17, 25);
            this.radioButton_Comma.Name = "radioButton_Comma";
            this.radioButton_Comma.Size = new System.Drawing.Size(60, 17);
            this.radioButton_Comma.TabIndex = 0;
            this.radioButton_Comma.TabStop = true;
            this.radioButton_Comma.Text = "Comma";
            this.radioButton_Comma.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(83, 227);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(73, 23);
            this.button_Ok.TabIndex = 2;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(12, 227);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(65, 23);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // checkBox_HeaderRow
            // 
            this.checkBox_HeaderRow.AutoSize = true;
            this.checkBox_HeaderRow.Location = new System.Drawing.Point(16, 200);
            this.checkBox_HeaderRow.Name = "checkBox_HeaderRow";
            this.checkBox_HeaderRow.Size = new System.Drawing.Size(130, 17);
            this.checkBox_HeaderRow.TabIndex = 0;
            this.checkBox_HeaderRow.Text = "Contains Header Row";
            this.checkBox_HeaderRow.UseVisualStyleBackColor = true;
            // 
            // Frm_CsvProperties
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(168, 257);
            this.Controls.Add(this.checkBox_HeaderRow);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_CsvProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.Frm_CsvProperties_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_Other;
        private System.Windows.Forms.RadioButton radioButton_Other;
        private System.Windows.Forms.RadioButton radioButton_Pipe;
        private System.Windows.Forms.RadioButton radioButton_Semicolon;
        private System.Windows.Forms.RadioButton radioButton_Tab;
        private System.Windows.Forms.RadioButton radioButton_Comma;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.CheckBox checkBox_HeaderRow;
    }
}