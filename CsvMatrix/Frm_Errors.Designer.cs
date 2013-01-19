namespace CsvMatrix
{
    partial class Frm_Errors
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
            this.listBox_Errors = new System.Windows.Forms.ListBox();
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.label_Message = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.Panel2.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_Errors
            // 
            this.listBox_Errors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Errors.FormattingEnabled = true;
            this.listBox_Errors.Location = new System.Drawing.Point(0, 0);
            this.listBox_Errors.Name = "listBox_Errors";
            this.listBox_Errors.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBox_Errors.Size = new System.Drawing.Size(437, 265);
            this.listBox_Errors.TabIndex = 0;
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.IsSplitterFixed = true;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            this.splitContainer_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.Controls.Add(this.label_Message);
            // 
            // splitContainer_Main.Panel2
            // 
            this.splitContainer_Main.Panel2.Controls.Add(this.listBox_Errors);
            this.splitContainer_Main.Size = new System.Drawing.Size(437, 294);
            this.splitContainer_Main.SplitterDistance = 25;
            this.splitContainer_Main.TabIndex = 1;
            // 
            // label_Message
            // 
            this.label_Message.AutoSize = true;
            this.label_Message.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Message.Location = new System.Drawing.Point(4, 6);
            this.label_Message.Name = "label_Message";
            this.label_Message.Size = new System.Drawing.Size(35, 13);
            this.label_Message.TabIndex = 0;
            this.label_Message.Text = "label1";
            // 
            // Frm_Errors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 294);
            this.Controls.Add(this.splitContainer_Main);
            this.Name = "Frm_Errors";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Errors found";
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            this.splitContainer_Main.Panel1.PerformLayout();
            this.splitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Errors;
        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private System.Windows.Forms.Label label_Message;
    }
}