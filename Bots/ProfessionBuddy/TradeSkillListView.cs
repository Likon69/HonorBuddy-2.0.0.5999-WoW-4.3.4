using System;
using System.Drawing;
using System.Windows.Forms;

namespace HighVoltz
{
    public class TradeSkillListView : TabPage
    {
        private readonly int index; // index to Professionbuddy.Instance.TradeSkillList
        private readonly TableLayoutPanel tabTableLayout;

        public TradeSkillListView()
            : this(0)
        {
        }

        public TradeSkillListView(int index)
        {
            this.index = index;
            // Filter TextBox
            FilterText = new TextBox();
            FilterText.Dock = DockStyle.Fill;
            // Category Combobox
            CategoryCombo = new ComboBox();
            CategoryCombo.Dock = DockStyle.Fill;
            // columns
            NameColumn = new DataGridViewTextBoxColumn();
            CraftableColumn = new DataGridViewTextBoxColumn();
            DifficultyColumn = new DataGridViewTextBoxColumn();
            NameColumn.HeaderText = Professionbuddy.Instance.Strings["UI_Name"];
            CraftableColumn.HeaderText = "#";
            NameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            CraftableColumn.MinimumWidth = 25;
            CraftableColumn.Width = 25;
            DifficultyColumn.MinimumWidth = 25;
            DifficultyColumn.Width = 25;
            // DataGridView
            TradeDataView = new DataGridView();
            TradeDataView.Dock = DockStyle.Fill;
            TradeDataView.AllowUserToAddRows = false;
            TradeDataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            TradeDataView.RowHeadersVisible = false;
            TradeDataView.Columns.Add(NameColumn);
            TradeDataView.Columns.Add(CraftableColumn);
            TradeDataView.Columns.Add(DifficultyColumn);
            TradeDataView.AllowUserToResizeRows = false;
            TradeDataView.EditMode = DataGridViewEditMode.EditProgrammatically;
            TradeDataView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            TradeDataView.ColumnHeadersHeight = 21;
            TradeDataView.RowTemplate.Height = 16;
            //table layout
            tabTableLayout = new TableLayoutPanel();
            tabTableLayout.ColumnCount = 2;
            tabTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tabTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tabTableLayout.RowCount = 2;
            tabTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tabTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tabTableLayout.Controls.Add(FilterText, 0, 0);
            tabTableLayout.Controls.Add(CategoryCombo, 1, 0);
            tabTableLayout.Controls.Add(TradeDataView, 0, 1);
            tabTableLayout.Dock = DockStyle.Fill;
            tabTableLayout.SetColumnSpan(TradeDataView, 2);
            // tab
            Controls.Add(tabTableLayout);
            Text = Professionbuddy.Instance.TradeSkillList[index].Name;
            // populate the controls with data
            CategoryCombo.Items.Add(""); // blank line will show all headers...

            foreach (var kv in Professionbuddy.Instance.TradeSkillList[index].KnownRecipes)
            {
                if (!CategoryCombo.Items.Contains(kv.Value.Header))
                {
                    CategoryCombo.Items.Add(kv.Value.Header);
                }
                TradeDataView.Rows.Add(new TradeSkillRecipeCell(index, kv.Key), Util.CalculateRecipeRepeat(kv.Value),
                                       (int) kv.Value.Difficulty); // make color column sortable by dificulty..
            }
            TradeDataView_SelectionChanged(null, null);
            // hook events
            FilterText.TextChanged += FilterText_TextChanged;
            CategoryCombo.SelectedValueChanged += SectionCombo_SelectedValueChanged;
            TradeDataView.SelectionChanged += TradeDataView_SelectionChanged;
            TradeDataView.CellFormatting += TradeDataView_CellFormatting;
        }

        public DataGridViewTextBoxColumn NameColumn { get; private set; }
        public DataGridViewTextBoxColumn CraftableColumn { get; private set; }
        public DataGridViewTextBoxColumn DifficultyColumn { get; private set; }
        public DataGridView TradeDataView { get; private set; }
        public TextBox FilterText { get; private set; }
        public ComboBox CategoryCombo { get; private set; }

        public int TradeIndex
        {
            get { return index; }
        }

        private void TradeDataView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (TradeDataView.Columns[e.ColumnIndex].HeaderText == "")
            {
                var tsrc = TradeDataView.Rows[e.RowIndex].Cells[0].Value as TradeSkillRecipeCell;
                e.CellStyle.ForeColor = tsrc.Recipe.Color;
                e.CellStyle.BackColor = e.CellStyle.ForeColor;
                e.CellStyle.SelectionBackColor = e.CellStyle.ForeColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }
        }

        private void TradeDataView_SelectionChanged(object sender, EventArgs e)
        {
            if (MainForm.IsValid)
            {
                MainForm.Instance.IngredientsView.Rows.Clear();
                if (TradeDataView.SelectedRows.Count > 0)
                {
                    var cell = (TradeSkillRecipeCell) TradeDataView.SelectedRows[0].Cells[0].Value;
                    Recipe _recipe = Professionbuddy.Instance.TradeSkillList[index].KnownRecipes[cell.RecipeID];
                    var row = new DataGridViewRow();
                    foreach (Ingredient ingred in _recipe.Ingredients)
                    {
                        uint inBags = ingred.InBagItemCount;
                        MainForm.Instance.IngredientsView.Rows.
                            Add(ingred.Name, ingred.Required, inBags);
                        if (ingred.InBagItemCount < ingred.Required)
                        {
                            MainForm.Instance.IngredientsView.Rows[MainForm.Instance.IngredientsView.Rows.Count - 1].
                                Cells[2].Style.SelectionBackColor = Color.Red;
                            MainForm.Instance.IngredientsView.Rows[MainForm.Instance.IngredientsView.Rows.Count - 1].
                                Cells[2].Style.ForeColor = Color.Red;
                        }
                        MainForm.Instance.IngredientsView.ClearSelection();
                    }
                }
            }
        }

        private void FilterText_TextChanged(object sender, EventArgs e)
        {
            filterTradeDateView();
        }

        private void SectionCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            filterTradeDateView();
        }

        private void filterTradeDateView()
        {
            TradeDataView.Rows.Clear();
            string filter = FilterText.Text.ToUpper();
            bool noFilter = string.IsNullOrEmpty(FilterText.Text);
            bool showAllCategories = string.IsNullOrEmpty(CategoryCombo.Text);
            foreach (var kv in Professionbuddy.Instance.TradeSkillList[index].KnownRecipes)
            {
                if ((noFilter || kv.Value.Name.ToUpper().Contains(filter)) &&
                    (showAllCategories || kv.Value.Header == CategoryCombo.Text))
                {
                    TradeDataView.Rows.Add(new TradeSkillRecipeCell(index, kv.Key), kv.Value.CanRepeatNum,
                                           kv.Value.Color);
                }
            }
        }
    }

    // attached to the TradeSkillListView cell values

    internal class TradeSkillRecipeCell
    {
        public TradeSkillRecipeCell(int index, uint id)
        {
            TradeSkillIndex = index;
            RecipeID = id;
        }

        public string RecipeName
        {
            get { return Professionbuddy.Instance.TradeSkillList[TradeSkillIndex].KnownRecipes[RecipeID].Name; }
        }

        public uint RecipeID { get; private set; }

        public Recipe Recipe
        {
            get { return Professionbuddy.Instance.TradeSkillList[TradeSkillIndex].KnownRecipes[RecipeID]; }
        }

        public int TradeSkillIndex { get; private set; }

        public override string ToString()
        {
            return RecipeName;
        }
    }
}