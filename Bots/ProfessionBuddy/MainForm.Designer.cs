namespace HighVoltz
{
    partial class MainForm
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
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelLeftSide = new System.Windows.Forms.TableLayoutPanel();
            this.PbToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSecretButton = new System.Windows.Forms.ToolStripButton();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ProfileTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelProfiles = new System.Windows.Forms.TableLayoutPanel();
            this.ProfileListView = new System.Windows.Forms.ListView();
            this.LoadProfileButton = new System.Windows.Forms.Button();
            this.ActionsTab = new System.Windows.Forms.TabPage();
            this.tableLayoutActionList = new System.Windows.Forms.TableLayoutPanel();
            this.ActionGridView = new System.Windows.Forms.DataGridView();
            this.ActionsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HelpTextBox = new System.Windows.Forms.RichTextBox();
            this.TradeSkillTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTradeSkil = new System.Windows.Forms.TableLayoutPanel();
            this.TradeSkillToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripAddCombo = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripAddNum = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripAddBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripReloadBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripMaterials = new System.Windows.Forms.ToolStripButton();
            this.TradeSkillTabControl = new System.Windows.Forms.TabControl();
            this.IngredientsView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelRightSide = new System.Windows.Forms.TableLayoutPanel();
            this.RightSideTab = new System.Windows.Forms.TabControl();
            this.TabPageProfile = new System.Windows.Forms.TabPage();
            this.ActionSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ActionTree = new System.Windows.Forms.TreeView();
            this.ActionGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripCut = new System.Windows.Forms.ToolStripButton();
            this.toolStripPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBotCombo = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripBotConfigButton = new System.Windows.Forms.ToolStripButton();
            this.IngredientsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NeedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BagsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BankColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.tableLayoutPanelLeftSide.SuspendLayout();
            this.PbToolStrip.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.ProfileTab.SuspendLayout();
            this.tableLayoutPanelProfiles.SuspendLayout();
            this.ActionsTab.SuspendLayout();
            this.tableLayoutActionList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ActionGridView)).BeginInit();
            this.TradeSkillTab.SuspendLayout();
            this.tableLayoutPanelTradeSkil.SuspendLayout();
            this.TradeSkillToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IngredientsView)).BeginInit();
            this.tableLayoutPanelRightSide.SuspendLayout();
            this.RightSideTab.SuspendLayout();
            this.TabPageProfile.SuspendLayout();
            this.ActionSplitContainer.Panel1.SuspendLayout();
            this.ActionSplitContainer.Panel2.SuspendLayout();
            this.ActionSplitContainer.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "xml";
            this.saveFileDialog.Filter = "profile|*.xml;*.package|All files|*.*";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "Profiles";
            this.openFileDialog.Filter = "Profiles|*.xml;*.package|All files|*.*";
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(4, 4);
            this.MainSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.tableLayoutPanelLeftSide);
            this.MainSplitContainer.Panel1MinSize = 0;
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.tableLayoutPanelRightSide);
            this.MainSplitContainer.Panel2MinSize = 0;
            this.MainSplitContainer.Size = new System.Drawing.Size(851, 684);
            this.MainSplitContainer.SplitterDistance = 356;
            this.MainSplitContainer.SplitterWidth = 5;
            this.MainSplitContainer.TabIndex = 30;
            // 
            // tableLayoutPanelLeftSide
            // 
            this.tableLayoutPanelLeftSide.ColumnCount = 1;
            this.tableLayoutPanelLeftSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeftSide.Controls.Add(this.PbToolStrip, 0, 0);
            this.tableLayoutPanelLeftSide.Controls.Add(this.MainTabControl, 0, 1);
            this.tableLayoutPanelLeftSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLeftSide.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLeftSide.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelLeftSide.Name = "tableLayoutPanelLeftSide";
            this.tableLayoutPanelLeftSide.RowCount = 2;
            this.tableLayoutPanelLeftSide.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanelLeftSide.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeftSide.Size = new System.Drawing.Size(356, 684);
            this.tableLayoutPanelLeftSide.TabIndex = 17;
            // 
            // PbToolStrip
            // 
            this.PbToolStrip.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tableLayoutPanelLeftSide.SetColumnSpan(this.PbToolStrip, 3);
            this.PbToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.PbToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpen,
            this.toolStripSave,
            this.toolStripHelp,
            this.toolStripSecretButton});
            this.PbToolStrip.Location = new System.Drawing.Point(4, 0);
            this.PbToolStrip.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.PbToolStrip.Name = "PbToolStrip";
            this.PbToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.PbToolStrip.Size = new System.Drawing.Size(352, 27);
            this.PbToolStrip.Stretch = true;
            this.PbToolStrip.TabIndex = 28;
            this.PbToolStrip.Text = "toolStrip1";
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Size = new System.Drawing.Size(49, 24);
            this.toolStripOpen.Text = "Open";
            this.toolStripOpen.Click += new System.EventHandler(this.ToolStripOpenClick);
            // 
            // toolStripSave
            // 
            this.toolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(44, 24);
            this.toolStripSave.Text = "Save";
            this.toolStripSave.ToolTipText = "Save";
            this.toolStripSave.Click += new System.EventHandler(this.ToolStripSaveClick);
            // 
            // toolStripHelp
            // 
            this.toolStripHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripHelp.Name = "toolStripHelp";
            this.toolStripHelp.Size = new System.Drawing.Size(45, 24);
            this.toolStripHelp.Text = "Help";
            this.toolStripHelp.Click += new System.EventHandler(this.ToolStripHelpClick);
            // 
            // toolStripSecretButton
            // 
            this.toolStripSecretButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSecretButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSecretButton.Name = "toolStripSecretButton";
            this.toolStripSecretButton.Size = new System.Drawing.Size(58, 24);
            this.toolStripSecretButton.Text = "Debug";
            this.toolStripSecretButton.Visible = false;
            this.toolStripSecretButton.Click += new System.EventHandler(this.ToolStripSecretButtonClick);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.ProfileTab);
            this.MainTabControl.Controls.Add(this.ActionsTab);
            this.MainTabControl.Controls.Add(this.TradeSkillTab);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 37);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.MainTabControl.Multiline = true;
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.Padding = new System.Drawing.Point(0, 0);
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(356, 647);
            this.MainTabControl.TabIndex = 17;
            // 
            // ProfileTab
            // 
            this.ProfileTab.Controls.Add(this.tableLayoutPanelProfiles);
            this.ProfileTab.Location = new System.Drawing.Point(4, 25);
            this.ProfileTab.Margin = new System.Windows.Forms.Padding(4);
            this.ProfileTab.Name = "ProfileTab";
            this.ProfileTab.Size = new System.Drawing.Size(348, 618);
            this.ProfileTab.TabIndex = 2;
            this.ProfileTab.Text = "Profiles";
            this.ProfileTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProfiles
            // 
            this.tableLayoutPanelProfiles.ColumnCount = 1;
            this.tableLayoutPanelProfiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProfiles.Controls.Add(this.ProfileListView, 0, 0);
            this.tableLayoutPanelProfiles.Controls.Add(this.LoadProfileButton, 0, 1);
            this.tableLayoutPanelProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProfiles.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelProfiles.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanelProfiles.Name = "tableLayoutPanelProfiles";
            this.tableLayoutPanelProfiles.RowCount = 2;
            this.tableLayoutPanelProfiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProfiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanelProfiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelProfiles.Size = new System.Drawing.Size(348, 618);
            this.tableLayoutPanelProfiles.TabIndex = 25;
            // 
            // ProfileListView
            // 
            this.ProfileListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProfileListView.HideSelection = false;
            this.ProfileListView.Location = new System.Drawing.Point(4, 4);
            this.ProfileListView.Margin = new System.Windows.Forms.Padding(4);
            this.ProfileListView.MultiSelect = false;
            this.ProfileListView.Name = "ProfileListView";
            this.ProfileListView.ShowGroups = false;
            this.ProfileListView.Size = new System.Drawing.Size(340, 573);
            this.ProfileListView.TabIndex = 26;
            this.ProfileListView.UseCompatibleStateImageBehavior = false;
            this.ProfileListView.View = System.Windows.Forms.View.List;
            this.ProfileListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ProfileListViewMouseDoubleClick);
            // 
            // LoadProfileButton
            // 
            this.LoadProfileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadProfileButton.Location = new System.Drawing.Point(4, 585);
            this.LoadProfileButton.Margin = new System.Windows.Forms.Padding(4);
            this.LoadProfileButton.Name = "LoadProfileButton";
            this.LoadProfileButton.Size = new System.Drawing.Size(340, 29);
            this.LoadProfileButton.TabIndex = 25;
            this.LoadProfileButton.Text = "Load Profile";
            this.LoadProfileButton.UseVisualStyleBackColor = true;
            this.LoadProfileButton.Click += new System.EventHandler(this.LoadProfileButtonClick);
            // 
            // ActionsTab
            // 
            this.ActionsTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ActionsTab.Controls.Add(this.tableLayoutActionList);
            this.ActionsTab.Location = new System.Drawing.Point(4, 25);
            this.ActionsTab.Margin = new System.Windows.Forms.Padding(0);
            this.ActionsTab.Name = "ActionsTab";
            this.ActionsTab.Padding = new System.Windows.Forms.Padding(0, 4, 4, 4);
            this.ActionsTab.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ActionsTab.Size = new System.Drawing.Size(348, 618);
            this.ActionsTab.TabIndex = 1;
            this.ActionsTab.Text = "Actions";
            this.ActionsTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutActionList
            // 
            this.tableLayoutActionList.ColumnCount = 1;
            this.tableLayoutActionList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutActionList.Controls.Add(this.ActionGridView, 0, 0);
            this.tableLayoutActionList.Controls.Add(this.HelpTextBox, 0, 1);
            this.tableLayoutActionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutActionList.Location = new System.Drawing.Point(0, 4);
            this.tableLayoutActionList.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutActionList.Name = "tableLayoutActionList";
            this.tableLayoutActionList.RowCount = 2;
            this.tableLayoutActionList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.08907F));
            this.tableLayoutActionList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.91093F));
            this.tableLayoutActionList.Size = new System.Drawing.Size(344, 610);
            this.tableLayoutActionList.TabIndex = 1;
            // 
            // ActionGridView
            // 
            this.ActionGridView.AllowUserToAddRows = false;
            this.ActionGridView.AllowUserToDeleteRows = false;
            this.ActionGridView.AllowUserToOrderColumns = true;
            this.ActionGridView.AllowUserToResizeColumns = false;
            this.ActionGridView.AllowUserToResizeRows = false;
            this.ActionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ActionGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActionsColumn});
            this.ActionGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ActionGridView.Location = new System.Drawing.Point(4, 4);
            this.ActionGridView.Margin = new System.Windows.Forms.Padding(4);
            this.ActionGridView.MultiSelect = false;
            this.ActionGridView.Name = "ActionGridView";
            this.ActionGridView.RowHeadersVisible = false;
            this.ActionGridView.RowHeadersWidth = 21;
            this.ActionGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ActionGridView.RowTemplate.Height = 16;
            this.ActionGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ActionGridView.Size = new System.Drawing.Size(336, 443);
            this.ActionGridView.TabIndex = 1;
            this.ActionGridView.SelectionChanged += new System.EventHandler(this.ActionGridViewSelectionChanged);
            this.ActionGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ActionGridViewMouseMove);
            // 
            // ActionsColumn
            // 
            this.ActionsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ActionsColumn.HeaderText = "Actions";
            this.ActionsColumn.Name = "ActionsColumn";
            // 
            // HelpTextBox
            // 
            this.HelpTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpTextBox.Location = new System.Drawing.Point(4, 455);
            this.HelpTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.HelpTextBox.Name = "HelpTextBox";
            this.HelpTextBox.ReadOnly = true;
            this.HelpTextBox.Size = new System.Drawing.Size(336, 151);
            this.HelpTextBox.TabIndex = 2;
            this.HelpTextBox.Text = "";
            // 
            // TradeSkillTab
            // 
            this.TradeSkillTab.Controls.Add(this.tableLayoutPanelTradeSkil);
            this.TradeSkillTab.ImageKey = "(none)";
            this.TradeSkillTab.Location = new System.Drawing.Point(4, 25);
            this.TradeSkillTab.Margin = new System.Windows.Forms.Padding(1);
            this.TradeSkillTab.Name = "TradeSkillTab";
            this.TradeSkillTab.Padding = new System.Windows.Forms.Padding(1);
            this.TradeSkillTab.Size = new System.Drawing.Size(348, 618);
            this.TradeSkillTab.TabIndex = 0;
            this.TradeSkillTab.Text = "TradeSkill";
            this.TradeSkillTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTradeSkil
            // 
            this.tableLayoutPanelTradeSkil.ColumnCount = 1;
            this.tableLayoutPanelTradeSkil.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTradeSkil.Controls.Add(this.TradeSkillToolStrip, 0, 1);
            this.tableLayoutPanelTradeSkil.Controls.Add(this.TradeSkillTabControl, 0, 0);
            this.tableLayoutPanelTradeSkil.Controls.Add(this.IngredientsView, 0, 2);
            this.tableLayoutPanelTradeSkil.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelTradeSkil.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanelTradeSkil.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanelTradeSkil.Name = "tableLayoutPanelTradeSkil";
            this.tableLayoutPanelTradeSkil.RowCount = 3;
            this.tableLayoutPanelTradeSkil.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTradeSkil.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelTradeSkil.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 164F));
            this.tableLayoutPanelTradeSkil.Size = new System.Drawing.Size(346, 606);
            this.tableLayoutPanelTradeSkil.TabIndex = 0;
            // 
            // TradeSkillToolStrip
            // 
            this.TradeSkillToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddCombo,
            this.toolStripAddNum,
            this.toolStripAddBtn,
            this.toolStripReloadBtn,
            this.toolStripMaterials});
            this.TradeSkillToolStrip.Location = new System.Drawing.Point(0, 412);
            this.TradeSkillToolStrip.Name = "TradeSkillToolStrip";
            this.TradeSkillToolStrip.Size = new System.Drawing.Size(346, 28);
            this.TradeSkillToolStrip.TabIndex = 32;
            this.TradeSkillToolStrip.Text = "toolStrip1";
            // 
            // toolStripAddCombo
            // 
            this.toolStripAddCombo.Items.AddRange(new object[] {
            "Specific",
            "Craftable"});
            this.toolStripAddCombo.Name = "toolStripAddCombo";
            this.toolStripAddCombo.Size = new System.Drawing.Size(105, 28);
            // 
            // toolStripAddNum
            // 
            this.toolStripAddNum.Name = "toolStripAddNum";
            this.toolStripAddNum.Size = new System.Drawing.Size(45, 28);
            this.toolStripAddNum.Text = "1";
            // 
            // toolStripAddBtn
            // 
            this.toolStripAddBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddBtn.Name = "toolStripAddBtn";
            this.toolStripAddBtn.Size = new System.Drawing.Size(41, 25);
            this.toolStripAddBtn.Text = "Add";
            this.toolStripAddBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripAddBtn.Click += new System.EventHandler(this.ToolStripAddBtnClick);
            // 
            // toolStripReloadBtn
            // 
            this.toolStripReloadBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripReloadBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripReloadBtn.Name = "toolStripReloadBtn";
            this.toolStripReloadBtn.Size = new System.Drawing.Size(60, 25);
            this.toolStripReloadBtn.Text = "Reload";
            this.toolStripReloadBtn.Click += new System.EventHandler(this.ToolStripReloadBtnClick);
            // 
            // toolStripMaterials
            // 
            this.toolStripMaterials.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMaterials.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMaterials.Name = "toolStripMaterials";
            this.toolStripMaterials.Size = new System.Drawing.Size(23, 25);
            this.toolStripMaterials.Text = "Material List";
            // 
            // TradeSkillTabControl
            // 
            this.TradeSkillTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TradeSkillTabControl.Location = new System.Drawing.Point(4, 4);
            this.TradeSkillTabControl.Margin = new System.Windows.Forms.Padding(4);
            this.TradeSkillTabControl.Multiline = true;
            this.TradeSkillTabControl.Name = "TradeSkillTabControl";
            this.TradeSkillTabControl.Padding = new System.Drawing.Point(0, 0);
            this.TradeSkillTabControl.SelectedIndex = 0;
            this.TradeSkillTabControl.Size = new System.Drawing.Size(338, 404);
            this.TradeSkillTabControl.TabIndex = 23;
            this.TradeSkillTabControl.Visible = false;
            // 
            // IngredientsView
            // 
            this.IngredientsView.AllowUserToAddRows = false;
            this.IngredientsView.AllowUserToDeleteRows = false;
            this.IngredientsView.AllowUserToResizeColumns = false;
            this.IngredientsView.AllowUserToResizeRows = false;
            this.IngredientsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IngredientsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IngredientsColumn,
            this.NeedColumn,
            this.BagsColumn,
            this.BankColumn});
            this.tableLayoutPanelTradeSkil.SetColumnSpan(this.IngredientsView, 3);
            this.IngredientsView.Dock = System.Windows.Forms.DockStyle.Top;
            this.IngredientsView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.IngredientsView.Location = new System.Drawing.Point(4, 446);
            this.IngredientsView.Margin = new System.Windows.Forms.Padding(4);
            this.IngredientsView.MultiSelect = false;
            this.IngredientsView.Name = "IngredientsView";
            this.IngredientsView.ReadOnly = true;
            this.IngredientsView.RowHeadersVisible = false;
            this.IngredientsView.RowTemplate.Height = 16;
            this.IngredientsView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.IngredientsView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.IngredientsView.Size = new System.Drawing.Size(338, 156);
            this.IngredientsView.TabIndex = 4;
            // 
            // tableLayoutPanelRightSide
            // 
            this.tableLayoutPanelRightSide.ColumnCount = 1;
            this.tableLayoutPanelRightSide.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRightSide.Controls.Add(this.RightSideTab, 0, 1);
            this.tableLayoutPanelRightSide.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanelRightSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRightSide.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRightSide.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelRightSide.Name = "tableLayoutPanelRightSide";
            this.tableLayoutPanelRightSide.RowCount = 2;
            this.tableLayoutPanelRightSide.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanelRightSide.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRightSide.Size = new System.Drawing.Size(490, 684);
            this.tableLayoutPanelRightSide.TabIndex = 0;
            // 
            // RightSideTab
            // 
            this.RightSideTab.Controls.Add(this.TabPageProfile);
            this.RightSideTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightSideTab.Location = new System.Drawing.Point(0, 37);
            this.RightSideTab.Margin = new System.Windows.Forms.Padding(0);
            this.RightSideTab.Name = "RightSideTab";
            this.RightSideTab.SelectedIndex = 0;
            this.RightSideTab.Size = new System.Drawing.Size(490, 647);
            this.RightSideTab.TabIndex = 33;
            // 
            // TabPageProfile
            // 
            this.TabPageProfile.Controls.Add(this.ActionSplitContainer);
            this.TabPageProfile.Location = new System.Drawing.Point(4, 25);
            this.TabPageProfile.Margin = new System.Windows.Forms.Padding(4);
            this.TabPageProfile.Name = "TabPageProfile";
            this.TabPageProfile.Padding = new System.Windows.Forms.Padding(4);
            this.TabPageProfile.Size = new System.Drawing.Size(482, 618);
            this.TabPageProfile.TabIndex = 0;
            this.TabPageProfile.Text = "Profile";
            this.TabPageProfile.UseVisualStyleBackColor = true;
            // 
            // ActionSplitContainer
            // 
            this.ActionSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionSplitContainer.Location = new System.Drawing.Point(4, 4);
            this.ActionSplitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.ActionSplitContainer.Name = "ActionSplitContainer";
            this.ActionSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ActionSplitContainer.Panel1
            // 
            this.ActionSplitContainer.Panel1.Controls.Add(this.ActionTree);
            // 
            // ActionSplitContainer.Panel2
            // 
            this.ActionSplitContainer.Panel2.Controls.Add(this.ActionGrid);
            this.ActionSplitContainer.Size = new System.Drawing.Size(474, 610);
            this.ActionSplitContainer.SplitterDistance = 445;
            this.ActionSplitContainer.SplitterWidth = 5;
            this.ActionSplitContainer.TabIndex = 35;
            // 
            // ActionTree
            // 
            this.ActionTree.AllowDrop = true;
            this.ActionTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionTree.HideSelection = false;
            this.ActionTree.Location = new System.Drawing.Point(0, 0);
            this.ActionTree.Margin = new System.Windows.Forms.Padding(0);
            this.ActionTree.Name = "ActionTree";
            this.ActionTree.Size = new System.Drawing.Size(474, 445);
            this.ActionTree.TabIndex = 28;
            this.ActionTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ActionTreeItemDrag);
            this.ActionTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ActionTreeAfterSelect);
            this.ActionTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.ActionTreeDragDrop);
            this.ActionTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.ActionTreeDragEnter);
            this.ActionTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ActionTreeKeyDown);
            // 
            // ActionGrid
            // 
            this.ActionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionGrid.HelpVisible = false;
            this.ActionGrid.Location = new System.Drawing.Point(0, 0);
            this.ActionGrid.Margin = new System.Windows.Forms.Padding(4);
            this.ActionGrid.Name = "ActionGrid";
            this.ActionGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ActionGrid.Size = new System.Drawing.Size(474, 160);
            this.ActionGrid.TabIndex = 29;
            this.ActionGrid.ToolbarVisible = false;
            this.ActionGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ActionGridPropertyValueChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStrip2.CanOverflow = false;
            this.tableLayoutPanelRightSide.SetColumnSpan(this.toolStrip2, 3);
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCopy,
            this.toolStripCut,
            this.toolStripPaste,
            this.toolStripDelete,
            this.toolStripSeparator4,
            this.toolStripBotCombo,
            this.toolStripBotConfigButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip2.MinimumSize = new System.Drawing.Size(427, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(486, 28);
            this.toolStrip2.Stretch = true;
            this.toolStrip2.TabIndex = 31;
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.Size = new System.Drawing.Size(47, 25);
            this.toolStripCopy.Text = "&Copy";
            this.toolStripCopy.ToolTipText = "Copy";
            this.toolStripCopy.Click += new System.EventHandler(this.ToolStripCopyClick);
            // 
            // toolStripCut
            // 
            this.toolStripCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCut.Name = "toolStripCut";
            this.toolStripCut.Size = new System.Drawing.Size(35, 25);
            this.toolStripCut.Text = "C&ut";
            this.toolStripCut.ToolTipText = "Cut";
            this.toolStripCut.Click += new System.EventHandler(this.ToolStripCutClick);
            // 
            // toolStripPaste
            // 
            this.toolStripPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPaste.Name = "toolStripPaste";
            this.toolStripPaste.Size = new System.Drawing.Size(48, 25);
            this.toolStripPaste.Text = "&Paste";
            this.toolStripPaste.ToolTipText = "Paste";
            this.toolStripPaste.Click += new System.EventHandler(this.ToolStripPasteClick);
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Size = new System.Drawing.Size(36, 25);
            this.toolStripDelete.Text = "&Del";
            this.toolStripDelete.ToolTipText = "Delete";
            this.toolStripDelete.Click += new System.EventHandler(this.ToolStripDeleteClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripBotCombo
            // 
            this.toolStripBotCombo.Name = "toolStripBotCombo";
            this.toolStripBotCombo.Size = new System.Drawing.Size(160, 28);
            this.toolStripBotCombo.SelectedIndexChanged += new System.EventHandler(this.ToolStripBotComboSelectedIndexChanged);
            // 
            // toolStripBotConfigButton
            // 
            this.toolStripBotConfigButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBotConfigButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBotConfigButton.Name = "toolStripBotConfigButton";
            this.toolStripBotConfigButton.Size = new System.Drawing.Size(57, 25);
            this.toolStripBotConfigButton.Text = "Config";
            this.toolStripBotConfigButton.Click += new System.EventHandler(this.ToolStripBotConfigButtonClick);
            // 
            // IngredientsColumn
            // 
            this.IngredientsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IngredientsColumn.HeaderText = "Ingredients";
            this.IngredientsColumn.MinimumWidth = 100;
            this.IngredientsColumn.Name = "IngredientsColumn";
            this.IngredientsColumn.ReadOnly = true;
            // 
            // NeedColumn
            // 
            this.NeedColumn.FillWeight = 35F;
            this.NeedColumn.HeaderText = "Need";
            this.NeedColumn.MinimumWidth = 50;
            this.NeedColumn.Name = "NeedColumn";
            this.NeedColumn.ReadOnly = true;
            this.NeedColumn.Width = 50;
            // 
            // BagsColumn
            // 
            this.BagsColumn.FillWeight = 35F;
            this.BagsColumn.HeaderText = "Bags";
            this.BagsColumn.MinimumWidth = 50;
            this.BagsColumn.Name = "BagsColumn";
            this.BagsColumn.ReadOnly = true;
            this.BagsColumn.Width = 50;
            // 
            // BankColumn
            // 
            this.BankColumn.FillWeight = 35F;
            this.BankColumn.HeaderText = "Bank";
            this.BankColumn.MinimumWidth = 50;
            this.BankColumn.Name = "BankColumn";
            this.BankColumn.ReadOnly = true;
            this.BankColumn.Width = 50;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(859, 692);
            this.Controls.Add(this.MainSplitContainer);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(874, 728);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "Profession Buddy";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResizeBegin += new System.EventHandler(this.MainFormResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainFormResizeEnd);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.ResumeLayout(false);
            this.tableLayoutPanelLeftSide.ResumeLayout(false);
            this.tableLayoutPanelLeftSide.PerformLayout();
            this.PbToolStrip.ResumeLayout(false);
            this.PbToolStrip.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.ProfileTab.ResumeLayout(false);
            this.tableLayoutPanelProfiles.ResumeLayout(false);
            this.ActionsTab.ResumeLayout(false);
            this.tableLayoutActionList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ActionGridView)).EndInit();
            this.TradeSkillTab.ResumeLayout(false);
            this.tableLayoutPanelTradeSkil.ResumeLayout(false);
            this.tableLayoutPanelTradeSkil.PerformLayout();
            this.TradeSkillToolStrip.ResumeLayout(false);
            this.TradeSkillToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IngredientsView)).EndInit();
            this.tableLayoutPanelRightSide.ResumeLayout(false);
            this.tableLayoutPanelRightSide.PerformLayout();
            this.RightSideTab.ResumeLayout(false);
            this.TabPageProfile.ResumeLayout(false);
            this.ActionSplitContainer.Panel1.ResumeLayout(false);
            this.ActionSplitContainer.Panel2.ResumeLayout(false);
            this.ActionSplitContainer.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLeftSide;
        private System.Windows.Forms.ToolStrip PbToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.ToolStripButton toolStripSave;
        private System.Windows.Forms.ToolStripButton toolStripHelp;
        private System.Windows.Forms.ToolStripButton toolStripSecretButton;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage ProfileTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProfiles;
        internal System.Windows.Forms.ListView ProfileListView;
        private System.Windows.Forms.Button LoadProfileButton;
        private System.Windows.Forms.TabPage ActionsTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutActionList;
        private System.Windows.Forms.DataGridView ActionGridView;
        private System.Windows.Forms.RichTextBox HelpTextBox;
        private System.Windows.Forms.TabPage TradeSkillTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTradeSkil;
        private System.Windows.Forms.TabControl TradeSkillTabControl;
        public System.Windows.Forms.DataGridView IngredientsView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRightSide;
        private System.Windows.Forms.TabControl RightSideTab;
        private System.Windows.Forms.TabPage TabPageProfile;
        private System.Windows.Forms.SplitContainer ActionSplitContainer;
        internal System.Windows.Forms.TreeView ActionTree;
        internal System.Windows.Forms.PropertyGrid ActionGrid;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripCopy;
        private System.Windows.Forms.ToolStripButton toolStripCut;
        private System.Windows.Forms.ToolStripButton toolStripPaste;
        private System.Windows.Forms.ToolStripButton toolStripDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox toolStripBotCombo;
        private System.Windows.Forms.ToolStripButton toolStripBotConfigButton;
        private System.Windows.Forms.ToolStrip TradeSkillToolStrip;
        private System.Windows.Forms.ToolStripComboBox toolStripAddCombo;
        private System.Windows.Forms.ToolStripTextBox toolStripAddNum;
        private System.Windows.Forms.ToolStripButton toolStripAddBtn;
        private System.Windows.Forms.ToolStripButton toolStripReloadBtn;
        private System.Windows.Forms.ToolStripButton toolStripMaterials;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActionsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn IngredientsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NeedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BagsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BankColumn;
    }
}

