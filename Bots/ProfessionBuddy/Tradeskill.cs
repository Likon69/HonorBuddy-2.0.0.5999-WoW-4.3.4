//!CompilerOption:Optimize:On
// Copyright Highvoltz 2012

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Patchables;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;

namespace HighVoltz
{

    #region TradeSkill

    public class TradeSkill
    {
        private const uint SkillLineAbilityFieldNum = 14;
        private const uint SkillLineAbilityEntrySize = SkillLineAbilityFieldNum*4; // number of fields * sizeof int

        public static readonly List<SkillLine> SupportedSkills = new List<SkillLine>
                                                                     {
                                                                         SkillLine.Alchemy,
                                                                         SkillLine.Blacksmithing,
                                                                         SkillLine.Cooking,
                                                                         SkillLine.Enchanting,
                                                                         SkillLine.Engineering,
                                                                         SkillLine.FirstAid,
                                                                         SkillLine.Inscription,
                                                                         SkillLine.Jewelcrafting,
                                                                         SkillLine.Leatherworking,
                                                                         SkillLine.Mining,
                                                                         // smelting
                                                                         SkillLine.Tailoring,
                                                                         SkillLine.Archaeology,
                                                                     };

        private static uint _knownSpellsPtr;
        private Dictionary<uint, IngredientSubClass> _ingredients;
        private List<Tool> _tools;

        static TradeSkill()
        {
            ProcessModule mod = ObjectManager.WoWProcess.MainModule;
            var baseAddress = (uint) mod.BaseAddress;
            if (GlobalPBSettings.Instance.WowVersion != mod.FileVersionInfo.FileVersion ||
                GlobalPBSettings.Instance.KnownSpellsPtr == 0)
            {
                Professionbuddy.Log("A new wow version has been detected\nScanning for new KnownSpellsPtr offset");
                try
                {
                    uint pointer =
                        Util.FindPattern(
                            "94 1E B3 00 8B CB 83 E1 1F B8 01 00 00 00 D3 E0 8B CB C1 E9 05 85 04 8A 0F 95 C0 84 C0 75",
                            "????xxxxxxxxxxxxxxxxxxxxxxxxxx");
                    GlobalPBSettings.Instance.KnownSpellsPtr = ObjectManager.Wow.Read<uint>(baseAddress + pointer) -
                                                               baseAddress;
                    GlobalPBSettings.Instance.WowVersion = mod.FileVersionInfo.FileVersion;
                    Professionbuddy.Log("Found KnownSpellsPtr offset for WoW Version {0} at offset 0x{1:X}",
                                        mod.FileVersionInfo.FileVersion, GlobalPBSettings.Instance.KnownSpellsPtr);
                    GlobalPBSettings.Instance.Save();
                }
                catch (InvalidDataException)
                {
                    Professionbuddy.Log(
                        "Unable to find KnownSpellsPtr offset for WoW Version {0}\nPlease notify the developer of this issue",
                        mod.FileVersionInfo.FileVersion);
                }
            }
        }

        private TradeSkill(WoWSkill skill)
        {
            WoWSkill = skill;
            Recipes = new Dictionary<uint, Recipe>();
        }

        public WoWSkill WoWSkill { get; internal set; }

        /// <summary>
        /// Amount of Bonus the player has to the Tradeskill 
        /// </summary>
        public uint Bonus
        {
            get { return WoWSkill.Bonus; }
        }

        /// <summary>
        /// Tradeskill level
        /// </summary>
        public int Level
        {
            get { return WoWSkill.CurrentValue; }
        }

        /// <summary>
        /// Maximum level of the Tradeskill 
        /// </summary>
        public int MaxLevel
        {
            get { return WoWSkill.MaxValue; }
        }

        /// <summary>
        /// Name of the Tradeskill
        /// </summary>
        public string Name
        {
            get { return WoWSkill.Name; }
        }

        public SkillLine SkillLine
        {
            get { return (SkillLine) WoWSkill.Id; }
        }

        /// <summary>
        /// List of ingredients 
        /// </summary>
        public Dictionary<uint, IngredientSubClass> Ingredients
        {
            get
            {
                if (_ingredients == null)
                {
                    InitIngredientList();
                }
                return _ingredients;
            }
        }

        /// <summary>
        /// List of Tools
        /// </summary>
        public List<Tool> Tools
        {
            get
            {
                if (_tools == null)
                {
                    InitToolList();
                }
                return _tools;
            }
        }

        /// <summary>
        /// List of known recipes
        /// </summary>
        public Dictionary<uint, Recipe> KnownRecipes
        {
            get
            {
                return Recipes.Where(r => r.Value.HasRecipe)
                    .ToDictionary(k => k.Key, v => v.Value);
            }
        }

        /// <summary>
        /// List of all recipes
        /// </summary>
        public Dictionary<uint, Recipe> Recipes { get; private set; }

        /// <summary>
        /// number of Recipes
        /// </summary>
        public int RecipeCount
        {
            get { return KnownRecipes.Count; }
        }

        private List<SkillLineAbilityEntry> GetSkillLineAbilityEntries()
        {
            var abilityList = new List<SkillLineAbilityEntry>();
            var targetSkillId = (int) SkillLine;

            WoWDb.DbTable table = StyxWoW.Db[ClientDb.SkillLineAbility];
            var minIndex = (uint) table.MinIndex;
            var topIndex = (uint) table.NumRows;
            uint bomIndex = 0;
            uint half;

            var firstRowPtr =
                ObjectManager.Wow.Read<uint>(((uint) ClientDb.SkillLineAbility + ObjectManager.Wow.ImageBase) + 0x14);
            uint id;
            // optimized search
            do
            {
                half = (topIndex + bomIndex)/2;
                id = ObjectManager.Wow.Read<uint>((firstRowPtr + half*SkillLineAbilityEntrySize) + 4); // skill
                if (id > targetSkillId)
                    topIndex = (topIndex + half)/2;
                else if (id < targetSkillId)
                    bomIndex = ((half + bomIndex)/2) + 1;
                else
                    break;
            } while (bomIndex < topIndex);

            var index = ObjectManager.Wow.Read<uint>((firstRowPtr + (half - 1)*SkillLineAbilityEntrySize));
            uint prevIndex = index;
            while (index > minIndex)
            {
                id = ObjectManager.Wow.Read<uint>((firstRowPtr + (half - 1)*SkillLineAbilityEntrySize) + 4);
                index = ObjectManager.Wow.Read<uint>((firstRowPtr + (half - 1)*SkillLineAbilityEntrySize));
                if (id != targetSkillId)
                    break;
                half--;
                prevIndex = index;
            }

            for (uint i = prevIndex; i <= table.MaxIndex;)
            {
                WoWDb.Row row = table.GetRow(i);
                var entry = row.GetStruct<SkillLineAbilityEntry>();
                if ((int) entry.SkillLine != targetSkillId)
                    break;
                abilityList.Add(entry);
                if (i != table.MaxIndex) // get next index
                    i = row.GetField<uint>((SkillLineAbilityFieldNum));
            }
            return abilityList;
        }

        private void AddRecipe(Recipe recipe)
        {
            if (!Recipes.ContainsKey(recipe.SpellId))
            {
                Recipes.Add(recipe.SpellId, recipe);
                recipe.InitIngredients();
                recipe.InitTools();
            }
        }

        private void InitIngredientList()
        {
            _ingredients = new Dictionary<uint, IngredientSubClass>();
            foreach (var recipePair in KnownRecipes)
            {
                recipePair.Value.InitIngredients();
            }
        }

        internal void InitToolList()
        {
            _tools = new List<Tool>();
            foreach (var recipePair in KnownRecipes)
            {
                recipePair.Value.InitTools();
            }
        }

        /// <summary>
        /// Syncs Ingredient and Tool list with Bags
        /// </summary>
        public void PulseBags()
        {
            if (!TreeRoot.IsRunning)
                ObjectManager.Update();
            foreach (var ingredPair in Ingredients)
            {
                ingredPair.Value.UpdateInBagsCount();
            }
            foreach (Tool tool in Tools)
            {
                tool.UpdateToolStatus();
            }
        }

        // syncs the TradeSkill, updating Skill level,recipe dificulty and adding new recipes that arent in list
        public void PulseSkill()
        {
            Update();
        }

        /// <summary>
        /// Updates the skill level, recipe difficulty and adds new recipes.
        /// </summary>
        public void Update()
        {
            if (!ObjectManager.IsInGame)
                return;
            // if HB is not running then we should maybe pulse objectmanger for item counts
            if (!TreeRoot.IsRunning)
                ObjectManager.Update();

            WoWSkill = ObjectManager.Me.GetSkill(SkillLine);
            Dictionary<uint, Recipe> oldList = KnownRecipes;
            // cycle through entire recipe list and check update the 'HasRecipe' property
            foreach (var recipe in Recipes)
            {
                recipe.Value.Update();
            }
            IEnumerable<KeyValuePair<uint, Recipe>> newRecipies = KnownRecipes.Except(oldList);
            foreach (var kv in newRecipies)
            {
                Professionbuddy.Log("Leaned a new recipe {0}", kv.Value.Name);
                using (new FrameLock())
                {
                    kv.Value.UpdateHeader();
                }
            }
            PulseBags();
        }

        /// <summary>
        /// Returns a list of recipes from selected skill
        /// </summary>
        /// <param name="skillLine"></param>
        /// <returns></returns>
        public static TradeSkill GetTradeSkill(SkillLine skillLine)
        {
            if (!ObjectManager.IsInGame)
                throw new InvalidOperationException("Must Be in game to call GetTradeSkill()");
            if (skillLine == 0 || !SupportedSkills.Contains(skillLine))
                throw new InvalidOperationException(String.Format("The tradekill {0} can not be loaded", skillLine));
            // if HB is not running then we need to pulse objectmanger for item counts
            if (!TreeRoot.IsRunning)
                ObjectManager.Update();
            //Stopwatch sw = new Stopwatch();
            TradeSkill tradeSkill = null;
            try
            {
                //using (new FrameLock())
                //{
                WoWSkill wowSkill = ObjectManager.Me.GetSkill(skillLine);
                // sw.Start();
                tradeSkill = new TradeSkill(wowSkill);

                List<SkillLineAbilityEntry> entries = tradeSkill.GetSkillLineAbilityEntries();
                foreach (SkillLineAbilityEntry entry in entries)
                {
                    // check if the entry is a recipe
                    if (entry.NextSpellId == 0 && entry.GreySkillLevel > 0)
                    {
                        var recipe = new Recipe(tradeSkill, entry);
                        recipe.UpdateHeader();
                        tradeSkill.AddRecipe(recipe);
                    }
                    //Logging.Write(entry.ToString());
                }
                //}
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
            //Logging.Write("it took {0} ms to load {1}", sw.ElapsedMilliseconds, skillLine);
            return tradeSkill;
        }

        /// <summary>
        /// Returns true if player has a given spell, works for recipes unlike the api that comes with HB.
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public static bool HasSpell(uint spellId)
        {
            if (spellId <= StyxWoW.Db[ClientDb.Spell].MaxIndex)
            {
                // GlobalPBSettings.Instance.KnownSpellsPtr is 0xB31E94 in WOW 4.3.3
                if (_knownSpellsPtr == 0)
                    _knownSpellsPtr =
                        ObjectManager.Wow.Read<uint>(GlobalPBSettings.Instance.KnownSpellsPtr +
                                                     ObjectManager.Wow.ImageBase);
                var value = ObjectManager.Wow.Read<uint>(_knownSpellsPtr + (spellId >> 5)*4);
                return (value & (1 << ((int) spellId & 0x1F))) != 0;
            }
            return false;
        }
    }

    #endregion

    #region Recipe

    public class Recipe
    {
        #region RecipeDifficulty enum

        public enum RecipeDifficulty
        {
            Optimal,
            Medium,
            Easy,
            Trivial
        }

        #endregion

        private readonly TradeSkill _parent;
        private uint? _craftedItemID;
        private string _header;
        private List<Ingredient> _ingredients;
        private string _name;
        private List<Tool> _tools;

        internal Recipe(TradeSkill parent, SkillLineAbilityEntry skillLineAbilityEntry)
        {
            _parent = parent;
            SpellId = skillLineAbilityEntry.SpellId;
            OrangeSkillLevel = skillLineAbilityEntry.OrangeSkillLevel;
            YellowSkillLevel = skillLineAbilityEntry.YellowSkillLevel;
            GreenSkillLevel = (skillLineAbilityEntry.YellowSkillLevel + skillLineAbilityEntry.GreySkillLevel)/2;
            GreySkillLevel = skillLineAbilityEntry.GreySkillLevel;
            OptimalSkillups = skillLineAbilityEntry.SkillPointsEarned;
            Skill = skillLineAbilityEntry.SkillLine;
            HasRecipe = TradeSkill.HasSpell(SpellId);
        }

        /// <summary>
        /// Returns the color that represents the recipes difficulty
        /// </summary>
        public Color Color
        {
            get
            {
                switch (Difficulty)
                {
                    case RecipeDifficulty.Optimal:
                        return Color.DarkOrange;
                    case RecipeDifficulty.Medium:
                        return Color.Yellow;
                    case RecipeDifficulty.Easy:
                        return Color.Lime;
                    default:
                        return Color.Gray;
                }
            }
        }

        /// <summary>
        /// The Number of times recipe can be crafted with current mats in bags using internal Ingredient list.
        /// </summary>
        public uint CanRepeatNum
        {
            get
            {
                uint repeat = uint.MaxValue;
                foreach (Ingredient ingred in Ingredients)
                {
                    var cnt = (uint) Math.Floor(ingred.InBagItemCount/(double) ingred.Required);
                    if (repeat > cnt)
                    {
                        repeat = cnt;
                    }
                }
                return repeat;
            }
        }

        /// <summary>
        /// The Number of times recipe can be crafted with current mats in bags. Checks directly with ObjectManager
        /// </summary>
        public uint CanRepeatNum2
        {
            get
            {
                uint repeat = uint.MaxValue;
                foreach (Ingredient ingred in Ingredients)
                {
                    var cnt = (uint) Math.Floor(Ingredient.GetInBagItemCount(ingred.ID)/(double) ingred.Required);
                    if (repeat > cnt)
                    {
                        repeat = cnt;
                    }
                }
                return repeat;
            }
        }

        /// <summary>
        /// Returns ID of the item the recipe makes
        /// </summary>
        public uint CraftedItemID
        {
            get
            {
                if (_craftedItemID.HasValue)
                    return _craftedItemID.Value;
                _craftedItemID = GetCraftedItemID();
                return (_craftedItemID.HasValue) ? _craftedItemID.Value : 0;
            }
        }

        public int DisplayOrder { get; private set; }

        /// <summary>
        /// Returns the difficulty of the Recipe
        /// </summary>
        public RecipeDifficulty Difficulty
        {
            get
            {
                int level = _parent.WoWSkill.CurrentValue;
                if (level < YellowSkillLevel)
                    return RecipeDifficulty.Optimal;
                if (level < GreenSkillLevel)
                    return RecipeDifficulty.Medium;
                if (level < GreySkillLevel)
                    return RecipeDifficulty.Easy;
                return RecipeDifficulty.Trivial;
            }
        }

        public bool HasRecipe { get; set; }

        // grab name from dbc

        public int GreySkillLevel { get; private set; }

        public int GreenSkillLevel { get; private set; }

        /// <summary>
        /// Name of header this recipe belongs to
        /// </summary>
        public string Header
        {
            get
            {
                if (_header == null || string.IsNullOrEmpty(_header))
                {
                    using (new FrameLock())
                    {
                        foreach (var recipe in _parent.KnownRecipes)
                        {
                            UpdateHeader();
                        }
                    }
                }
                return _header;
            }
        }

        /// <summary>
        /// List of ingredients required for the recipe
        /// </summary>
        public ReadOnlyCollection<Ingredient> Ingredients
        {
            get
            {
                if (_ingredients == null)
                    InitIngredients();
                if (_ingredients != null)
                    return _ingredients.AsReadOnly();
                return null;
            }
        }

        /// <summary>
        /// Name of the Recipe
        /// </summary>
        public string Name
        {
            get { return _name ?? (_name = GetName()); }
        }

        public int OrangeSkillLevel { get; private set; }

        /// <summary>
        /// Number of skillups earned when recipe difficulty is orange (optimal)
        /// </summary>
        public int OptimalSkillups { get; private set; }

        public uint SpellId { get; private set; }

        /// <summary>
        /// The Skill this recipe is from
        /// </summary>
        public SkillLine Skill { get; private set; }

        /// <summary>
        /// Number of Skillup this recipe will grant when crafted using current tradeskill level
        /// </summary>
        public int Skillups
        {
            get
            {
                switch (Difficulty)
                {
                    case RecipeDifficulty.Optimal:
                        return OptimalSkillups;
                    case RecipeDifficulty.Medium:
                    case RecipeDifficulty.Easy:
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        /// returns the spell that's attached to the recipe
        /// </summary>
        public WoWSpell Spell
        {
            get { return WoWSpell.FromId((int) SpellId); }
        }

        public ReadOnlyCollection<Tool> Tools
        {
            get
            {
                if (_tools == null)
                    InitTools();
                if (_tools != null)
                    return _tools.AsReadOnly();
                return null;
            }
        }

        public int YellowSkillLevel { get; private set; }

        private uint? GetCraftedItemID()
        {
            if (Spell != null)
            {
                return Spell.SpellEffect1.ItemType;
            }
            return null;
        }

        internal void InitIngredients()
        {
            // instantizing ingredients in here and doing a null check to prevent recursion from Trade.Ingredients() 
            if (_ingredients != null)
                return;
            uint recipeID = SpellId;
            _ingredients = new List<Ingredient>();
            WoWDb.DbTable spelldbTable = StyxWoW.Db[ClientDb.Spell];
            if (spelldbTable != null && recipeID <= spelldbTable.MaxIndex && recipeID >= spelldbTable.MinIndex)
            {
                WoWDb.Row spelldbRow = spelldbTable.GetRow(recipeID);
                if (spelldbRow != null)
                {
                    var reagentIndex = spelldbRow.GetField<uint>((uint) SpellDB.SpellReagentsIndex);
                        // Changed to 43 in WoW 4.2
                    WoWDb.DbTable reagentDbTable = StyxWoW.Db[ClientDb.SpellReagents];
                    if (reagentDbTable != null && reagentIndex <= reagentDbTable.MaxIndex &&
                        reagentIndex >= reagentDbTable.MinIndex)
                    {
                        WoWDb.Row reagentDbRow = reagentDbTable.GetRow(reagentIndex);
                        for (uint index = 1; index <= 8; index++)
                        {
                            var id = reagentDbRow.GetField<uint>(index);
                            if (id != 0)
                            {
                                _ingredients.Add(new Ingredient(id, reagentDbRow.GetField<uint>(index + 8),
                                                                _parent.Ingredients));
                            }
                        }
                    }
                }
            }
        }

        public void UpdateHeader()
        {
            if (!string.IsNullOrEmpty(_header))
                return;
            WoWDb.DbTable dbTable;
            WoWDb.Row dbRow;
            if (CraftedItemID == 0)
            {
                dbTable = StyxWoW.Db[ClientDb.SkillLine];
                dbRow = dbTable.GetRow((uint) Skill);
                _header = ObjectManager.Wow.Read<string>(dbRow.GetField<uint>(5));
            }
            else
            {
                // we need to iterate through ClientDb.ItemSubClass till we find matching
                WoWCache.InfoBlock cache = StyxWoW.Cache[CacheDb.ItemClass].GetInfoBlockById(CraftedItemID);

                if (cache != null)
                {
                    dbTable = StyxWoW.Db[ClientDb.ItemSubClass];
                    for (int i = dbTable.MinIndex; i <= dbTable.MaxIndex; i++)
                    {
                        dbRow = dbTable.GetRow((uint) i);
                        var iSubClass1 = dbRow.GetField<int>(1);
                        var iSubClass2 = dbRow.GetField<int>(2);
                        if (iSubClass1 == cache.Item.ClassId && iSubClass2 == cache.Item.SubClassId)
                        {
                            var stringPtr = dbRow.GetField<uint>(12);
                            // if pointer in field (12) is 0 or it points to null string then use field (11)
                            if (stringPtr == 0 ||
                                string.IsNullOrEmpty(_header = ObjectManager.Wow.Read<string>(stringPtr)))
                            {
                                stringPtr = dbRow.GetField<uint>(11);
                                if (stringPtr != 0)
                                    _header = ObjectManager.Wow.Read<string>(stringPtr);
                            }
                            break;
                        }
                    }
                }
            }
        }

        // grab name from dbc
        private string GetName()
        {
            string getName = null;
            WoWDb.DbTable t = StyxWoW.Db[ClientDb.Spell];
            WoWDb.Row r = t.GetRow(SpellId);
            var stringPtr = r.GetField<uint>((uint) SpellDB.NamePtr);
            if (stringPtr != 0)
            {
                getName = ObjectManager.Wow.Read<string>(stringPtr);
            }
            return getName;
        }

        internal void InitTools()
        {
            // instantizing tools in here and doing a null check to prevent recursion from Trade.Tools() 
            if (_tools != null)
                return;
            _tools = new List<Tool>();
            WoWDb.DbTable t = StyxWoW.Db[ClientDb.Spell];
            WoWDb.Row spellDbRow = t.GetRow(SpellId);
            var spellReqIndex = spellDbRow.GetField<uint>((uint) SpellDB.SpellCastingReqIndex);
                // changed from 33 to 34 in WOW 4.2
            if (spellReqIndex != 0)
            {
                t = StyxWoW.Db[ClientDb.SpellCastingRequirements];
                WoWDb.Row spellReqDbRow = t.GetRow(spellReqIndex);
                var spellFocusIndex = spellReqDbRow.GetField<uint>(6);
                // anvils, forge etc
                if (spellFocusIndex != 0)
                {
                    _tools.Add(GetTool(spellFocusIndex, Tool.ToolType.SpellFocus));
                }
                var areaGroupIndex = spellReqDbRow.GetField<uint>(4);
                if (areaGroupIndex != 0)
                {
                    t = StyxWoW.Db[ClientDb.AreaGroup];
                    WoWDb.Row areaGroupDbRow = t.GetRow(areaGroupIndex);
                    var areaTableIndex = areaGroupDbRow.GetField<uint>(1);
                    // not sure which kind of tools this covers
                    if (areaTableIndex != 0)
                    {
                        _tools.Add(GetTool(areaTableIndex, Tool.ToolType.AreaTable));
                    }
                }
            }
            var spellTotemsIndex = spellDbRow.GetField<uint>((uint) SpellDB.SpellTotemsIndex);
                // changed from 45 to 46 in WOW 4.2
            if (spellTotemsIndex != 0)
            {
                t = StyxWoW.Db[ClientDb.SpellTotems];
                WoWDb.Row spellTotemsDbRow = t.GetRow(spellTotemsIndex);
                uint cacheIndex = 0, totemCategoryIndex = 0;
                for (uint i = 1; i <= 4 && cacheIndex == 0; i++)
                {
                    if (cacheIndex == 0 && i >= 3)
                        cacheIndex = spellTotemsDbRow.GetField<uint>(i);
                    if (totemCategoryIndex == 0 && i <= 2)
                        totemCategoryIndex = spellTotemsDbRow.GetField<uint>(i);
                }
                // not sure which kind of tools this covers
                if (cacheIndex != 0)
                {
                    _tools.Add(GetTool(cacheIndex, Tool.ToolType.Item));
                }
                // Blacksmith hammer, mining pick 
                if (totemCategoryIndex != 0)
                {
                    _tools.Add(GetTool(totemCategoryIndex, Tool.ToolType.Totem));
                }
            }
        }

        // this basically checks if the master tool list already contains this tool and 
        // returns that tool if it does, otherwise it adds the tool to the master Tool list
        // and returns it.
        private Tool GetTool(uint index, Tool.ToolType toolType)
        {
            var newTool = new Tool(index, toolType);
            Tool tool = _parent.Tools.Find(a => a.Equals(newTool));
            if (tool == null)
            {
                tool = newTool;
                _parent.Tools.Add(tool);
            }
            return tool;
        }

        public void Update()
        {
            HasRecipe = TradeSkill.HasSpell(SpellId);
        }

        #region Nested type: SpellDB

        private enum SpellDB
        {
            NamePtr = 21,
            SpellCastingReqIndex = 34,
            SpellReagentsIndex = 43,
            SpellTotemsIndex = 46,
        };

        #endregion
    }

    #endregion

    #region IngredientSubClass

    // This class is used in a dictionary in the TradeSkill class and the purpose is to 
    // keep track of the number of ingredients in players bag,updated via Pulse().
    // this saves cpu cycles because it doesnt need to iterate through all the recipes.
    // Also this saves memory usage because each ingredient name gets loaded only once per ingredient.

    // someone give this a good name, kthz
    public class IngredientSubClass
    {
        private static readonly object NameLockObject = new object();
        internal Ingredient Parent;
        private string _name;

        internal IngredientSubClass(Ingredient parent, uint inBagCount)
        {
            Parent = parent;
            InBagsCount = inBagCount;
        }

        public string Name
        {
            get
            {
                lock (NameLockObject)
                {
                    return _name ?? (_name = GetName());
                }
            }
        }

        /// <summary>
        /// number of ingredients in players bags
        /// </summary>
        public uint InBagsCount { get; internal set; }

        private string GetName()
        {
            string name = Util.GetItemCacheName(Parent.ID);
            if (name == null)
            {
                // ok so it couldn't find the item in cache. since we're going to need to force a load
                // might as well do a framelock and try load all the items. 
                IEnumerable<uint> ids = Parent.MasterList.Keys.Where(id => id != Parent.ID);

                using (new FrameLock())
                {
                    foreach (uint id in ids)
                    {
                        name = Util.GetItemCacheName(id);
                    }
                }
            }
            return name;
        }

        /// <summary>
        /// updates the InBagsCount
        /// </summary>
        internal void UpdateInBagsCount()
        {
            InBagsCount = Ingredient.GetInBagItemCount(Parent.ID);
        }
    }

    #endregion

    #region Ingredient

    public class Ingredient
    {
        // someone give this a good name, kthz
        private readonly IngredientSubClass _subclass;
        // list where every ingredient is stored, used to save memory usage,
        // this points to the one initilized in a TradeSkill instance
        internal Dictionary<uint, IngredientSubClass> MasterList;

        internal Ingredient(uint id, uint requiredNum, Dictionary<uint, IngredientSubClass> masterList)
        {
            ID = id;
            Required = requiredNum;
            MasterList = masterList;
            if (masterList.ContainsKey(id))
            {
                _subclass = masterList[id];
            }
            else
            {
                _subclass = new IngredientSubClass(this, GetInBagItemCount(id));
                masterList.Add(id, _subclass);
            }
        }

        /// <summary>
        /// Name of the Reagent
        /// </summary>
        public string Name
        {
            get { return _subclass.Name; }
        }

        /// <summary>
        /// The required number of this reagent needed
        /// </summary>
        public uint Required { get; private set; }

        public uint ID { get; private set; }

        /// <summary>
        /// Number of this Reagent in players possession
        /// </summary>
        public uint InBagItemCount
        {
            get { return _subclass.InBagsCount; }
        }

        public static uint GetInBagItemCount(uint id)
        {
            try
            {
                return
                    (uint)
                    ObjectManager.Me.BagItems.Sum(i => i != null && i.IsValid && i.Entry == id ? i.StackCount : 0);
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
                return 0;
            }
        }
    }

    #endregion

    #region Tool

    public class Tool
    {
        private readonly uint _index; // index to some DBC, depends on type
        private readonly ToolType _toolType;
        private string _name;

        internal Tool(uint index, ToolType toolType)
        {
            _index = index;
            _toolType = toolType;
            UpdateToolStatus();
            ID = toolType == ToolType.Item ? index : 0;
        }

        public uint ID { get; private set; }

        /// <summary>
        /// returns true if tool is in players bags
        /// </summary>
        public bool HasTool { get; internal set; }

        /// <summary>
        /// Name of the tool
        /// </summary>
        public string Name
        {
            get { return _name ?? (_name = GetName()); }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Tool)
            {
                var t = obj as Tool;
                return _index == t._index && _toolType == t._toolType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int) _index + (int) _toolType*100000;
        }

        private string GetName()
        {
            string name = null;
            uint stringPtr = 0;
            switch (_toolType)
            {
                case ToolType.SpellFocus:
                    WoWDb.DbTable t = StyxWoW.Db[ClientDb.SpellFocusObject];
                    WoWDb.Row r = t.GetRow(_index);
                    stringPtr = r.GetField<uint>(1);
                    break;
                case ToolType.AreaTable:
                    t = StyxWoW.Db[ClientDb.AreaTable];
                    r = t.GetRow(_index);
                    stringPtr = r.GetField<uint>(11);
                    break;
                case ToolType.Item:
                    name = Util.GetItemCacheName(_index);
                    break;
                case ToolType.Totem:
                    t = StyxWoW.Db[ClientDb.TotemCategory];
                    r = t.GetRow(_index);
                    stringPtr = r.GetField<uint>(1);
                    break;
            }
            if (stringPtr != 0)
            {
                name = ObjectManager.Wow.Read<string>(stringPtr);
            }
            return name;
        }

        internal void UpdateToolStatus()
        {
            switch (_toolType)
            {
                case ToolType.SpellFocus:
                case ToolType.AreaTable:
                    HasTool = true;
                    break;
                case ToolType.Item:
                case ToolType.Totem:
                    if (ID != 0)
                    {
                        HasTool = ObjectManager.GetObjectsOfType<WoWItem>().Any(i => i.Entry == ID);
                    }
                    else
                    {
                        WoWItem item = ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(i => i.Name == Name);
                        if (item != null)
                        {
                            ID = item.Entry;
                            HasTool = true;
                        }
                        else
                            HasTool = false;
                    }
                    break;
            }
        }

        #region Nested type: ToolType

        internal enum ToolType
        {
            SpellFocus,
            AreaTable,
            Item,
            Totem
        }

        #endregion
    }

    #endregion

    #region SkillLineAbilityEntry

    internal struct SkillLineAbilityEntry
    {
        #region RecipeAquireMethod enum

        public enum RecipeAquireMethod
        {
            /// <summary>
            /// Recipes that are bought from the trainer
            /// </summary>
            TrainerBuy,

            /// <summary>
            /// Recipes that are aquired automatically upon training a skill
            /// </summary>
            TrainerAuto,

            /// <summary>
            /// Recipes that are aquired through a quest.
            /// </summary>
            Quest
        }

        #endregion

        /// <summary>
        /// SkillLineAbilityId
        /// </summary>
        public uint Id { get; private set; }

        public SkillLine SkillLine { get; private set; }

        /// <summary>
        /// The SpellId for recipe
        /// </summary>
        public uint SpellId { get; private set; }

        /// <summary>
        /// Required race bitmask for ChrRaces.dbc
        /// </summary>
        public uint ReqRaces { get; private set; }

        /// <summary>
        /// Required class bitmask for ChrClasses.dbc
        /// </summary>
        public uint ReqClasses { get; private set; }

        /// <summary>
        /// Excluded race bitmask for ChrRaces.dbc
        /// </summary>
        public uint ExclRaces { get; private set; }

        /// <summary>
        /// Excluded class bitmask for ChrClasses.dbc
        /// </summary>
        public uint ExclClasses { get; private set; }

        /// <summary>
        /// The skill level that recipe is shown as orange (optimal) difficulty
        /// </summary>
        public int OrangeSkillLevel { get; private set; }

        /// <summary>
        /// The next Skill rank spellId - 0 for Recipes
        /// </summary>
        public uint NextSpellId { get; private set; }

        /// <summary>
        /// How the recipe is aquired
        /// </summary>
        public RecipeAquireMethod AquireMethod { get; private set; }

        /// <summary>
        /// The skill level that recipe is shown as gray (trivial) difficulty
        /// </summary>
        public int GreySkillLevel { get; private set; }

        /// <summary>
        /// The skill level that recipe is shown as yellow (medium) difficulty
        /// </summary>
        public int YellowSkillLevel { get; private set; }

        /// <summary>
        /// The amount of skill points earned when gaining a skillup while recipe is at optimal difficulty
        /// </summary>
        public int SkillPointsEarned { get; private set; }

        /// <summary>
        /// The order in which the entries are displayed in the tradeskill frame.
        /// </summary>
        public int DisplayOrder { get; private set; }

        public override string ToString()
        {
            return
                string.Format(
                    "{0} SkillLine: {1},NextSpellId: {2},AquireMethod: {3},OrangeSkillLevel: {4},YellowSkillLevel: {5}\n\tGreenSkillLevel: {6},GreySkillLevel: {7},SkillPointsEarned: {8},DisplayOrder: {9}",
                    SpellId, SkillLine, NextSpellId, AquireMethod, OrangeSkillLevel, YellowSkillLevel,
                    (YellowSkillLevel + GreySkillLevel)/2, GreySkillLevel, SkillPointsEarned, DisplayOrder);
        }
    }

    #endregion
}