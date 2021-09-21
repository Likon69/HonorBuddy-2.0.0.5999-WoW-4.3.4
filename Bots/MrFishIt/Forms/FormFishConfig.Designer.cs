namespace MrFishIt.Forms
{
    partial class FormFishConfig
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
            this.pbNemo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUseLure = new System.Windows.Forms.CheckBox();
            this.cbxLures = new System.Windows.Forms.ComboBox();
            this.btnSaveClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbNemo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbNemo
            // 
            this.pbNemo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbNemo.Location = new System.Drawing.Point(0, 0);
            this.pbNemo.Name = "pbNemo";
            this.pbNemo.Size = new System.Drawing.Size(179, 117);
            this.pbNemo.TabIndex = 0;
            this.pbNemo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 8.75F, System.Drawing.FontStyle.Italic);
            this.label1.Location = new System.Drawing.Point(9, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lures";
            // 
            // cbUseLure
            // 
            this.cbUseLure.AutoSize = true;
            this.cbUseLure.Font = new System.Drawing.Font("Trebuchet MS", 8.75F, System.Drawing.FontStyle.Italic);
            this.cbUseLure.Location = new System.Drawing.Point(12, 123);
            this.cbUseLure.Name = "cbUseLure";
            this.cbUseLure.Size = new System.Drawing.Size(74, 22);
            this.cbUseLure.TabIndex = 2;
            this.cbUseLure.Text = "Use Lure";
            this.cbUseLure.UseVisualStyleBackColor = true;
            // 
            // cbxLures
            // 
            this.cbxLures.FormattingEnabled = true;
            this.cbxLures.Location = new System.Drawing.Point(12, 168);
            this.cbxLures.Name = "cbxLures";
            this.cbxLures.Size = new System.Drawing.Size(155, 21);
            this.cbxLures.TabIndex = 3;
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.Location = new System.Drawing.Point(12, 195);
            this.btnSaveClose.Name = "btnSaveClose";
            this.btnSaveClose.Size = new System.Drawing.Size(94, 23);
            this.btnSaveClose.TabIndex = 4;
            this.btnSaveClose.Text = "Save and Close";
            this.btnSaveClose.UseVisualStyleBackColor = true;
            // 
            // FormFishConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 229);
            this.Controls.Add(this.btnSaveClose);
            this.Controls.Add(this.cbxLures);
            this.Controls.Add(this.cbUseLure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbNemo);
            this.Name = "FormFishConfig";
            this.Text = "MrFishIt";
            this.Load += new System.EventHandler(this.FormFishConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbNemo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbNemo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbUseLure;
        private System.Windows.Forms.ComboBox cbxLures;
        private System.Windows.Forms.Button btnSaveClose;
    }
}