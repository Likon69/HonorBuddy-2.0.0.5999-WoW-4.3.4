namespace Bobby53
{
    partial class IssueForm
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
            if (disposing && (components != null))
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(330, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "To obtain assistance with a problem or behavior, you need to Post a \r\nspecific de" +
                "scription of the problem and attach a Debug Log file. ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(269, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Visit the ShamWOW thread on the HonorBuddy forums.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(32, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(330, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "While viewing the ShamWOW thread, scroll to the bottom of the \r\npage and click Go" +
                " Advanced";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(32, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(330, 26);
            this.label4.TabIndex = 3;
            this.label4.Text = "Copy and paste the following template into your post";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(54, 160);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(311, 133);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "HonorBuddy Mode: Grind/PVP/Mixed/Quest/RaF\r\n\r\nShamans Location:\r\n\r\nWhat should ha" +
                "ve happened (be specific):\r\n\r\nWhat did happen (be specific):\r\n\r\nSystem time of i" +
                "ssue (log time stamp):";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(35, 307);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(330, 26);
            this.label5.TabIndex = 5;
            this.label5.Text = "Attach your debug log file located in the Logs folder of your HonorBuddy director" +
                "y to the post.";
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(62, 355);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(104, 23);
            this.btnReport.TabIndex = 6;
            this.btnReport.Text = "Go to Forum...";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(199, 355);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // IssueForm
            // 
            this.AcceptButton = this.btnReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(387, 399);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "IssueForm";
            this.Text = "Report an Issue with ShamWOW...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}