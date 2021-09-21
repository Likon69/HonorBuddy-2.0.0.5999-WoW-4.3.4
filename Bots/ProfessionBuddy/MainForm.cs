//#define PBDEBUG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using HighVoltz.Composites;
using HighVoltz.Dynamic;
using Styx;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Profiles;
using TreeSharp;

namespace HighVoltz
{
    public partial class MainForm : Form
    {
        #region Callbacks

        private readonly Professionbuddy _pb;
        private readonly FileSystemWatcher _profileWatcher;
        private PropertyBag _profilePropertyBag;
        private CopyPasteOperactions _copyAction = CopyPasteOperactions.Cut;

        private TreeNode _copySource;

        public static MainForm Instance { get; private set; }

        public static bool IsValid
        {
            get { return Instance != null && Instance.Visible && !Instance.Disposing && !Instance.IsDisposed; }
        }

        // used to update GUI controls via other threads

        private void ActionTreeDragDrop(object sender, DragEventArgs e)
        {
            _copyAction = CopyPasteOperactions.Cut;

            if ((e.KeyState & 4) > 0) // shift key
                _copyAction |= CopyPasteOperactions.IgnoreRoot;
            if ((e.KeyState & 8) > 0) // ctrl key
                _copyAction |= CopyPasteOperactions.Copy;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode dest = ((TreeView) sender).GetNodeAt(pt);
                var newNode = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode");
                PasteAction(newNode, dest);
            }
            else if (e.Data.GetDataPresent("System.Windows.Forms.DataGridViewRow", false))
            {
                Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode dest = ((TreeView) sender).GetNodeAt(pt);
                var row = (DataGridViewRow) e.Data.GetData("System.Windows.Forms.DataGridViewRow");
                if (row.Tag.GetType().GetInterface("IPBComposite") != null)
                {
                    var pa = (IPBComposite) Activator.CreateInstance(row.Tag.GetType());
                    AddToActionTree(pa, dest);
                }
            }
        }

        private void PasteAction(TreeNode source, TreeNode dest)
        {
            if (dest != source && (!IsChildNode(source, dest) || dest == null))
            {
                var gc = (GroupComposite) ((Composite) source.Tag).Parent;
                if ((_copyAction & CopyPasteOperactions.Copy) != CopyPasteOperactions.Copy)
                    gc.Children.Remove((Composite) source.Tag);
                AddToActionTree(source, dest);
                if ((_copyAction & CopyPasteOperactions.Copy) != CopyPasteOperactions.Copy) // ctrl key
                    source.Remove();
                _copySource = null; // free any ref..
            }
        }

        private void ActionTreeDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ActionTreeItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ActionTreeKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                ActionTree.SelectedNode = null;
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Delete)
            {
                if (ActionTree.SelectedNode != null)
                    RemoveSelectedNodes();
            }
        }

        private void ActionTreeAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!IsValid)
                return;
            var comp = (IPBComposite) e.Node.Tag;
            if (comp != null && comp.Properties != null)
            {
                Instance.ActionGrid.SelectedObject = comp.Properties;
            }
        }

        private void OnTradeSkillsLoadedEventHandler(object sender, EventArgs e)
        {
            // must create GUI elements on its parent thread
            if (IsHandleCreated)
                BeginInvoke(new InitDelegate(Initialize));
            else
            {
                HandleCreated += MainFormHandleCreated;
            }
            _pb.OnTradeSkillsLoaded -= OnTradeSkillsLoadedEventHandler;
        }

        private void MainFormHandleCreated(object sender, EventArgs e)
        {
            BeginInvoke(new InitDelegate(Initialize));
            HandleCreated -= MainFormHandleCreated;
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            if (!_pb.IsTradeSkillsLoaded)
            {
                _pb.OnTradeSkillsLoaded -= OnTradeSkillsLoadedEventHandler;
                _pb.OnTradeSkillsLoaded += OnTradeSkillsLoadedEventHandler;
            }
            else
                Initialize();
            if (DynamicCodeCompiler.CodeWasModified)
                DynamicCodeCompiler.GenorateDynamicCode();
        }

        private void MainFormResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void MainFormResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout();
        }

        private void ActionGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (ActionGrid.SelectedObject is CastSpellAction && ((CastSpellAction) ActionGrid.SelectedObject).IsRecipe)
            {
                _pb.UpdateMaterials();
                RefreshTradeSkillTabs();
                RefreshActionTree(typeof (CastSpellAction));
            }
            else
            {
                ActionTree.SuspendLayout();
                UdateTreeNode(ActionTree.SelectedNode, null, null, false);
                ActionTree.ResumeLayout();
            }

            if (DynamicCodeCompiler.CodeWasModified)
            {
                new Thread(DynamicCodeCompiler.GenorateDynamicCode) {IsBackground = true}.Start();
            }
        }

        private void RemoveSelectedNodes()
        {
            if (ActionTree.SelectedNode != null)
            {
                var comp = (Composite) ActionTree.SelectedNode.Tag;
                ((GroupComposite) comp.Parent).Children.Remove(comp);
                if (comp is CastSpellAction && ((CastSpellAction) comp).IsRecipe)
                {
                    _pb.UpdateMaterials();
                    RefreshTradeSkillTabs();
                }
                if (ActionTree.SelectedNode.Parent != null)
                    ActionTree.SelectedNode.Parent.Nodes.RemoveAt(ActionTree.SelectedNode.Index);
                else
                    ActionTree.Nodes.RemoveAt(ActionTree.SelectedNode.Index);
            }
        }

        private void ActionGridViewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ActionGridView.DoDragDrop(ActionGridView.CurrentRow, DragDropEffects.All);
            }
        }

        private void ActionGridViewSelectionChanged(object sender, EventArgs e)
        {
            if (ActionGridView.SelectedRows.Count > 0)
                HelpTextBox.Text = ((IPBComposite) ActionGridView.SelectedRows[0].Tag).Help;
        }

        private void ToolStripOpenClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ProfileManager.LoadNew(openFileDialog.FileName, true);
                if (_pb.ProfileSettings.SettingsDictionary.Count > 0)
                    AddProfileSettingsTab();
                else
                    RemoveProfileSettingsTab();
            }
        }

        private void ToolStripSaveClick(object sender, EventArgs e)
        {
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = _pb.CurrentProfile != null && _pb.CurrentProfile.XmlPath != null
                                          ? _pb.CurrentProfile.XmlPath
                                          : "";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                bool zip = extension != null && extension.Equals(".package",
                                                                 StringComparison.InvariantCultureIgnoreCase);
                // if we are saving to a zip check if CurrentProfile.XmlPath is not blank/null and use it if not. 
                // otherwise use the selected zipname with xml ext.
                if (_pb.CurrentProfile != null)
                {
                    string xmlfile = zip
                                         ? (_pb.CurrentProfile != null &&
                                            string.IsNullOrEmpty(_pb.CurrentProfile.XmlPath)
                                                ? Path.ChangeExtension(saveFileDialog.FileName, ".xml")
                                                : _pb.CurrentProfile.XmlPath)
                                         : saveFileDialog.FileName;
                    Professionbuddy.Log("Saving profile to {0}", saveFileDialog.FileName);
                    if (_pb.CurrentProfile != null)
                    {
                        _pb.CurrentProfile.SaveXml(xmlfile);
                        if (zip)
                            _pb.CurrentProfile.CreatePackage(saveFileDialog.FileName, xmlfile);
                    }
                }
                _pb.MySettings.LastProfile = saveFileDialog.FileName;
                _pb.MySettings.Save();
                UpdateControls();
            }
        }

        private void ToolStripAddBtnClick(object sender, EventArgs e)
        {
            var compositeList = new List<IPBComposite>();
            // if the tradeskill tab is selected
            if (MainTabControl.SelectedTab == TradeSkillTab)
            {
                var tv = TradeSkillTabControl.SelectedTab as TradeSkillListView;

                if (tv != null)
                {
                    DataGridViewSelectedRowCollection rowCollection = tv.TradeDataView.SelectedRows;
                    foreach (DataGridViewRow row in rowCollection)
                    {
                        var cell = (TradeSkillRecipeCell) row.Cells[0].Value;
                        Recipe recipe = _pb.TradeSkillList[tv.TradeIndex].KnownRecipes[cell.RecipeID];
                        int repeat;
                        int.TryParse(toolStripAddNum.Text, out repeat);
                        CastSpellAction.RepeatCalculationType repeatType =
                            CastSpellAction.RepeatCalculationType.Specific;
                        switch (toolStripAddCombo.SelectedIndex)
                        {
                            case 1:
                                repeatType = CastSpellAction.RepeatCalculationType.Craftable;
                                break;
                            case 2:
                                repeatType = CastSpellAction.RepeatCalculationType.Banker;
                                break;
                        }
                        var ca = new CastSpellAction(recipe, repeat, repeatType);
                        compositeList.Add(ca);
                    }
                }
            }
            else if (MainTabControl.SelectedTab == ActionsTab)
            {
                compositeList.AddRange(from DataGridViewRow row in ActionGridView.SelectedRows
                                       select (IPBComposite) Activator.CreateInstance(row.Tag.GetType()));
            }
            _copyAction = CopyPasteOperactions.Copy;
            foreach (IPBComposite composite in compositeList)
            {
                AddToActionTree(composite, ActionTree.SelectedNode);
            }
            // now update the CanRepeatCount. 
            _pb.UpdateMaterials();
            RefreshTradeSkillTabs();
        }

        private void ToolStripDeleteClick(object sender, EventArgs e)
        {
            RemoveSelectedNodes();
        }

        private void ToolStripHelpClick(object sender, EventArgs e)
        {
            var helpWindow = new Form {Height = 600, Width = 600, Text = "ProfessionBuddy Guide"};
            var helpView = new RichTextBox {Dock = DockStyle.Fill, ReadOnly = true};

            helpView.LoadFile(Path.Combine(Professionbuddy.BotPath, "Guide.rtf"));
            helpWindow.Controls.Add(helpView);
            helpWindow.Show();
        }

        private void ToolStripCopyClick(object sender, EventArgs e)
        {
            _copySource = ActionTree.SelectedNode;
            if (_copySource != null)
                _copyAction = CopyPasteOperactions.Copy;
        }

        private void ToolStripPasteClick(object sender, EventArgs e)
        {
            if (_copySource != null && ActionTree.SelectedNode != null)
                PasteAction(_copySource, ActionTree.SelectedNode);
        }

        private void ToolStripCutClick(object sender, EventArgs e)
        {
            _copySource = ActionTree.SelectedNode;
            if (_copySource != null)
                _copyAction = CopyPasteOperactions.Cut;
        }

        private void ToolStripSecretButtonClick(object sender, EventArgs e)
        {
            var ps = TreeRoot.Current.Root as PrioritySelector;
            int n = 0;

            Logging.Write("** BotBase **");
            if (ps != null)
                foreach (Composite p in ps.Children)
                {
                    // add alternating amount of spaces to the end of log entries to prevent spam filter from blocking it
                    n = (n + 1)%2;
                    Logging.Write("[{0}] {1}", p.GetType(), new string(' ', n));
                }

            //Logging.Write("** Profile Settings **");
            //foreach (var kv in PB.ProfileSettings.Settings)
            //    Logging.Write("{0} {1}", kv.Key, kv.Value);

            Logging.Write("** ActionSelector **");
            printComposite(_pb.PbBehavior, 0);

            //Logging.Write("** Material List **");
            //foreach (var kv in PB.MaterialList)
            //    Logging.Write("Ingredient ID: {0} Amount required:{1}", kv.Key, kv.Value);

            //Logging.Write("** DataStore **");
            //foreach (var kv in PB.DataStore)
            //    Logging.Write("item ID: {0} Amount in bag/bank/ah/alts:{1}", kv.Key, kv.Value);

            //if (PB.CsharpStringBuilder != null)
            //    Logging.Write(PB.CsharpStringBuilder.ToString());
        }

        private void printComposite(Composite comp, int cnt)
        {
            string name;
            if (comp is IPBComposite)
                name = ((IPBComposite) comp).Title;
            else
                name = comp.GetType().ToString();
            var pbComposite = comp as IPBComposite;
            if (pbComposite != null)
                Logging.Write("{0}{1} IsDone:{2} LastStatus:{3}", new string(' ', cnt*4), name,
                              (pbComposite).IsDone, comp.LastStatus);
            var groupComposite = comp as GroupComposite;
            if (groupComposite != null)
            {
                foreach (Composite child in (groupComposite).Children)
                {
                    printComposite(child, cnt + 1);
                }
            }
        }

        private void ProfileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            RefreshProfileList();
        }

        private void LoadProfileButtonClick(object sender, EventArgs e)
        {
            if (ProfileListView.SelectedItems.Count > 0)
            {
                // Professionbuddy.LoadProfile(Path.Combine(PB.ProfilePath, ProfileListView.SelectedItems[0].Name));
                ProfileManager.LoadNew(Path.Combine(_pb.ProfilePath, ProfileListView.SelectedItems[0].Name), true);
                // check for a LoadProfileAction and load the profile to stop all the crying from the lazy noobs 
                if (_pb.ProfileSettings.SettingsDictionary.Count > 0)
                    AddProfileSettingsTab();
                else
                    RemoveProfileSettingsTab();
            }
        }

        private void ToolStripReloadBtnClick(object sender, EventArgs e)
        {
            _pb.OnTradeSkillsLoaded += _pb.Professionbuddy_OnTradeSkillsLoaded;
            _pb.LoadTradeSkills();
        }

        private void ToolStripBotComboSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Professionbuddy.ChangeSecondaryBot((string) toolStripBotCombo.SelectedItem);
            }
            catch (Exception ex)
            {
                Professionbuddy.Err(ex.ToString());
            }
        }

        private void ToolStripBotConfigButtonClick(object sender, EventArgs e)
        {
            if (_pb.SecondaryBot != null)
                _pb.SecondaryBot.ConfigurationForm.ShowDialog();
        }

        private void ProfileListViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            LoadProfileButtonClick(null, null);
        }

        #region Nested type: CopyPasteOperactions

        [Flags]
        private enum CopyPasteOperactions
        {
            Cut = 0,
            IgnoreRoot = 1,
            Copy = 2
        };

        #endregion

        #region Nested type: guiInvokeCB

        private delegate void GuiInvokeCB();

        #endregion

        #region Nested type: refreshActionTreeDelegate

        private delegate void RefreshActionTreeDelegate(IPBComposite pbComposite, Type type);

        #endregion

        #region Initalize/update methods

        private PropertyGrid _settingsPropertyGrid;

        public MainForm()
        {
            try
            {
                Instance = this;
                _pb = Professionbuddy.Instance;
                InitializeComponent();
                // assign the localized strings
                toolStripOpen.Text = _pb.Strings["UI_FileOpen"];
                toolStripSave.Text = _pb.Strings["UI_FileSave"];
                toolStripHelp.Text = _pb.Strings["UI_Help"];
                toolStripCopy.Text = _pb.Strings["UI_Copy"];
                toolStripCut.Text = _pb.Strings["UI_Cut"];
                toolStripPaste.Text = _pb.Strings["UI_Paste"];
                toolStripDelete.Text = _pb.Strings["UI_Delete"];
                toolStripBotConfigButton.Text = _pb.Strings["UI_Settings"];
                ProfileTab.Text = _pb.Strings["UI_Profiles"];
                ActionsColumn.HeaderText = ActionsTab.Text = _pb.Strings["UI_Actions"];
                TradeSkillTab.Text = _pb.Strings["UI_Tradeskill"];
                TabPageProfile.Text = _pb.Strings["UI_Profile"];
                IngredientsColumn.HeaderText = _pb.Strings["UI_Ingredients"];
                NeedColumn.HeaderText = _pb.Strings["UI_Need"];
                BagsColumn.HeaderText = _pb.Strings["UI_Bags"];
                BankColumn.HeaderText = _pb.Strings["UI_Bank"];
                toolStripAddBtn.Text = _pb.Strings["UI_Add"];
                toolStripReloadBtn.Text = _pb.Strings["UI_Reload"];
                LoadProfileButton.Text = _pb.Strings["UI_LoadProfile"];

                saveFileDialog.InitialDirectory = _pb.ProfilePath;
                _profileWatcher = new FileSystemWatcher(_pb.ProfilePath)
                                      {NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName};
                _profileWatcher.Changed += ProfileWatcherChanged;
                _profileWatcher.Created += ProfileWatcherChanged;
                _profileWatcher.Deleted += ProfileWatcherChanged;
                _profileWatcher.Renamed += ProfileWatcherChanged;
                _profileWatcher.EnableRaisingEvents = true;

                // used by the dev to display the 'Secret button', a button that dumps some debug info of the Task list.
                if (Environment.UserName == "highvoltz")
                {
                    toolStripSecretButton.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Professionbuddy.Err(ex.ToString());
            }
        }

        public PropertyGrid SettingsPropertyGrid
        {
            get { return _settingsPropertyGrid; }
        }


        private void Initialize()
        {
            MainSplitContainer.Panel2MinSize = 390;
            RefreshProfileList();
            InitTradeSkillTab();
            InitActionTree();
            PopulateActionGridView();
            toolStripBotCombo.Items.AddRange(
                BotManager.Instance.Bots.Where(kv => kv.Key != _pb.Name).Select(kv => kv.Key).ToArray());
            UpdateBotCombo();
            if (_pb.HasDataStoreAddon && !toolStripAddCombo.Items.Contains("Banker"))
                toolStripAddCombo.Items.Add("Banker");
            toolStripAddCombo.SelectedIndex = 0;

            string imagePath = Path.Combine(Professionbuddy.BotPath, "Icons\\");
            toolStripOpen.Image = Image.FromFile(imagePath + "OpenPL.bmp");
            toolStripSave.Image = Image.FromFile(imagePath + "SaveHL.bmp");
            toolStripCopy.Image = Image.FromFile(imagePath + "copy.png");
            toolStripCut.Image = Image.FromFile(imagePath + "cut.png");
            toolStripPaste.Image = Image.FromFile(imagePath + "paste_32x32.png");
            toolStripDelete.Image = Image.FromFile(imagePath + "delete.png");
            toolStripAddBtn.Image = Image.FromFile(imagePath + "112_RightArrowLong_Orange_32x32_72.png");
            toolStripMaterials.Image = Image.FromFile(imagePath + "Notepad_32x32.png");
            toolStripHelp.Image = Image.FromFile(imagePath + "109_AllAnnotations_Help_32x32_72.png");

            if (_pb.ProfileSettings.SettingsDictionary.Count > 0)
                AddProfileSettingsTab();
            else
                RemoveProfileSettingsTab();

            UpdateControls();
        }

        private bool IsChildNode(TreeNode parent, TreeNode child)
        {
            if (child != null && (child.Parent == null))
                return false;
            if (child != null && child.Parent == parent)
                return true;
            return child != null && IsChildNode(parent, child.Parent);
        }


        private void AddToActionTree(object action, TreeNode dest)
        {
            bool ignoreRoot = (_copyAction & CopyPasteOperactions.IgnoreRoot) == CopyPasteOperactions.IgnoreRoot;
            bool cloneActions = (_copyAction & CopyPasteOperactions.Copy) == CopyPasteOperactions.Copy;
            TreeNode newNode;
            if (action is TreeNode)
            {
                if (cloneActions)
                {
                    newNode = RecursiveCloning(((TreeNode) action));
                }
                else
                    newNode = (TreeNode) ((TreeNode) action).Clone();
            }
            else if (action.GetType().GetInterface("IPBComposite") != null)
            {
                var composite = (IPBComposite) action;
                newNode = new TreeNode(composite.Title) {ForeColor = composite.Color, Tag = composite};
            }
            else
                return;
            ActionTree.SuspendLayout();
            if (dest != null)
            {
                int treeIndex = action is TreeNode && ((TreeNode) action).Parent == dest.Parent &&
                                ((TreeNode) action).Index <= dest.Index && !cloneActions
                                    ? dest.Index + 1
                                    : dest.Index;
                GroupComposite gc;
                // If, While and SubRoutines are Decorators...
                if (!ignoreRoot && dest.Tag is GroupComposite)
                    gc = (GroupComposite) dest.Tag;
                else
                    gc = (GroupComposite) ((Composite) dest.Tag).Parent;

                if ((dest.Tag is If || dest.Tag is SubRoutine) && !ignoreRoot)
                {
                    dest.Nodes.Add(newNode);
                    gc.AddChild((Composite) newNode.Tag);
                    if (!dest.IsExpanded)
                        dest.Expand();
                }
                else
                {
                    if (dest.Index >= gc.Children.Count)
                        gc.AddChild((Composite) newNode.Tag);
                    else
                        gc.InsertChild(dest.Index, (Composite) newNode.Tag);
                    if (dest.Parent == null)
                    {
                        if (treeIndex >= ActionTree.Nodes.Count)
                            ActionTree.Nodes.Add(newNode);
                        else
                            ActionTree.Nodes.Insert(treeIndex, newNode);
                    }
                    else
                    {
                        if (treeIndex >= dest.Parent.Nodes.Count)
                            dest.Parent.Nodes.Add(newNode);
                        else
                            dest.Parent.Nodes.Insert(treeIndex, newNode);
                    }
                }
            }
            else
            {
                ActionTree.Nodes.Add(newNode);
                _pb.PbBehavior.AddChild((Composite) newNode.Tag);
            }
            ActionTree.ResumeLayout();
        }

        private TreeNode RecursiveCloning(TreeNode node)
        {
            var newComp = (IPBComposite) (((IPBComposite) node.Tag).Clone());
            var newNode = new TreeNode(newComp.Title) {ForeColor = newComp.Color, Tag = newComp};
            foreach (TreeNode child in node.Nodes)
            {
                // If, While and SubRoutine are Decorators.
                var groupComposite = newComp as GroupComposite;
                if (groupComposite != null)
                {
                    GroupComposite gc = groupComposite;

                    TreeNode newChildNode = RecursiveCloning(child);
                    gc.AddChild((Composite) newChildNode.Tag);
                    newNode.Nodes.Add(newChildNode);
                }
            }
            return newNode;
        }

        private void DisableControls()
        {
            ActionTree.Enabled = false;
            toolStripAddBtn.Enabled = false;
            toolStripOpen.Enabled = false;
            toolStripDelete.Enabled = false;
            toolStripCopy.Enabled = false;
            toolStripCut.Enabled = false;
            toolStripPaste.Enabled = false;
            ActionGrid.Enabled = false;
            LoadProfileButton.Enabled = false;
            ProfileListView.Enabled = false;
            toolStripBotCombo.Enabled = false;
        }

        private void EnableControls()
        {
            ActionTree.Enabled = true;
            toolStripAddBtn.Enabled = true;
            toolStripOpen.Enabled = true;
            toolStripDelete.Enabled = true;
            toolStripCopy.Enabled = true;
            toolStripCut.Enabled = true;
            toolStripPaste.Enabled = true;
            ActionGrid.Enabled = true;
            LoadProfileButton.Enabled = true;
            ProfileListView.Enabled = true;
            toolStripBotCombo.Enabled = true;
        }

        public void AddProfileSettingsTab()
        {
            if (!IsValid)
                return;
            if (ProfileTab.InvokeRequired)
                ProfileTab.BeginInvoke(new GuiInvokeCB(AddProfileSettingsTabCallback));
            else
                AddProfileSettingsTabCallback();
        }

        private void AddProfileSettingsTabCallback()
        {
            RightSideTab.SuspendLayout();
            if (RightSideTab.TabPages.ContainsKey("ProfileSettings"))
            {
                RightSideTab.TabPages.RemoveByKey("ProfileSettings");
            }
            RightSideTab.TabPages.Add("ProfileSettings", _pb.Strings["UI_ProfileSettings"]);

            _settingsPropertyGrid = new PropertyGrid {Dock = DockStyle.Fill};
            RightSideTab.TabPages["ProfileSettings"].Controls.Add(_settingsPropertyGrid);

            _profilePropertyBag = new PropertyBag();
            foreach (var kv in _pb.ProfileSettings.SettingsDictionary)
            {
                if (!kv.Value.Hidden)
                {
                    _profilePropertyBag[kv.Key] = new MetaProp(kv.Key, kv.Value.Value.GetType(),
                                                               new DescriptionAttribute(kv.Value.Summary),
                                                               new CategoryAttribute(kv.Value.Category))
                                                      {Value = kv.Value.Value};
                    _profilePropertyBag[kv.Key].PropertyChanged += ProfileSettingsPropertyChanged;
                }
            }
            _settingsPropertyGrid.SelectedObject = _profilePropertyBag;
            RightSideTab.SelectTab(1);
            RightSideTab.ResumeLayout();
        }

        public void RefreshSettingsPropertyGrid()
        {
            if (!IsValid)
                return;
            if (ProfileTab.InvokeRequired)
                ProfileTab.BeginInvoke(new GuiInvokeCB(RefreshSettingsPropertyGridCallback));
            else
                RefreshSettingsPropertyGridCallback();
        }

        private void RefreshSettingsPropertyGridCallback()
        {
            foreach (var kv in _pb.ProfileSettings.SettingsDictionary)
            {
                MetaProp prop = _profilePropertyBag[kv.Key];
                if (prop != null)
                {
                    prop.PropertyChanged -= ProfileSettingsPropertyChanged;
                    prop.Value = kv.Value.Value;
                    prop.PropertyChanged += ProfileSettingsPropertyChanged;
                }
            }
            _settingsPropertyGrid.Refresh();
        }

        private void ProfileSettingsPropertyChanged(object sender, MetaPropArgs e)
        {
            _pb.ProfileSettings[((MetaProp) sender).Name] = ((MetaProp) sender).Value;
        }


        public void RemoveProfileSettingsTab()
        {
            if (!IsValid)
                return;
            if (ProfileTab.InvokeRequired)
                ProfileTab.BeginInvoke(new GuiInvokeCB(RemoveProfileSettingsTabCallback));
            else
                RemoveProfileSettingsTabCallback();
        }

        private void RemoveProfileSettingsTabCallback()
        {
            if (RightSideTab.TabPages.ContainsKey("ProfileSettings"))
            {
                _profilePropertyBag = null;
                RightSideTab.TabPages.RemoveByKey("ProfileSettings");
            }
        }

        private void UdateTreeNode(TreeNode node, IPBComposite pbComp, Type type, bool recursive)
        {
            var comp = (IPBComposite) node.Tag;
            if ((pbComp == null && type == null) ||
                (pbComp != null && pbComp == node.Tag) ||
                (type != null && type.IsInstanceOfType(node.Tag))
                )
            {
                node.ForeColor = comp.Color;
                node.Text = comp.Title;
            }
            if (recursive)
            {
                foreach (TreeNode child in node.Nodes)
                {
                    UdateTreeNode(child, pbComp, type, true);
                }
            }
        }

        private void ActionTreeAddChildren(GroupComposite ds, TreeNode node)
        {
            foreach (IPBComposite child in ds.Children)
            {
                var childNode = new TreeNode(child.Title) {ForeColor = child.Color, Tag = child};
                // If, While and SubRoutine are Decorators.
                var groupComposite = child as GroupComposite;
                if (groupComposite != null)
                {
                    ActionTreeAddChildren(groupComposite, childNode);
                }
                node.Nodes.Add(childNode);
            }
        }

        private void PopulateActionGridView()
        {
            ActionGridView.Rows.Clear();
            IEnumerable<Type> pbTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                                        where (typeof (IPBComposite)).IsAssignableFrom(t) && !t.IsAbstract
                                        select t;


            foreach (Type type in pbTypes)
            {
                var pa = (IPBComposite) Activator.CreateInstance(type);
                var row = new DataGridViewRow();
                var cell = new DataGridViewTextBoxCell {Value = pa.Name};
                row.Cells.Add(cell);
                row.Tag = pa;
                row.Height = 16;
                ActionGridView.Rows.Add(row);
                row.DefaultCellStyle.ForeColor = pa.Color;
            }
        }

        #region ProfileTab

        private void RefreshProfileList()
        {
            if (!IsValid)
                return;
            if (ProfileTab.InvokeRequired)
                ProfileTab.BeginInvoke(new GuiInvokeCB(RefreshProfileListCallback));
            else
                RefreshProfileListCallback();
        }

        private void RefreshProfileListCallback()
        {
            ProfileListView.SuspendLayout();
            ProfileListView.Clear();
            string[] profiles = Directory.GetFiles(_pb.ProfilePath, "*.xml", SearchOption.TopDirectoryOnly).
                Select(s => Path.GetFileName(s)).Union(Directory.GetFiles(_pb.ProfilePath, "*.package",
                                                                          SearchOption.TopDirectoryOnly)).
                Select(s => Path.GetFileName(s)).ToArray();
            // remove all profile names from ListView that are not in the 'profile' array              
            for (int i = 0; i < ProfileListView.Items.Count; i++)
            {
                if (!profiles.Contains(ProfileListView.Items[i].Name))
                    ProfileListView.Items.RemoveAt(i);
            } // Add all profiles that are not in ListView             
            foreach (string profile in profiles)
            {
                if (!ProfileListView.Items.ContainsKey(profile))
                    ProfileListView.Items.Add(profile, profile, null);
            }
            ProfileListView.ResumeLayout();
        }

        #endregion

        #region TradeSkillTab

        public void InitTradeSkillTab()
        {
            if (!IsValid)
                return;
            if (TradeSkillTabControl.InvokeRequired)
                TradeSkillTabControl.BeginInvoke(new GuiInvokeCB(InitTradeSkillTabCallback));
            else
                InitTradeSkillTabCallback();
        }

        private void InitTradeSkillTabCallback()
        {
            TradeSkillTabControl.SuspendLayout();
            TradeSkillTabControl.TabPages.Clear();
            for (int i = 0; i < _pb.TradeSkillList.Count; i++)
            {
                TradeSkillTabControl.TabPages.Add(new TradeSkillListView(i));
            }
            TradeSkillTabControl.ResumeLayout();

            if (_pb.TradeSkillList.Count > 0)
                TradeSkillTabControl.Visible = true;
        }

        #endregion

        #region UpdateBotCombo

        public void UpdateBotCombo()
        {
            if (!IsValid)
                return;
            if (TradeSkillTabControl.InvokeRequired)
                TradeSkillTabControl.BeginInvoke(new GuiInvokeCB(UpdateBotComboCallback));
            else
                UpdateBotComboCallback();
        }

        private void UpdateBotComboCallback()
        {
            int i = toolStripBotCombo.Items.IndexOf(ProfessionBuddySettings.Instance.LastBotBase);
            toolStripBotCombo.SelectedIndex = i >= 0 ? i : 1;
        }

        #endregion

        #region RefreshActionTree

        public void RefreshActionTree(Type type)
        {
            RefreshActionTree(null, type);
        }

        public void RefreshActionTree(IPBComposite pbComp)
        {
            RefreshActionTree(pbComp, null);
        }

        public void RefreshActionTree()
        {
            RefreshActionTree(null, null);
        }

        /// <summary>
        /// Refreshes all actions of specified type in ActionTree or all if type is null
        /// </summary>
        /// <param name="type"></param>
        public void RefreshActionTree(IPBComposite pbComp, Type type)
        {
            // Don't update ActionTree while PB is running to improve performance.
            if (_pb.IsRunning || !IsValid)
                return;
            if (ActionTree.InvokeRequired)
                ActionTree.BeginInvoke(new RefreshActionTreeDelegate(RefreshActionTreeCallback), pbComp, type);
            else
                RefreshActionTreeCallback(pbComp, type);
        }

        private void RefreshActionTreeCallback(IPBComposite pbComp, Type type)
        {
            ActionTree.SuspendLayout();
            foreach (TreeNode node in ActionTree.Nodes)
            {
                UdateTreeNode(node, pbComp, type, true);
            }
            ActionTree.ResumeLayout();
        }

        #endregion

        #region InitActionTree

        public void InitActionTree()
        {
            if (!IsValid)
                return;
            if (ActionTree.InvokeRequired)
                ActionTree.BeginInvoke(new GuiInvokeCB(InitActionTreeCallback));
            else
                InitActionTreeCallback();
        }

        private void InitActionTreeCallback()
        {
            ActionTree.SuspendLayout();
            int selectedIndex = -1;
            if (ActionTree.SelectedNode != null)
                selectedIndex = ActionTree.Nodes.IndexOf(ActionTree.SelectedNode);
            ActionTree.Nodes.Clear();
            foreach (IPBComposite composite in _pb.PbBehavior.Children)
            {
                var node = new TreeNode(composite.Title) {ForeColor = composite.Color, Tag = composite};
                if (composite is GroupComposite)
                {
                    ActionTreeAddChildren((GroupComposite) composite, node);
                }
                ActionTree.Nodes.Add(node);
            }
            //ActionTree.ExpandAll();
            if (selectedIndex != -1)
            {
                if (selectedIndex < ActionTree.Nodes.Count)
                    ActionTree.SelectedNode = ActionTree.Nodes[selectedIndex];
                else
                    ActionTree.SelectedNode = ActionTree.Nodes[ActionTree.Nodes.Count - 1];
            }
            ActionTree.ResumeLayout();
        }

        #endregion

        #region RefreshTradeSkillTabs

        public void RefreshTradeSkillTabs()
        {
            if (!IsValid)
                return;
            if (TradeSkillTabControl.InvokeRequired)
                TradeSkillTabControl.BeginInvoke(new GuiInvokeCB(RefreshTradeSkillTabsCallback));
            else
                RefreshTradeSkillTabsCallback();
        }

        private void RefreshTradeSkillTabsCallback()
        {
            foreach (TradeSkillListView tv in TradeSkillTabControl.TabPages)
            {
                tv.TradeDataView.SuspendLayout();
                foreach (DataGridViewRow row in tv.TradeDataView.Rows)
                {
                    var cell = (TradeSkillRecipeCell) row.Cells[0].Value;
                    row.Cells[1].Value = Util.CalculateRecipeRepeat(cell.Recipe);
                    row.Cells[2].Value = cell.Recipe.Difficulty;
                }
                tv.TradeDataView.ResumeLayout();
            }
        }

        #endregion

        #region UpdateControls

        // locks/unlocks controls depending on if PB is running on not.
        public void UpdateControls()
        {
            if (!IsValid)
                return;
            if (InvokeRequired)
                BeginInvoke(new GuiInvokeCB(UpdateControlsCallback));
            else
                UpdateControlsCallback();
        }

        private void UpdateControlsCallback()
        {
            if (_pb.IsRunning)
            {
                DisableControls();
                Text = string.Format("Profession Buddy - Running {0}",
                                     !string.IsNullOrEmpty(_pb.MySettings.LastProfile)
                                         ? "(" + Path.GetFileName(_pb.MySettings.LastProfile) + ")"
                                         : "");
            }
            else
            {
                EnableControls();
                Text = string.Format("Profession Buddy - Stopped {0}",
                                     !string.IsNullOrEmpty(_pb.MySettings.LastProfile)
                                         ? "(" + Path.GetFileName(_pb.MySettings.LastProfile) + ")"
                                         : "");
            }
        }

        #endregion

        private delegate void InitDelegate();

        #endregion
    }

    #endregion
}