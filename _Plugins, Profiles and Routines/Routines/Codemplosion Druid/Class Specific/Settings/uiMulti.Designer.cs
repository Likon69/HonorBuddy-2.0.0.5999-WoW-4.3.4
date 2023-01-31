namespace Hera.Class_Specific.Settings
{
    partial class uiMulti
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
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboConditionEvaluation = new System.Windows.Forms.ComboBox();
            this.clb1 = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(169, 331);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(88, 331);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboConditionEvaluation);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(250, 42);
            this.panel2.TabIndex = 7;
            // 
            // comboConditionEvaluation
            // 
            this.comboConditionEvaluation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboConditionEvaluation.FormattingEnabled = true;
            this.comboConditionEvaluation.Items.AddRange(new object[] {
            "Options evaluated as \'OR\' condition",
            "Options evaluated as \'AND\' condition"});
            this.comboConditionEvaluation.Location = new System.Drawing.Point(12, 12);
            this.comboConditionEvaluation.Name = "comboConditionEvaluation";
            this.comboConditionEvaluation.Size = new System.Drawing.Size(226, 21);
            this.comboConditionEvaluation.TabIndex = 0;
            this.comboConditionEvaluation.SelectedIndexChanged += new System.EventHandler(this.comboConditionEvaluation_SelectedIndexChanged);
            // 
            // clb1
            // 
            this.clb1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clb1.Dock = System.Windows.Forms.DockStyle.Top;
            this.clb1.FormattingEnabled = true;
            this.clb1.Location = new System.Drawing.Point(0, 42);
            this.clb1.Name = "clb1";
            this.clb1.Size = new System.Drawing.Size(250, 227);
            this.clb1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.labelDescription);
            this.panel1.Location = new System.Drawing.Point(0, 273);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 52);
            this.panel1.TabIndex = 9;
            // 
            // labelDescription
            // 
            this.labelDescription.BackColor = System.Drawing.SystemColors.ControlDark;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(250, 52);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "All options will be evaluated using \'OR\' logic. If ANY of the selected conditions" +
                " are met then the spell will be cast. If no conditions are met the spell will no" +
                "t be cast.";
            // 
            // uiMulti
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(250, 360);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.clb1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "uiMulti";
            this.Text = "Select multiple options...";
            this.Load += new System.EventHandler(this.uiMulti_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboConditionEvaluation;
        private System.Windows.Forms.CheckedListBox clb1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelDescription;
    }
}