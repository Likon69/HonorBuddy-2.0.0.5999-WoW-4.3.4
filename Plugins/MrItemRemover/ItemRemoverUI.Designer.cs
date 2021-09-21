namespace MrItemRemover
{
    partial class ItemRemoverUI
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
            this.RemoveList = new System.Windows.Forms.ListBox();
            this.RSI = new System.Windows.Forms.Button();
            this.AddItem = new System.Windows.Forms.Button();
            this.AdditemName = new System.Windows.Forms.TextBox();
            this.Save = new System.Windows.Forms.Button();
            this.AddToRL = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resf = new System.Windows.Forms.PictureBox();
            this.CurinList = new System.Windows.Forms.ListView();
            this.ItemName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemSoulbound = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.GrayItems = new System.Windows.Forms.CheckBox();
            this.GoldGrays = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MinPass = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Qitem = new System.Windows.Forms.CheckBox();
            this.RCK = new System.Windows.Forms.Button();
            this.SilverGrays = new System.Windows.Forms.TextBox();
            this.CopperGrays = new System.Windows.Forms.TextBox();
            this.GoldBox = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.SellList = new System.Windows.Forms.ListBox();
            this.RemoveSellItem = new System.Windows.Forms.Button();
            this.AddToSL = new System.Windows.Forms.Button();
            this.SellGray = new System.Windows.Forms.CheckBox();
            this.SellGreen = new System.Windows.Forms.CheckBox();
            this.SellWhite = new System.Windows.Forms.CheckBox();
            this.EnableSell = new System.Windows.Forms.CheckBox();
            this.EnableRemove = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resf)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GoldBox)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // RemoveList
            // 
            this.RemoveList.FormattingEnabled = true;
            this.RemoveList.Location = new System.Drawing.Point(3, 28);
            this.RemoveList.Name = "RemoveList";
            this.RemoveList.Size = new System.Drawing.Size(137, 186);
            this.RemoveList.TabIndex = 0;
            this.RemoveList.SelectedIndexChanged += new System.EventHandler(this.RemoveList_SelectedIndexChanged);
            // 
            // RSI
            // 
            this.RSI.Location = new System.Drawing.Point(3, 220);
            this.RSI.Name = "RSI";
            this.RSI.Size = new System.Drawing.Size(137, 23);
            this.RSI.TabIndex = 1;
            this.RSI.Text = "Remove Selected Item";
            this.RSI.UseVisualStyleBackColor = true;
            this.RSI.Click += new System.EventHandler(this.RSI_Click);
            // 
            // AddItem
            // 
            this.AddItem.Location = new System.Drawing.Point(253, 3);
            this.AddItem.Name = "AddItem";
            this.AddItem.Size = new System.Drawing.Size(86, 23);
            this.AddItem.TabIndex = 2;
            this.AddItem.Text = "Add New Item";
            this.AddItem.UseVisualStyleBackColor = true;
            this.AddItem.Click += new System.EventHandler(this.AddItem_Click);
            // 
            // AdditemName
            // 
            this.AdditemName.Location = new System.Drawing.Point(105, 6);
            this.AdditemName.Name = "AdditemName";
            this.AdditemName.Size = new System.Drawing.Size(142, 20);
            this.AdditemName.TabIndex = 3;
            this.AdditemName.TextChanged += new System.EventHandler(this.AdditemName_TextChanged);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(505, 331);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 4;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // AddToRL
            // 
            this.AddToRL.Location = new System.Drawing.Point(4, 220);
            this.AddToRL.Name = "AddToRL";
            this.AddToRL.Size = new System.Drawing.Size(153, 23);
            this.AddToRL.TabIndex = 6;
            this.AddToRL.Text = "Add Selected to Remove List";
            this.AddToRL.UseVisualStyleBackColor = true;
            this.AddToRL.Click += new System.EventHandler(this.AddToRL_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Add Item by Name";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.AddToSL);
            this.panel1.Controls.Add(this.resf);
            this.panel1.Controls.Add(this.CurinList);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.AddToRL);
            this.panel1.Location = new System.Drawing.Point(1, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 256);
            this.panel1.TabIndex = 8;
            // 
            // resf
            // 
            this.resf.Location = new System.Drawing.Point(110, 8);
            this.resf.Name = "resf";
            this.resf.Size = new System.Drawing.Size(16, 16);
            this.resf.TabIndex = 9;
            this.resf.TabStop = false;
            this.resf.Click += new System.EventHandler(this.refresh_Click);
            // 
            // CurinList
            // 
            this.CurinList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CurinList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemName,
            this.ItemCount,
            this.ItemSoulbound});
            this.CurinList.ForeColor = System.Drawing.Color.Red;
            this.CurinList.Location = new System.Drawing.Point(7, 28);
            this.CurinList.MultiSelect = false;
            this.CurinList.Name = "CurinList";
            this.CurinList.Size = new System.Drawing.Size(279, 186);
            this.CurinList.TabIndex = 8;
            this.CurinList.UseCompatibleStateImageBehavior = false;
            this.CurinList.View = System.Windows.Forms.View.Details;
            this.CurinList.SelectedIndexChanged += new System.EventHandler(this.CurinList_SelectedIndexChanged);
            // 
            // ItemName
            // 
            this.ItemName.Text = "Name";
            this.ItemName.Width = 154;
            // 
            // ItemCount
            // 
            this.ItemCount.Text = "Count";
            this.ItemCount.Width = 40;
            // 
            // ItemSoulbound
            // 
            this.ItemSoulbound.Text = "SoulBound";
            this.ItemSoulbound.Width = 65;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "My Current Inventory";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.RemoveList);
            this.panel2.Controls.Add(this.RSI);
            this.panel2.Location = new System.Drawing.Point(291, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(142, 256);
            this.panel2.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Inventory To Be Removed";
            // 
            // GrayItems
            // 
            this.GrayItems.AutoSize = true;
            this.GrayItems.Location = new System.Drawing.Point(8, 299);
            this.GrayItems.Name = "GrayItems";
            this.GrayItems.Size = new System.Drawing.Size(132, 17);
            this.GrayItems.TabIndex = 10;
            this.GrayItems.Text = "Remove all Gray Items";
            this.GrayItems.UseVisualStyleBackColor = true;
            this.GrayItems.CheckedChanged += new System.EventHandler(this.GrayItems_CheckedChanged);
            // 
            // GoldGrays
            // 
            this.GoldGrays.BackColor = System.Drawing.Color.Black;
            this.GoldGrays.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GoldGrays.ForeColor = System.Drawing.Color.White;
            this.GoldGrays.Location = new System.Drawing.Point(30, 341);
            this.GoldGrays.MaxLength = 4;
            this.GoldGrays.Name = "GoldGrays";
            this.GoldGrays.Size = new System.Drawing.Size(26, 13);
            this.GoldGrays.TabIndex = 11;
            this.GoldGrays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.GoldGrays.TextChanged += new System.EventHandler(this.GoldGrays_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 319);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Dont Remove Gray Items Priced Above";
            // 
            // MinPass
            // 
            this.MinPass.Location = new System.Drawing.Point(530, 293);
            this.MinPass.Name = "MinPass";
            this.MinPass.Size = new System.Drawing.Size(49, 20);
            this.MinPass.TabIndex = 15;
            this.MinPass.TextChanged += new System.EventHandler(this.MinPass_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(395, 295);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Minutes between checks.";
            // 
            // Qitem
            // 
            this.Qitem.AutoSize = true;
            this.Qitem.Location = new System.Drawing.Point(398, 313);
            this.Qitem.Name = "Qitem";
            this.Qitem.Size = new System.Drawing.Size(178, 17);
            this.Qitem.TabIndex = 17;
            this.Qitem.Text = "Remove Items that begin quests";
            this.Qitem.UseVisualStyleBackColor = true;
            this.Qitem.CheckedChanged += new System.EventHandler(this.Qitem_CheckedChanged);
            // 
            // RCK
            // 
            this.RCK.Location = new System.Drawing.Point(398, 331);
            this.RCK.Name = "RCK";
            this.RCK.Size = new System.Drawing.Size(97, 23);
            this.RCK.TabIndex = 18;
            this.RCK.Text = "Run Check Now";
            this.RCK.UseVisualStyleBackColor = true;
            this.RCK.Click += new System.EventHandler(this.RCK_Click);
            // 
            // SilverGrays
            // 
            this.SilverGrays.BackColor = System.Drawing.Color.Black;
            this.SilverGrays.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SilverGrays.ForeColor = System.Drawing.Color.White;
            this.SilverGrays.Location = new System.Drawing.Point(77, 341);
            this.SilverGrays.MaxLength = 2;
            this.SilverGrays.Name = "SilverGrays";
            this.SilverGrays.Size = new System.Drawing.Size(17, 13);
            this.SilverGrays.TabIndex = 19;
            this.SilverGrays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SilverGrays.TextChanged += new System.EventHandler(this.SilverGrays_TextChanged);
            // 
            // CopperGrays
            // 
            this.CopperGrays.BackColor = System.Drawing.Color.Black;
            this.CopperGrays.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CopperGrays.ForeColor = System.Drawing.Color.White;
            this.CopperGrays.Location = new System.Drawing.Point(115, 341);
            this.CopperGrays.MaxLength = 2;
            this.CopperGrays.Name = "CopperGrays";
            this.CopperGrays.Size = new System.Drawing.Size(18, 13);
            this.CopperGrays.TabIndex = 20;
            this.CopperGrays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CopperGrays.TextChanged += new System.EventHandler(this.CopperGrays_TextChanged);
            // 
            // GoldBox
            // 
            this.GoldBox.Location = new System.Drawing.Point(4, 336);
            this.GoldBox.Name = "GoldBox";
            this.GoldBox.Size = new System.Drawing.Size(151, 24);
            this.GoldBox.TabIndex = 22;
            this.GoldBox.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.SellList);
            this.panel3.Controls.Add(this.RemoveSellItem);
            this.panel3.Location = new System.Drawing.Point(437, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(143, 256);
            this.panel3.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Inventory To Be Sold";
            // 
            // SellList
            // 
            this.SellList.FormattingEnabled = true;
            this.SellList.Location = new System.Drawing.Point(5, 26);
            this.SellList.Name = "SellList";
            this.SellList.Size = new System.Drawing.Size(135, 186);
            this.SellList.TabIndex = 0;
            // 
            // RemoveSellItem
            // 
            this.RemoveSellItem.Location = new System.Drawing.Point(3, 220);
            this.RemoveSellItem.Name = "RemoveSellItem";
            this.RemoveSellItem.Size = new System.Drawing.Size(137, 23);
            this.RemoveSellItem.TabIndex = 1;
            this.RemoveSellItem.Text = "Remove Selected Item";
            this.RemoveSellItem.UseVisualStyleBackColor = true;
            this.RemoveSellItem.Click += new System.EventHandler(this.RemoveSellItem_Click);
            // 
            // AddToSL
            // 
            this.AddToSL.Location = new System.Drawing.Point(159, 220);
            this.AddToSL.Name = "AddToSL";
            this.AddToSL.Size = new System.Drawing.Size(131, 23);
            this.AddToSL.TabIndex = 10;
            this.AddToSL.Text = "Add Selected to Sell List";
            this.AddToSL.UseVisualStyleBackColor = true;
            this.AddToSL.Click += new System.EventHandler(this.AddToSL_Click);
            // 
            // SellGray
            // 
            this.SellGray.AutoSize = true;
            this.SellGray.Location = new System.Drawing.Point(290, 291);
            this.SellGray.Name = "SellGray";
            this.SellGray.Size = new System.Drawing.Size(96, 17);
            this.SellGray.TabIndex = 24;
            this.SellGray.Text = "Sell Gray Items";
            this.SellGray.UseVisualStyleBackColor = true;
            this.SellGray.CheckedChanged += new System.EventHandler(this.SellGray_CheckedChanged);
            // 
            // SellGreen
            // 
            this.SellGreen.AutoSize = true;
            this.SellGreen.Location = new System.Drawing.Point(290, 313);
            this.SellGreen.Name = "SellGreen";
            this.SellGreen.Size = new System.Drawing.Size(103, 17);
            this.SellGreen.TabIndex = 25;
            this.SellGreen.Text = "Sell Green Items";
            this.SellGreen.UseVisualStyleBackColor = true;
            this.SellGreen.CheckedChanged += new System.EventHandler(this.SellGreen_CheckedChanged);
            // 
            // SellWhite
            // 
            this.SellWhite.AutoSize = true;
            this.SellWhite.Location = new System.Drawing.Point(290, 337);
            this.SellWhite.Name = "SellWhite";
            this.SellWhite.Size = new System.Drawing.Size(102, 17);
            this.SellWhite.TabIndex = 26;
            this.SellWhite.Text = "Sell White Items";
            this.SellWhite.UseVisualStyleBackColor = true;
            this.SellWhite.CheckedChanged += new System.EventHandler(this.SellWhite_CheckedChanged);
            // 
            // EnableSell
            // 
            this.EnableSell.AutoSize = true;
            this.EnableSell.Location = new System.Drawing.Point(454, 6);
            this.EnableSell.Name = "EnableSell";
            this.EnableSell.Size = new System.Drawing.Size(93, 17);
            this.EnableSell.TabIndex = 27;
            this.EnableSell.Text = "Enable Selling";
            this.EnableSell.UseVisualStyleBackColor = true;
            this.EnableSell.CheckedChanged += new System.EventHandler(this.EnableSell_CheckedChanged);
            // 
            // EnableRemove
            // 
            this.EnableRemove.AutoSize = true;
            this.EnableRemove.Location = new System.Drawing.Point(345, 6);
            this.EnableRemove.Name = "EnableRemove";
            this.EnableRemove.Size = new System.Drawing.Size(110, 17);
            this.EnableRemove.TabIndex = 28;
            this.EnableRemove.Text = "Enable Removing";
            this.EnableRemove.UseVisualStyleBackColor = true;
            this.EnableRemove.CheckedChanged += new System.EventHandler(this.EnableRemove_CheckedChanged);
            // 
            // ItemRemoverUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 361);
            this.Controls.Add(this.EnableRemove);
            this.Controls.Add(this.EnableSell);
            this.Controls.Add(this.SellWhite);
            this.Controls.Add(this.SellGreen);
            this.Controls.Add(this.SellGray);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.CopperGrays);
            this.Controls.Add(this.SilverGrays);
            this.Controls.Add(this.RCK);
            this.Controls.Add(this.Qitem);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.MinPass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.GoldGrays);
            this.Controls.Add(this.GrayItems);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.AdditemName);
            this.Controls.Add(this.AddItem);
            this.Controls.Add(this.GoldBox);
            this.MaximizeBox = false;
            this.Name = "ItemRemoverUI";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Mr.ItemRemover";
            this.Load += new System.EventHandler(this.ItemRemoverUI_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resf)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GoldBox)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox RemoveList;
        private System.Windows.Forms.Button RSI;
        private System.Windows.Forms.Button AddItem;
        private System.Windows.Forms.TextBox AdditemName;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button AddToRL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox GrayItems;
        private System.Windows.Forms.TextBox GoldGrays;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MinPass;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox Qitem;
        private System.Windows.Forms.ListView CurinList;
        private System.Windows.Forms.ColumnHeader ItemName;
        private System.Windows.Forms.ColumnHeader ItemCount;
        private System.Windows.Forms.ColumnHeader ItemSoulbound;
        private System.Windows.Forms.Button RCK;
        private System.Windows.Forms.TextBox SilverGrays;
        private System.Windows.Forms.TextBox CopperGrays;
        private System.Windows.Forms.PictureBox GoldBox;
        private System.Windows.Forms.PictureBox resf;
        private System.Windows.Forms.Button AddToSL;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox SellList;
        private System.Windows.Forms.Button RemoveSellItem;
        private System.Windows.Forms.CheckBox SellGray;
        private System.Windows.Forms.CheckBox SellGreen;
        private System.Windows.Forms.CheckBox SellWhite;
        private System.Windows.Forms.CheckBox EnableSell;
        private System.Windows.Forms.CheckBox EnableRemove;
    }
}