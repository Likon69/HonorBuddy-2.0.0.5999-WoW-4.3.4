namespace FelMaster
{
    partial class FelMasterForm7
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
            this.T1 = new System.Windows.Forms.CheckBox();
            this.T2 = new System.Windows.Forms.CheckBox();
            this.SnD = new System.Windows.Forms.CheckBox();
            this.ND = new System.Windows.Forms.CheckBox();
            this.savecfg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // T1
            // 
            this.T1.AutoSize = true;
            this.T1.BackColor = System.Drawing.Color.Transparent;
            this.T1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.T1.Location = new System.Drawing.Point(72, 57);
            this.T1.Name = "T1";
            this.T1.Size = new System.Drawing.Size(89, 24);
            this.T1.TabIndex = 0;
            this.T1.Text = "Trinket 1";
            this.T1.UseVisualStyleBackColor = false;
            this.T1.CheckedChanged += new System.EventHandler(this.T1_CheckedChanged);
            // 
            // T2
            // 
            this.T2.AutoSize = true;
            this.T2.BackColor = System.Drawing.Color.Transparent;
            this.T2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.T2.Location = new System.Drawing.Point(72, 108);
            this.T2.Name = "T2";
            this.T2.Size = new System.Drawing.Size(89, 24);
            this.T2.TabIndex = 1;
            this.T2.Text = "Trinket 2";
            this.T2.UseVisualStyleBackColor = false;
            this.T2.CheckedChanged += new System.EventHandler(this.T2_CheckedChanged);
            // 
            // SnD
            // 
            this.SnD.AutoSize = true;
            this.SnD.BackColor = System.Drawing.Color.Transparent;
            this.SnD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SnD.Location = new System.Drawing.Point(72, 159);
            this.SnD.Name = "SnD";
            this.SnD.Size = new System.Drawing.Size(89, 24);
            this.SnD.TabIndex = 0;
            this.SnD.Text = "Slice N Dice";
            this.SnD.UseVisualStyleBackColor = false;
            this.SnD.CheckedChanged += new System.EventHandler(this.SnD_CheckedChanged);
            // 
            // ND
            // 
            this.ND.AutoSize = true;
            this.ND.BackColor = System.Drawing.Color.Transparent;
            this.ND.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ND.Location = new System.Drawing.Point(72, 209);
            this.ND.Name = "ND";
            this.ND.Size = new System.Drawing.Size(89, 24);
            this.ND.TabIndex = 0;
            this.ND.Text = "Dismantle";
            this.ND.UseVisualStyleBackColor = false;
            this.ND.CheckedChanged += new System.EventHandler(this.ND_CheckedChanged);


            // 
            // savecfg
            // 
            this.savecfg.Location = new System.Drawing.Point(72, 260);
            this.savecfg.Name = "savecfg";
            this.savecfg.Size = new System.Drawing.Size(89, 23);
            this.savecfg.TabIndex = 2;
            this.savecfg.Text = "Save Config";
            this.savecfg.UseVisualStyleBackColor = true;
            this.savecfg.Click += new System.EventHandler(this.savecfg_Click);
            // 
            // FelMasterForm7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 300);
            this.Controls.Add(this.savecfg);
            this.Controls.Add(this.T2);
            this.Controls.Add(this.T1);
            this.Controls.Add(this.SnD);
            this.Controls.Add(this.ND);
            this.Name = "FelMasterForm7";
            this.Text = "FelMasterForm";
            this.Load += new System.EventHandler(this.FelMasterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox T1;
        private System.Windows.Forms.CheckBox T2;
        private System.Windows.Forms.CheckBox SnD;
        private System.Windows.Forms.CheckBox ND;
        private System.Windows.Forms.Button savecfg;

    }
}