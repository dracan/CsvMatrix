namespace CsvMatrix
{
    partial class About
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
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel_GithubUrl = new System.Windows.Forms.LinkLabel();
            this.button_OK = new System.Windows.Forms.Button();
            this.label_VersionNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "CsvMatrix";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Developed by Dan Clarke";
            // 
            // linkLabel_GithubUrl
            // 
            this.linkLabel_GithubUrl.AutoSize = true;
            this.linkLabel_GithubUrl.Location = new System.Drawing.Point(12, 110);
            this.linkLabel_GithubUrl.Name = "linkLabel_GithubUrl";
            this.linkLabel_GithubUrl.Size = new System.Drawing.Size(184, 13);
            this.linkLabel_GithubUrl.TabIndex = 2;
            this.linkLabel_GithubUrl.TabStop = true;
            this.linkLabel_GithubUrl.Text = "https://github.com/dracan/CsvMatrix";
            this.linkLabel_GithubUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_GithubUrl_LinkClicked);
            // 
            // button_OK
            // 
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_OK.Location = new System.Drawing.Point(63, 140);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 3;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label_VersionNumber
            // 
            this.label_VersionNumber.AutoSize = true;
            this.label_VersionNumber.Location = new System.Drawing.Point(60, 56);
            this.label_VersionNumber.Name = "label_VersionNumber";
            this.label_VersionNumber.Size = new System.Drawing.Size(72, 13);
            this.label_VersionNumber.TabIndex = 4;
            this.label_VersionNumber.Text = "Version X.Y.Z";
            // 
            // About
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_OK;
            this.ClientSize = new System.Drawing.Size(209, 172);
            this.Controls.Add(this.label_VersionNumber);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.linkLabel_GithubUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel_GithubUrl;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label_VersionNumber;
    }
}