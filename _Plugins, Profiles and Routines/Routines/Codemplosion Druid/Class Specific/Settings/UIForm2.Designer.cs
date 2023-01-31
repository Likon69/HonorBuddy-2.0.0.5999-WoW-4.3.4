namespace Hera
{
    partial class UIForm2
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
            this.components = new System.ComponentModel.Container();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.autoSetup = new System.Windows.Forms.ComboBox();
            this.environmentSettings = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.lblCCName = new System.Windows.Forms.Label();
            this.SaveSettings = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.BackColor = System.Drawing.Color.White;
            this.BottomPanel.Controls.Add(this.autoSetup);
            this.BottomPanel.Controls.Add(this.environmentSettings);
            this.BottomPanel.Controls.Add(this.label20);
            this.BottomPanel.Controls.Add(this.lblCCName);
            this.BottomPanel.Controls.Add(this.SaveSettings);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.Location = new System.Drawing.Point(0, 658);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(639, 47);
            this.BottomPanel.TabIndex = 1;
            // 
            // autoSetup
            // 
            this.autoSetup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoSetup.FormattingEnabled = true;
            this.autoSetup.Items.AddRange(new object[] {
            "Balance Template",
            "Feral Template",
            "Restoration Template"});
            this.autoSetup.Location = new System.Drawing.Point(379, 14);
            this.autoSetup.Name = "autoSetup";
            this.autoSetup.Size = new System.Drawing.Size(122, 21);
            this.autoSetup.Sorted = true;
            this.autoSetup.TabIndex = 55;
            this.toolTip1.SetToolTip(this.autoSetup, "Populate settings from a template based on the selected spec.");
            this.autoSetup.Visible = false;
            this.autoSetup.SelectedIndexChanged += new System.EventHandler(this.autoSetup_SelectedIndexChanged);
            // 
            // environmentSettings
            // 
            this.environmentSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.environmentSettings.FormattingEnabled = true;
            this.environmentSettings.Items.AddRange(new object[] {
            "Instance",
            "PVE",
            "PVP"});
            this.environmentSettings.Location = new System.Drawing.Point(274, 14);
            this.environmentSettings.Name = "environmentSettings";
            this.environmentSettings.Size = new System.Drawing.Size(65, 21);
            this.environmentSettings.Sorted = true;
            this.environmentSettings.TabIndex = 55;
            this.environmentSettings.SelectedIndexChanged += new System.EventHandler(this.environmentSettings_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.DimGray;
            this.label20.Location = new System.Drawing.Point(5, 32);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(61, 13);
            this.label20.TabIndex = 54;
            this.label20.Text = "by Deathburn";
            // 
            // lblCCName
            // 
            this.lblCCName.AutoSize = true;
            this.lblCCName.BackColor = System.Drawing.Color.Transparent;
            this.lblCCName.Font = new System.Drawing.Font("Impact", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCCName.ForeColor = System.Drawing.Color.Gray;
            this.lblCCName.Location = new System.Drawing.Point(1, -1);
            this.lblCCName.Name = "lblCCName";
            this.lblCCName.Size = new System.Drawing.Size(81, 39);
            this.lblCCName.TabIndex = 53;
            this.lblCCName.Text = "HERA";
            // 
            // SaveSettings
            // 
            this.SaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSettings.Location = new System.Drawing.Point(507, 12);
            this.SaveSettings.Name = "SaveSettings";
            this.SaveSettings.Size = new System.Drawing.Size(120, 23);
            this.SaveSettings.TabIndex = 0;
            this.SaveSettings.Text = "Save && Close";
            this.SaveSettings.UseVisualStyleBackColor = true;
            this.SaveSettings.Click += new System.EventHandler(this.SaveSettings_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(639, 658);
            this.tabControl1.TabIndex = 2;
            // 
            // UIForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 705);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.BottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UIForm2";
            this.Text = "UIForm2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UIForm2_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UIForm2_FormClosed);
            this.Load += new System.EventHandler(this.UIForm2_Load);
            this.BottomPanel.ResumeLayout(false);
            this.BottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblCCName;
        private System.Windows.Forms.Button SaveSettings;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ComboBox environmentSettings;
        private System.Windows.Forms.ComboBox autoSetup;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}