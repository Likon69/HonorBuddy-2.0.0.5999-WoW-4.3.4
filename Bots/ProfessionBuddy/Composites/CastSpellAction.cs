using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using HighVoltz.Dynamic;
using Styx;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{

    #region CastSpellAction

    public class RecipeConverter : ExpandableObjectConverter
    {
    }

    public sealed class CastSpellAction : PBAction
    {
        // number of times the recipe will be crafted

        #region RepeatCalculationType enum

        public enum RepeatCalculationType
        {
            Specific,
            Craftable,
            Banker,
        }

        #endregion

        private readonly Stopwatch _spamControl;
        private uint _recastTime;
        private uint _waitTime;

        public CastSpellAction()
        {
            _spamControl = new Stopwatch();
            QueueIsRunning = false;
            Properties["Casted"] = new MetaProp("Casted", typeof (int), new ReadOnlyAttribute(true),
                                                new DisplayNameAttribute(Pb.Strings["Action_CastSpellAction_Casted"]));

            Properties["SpellName"] = new MetaProp("SpellName", typeof (string), new ReadOnlyAttribute(true),
                                                   new DisplayNameAttribute(Pb.Strings["Action_Common_SpellName"]));

            Properties["Repeat"] = new MetaProp("Repeat", typeof (DynamicProperty<int>),
                                                new TypeConverterAttribute(
                                                    typeof (DynamicProperty<int>.DynamivExpressionConverter)),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_Repeat"]));

            Properties["Entry"] = new MetaProp("Entry", typeof (uint),
                                               new DisplayNameAttribute(Pb.Strings["Action_Common_SpellEntry"]));

            Properties["CastOnItem"] = new MetaProp("CastOnItem", typeof (bool),
                                                    new DisplayNameAttribute(
                                                        Pb.Strings["Action_CastSpellAction_CastOnItem"]));

            Properties["ItemType"] = new MetaProp("ItemType", typeof (InventoryType),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_ItemType"]));

            Properties["ItemId"] = new MetaProp("ItemId", typeof (uint),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_ItemEntry"]));

            Properties["RepeatType"] = new MetaProp("RepeatType", typeof (RepeatCalculationType),
                                                    new DisplayNameAttribute(
                                                        Pb.Strings["Action_CastSpellAction_RepeatType"]));
            // Properties["Recipe"] = new MetaProp("Recipe", typeof(Recipe), new TypeConverterAttribute(typeof(RecipeConverter)));

            Casted = 0;
            Repeat = new DynamicProperty<int>(this, "1");
            RegisterDynamicProperty("Repeat");
            Entry = 0u;
            RepeatType = RepeatCalculationType.Craftable;
            Recipe = null;
            CastOnItem = false;
            ItemType = InventoryType.Chest;
            ItemId = 0u;
            Properties["SpellName"].Value = SpellName;

            //Properties["Recipe"].Show = false;
            Properties["ItemType"].Show = false;
            Properties["ItemId"].Show = false;
            Properties["Casted"].PropertyChanged += OnCounterChanged;
            CheckTradeskillList();
            Properties["RepeatType"].PropertyChanged += CastSpellActionPropertyChanged;
            Properties["Entry"].PropertyChanged += OnEntryChanged;
            Properties["CastOnItem"].PropertyChanged += CastOnItemChanged;
        }

        public CastSpellAction(Recipe recipe, int repeat, RepeatCalculationType repeatType)
            : this()
        {
            Recipe = recipe;
            Repeat = new DynamicProperty<int>(this, repeat.ToString(CultureInfo.InvariantCulture));
            Entry = recipe.SpellId;
            RepeatType = repeatType;
            //Properties["Recipe"].Show = true;
            Properties["SpellName"].Value = SpellName;
            Pb.UpdateMaterials();
        }

        [PbXmlAttribute]
        public RepeatCalculationType RepeatType
        {
            get { return (RepeatCalculationType) Properties["RepeatType"].Value; }
            set { Properties["RepeatType"].Value = value; }
        }

        public int CalculatedRepeat
        {
            get { return CalculateRepeat(); }
        }

        [PbXmlAttribute]
        [TypeConverter(typeof (DynamicProperty<int>.DynamivExpressionConverter))]
        public DynamicProperty<int> Repeat
        {
            get { return (DynamicProperty<int>) Properties["Repeat"].Value; }
            set { Properties["Repeat"].Value = value; }
        }

        // number of times repeated.
        public int Casted
        {
            get { return (int) Properties["Casted"].Value; }
            set { Properties["Casted"].Value = value; }
        }

        // number of times repeated.
        [PbXmlAttribute]
        public uint Entry
        {
            get { return (uint) Properties["Entry"].Value; }
            set { Properties["Entry"].Value = value; }
        }

        public Recipe Recipe { get; private set; }

        [PbXmlAttribute]
        public bool CastOnItem
        {
            get { return (bool) Properties["CastOnItem"].Value; }
            set { Properties["CastOnItem"].Value = value; }
        }

        [PbXmlAttribute]
        public InventoryType ItemType
        {
            get { return (InventoryType) Properties["ItemType"].Value; }
            set { Properties["ItemType"].Value = value; }
        }

        [PbXmlAttribute]
        public uint ItemId
        {
            get { return (uint) Properties["ItemId"].Value; }
            set { Properties["ItemId"].Value = value; }
        }

        public string SpellName
        {
            get { return Recipe != null ? Recipe.Name : Entry.ToString(CultureInfo.InvariantCulture); }
        }

        public bool IsRecipe
        {
            get { return Recipe != null; }
        }

        // used to confim if a spell finished. set in the lua OnCastSucceeded callback.
        private bool Confimed { get; set; }
        private bool QueueIsRunning { get; set; }

        private WoWItem TargetedItem
        {
            get
            {
                return ObjectManager.Me.BagItems.Where(i => (i.ItemInfo.InventoryType == ItemType && ItemId == 0) ||
                                                            (ItemId > 0 && i.Entry == ItemId)).
                    OrderByDescending(i => i.ItemInfo.Level).ThenBy(i => i.Quality).FirstOrDefault();
            }
        }

        public override Color Color
        {
            get { return IsRecipe ? Color.DarkRed : Color.Black; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_CastSpellAction_Name"]; }
        }

        public override string Title
        {
            get { return string.Format("{0}: {1} x{2} ({3})", Name, SpellName, CalculatedRepeat, CalculatedRepeat - Casted); }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_CastSpellAction_Help"]; }
        }

        private void OnEntryChanged(object sender, MetaPropArgs e)
        {
            CheckTradeskillList();
        }

        private void CastOnItemChanged(object sender, MetaPropArgs e)
        {
            if (CastOnItem)
            {
                Properties["ItemType"].Show = true;
                Properties["ItemId"].Show = true;
            }
            else
            {
                Properties["ItemType"].Show = false;
                Properties["ItemId"].Show = false;
            }
            RefreshPropertyGrid();
        }

        private void CastSpellActionPropertyChanged(object sender, MetaPropArgs e)
        {
            IsDone = false;
            Pb.UpdateMaterials();
        }

        private int CalculateRepeat()
        {
            try
            {
                switch (RepeatType)
                {
                    case RepeatCalculationType.Specific:
                        return Repeat;
                    case RepeatCalculationType.Craftable:
                        return IsRecipe ? (int) Recipe.CanRepeatNum2 : Repeat;
                    case RepeatCalculationType.Banker:
                        if (IsRecipe && Pb.DataStore.ContainsKey(Recipe.CraftedItemID))
                        {
                            return Repeat > Pb.DataStore[Recipe.CraftedItemID]
                                       ? Repeat - Pb.DataStore[Recipe.CraftedItemID]
                                       : 0;
                        }
                        return Repeat;
                }
                return Repeat;
            }
            catch
            {
                return 0;
            }
        }

        private void OnCounterChanged(object sender, MetaPropArgs e)
        {
            RefreshPropertyGrid();
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                if (!IsRecipe)
                    CheckTradeskillList();
                if (Casted >= CalculatedRepeat)
                {
                    if (ObjectManager.Me.IsCasting && ObjectManager.Me.CastingSpell.Id == Entry)
                        SpellManager.StopCasting();
                    _spamControl.Stop();
                    _spamControl.Reset();
                    Lua.Events.DetachEvent("UNIT_SPELLCAST_SUCCEEDED", OnUnitSpellCastSucceeded);
                    IsDone = true;
                    return RunStatus.Failure;
                }
                // can't make recipe so stop trying.
                if (IsRecipe && Recipe.CanRepeatNum2 <= 0)
                {
                    Professionbuddy.Debug("{0} doesn't have enough material to craft.", SpellName);
                    IsDone = true;
                    return RunStatus.Failure;
                }
                if (Me.IsCasting && Me.CastingSpellId != Entry)
                    SpellManager.StopCasting();
                // we should confirm the last recipe in list so we don't make an axtra
                if (!Me.IsFlying && Casted + 1 < CalculatedRepeat || (Casted + 1 == CalculatedRepeat &&
                                                                      (Confimed || !_spamControl.IsRunning ||
                                                                       (_spamControl.ElapsedMilliseconds >=
                                                                        (_recastTime + (_recastTime/2)) + _waitTime &&
                                                                        !ObjectManager.Me.IsCasting))))
                {
                    if (!_spamControl.IsRunning || _spamControl.ElapsedMilliseconds >= _recastTime ||
                        (!ObjectManager.Me.IsCasting && _spamControl.ElapsedMilliseconds >= _waitTime))
                    {
                        if (ObjectManager.Me.IsMoving)
                            WoWMovement.MoveStop();
                        if (!QueueIsRunning)
                        {
                            Lua.Events.AttachEvent("UNIT_SPELLCAST_SUCCEEDED", OnUnitSpellCastSucceeded);
                            QueueIsRunning = true;
                            TreeRoot.StatusText = string.Format("Casting: {0}",
                                                                IsRecipe
                                                                    ? Recipe.Name
                                                                    : Entry.ToString(CultureInfo.InvariantCulture));
                        }
                        WoWSpell spell = WoWSpell.FromId((int) Entry);
                        if (spell == null)
                        {
                            Professionbuddy.Err("{0}: {1}", Pb.Strings["Error_UnableToFindSpellWithEntry"], Entry);
                            return RunStatus.Failure;
                        }
                        _recastTime = spell.CastTime;
                        Professionbuddy.Debug("Casting {0}, recast :{1}", spell.Name, _recastTime);
                        if (CastOnItem)
                        {
                            WoWItem item = TargetedItem;
                            if (item != null)
                                spell.CastOnItem(item);
                            else
                            {
                                Professionbuddy.Err("{0}: {1}",
                                                    Pb.Strings["Error_UnableToFindItemToCastOn"],
                                                    IsRecipe
                                                        ? Recipe.Name
                                                        : Entry.ToString(CultureInfo.InvariantCulture));
                                IsDone = true;
                            }
                        }
                        else
                            spell.Cast();
                        _waitTime = StyxWoW.WoWClient.Latency*2;
                        Confimed = false;
                        _spamControl.Reset();
                        _spamControl.Start();
                    }
                }
                if (!IsDone)
                    return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        private void OnUnitSpellCastSucceeded(object obj, LuaEventArgs args)
        {
            try
            {
                if ((string) args.Args[0] == "player" && (uint) ((double) args.Args[4]) == Entry)
                {
                    // confirm last recipe
                    if (Casted + 1 == CalculatedRepeat)
                    {
                        Confimed = true;
                    }
                    if (RepeatType != RepeatCalculationType.Craftable)
                        Casted++;
                    Professionbuddy.Instance.UpdateMaterials();
                    if (MainForm.IsValid)
                    {
                        MainForm.Instance.RefreshTradeSkillTabs();
                        MainForm.Instance.RefreshActionTree(typeof (CastSpellAction));
                    }
                }
            }
            catch (Exception ex)
            {
                Professionbuddy.Err(ex.ToString());
            }
        }

        // check tradeskill list if spell is a recipe the player knows and updates Recipe if so.

        public void CheckTradeskillList()
        {
            if (Pb.IsTradeSkillsLoaded)
            {
                Recipe =
                    Pb.TradeSkillList.Where(t => t.KnownRecipes.ContainsKey(Entry)).Select(t => t.KnownRecipes[Entry]).
                        FirstOrDefault();
                if (IsRecipe)
                {
                    //Properties["Recipe"].Show = true;
                    Properties["SpellName"].Value = SpellName;
                    Pb.UpdateMaterials();
                }
                else
                {
                    //Properties["Recipe"].Show = false;
                    Properties["SpellName"].Value = SpellName;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            _spamControl.Stop();
            _spamControl.Reset();
            Lua.Events.DetachEvent("UNIT_SPELLCAST_SUCCEEDED", OnUnitSpellCastSucceeded);
            Casted = 0;
            QueueIsRunning = false;
            Confimed = false;
        }

        public override object Clone()
        {
            return new CastSpellAction
                       {
                           Entry = Entry,
                           Repeat = Repeat,
                           ItemType = ItemType,
                           RepeatType = RepeatType,
                           ItemId = ItemId,
                           CastOnItem = CastOnItem
                       };
        }


        public static List<CastSpellAction> GetCastSpellActionList(Composite pa)
        {
            if (pa is CastSpellAction)
                return new List<CastSpellAction> {(pa as CastSpellAction)};

            List<CastSpellAction> ret = null;
            var groupComposite = pa as GroupComposite;
            if (groupComposite != null)
            {
                foreach (Composite comp in (groupComposite).Children)
                {
                    List<CastSpellAction> tmp = GetCastSpellActionList(comp);
                    if (tmp != null)
                    {
                        // lets create a list only if we need to... (optimization)
                        if (ret == null)
                            ret = new List<CastSpellAction>();
                        ret.AddRange(tmp);
                    }
                }
            }
            return ret;
        }
    }

    #endregion

    internal static class WoWSpellExt
    {
        public static void CastOnItem(this WoWSpell spell, WoWItem item)
        {
            using (new FrameLock())
            {
                spell.Cast();
                Lua.DoString("UseContainerItem({0}, {1})", item.BagIndex + 1, item.BagSlot + 1);
            }
        }
    }
}