/*
 * NOTE:    DO NOT POST ANY MODIFIED VERSIONS OF THIS TO THE FORUMS.
 * 
 *          DO NOT UTILIZE ANY PORTION OF THIS COMBAT CLASS WITHOUT
 *          THE PRIOR PERMISSION OF AUTHOR.  PERMITTED USE MUST BE
 *          ACCOMPANIED BY CREDIT/ACKNOWLEDGEMENT TO ORIGINAL AUTHOR.
 * 
 * ShamWOW Shaman CC - Version: 4.3.15
 * 
 * Author:  Bobby53
 * 
 * See the ShamWOW.chm file for Help
 *
 */

//#define BUNDLED_WITH_HONORBUDDY
#define HIDE_PLAYER_NAMES
// #define DEBUG
// #define LIST_HEAL_TARGETS
// #define DISABLE_TARGETING_FOR_INSTANCEBUDDY
// #define HONORBUDDY_SEQUENCE_MANAGER_FIXED
// #define HEALER_DONT_WINDSHEAR
// #define HEALER_IGNORE_TELLURIC_CURRENTS
// #define HEALER_IGNORE_FOCUSED_INSIGHT

/*************************************************************************
 *   !!!!! DO NOT CHANGE ANYTHING IN THIS FILE !!!!!
 *   
 *   User customization is only supported through changing the values
 *   in the SHAMWOW-REALM-CHARNAME.CONFIG file in your Custom Classes folder tree
*************************************************************************/
#pragma warning disable 642

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;


namespace Bobby53
{
    public static class Extensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }

    partial class Shaman : CombatRoutine
    {
        public static string Version { get { return "4.3.15"; } }
        public override WoWClass Class { get { return WoWClass.Shaman; } }
#if    BUNDLED_WITH_HONORBUDDY
		public override string Name { get { return "Default Shaman v" + Version + " by Bobby53"; } }
#else
        public override string Name { get { return "ShamWOW v" + Version + " by Bobby53"; } }
#endif

        #region Global Variables

        public static ConfigValues cfg;
        private static HealSpellManager hsm;
        public static Shaman _local;

        private readonly TalentManager talents = new TalentManager();

        public static readonly string ConfigPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), Path.Combine("CustomClasses", "ShamWOW\\Config"));
        public static readonly string CCPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), Path.Combine("CustomClasses", "ShamWOW"));

        public static string ConfigFilename;

        private uint _countDrinks;
        private uint _countFood;
        private readonly Countdown timeStart = new Countdown();

        private static readonly Dictionary<WoWSpellSchool, HashSet<uint>> ImmunityMap = new Dictionary<WoWSpellSchool, HashSet<uint>>();

        private static BotEvents.OnBotStartDelegate btStrt;
        private static BotEvents.OnBotStopDelegate btStp;
        // private static BotEvents.Player.PlayerDiedDelegate btDied;

        private List<WoWItem> trinkMana;      // regens mana
        private List<WoWItem> trinkHealth;    // regens health
        private List<WoWItem> trinkPVP;       // restores player control
        private List<WoWItem> trinkCombat;    // use off cd when in combat

        private ConfigValues.SpellPriority priorityCleanse;
        private ConfigValues.SpellPriority priorityPurge;

        enum ShieldType
        {
            None = 0,
            Lightning,
            Water,
            Earth
        }

        private const int HEALING_WAVE = 331;

        private static WoWPartyMember.GroupRole _myGroupRole;

        private static WoWPlayer ___GroupHealer = null;
        private static WoWPlayer GroupHealer
        {
            get
            {
                try
                {
                    // we don't care about .IsAlive other than it will throw an exception if reference invalid
                    if (___GroupHealer != null && ___GroupHealer.CurrentHealth > 1)
                        return ___GroupHealer;
                }
                catch
                {
                    Dlog("GroupHealer:  healer reference is now invalid, so resetting ShamWOW's GroupHealer reference");
                    ___GroupHealer = null;
                }
                return ___GroupHealer;
            }

            set
            {
                ___GroupHealer = value;
            }
        }

        private static WoWPlayer GroupTank
        {
            get
            {
                try
                {
                    // simple test to protect from exceptions on a value we know will go invalid at some point
                    if (RaFHelper.Leader != null && RaFHelper.Leader.CurrentHealth > 1)
                        return RaFHelper.Leader;
                }
                catch
                {
                    Dlog("GroupTank:  tank reference invalid, treating as (null) value");
                    return null;
                }

                return RaFHelper.Leader;
            }
        }

        private static int minGroupHealth = 100;

        private static double _maxDistForRangeAttack
        {
            get
            {
                double dist = 25.0;

                // use Lightning Bolt range as ranged distance, 
                //  ... unless Nature Immune, figure out how close we need to be
                try
                {
                    dist = SpellManager.Spells["Lightning Bolt"].MaxRange;
                    if (IsPVP() || IsRAF() || cfg.PVE_PullType == ConfigValues.TypeOfPull.Fast)
                    {
                        dist = Math.Min(dist, SpellManager.Spells["Earth Shock"].MaxRange);
                    }
                }
                catch (ThreadAbortException) { throw; }
                catch
                { ;/* do nothing, default initialized above */ }

                return dist;
            }
        }

        private double _offsetForRangedPull
        {
            get
            {
                double dist = _maxDistForRangeAttack;
                if (_me.GotTarget && IsImmunneToNature(_me.CurrentTarget))
                {
                    /* if (SpellManager.HasSpell("Lava Lash"))
                        dist = SpellManager.Spells["Lava Lash"].MaxRange;
                    else */
                    if (SpellManager.HasSpell("Flame Shock"))    // go into melee then
                        dist = SpellManager.Spells["Flame Shock"].MaxRange;
                    else
                        dist = _offsetForMeleePull + 4;

                    Slog("Nature immune:  making Ranged pull distance for target {0:F1} yds", dist - 4);
                }

                return dist - 4.0;
            }
        }

        private const double _maxDistForMeleeAttack = STD_MELEE_RANGE;
        public static double _offsetForMeleePull
        {
            get
            {
                if (IsPVP())
                {
                    if (_me.GotTarget && _me.CurrentTarget.IsMoving )
                    {
                        Dlog("using fleeing target range of 0.0");
                        return 1.0;
                    }

                    return 2.0;
                }

                return 3.0;
            }
        }

        public static List<WoWPlayer> GroupMembers
        {
            get
            {
                if ( !_me.IsInRaid )
                    return _me.PartyMembers;

                if (cfg.RAF_HealStyle == ConfigValues.RafHealStyle.Auto)
                    return _me.RaidMembers;
                
                List<WoWPlayer> members = new List<WoWPlayer>();
                if (cfg.RAF_HealStyle == ConfigValues.RafHealStyle.TankOnly)
                {
                    if (GroupTank != null)
                        members.Add(GroupTank);
                    return members;
                }
                else
                {
                    return _me.RaidMembers;
                }
            }
        }

        public static List<WoWPartyMember> GroupMemberInfos 
        { 
            get 
            { 
                return !_me.IsInRaid ? _me.PartyMemberInfos : _me.RaidMemberInfos; 
            } 
        }

        public static LocalPlayer _me { get { return ObjectManager.Me; } }

        private Countdown _pullTimer = new Countdown();             // kill timer from when we target for pull
        private ulong _pullTargGuid;
        // private bool _pullTargHasBeenInMelee;       // flag indicating that we got in melee range of mob
        private int _pullAttackCount;
        // private WoWPoint _pullStart;

        private uint _killCount;
        private uint _killCountBase;
        private uint _deathCount;
        private uint _deathCountBase;

        private ShieldType _lastShieldUsed;       // toggle remember which shield used last shield twist

        private bool _RecallTotems;

        // private bool _lastCheckWasInCombat = true;  
        private bool _castGhostWolfForm;            // 
        private bool _castCleanse;
        private WoWPlayer _rezTarget;

        private static bool _BigScaryGuyHittingMe;      // mob sufficiently higher than us (3+ lvls)
        private static bool _OpposingPlayerGanking;    // player from other faction attacking me
        private static int _countMeleeEnemy;               // # of melee mobs in combat with me
        private static int _count8YardEnemy;               // # of mobs in combat with me within  8 yards
        private static int _count10YardEnemy;               // # of mobs in combat with me within 10 yards
        private static int _countRangedEnemy;              // # of ranged mobs in combat with me
        private static int _countAoe8Enemy;          // # of mobs within 8 yards of current target
        private static int _countAoe12Enemy;          // # of mobs within 12 yards of current target
        private static int _countFireNovaEnemy;    // # of mobs within Fire Nova range of fire totem
        private int _countMobs;                     // # of faction mobs as spec'd in current profile
        //  private double _distClosestEnemy ;           // distance to closest hostile or faction mob
        //  private WoWPoint _ptClosestEnemy;

        private bool _needClearWeaponEnchants = true;
        private bool _needTotemBarSetup = true;


        private bool _lastCheckInBattleground;    // 
        private bool _lastCheckInGroup;
        private int lastCheckTalentGroup;
        private int[] lastCheckTabPoints = new int[4];
        private bool lastCheckConfig;
        private int lastCheckSpellCount = 0;

#if EQUIP_SUPPORTED
		private string EquipDefault;
		private string EquipPVP;
		private string EquipRAF;
#endif
        // public static WoWPlayer _followTarget;

        private static bool _isPluginMrAutoFight;
        private static bool _isBotLazyRaider;
        private static bool _isBotInstanceBuddy;

        public static bool foundMobsThatFear;
        // public static ulong foundMobToInterrupt; 
        private readonly Stopwatch _potionTimer = new Stopwatch();


        // private int _pointsImprovedStormstrike;
        private static bool _hasTalentFulmination;
        private static bool _hasTalentImprovedCleanseSpirit;
        private static bool _hasTalentAncestralSwiftness;
        // private static bool _hasTalentImprovedLavaLash;
        private static bool _hasTalentMaelstromWeapon;
        private static bool _hasTalentFocusedInsight;
        private static bool _hasTalentTelluricCurrents;
        private static bool _hasGlyphOfChainLightning;
        private static bool _hasGlyphOfHealingStreamTotem;
        private static bool _hasGlyphOfStoneClaw;
        private static bool _hasGlyphOfShamanisticRage;
        private static bool _hasGlyphOfFireNova;
        public static bool _hasGlyphOfWaterWalking;
        public static bool _hasGlyphOfWaterBreathing;
        private static bool _hasGlyphOfUnleashedLightning;

        public enum ShamanType
        {
            Unknown,
            Elemental,
            Enhance,
            Resto
        };

        public static ShamanType typeShaman = ShamanType.Unknown;

        private static int countEnemy
        {
            get
            {
                return _countMeleeEnemy + _countRangedEnemy;
            }
        }

        public static bool IsFightStressful()
        {
            return countEnemy >= cfg.PVE_StressfulMobCount || _BigScaryGuyHittingMe || _OpposingPlayerGanking;
        }

        private bool DidWeSwitchModes()
        {
            return DidWeSwitchModes(true);
        }

        private bool DidWeSwitchModes(bool verboseCheck)
        {
            bool dirtyFlag = false;

            if (lastCheckConfig)
            {
                if (verboseCheck)
                    Slog(Color.DarkGreen, ">>> Configuration updated, Initializing...");
                dirtyFlag = true;
                lastCheckConfig = false;
            }

            if (_lastCheckInGroup != (_me.IsInParty || _me.IsInRaid))
            {
                if (verboseCheck)
                    Slog(Color.DarkGreen, ">>> Left/joined a group, Initializing...");
                dirtyFlag = true;
                _lastCheckInGroup = _me.IsInParty || _me.IsInRaid;
            }

            if (_lastCheckInBattleground != IsPVP())
            {
                if (verboseCheck)
                    Slog(Color.DarkGreen, ">>> Left/joined a battleground, Initializing...");
                dirtyFlag = true;
                _lastCheckInBattleground = IsPVP();
            }

            if (lastCheckTalentGroup != talents.GroupIndex)
            {
                if (verboseCheck)
                    Slog(Color.DarkGreen, ">>> New talent group active, Initializing...");
                dirtyFlag = true;
                lastCheckTalentGroup = talents.GroupIndex;
            }

            for (int tab = 1; tab <= 3; tab++)
            {
                if (lastCheckTabPoints[tab] != talents.TabPoints[tab])
                {
                    if (!dirtyFlag)
                        Slog(Color.DarkGreen, ">>> Talent spec changed, Initializing...");

                    dirtyFlag = true;
                    lastCheckTabPoints[tab] = talents.TabPoints[tab];
                }
            }

            return dirtyFlag;
        }

        #endregion

        #region Private Members

#if CTOR_NO_LONGER_NEEDED      

		/*
		 * Ctor
		 * 
		 * initialize and post load messages/checks for user
		 */
		public ShamWOW()
		{
			if (_me.Class != WoWClass.Shaman)
			{
				return;
			}
		}

		/*
		 * Dtor
		 */
		~ShamWOW()
		{
			if (_me.Class != WoWClass.Shaman)
			{
				return;
			}

			Dlog("UNLOAD:  " + Name);
		}

#endif

        private static bool firstInitialize = true;

        public override void Initialize()
        {
            if (firstInitialize)
            {
                firstInitialize = false;
                InitializeOnce();
            }

            _hashCleanseBlacklist = new HashSet<int>();
            _hashPurgeWhitelist = new HashSet<int>();
            _dictMob = new Dictionary<int,Mob>();

            string MainConfig = Path.Combine(ConfigPath, "ShamWOW.config");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(MainConfig);
                XmlNodeList nodeList = doc.SelectNodes("//ShamWOW/CleanseBlacklist/Spell");
                Dlog("");
                Dlog( "CLEANSE BLACKLIST");
                foreach (XmlElement spell in nodeList)
                {
                    int id = int.Parse(spell.GetAttribute("Id"));
                    Dlog( "spellId:{0}  spellName:{1}", id, WoWSpell.FromId(id));
                    _hashCleanseBlacklist.Add(id);
                }

                nodeList = doc.SelectNodes("//ShamWOW/PurgeWhitelist/Spell");
                Dlog("");
                Dlog("PURGE WHITELIST");
                foreach (XmlElement spell in nodeList)
                {
                    int id = int.Parse(spell.GetAttribute("Id"));
                    Dlog( "spellId:{0}  spellName:{1}", id, WoWSpell.FromId(id));
                    _hashPurgeWhitelist.Add(int.Parse(spell.GetAttribute("Id")));
                }

                nodeList = doc.SelectNodes("//ShamWOW/MobList/Mob");
                Dlog("");
                Dlog("MOB CUSTOMIZATION LIST");
                foreach (XmlElement node in nodeList)
                {
                    Mob mob = new Mob(  int.Parse(node.GetAttribute("Id")),
                                        node.GetAttribute("Name"),
                                        int.Parse(node.GetAttribute("HitBox")));
                    Dlog("Mob Id:{0} Name:{1} HitBox:{2}", mob.Id, mob.Name, mob.HitBox);
                    _dictMob.Add(int.Parse(node.GetAttribute("Id")), mob);
                }
                Dlog( "");
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Elog("Error loading Global ShamWOW Config info from '{0}'", MainConfig );
                Logging.WriteDebug(">>> EXCEPTION: Global config file ShamWOW.config contains bad XML:");
                Logging.WriteException(e);
            }

            if (talents.Spec == 1)
                typeShaman = ShamanType.Elemental;
            else if (talents.Spec == 2)
                typeShaman = ShamanType.Enhance;
            else if (talents.Spec == 3)
                typeShaman = ShamanType.Resto;
            else if (cfg.MeleeCombatBeforeLevel10)
            {
                typeShaman = ShamanType.Enhance;
                Slog("Low-level Shaman played as Enhancement due to config setting.");
            }
            else
            {
                typeShaman = ShamanType.Elemental;
                Slog("Low-level Shaman being played as Elemental.  See the [Melee Combat Before Level 10] configuration setting to force melee combat.");
            }


            // reset the Heal Targets list and force a refresh
            _healTargets = null;
            minGroupHealth = 100;

            // get important group assignments.  we want our role and to identify the healer (in case its not us)
            CheckGroupRoleAssignments();

            Slog("");
            string sSpecType = talents.TabNames[talents.Spec];
            Slog("Your Level " + _me.Level + " " + _me.Race + " " + sSpecType + " Shaman Build is:  ");
            Slog(talents.TabNames[1].Substring(0, 5) + "/" + talents.TabNames[2].Substring(0, 5) + "/" + talents.TabNames[3].Substring(0, 5)
            + "   " + talents.TabPoints[1] + "/" + talents.TabPoints[2] + "/" + talents.TabPoints[3]);

            Slog("... running the {0} bot {1} as {2} in {3}",
                 TreeRoot.Current == null ? "unknown bot" : TreeRoot.Current.Name,
                 _me.IsInRaid ? "in a Raid" : _me.IsInParty ? "in a Party" : "Solo",
                 IsHealerOnly() ? "Healer Only" : (IsHealer() ? "Healer over Combat" : "Combat Only"),
                 _me.RealZoneText
                );

            Logging.WriteDebug(" ");
            Logging.WriteDebug("Initialize:  Battleground: {0}", IsPVP());
            Logging.WriteDebug("Initialize:  RAF.........: {0}", IsRAF());
            Logging.WriteDebug("Initialize:  IsInInstance: {0}", _me.IsInInstance);
            Logging.WriteDebug("Initialize:  IsCombatOnly: {0}", IsCombatOnly());
            Logging.WriteDebug("Initialize:  IsHealer....: {0}", IsHealer());
            Logging.WriteDebug("Initialize:  IsHealerOnly: {0}", IsHealer());

            Slog("");

            if (talents.UnspentPoints > 0)
                Wlog("WARNING: {0} unspent Talent Points. Use a talent plug-in or spec manually", talents.UnspentPoints);

            _isBotInstanceBuddy = TreeRoot.Current != null && "INSTANCEBUDDY" == TreeRoot.Current.Name.ToUpper();
            if (_isBotInstanceBuddy)
                Slog("InstanceBuddy detected... ShamWOW fast attack targeting disabled");

            _isBotLazyRaider = TreeRoot.Current != null && "LAZYRAIDER" == TreeRoot.Current.Name.ToUpper();
            if (_isBotLazyRaider)
                Wlog("CC movevement disabled to work with LazyRaider Bot");

            _isPluginMrAutoFight = CharacterSettings.Instance.EnabledPlugins != null && CharacterSettings.Instance.EnabledPlugins.Contains("Mr.AutoFight");
            if (_isPluginMrAutoFight)
                Wlog("CC movevement disabled to work with Mr.AutoFight");

            if (cfg.DisableMovement)
                Slog("CC movevement disabled due to Config Setting");

            if (IsTargetingDisabled())
                Slog("CC targeting disabled due to Config Setting");

            _deathCountBase = InfoPanel.Deaths;

            Slog("Max Pull Ranged:   {0}", _maxDistForRangeAttack);
            Slog("HB Pull Distance:  {0}", Targeting.PullDistance);

            if (cfg.DistanceForGhostWolf < (Targeting.PullDistance + 5))
            {
                Wlog("Increasing Ghost Wolf Distance from {0} to {1} (Pull Distance + 5)", cfg.DistanceForGhostWolf, (int)Targeting.PullDistance + 5);
                cfg.DistanceForGhostWolf = (int)Targeting.PullDistance + 5;
            }

            Slog("");

            _hasTalentFulmination = 0 < talents.GetTalentInfo(1, 13);
            _hasTalentImprovedCleanseSpirit = SpellManager.HasSpell("Cleanse Spirit") && 0 < talents.GetTalentInfo(3, 12);
            _hasTalentAncestralSwiftness = SpellManager.HasSpell("Ghost wolf") && (2 == talents.GetTalentInfo(2, 6));
            _hasTalentMaelstromWeapon = (1 <= talents.GetTalentInfo(2, 17));
            // _hasTalentImprovedLavaLash = SpellManager.HasSpell("Lava Lash") && (1 <= talents.GetTalentInfo(2, 18));
            _hasTalentFocusedInsight = (1 <= talents.GetTalentInfo(3, 6));
            _hasTalentTelluricCurrents = (1 <= talents.GetTalentInfo(3, 16));
            _hasGlyphOfChainLightning = SpellManager.HasSpell("Chain Lightning") && talents._glyphs.ContainsKey(55449);
            _hasGlyphOfHealingStreamTotem = HasTotemSpell(TotemId.HEALING_STREAM_TOTEM) && talents._glyphs.ContainsKey(55456);
            _hasGlyphOfStoneClaw = SpellManager.HasSpell((int)TotemId.STONECLAW_TOTEM) && talents._glyphs.ContainsKey(63298);
            _hasGlyphOfShamanisticRage = SpellManager.HasSpell("Shamanistic Rage") && talents._glyphs.ContainsKey(63280);
            _hasGlyphOfFireNova = SpellManager.HasSpell("Fire Nova") && talents._glyphs.ContainsKey(55450);
            _hasGlyphOfWaterWalking = SpellManager.HasSpell("Water Walking") && talents._glyphs.ContainsKey(58057);
            _hasGlyphOfWaterBreathing = SpellManager.HasSpell("Water Breathing") && talents._glyphs.ContainsKey(89646);
            _hasGlyphOfUnleashedLightning = SpellManager.HasSpell("Lightning Bolt") && talents._glyphs.ContainsKey(101052);

            if (_hasTalentFulmination)
                Slog("[talent] Fulmination: will wait for 7+ stacks of Lightning Shield before using Earth Shock");
            if (SpellManager.HasSpell("Cleanse Spirit"))
                Slog("[talent] Cleanse Spirit: {0}", _hasTalentImprovedCleanseSpirit ? "can remove Curses and Magic" : "can only remove Curses");
            if (SpellManager.HasSpell("Ghost wolf"))
                Slog("[talent] Ancestral Swiftness: {0}", _hasTalentAncestralSwiftness ? "can cast Ghost Wolf on the run" : "must stop to cast Ghost Wolf");
            if (_hasTalentMaelstromWeapon)
                Slog("[talent] Maelstrom Weapon: will cast Lightning Bolt or Chain Lightning at 5 stacks");
            // if (SpellManager.HasSpell("Lava Lash"))
            //      Slog("Lava Lash: cast {0}", _hasTalentImprovedLavaLash ? "will wait for 5 stacks of Searing Flames" : "when off cooldown");
            if (_hasTalentFocusedInsight)
                Slog("[talent] Focused Insight: will cast Earth Shock to boost healing");
            if (_hasTalentTelluricCurrents)
                Slog("[talent] Telluric Currents: will cast Lightning Bolt to regen mana");
            if (SpellManager.HasSpell("Earthquake"))
                Slog("[glyph] Chain Lightning: {0}, will use for AoE instead of Earthquake unless {1}+ mobs in range", _hasGlyphOfChainLightning ? "found" : "not found", _hasGlyphOfChainLightning ? 7 : 5);
            if (HasTotemSpell(TotemId.STONECLAW_TOTEM))
                Slog("[glyph] Stoneclaw Totem: {0}", _hasGlyphOfStoneClaw ? "found, will use as Shaman Bubble" : "not found, no Shaman Bubble available");
            if (SpellManager.HasSpell("Shamanistic Rage"))
                Slog("[glyph] Shamanistic Rage: {0}", _hasGlyphOfShamanisticRage ? "found, will use as Magic Cleanse" : "not found, no Magic Cleanse available");
            if (SpellManager.HasSpell("Fire Nova") && _hasGlyphOfFireNova)
                Slog("[glyph] Fire Nova: found, range extended by 5 yards");
            if (_hasGlyphOfHealingStreamTotem)
                Slog("[glyph] Healing Stream Totem: found, will use instead of Elemental Resistance Totem");
            if (_hasGlyphOfUnleashedLightning)
                Slog("[glyph] Unleashed Lightning: found, will allow use Lightning Bolt while moving");

            if ( IsPVP() && SpellManager.HasSpell("Water Walking") && _hasGlyphOfWaterWalking)
                Slog("[glyph] Water Walking: found, will buff battleground and arena members in range");
            if ( IsPVP() && SpellManager.HasSpell("Water Breathing") && _hasGlyphOfWaterBreathing)
                Slog("[glyph] Water Breathing: found, will buff battleground and arena members in range");

            Slog("");

            hsm = new HealSpellManager();
            if (_me.IsInRaid || _me.IsInParty)
            {
                Logging.WriteDebug("-- Current {0} Heal Settings --", _me.IsInRaid ? "Raid" : "Party");
                hsm.Dump();
            }

            DidWeSwitchModes(false);                 // set the mode change tracking variables
            _needClearWeaponEnchants = true;
            _needTotemBarSetup = true;

            // InfoPanel.Reset();
            _killCountBase = InfoPanel.MobsKilled;
            _deathCountBase = InfoPanel.Deaths;

            // reset trinket pointers
            trinkMana = new List<WoWItem>();
            trinkHealth = new List<WoWItem>();
            trinkPVP = new List<WoWItem>();
            trinkCombat = new List<WoWItem>();

            // now assign item points based upon use spell
            CheckGearForUsables(_me.Inventory.Equipped.Trinket1);
            CheckGearForUsables(_me.Inventory.Equipped.Trinket2);

            foreach (WoWItem item in trinkPVP)
                Slog("Detected PVP Trinket:  {0}", item.Name);
            foreach (WoWItem item in trinkHealth)
                Slog("Detected Health Trinket:  {0}", item.Name);
            foreach (WoWItem item in trinkMana)
                Slog("Detected Mana Trinket:  {0}", item.Name);
            foreach (WoWItem item in trinkCombat)
                Slog("Detected Combat Trinket:  {0}", item.Name);

            if (_me.Inventory.Equipped.Hands != null)
            {
                foreach (string tink in _hashTinkerCombat)
                {
                    if (null != _me.Inventory.Equipped.Hands.GetEnchantment(tink))
                    {
                        trinkCombat.Add(_me.Inventory.Equipped.Hands);
                        Slog("Combat Enchant:  '{0}' on {1}", tink, _me.Inventory.Equipped.Hands.Name);
                    }
                }

                if (null != _me.Inventory.Equipped.Hands.GetEnchantment("Z50 Mana Gulper"))
                {
                    trinkMana.Add(_me.Inventory.Equipped.Hands);
                    Slog("Mana Enchant:  'Z50 Mana Gulper' on {0}", _me.Inventory.Equipped.Hands.Name);
                }
                else if (null != _me.Inventory.Equipped.Hands.GetEnchantment("Spinal Healing Injector"))
                {
                    trinkHealth.Add(_me.Inventory.Equipped.Hands);
                    Slog("Combat Enchant:  'Spinal Healing Injector' on {0}", _me.Inventory.Equipped.Hands.Name);
                }
            }

            if (_me.Inventory.Equipped.Waist != null)
            {
                if (null != _me.Inventory.Equipped.Waist.GetEnchantment("Grounded Plasma Shield"))
                {
                    trinkCombat.Add(_me.Inventory.Equipped.Waist);
                    Slog("Combat Enchant:  'Grounded Plasma Shield' on {0}", _me.Inventory.Equipped.Waist.Name);
                }
            }

            if ( IsPVP() )
            {
                priorityCleanse = cfg.PVP_CleansePriority;
                priorityPurge   = cfg.PVP_PurgePriority;
            }
            else if ( IsRAF() )
            {
                priorityCleanse = cfg.RAF_CleansePriority;
                priorityPurge = cfg.RAF_PurgePriority;
            }
            else
            {
                priorityCleanse = ConfigValues.SpellPriority.Low;
                priorityPurge = ConfigValues.SpellPriority.Low;
            }

            Slog("");

            Dlog("Effective Cleanse Priority: {0}", priorityCleanse);
            Dlog("Effective Purge Priority: {0}", priorityPurge);

            Slog("");
        }

        public static bool IsMovementDisabled()
        {
            return _isPluginMrAutoFight || _isBotLazyRaider || cfg.DisableMovement;
        }

        public static bool IsTargetingDisabled()
        {
            return IsMovementDisabled() && cfg.DisableTargeting;
        }

        public static bool IsImmunityCheckDisabled()
        {
            return !cfg.DetectImmunities || IsTargetingDisabled();
        }

        private void InitializeOnce()
        {
            //==============================================================================================
            //  Now do ONE TIME Initialization (needs to occur after we know what spec we are)
            //==============================================================================================
            Slog("LOADED:  " + Name);
            _local = this;

            // load config file (create if doesn't exist)
            Logging.WriteDebug("% InitializeOnce:  getting realm name");
            string realmName = Lua.GetReturnVal<string>("return GetRealmName()", 0);
            ConfigFilename = Path.Combine(ConfigPath, "ShamWOW-" + realmName + "-" + _me.Name + ".config");

            if (!Directory.Exists(ConfigPath))
            {
                try
                {
                    Directory.CreateDirectory(ConfigPath);
                }
                catch (ThreadAbortException) { throw; }
                catch
                {
                    Wlog("Folder could not be created: '{0}'", ConfigPath);
                    Wlog("Create the folder manually if needed and restart HB");
                    return;
                }
            }

            bool didUpgrade;
            cfg = new ConfigValues();
            if (File.Exists(ConfigFilename))
            {
                cfg.FileLoad(ConfigFilename, out didUpgrade);
                Slog("Character specific config file loaded");
            }
            else
            {
                didUpgrade = true;
                cfg.Save(ConfigFilename);
                Slog("Creating a character specific config file ");
            }



//            if (didUpgrade)
//            {
//                MessageBox.Show(
//#if SAVE_THIS_MESSAGE
//                    "Click the CC Configuration button found"     +Environment.NewLine +
//                    "on the General tab of HB for options."       +Environment.NewLine +
//                    ""                                          +Environment.NewLine +
//                    "All issues/problems posted on the forum"   +Environment.NewLine +
//                    "must include a complete debug LOG FILE."   +Environment.NewLine +
//                    "Posts missing this will be ignored or"     +Environment.NewLine +
//                    "receive a response requesting the file."   + Environment.NewLine +
//                    "" + Environment.NewLine +
//                    "If there is anything you would like this"   + Environment.NewLine +
//                    "CC to do, ask in the forum for direction"   + Environment.NewLine +
//                    "because it probably already does."          + Environment.NewLine +
//#else
//"ShamWOW BETA User Acknowledgement" + Environment.NewLine +
//                    "" + Environment.NewLine +
//                    "By using this BETA software, you agree" + Environment.NewLine +
//                    "to ATTACH A COMPLETE DEBUG LOG FILE to" + Environment.NewLine +
//                    "any forum post you make containing a" + Environment.NewLine +
//                    "question, criticism, or bug report." + Environment.NewLine +
//#endif
// "" + Environment.NewLine +
//                    "Thanks, Bobby53" + Environment.NewLine,
//                    Name,
//                    MessageBoxButtons.OK,
//                    MessageBoxIcon.Information
//                    );
//            }

            cfg.DebugDump();

            lastCheckSpellCount = LegacySpellManager.KnownSpells.Count();

#if  USE_CUSTOM_PATH_PRECISION
            const float minPathPrecision = 2.5f;
			float prevPrec = Navigator.PathPrecision;
			if (prevPrec != minPathPrecision)
			{
				Navigator.PathPrecision = minPathPrecision;
				Slog("Changed Navigator precision from {0} to {1}", prevPrec, minPathPrecision);
			}
#else
            Slog("Navigator.PathPrecision is: {0}", Navigator.PathPrecision);
#endif

            btStrt = new BotEvents.OnBotStartDelegate(startBot);
            BotEvents.OnBotStart += btStrt;
            btStp = new BotEvents.OnBotStopDelegate(stopBot);
            BotEvents.OnBotStop += btStp;

            Targeting.Instance.IncludeTargetsFilter += IncludeTargetsFilter;

            Mount.OnMountUp += new EventHandler<MountUpEventArgs>(Mount_OnMountUp);

            //////////////////////////////////// Following would go in dtor ///////////////////
            // BotEvents.OnBotStart -= btStrt;
            // BotEvents.OnBotStopped -= btStp;
#if HONORBUDDY_SEQUENCE_MANAGER_FIXED
			SequenceManager.AddSequenceExecutorOverride(Sequence.ReleaseSpirit, SequenceOverride_ReleaseSpirit);
#endif
            // Lua.Events.AttachEvent("UNIT_SPELLCAST_INTERRUPTIBLE", HandleCastInterruptible);
            // Lua.Events.AttachEvent("UNIT_SPELLCAST_NOT_INTERRUPTIBLE", HandleCastNotInterruptible);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", HandleTalentGroupChange); //goes to init()
            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", HandlePlayerTalentUpdate); //goes to init()
            Lua.Events.AttachEvent("PARTY_MEMBERS_CHANGED", HandlePartyMembersChanged);
            Lua.Events.AttachEvent("TRAINER_CLOSED", HandleTrainerClosed);
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLogEvent);
            Lua.Events.AttachEvent("PLAYER_TARGET_CHANGED", HandlePlayerTargetChanged);
            Lua.Events.AttachEvent("UNIT_TARGET", HandlePlayerTargetChanged);

            talents.Load();
#if COMMENT
			if (SpellManager.HasSpell(331))
				DumpSpellEffectInformation(WoWSpell.FromId(331));
			if (SpellManager.HasSpell(77472))
				DumpSpellEffectInformation(WoWSpell.FromId(77472));
			if (SpellManager.HasSpell(8004))
				DumpSpellEffectInformation(WoWSpell.FromId(8004));
#endif
            //==============================================================================================
            //  end of first time initialize
            //==============================================================================================            
        }

        void Mount_OnMountUp(object sender, MountUpEventArgs e)
        {
            // wait here until any current cast has completed
            // .. but don't wait here on a GCD
            WaitForCurrentCast();

            // add a check here for ....
            //  ... if we are healing and heal targets nearby, suppress
            //  ... if we have attack targets nearby, suppress
            if (IsRAF() && GroupTank != null && !GroupTank.Mounted && !GroupTank.IsFlying && GroupTank.Distance < 60)
            {
                Dlog("MountUp:  suppressed for HB RaF Combat Assist /follow bug - leader not mounted and only {0:F1} yds away", GroupTank.Location);
                e.Cancel = true;
                return;
            }

            if (typeShaman == ShamanType.Resto
                || (IsPVP() && cfg.PVP_CombatStyle != ConfigValues.PvpCombatStyle.CombatOnly)
                || (IsRAF() && cfg.RAF_CombatStyle != ConfigValues.RafCombatStyle.CombatOnly))
            {
                WoWPlayer p = ChooseHealTarget(hsm.NeedHeal, SpellRange.Check);
                if (p != null && !p.IsMe)
                {
                    Dlog("MountUp:  suppressed for heal - {0}[{1}] at {2:F1} yds needs heal", p.Class, p.Level, p.Distance);
                    e.Cancel = true;
                    return;
                }
            }

            if (_WereTotemsSet)
            {
                Dlog("MountUp:  HB wants to mount and totems exist... recalling totems");
                _local.RecallTotemsForMana();
            }

            if (cfg.WaterWalking && SpellManager.HasSpell("Water Walking"))
                Safe_CastSpell(_me, "Water Walking", SpellRange.NoCheck, SpellWait.NoWait);

            if (_me.IsIndoors && cfg.UseGhostWolfForm && !InGhostwolfForm())
            {
                Dlog("MountUp: indoors so using Ghost Wolf");
                if (GhostWolf())
                {
                    e.Cancel = true;
                    return;
                }
            }

            Dlog("MountUp:  detected HonorBuddy trying to mount");
        }

        private static bool GotAlivePet()
        {
            return _me.GotAlivePet || TotemExist(TotemId.EARTH_ELEMENTAL_TOTEM) || TotemExist(TotemId.FIRE_ELEMENTAL_TOTEM);
        }

        private static void IncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            int cntOut = 0;
            if (!GotAlivePet())
                return;

            for (int i = 0; i < incomingUnits.Count; i++)
            {
                if (incomingUnits[i] is WoWUnit)
                {
                    WoWUnit u = incomingUnits[i].ToUnit();
                    if (u.Combat && IsTargetingMeOrMyStuff(u))
                    {
                        cntOut++;
                        outgoingUnits.Add(u);
                        Dlog("IncludeTargetsFilter: added {0} targeting {1}", Safe_UnitName(u), Safe_UnitName(u.CurrentTarget));
                    }
                }
            }

            Dlog("IncludeTargetsFilter:  got alive pet(s) and added {0} mobs targeting them", cntOut);
        }

        private static void DumpSpellEffectInformation(WoWSpell spell)
        {
            Dlog("----------- {0} -----------", spell.Name.ToUpper());
            Dlog("Tooltip >>> {0}", spell.Tooltip);
            for (int i = 0; i <= 4; i++)
            {
                SpellEffect se = spell.GetSpellEffect(i);
                if (se == null)
                    Dlog("SpellEffect({0}):  null", i);
                else
                {
                    Dlog("SpellEffect({0}): Amplitude          {1}", i, se.Amplitude);
                    Dlog("SpellEffect({0}): AuraType           {1}", i, se.AuraType);
                    Dlog("SpellEffect({0}): BasePoints         {1}", i, se.BasePoints);
                    Dlog("SpellEffect({0}): EffectType         {1}", i, se.EffectType);
                    Dlog("SpellEffect({0}): Mechanic           {1}", i, se.Mechanic);
                    Dlog("SpellEffect({0}): MiscValueA         {1}", i, se.MiscValueA);
                    Dlog("SpellEffect({0}): MiscValueB         {1}", i, se.MiscValueB);
                    Dlog("SpellEffect({0}): MultipleValue      {1}", i, se.MultipleValue);
                    Dlog("SpellEffect({0}): RadiusIndex        {1}", i, se.RadiusIndex);
                    Dlog("SpellEffect({0}): RealPointsPerLevel {1}", i, se.RealPointsPerLevel);
                    Dlog("SpellEffect({0}): TriggerSpell       {1}", i, se.TriggerSpell);
                    Dlog("SpellEffect({0}):  {1}", i, se.ToString());
                }
            }
        }


        public static string SafeLogException(string msg)
        {
            msg = msg.Replace("{", "(");
            msg = msg.Replace("}", ")");
            return msg;
        }

        /* Log()
         * 
         * write 'msg' to log window.  message is suppressed if it is identical
         * to prior message.  Intent is to prevent log window spam
         */
        public static void Log(string msg, params object[] args)
        {
            Log(Color.DarkSlateGray, msg, args);
        }


        private static uint lineCount = 0;

        public static void Log(Color clr, string msg, params object[] args)
        {
            try
            {
                // following linecount hack is to stop dup line suppression of Log window
                Logging.Write(clr, msg + (++lineCount % 2 == 0 ? "" : " "), args);
                _Slogspam = msg;
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug(">>> EXCEPTION: occurred logging msg: \n\t\"" + SafeLogException(msg) + "\"");
                Logging.WriteException(e);
            }
        }


        /* Slog()
         * 
         * write 'msg' to log window.  message is suppressed if it is identical
         * to prior message.  Intent is to prevent log window spam
         */
        private static string _Slogspam;

        public static void Slog(Color clr, string msg, params object[] args)
        {
            try
            {
                msg = String.Format(msg, args);
                if (msg == _Slogspam)
                    return;

                Log(clr, msg);
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug(">>> EXCEPTION: occurred logging msg: \n\t\"" + SafeLogException(msg) + "\"");
                Logging.WriteException(e);
            }
        }

        public static void Slog(string msg, params object[] args)
        {
            Slog(Color.Black, msg, args);
        }


        public static void Elog(string msg, params object[] args)
        {
            Slog(Color.Red, msg, args);
        }

        /* Wlog()
         * 
         * write 'msg' to log window, but only if it hasn't been written already.
         */
        private static readonly List<string> _warnList = new List<string>();    // tracks warning messages issued by Wlog()

        public static void Wlog(string msg, params object[] args)
        {
            msg = String.Format(msg, args);
            String found = _warnList.Find(s => 0 == s.CompareTo(msg));
            if (found == null)
            {
                _warnList.Add(msg);
                Log(Color.Red, msg);
            }
        }

        /* Dlog()
         * 
         * Write Debug log message to log window.  message is suppressed if it
         * is identical to prior log message or verbose mode is turned off.  These
         * messages are trace type in nature to follow in more detail what has occurred
         * in the code.
         * 
         * NOTE:  I am intentionally putting debug message in the Log().  At this point,
         * it helps more having all data be time sequenced in the same window.  This will
         * near the close of development move to the Debug window instead
         */
        static string _Dlogspam;

        public static void Dlog(string msg, params object[] args)
        {
            try
            {
                msg = String.Format(msg, args);
                if (msg == _Dlogspam) // || cfg.Debug == false)
                    return;

                if (cfg.Debug)
                    Logging.WriteDebug("%   " + msg);
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug(">>> EXCEPTION: occurred logging msg: \n\t\"" + SafeLogException(msg) + "\"");
                Logging.WriteException(e);
            }

            _Dlogspam = msg;
        }

        private void ReportBodyCount()
        {
            bool rptKill = (_killCountBase + _killCount) < InfoPanel.MobsKilled;
            bool rptDeath = (_deathCountBase + _deathCount) < InfoPanel.Deaths;

            if (rptDeath)
            {
                _deathCount = InfoPanel.Deaths - _deathCountBase;
                Slog("! Death #{0} at {1:F1} per hour fighting at x={2},y={3},z={4}",
                InfoPanel.Deaths,
                InfoPanel.DeathsPerHour,
                _me.Location.X,
                _me.Location.Y,
                _me.Location.Z
                );
            }

            if (rptKill)
            {
                _killCount = InfoPanel.MobsKilled - _killCountBase;
                Slog("! Kill #{0} at {1:F0} xp per hour fighting at x={2},y={3},z={4}",
                InfoPanel.MobsKilled,
                InfoPanel.XPPerHour,
                _me.Location.X,
                _me.Location.Y,
                _me.Location.Z
                );
            }
        }

        //private delegate void startBot();
        private void startBot(EventArgs args)
        {
            Slog("");
            Slog(Color.DarkGreen, ">>> STARTING {0}", Name);
            talents.Load();

            // following settings aren't checked in mode change. even though they can 
            // .. we'll check only at bot start, so a change in enabled status of Mr. AutoFight
            // .. while running will still require a Stop / Start for ShamWOW to recognize
            _isPluginMrAutoFight = CharacterSettings.Instance.EnabledPlugins != null && CharacterSettings.Instance.EnabledPlugins.Contains("Mr.AutoFight");
            _isBotInstanceBuddy = TreeRoot.Current != null && "INSTANCEBUDDY" == TreeRoot.Current.Name.ToUpper();

            //            if (DidWeSwitchModes())
            {
                Initialize();
            }
        }

        private void stopBot(EventArgs args)
        {
            Slog("");
            Slog(Color.DarkGreen, ">>> STOPPING {0}", Name);
        }

#if HONORBUDDY_SEQUENCE_MANAGER_FIXED
		private static void SequenceOverride_ReleaseSpirit()
		{
			_countMeleeEnemy = 0;               // # of melee mobs in combat with me
			_count10YardEnemy = 0;             // # of mobs withing 10 yards in combat with me
			_countRangedEnemy = 0;              // # of ranged mobs in combat with me
			_WereTotemsSet = false;

			// next phase
			// ... if selfrez available, inspect surrounding area if clear within
			// ... a configurable time delay, then selfrez, otherwise repop
			List<string> hasSoulstone = Lua.GetReturnValues("return HasSoulstone()", "hawker.lua");
			if (hasSoulstone != null && hasSoulstone.Count > 0 && hasSoulstone[0] != "" && hasSoulstone[0].ToLower() != "nil")
			{
				/*
				Lua.DoString("UseSoulstone()");
                Countdown tickCount = new Countdown( 7500);
				while (!ObjectManager.Me.IsAlive && !tickCount.Done )
					Thread.Sleep(100);
				if (ObjectManager.Me.IsAlive)
					return;
				 */

				Slog("Skipping use of '{0}'", hasSoulstone[0]);
			}

			// SequenceManager.CallDefaultSequenceExecutor(Sequence.ReleaseSpirit);
			Lua.DoString("RepopMe()", "hawker.lua");

            Countdown tickCount = new Countdown( 10000);
			while (!ObjectManager.Me.IsAlive && !tickCount.Done)
				Thread.Sleep(100);

			if (!ObjectManager.Me.IsAlive)
			{
				SequenceManager.CallDefaultSequenceExecutor(Sequence.ReleaseSpirit);
			}
		}

#endif

        private void HandleTalentGroupChange(object sender, LuaEventArgs args) // to anywhere
        {
            Dlog("HandleTalentGroupChange:  event received");
            talents.Load();

            if (DidWeSwitchModes())
            {
                Slog("^EVENT:  Active Talent Group Changed : initializing...");
                Initialize();
            }
        }

        private void HandlePlayerTalentUpdate(object sender, LuaEventArgs args) // to anywhere
        {
            Dlog("HandlePlayerTalentUpdate:  event received");
            talents.Load();

            if (DidWeSwitchModes())
            {
                Slog("^EVENT:  Player Level/Talent Update : initializing...");
                Initialize();
            }
        }

        private void HandleTrainerClosed(object sender, LuaEventArgs args)
        {
            Dlog("HandleTrainerClosed:  event received");
            LegacySpellManager.Refresh();
            int chkCount = LegacySpellManager.KnownSpells.Count();
            if (chkCount != lastCheckSpellCount)
            {
                Slog("^EVENT:  Trainer Window closed : initializing...");
                Initialize();
                lastCheckSpellCount = chkCount;
            }
        }


        private void HandleCombatLogEvent(object sender, LuaEventArgs args)
        {
            if (args.Args[1].ToString() != "SPELL_MISSED")
                return;

            try
            {
                ulong guid = ulong.Parse(args.Args[5].ToString().Replace("0x", ""), NumberStyles.HexNumber);
                WoWUnit unit = ObjectManager.GetObjectsOfType<WoWUnit>(true, false).FirstOrDefault(o => o.DescriptorGuid == guid);
                switch (args.Args[11].ToString())
                {
                    case "EVADE":
                        Slog("### EVADE mob, blacklisting: {0} for 1 hour", Safe_UnitName(unit));
                        Blacklist.Add(unit.Guid, System.TimeSpan.FromHours(1));
                        if (_me.CurrentTarget == unit)
                        {
                            StopAutoAttack();
                            Safe_SetCurrentTarget(null);
                        }
                        break;

                    case "IMMUNE":
                        WoWSpellSchool spellSchool = (WoWSpellSchool)(int)(double)args.Args[10];
                        if (unit == null || unit is WoWPlayer || IsImmunityCheckDisabled())
                            break;

                        uint entry = unit.Entry;

                        if (unit.Buffs.Any(a => a.Value.Spell.GetSpellEffect(0).AuraType == WoWApplyAuraType.SchoolImmunity))
                        {
                            Slog("### TEMPORARILY IMMUNE to {0}, should not cast {1} on {2} [#{3}]", spellSchool, args.Args[9], unit.Name, entry);
                            return;
                        }

                        if (!ImmunityMap.ContainsKey(spellSchool))
                            ImmunityMap[spellSchool] = new HashSet<uint>();
                        if (ImmunityMap[spellSchool].Contains(entry))
                        {
                            Slog("### IMMUNE to {0}, should not cast {1} on {2} [#{3}]", spellSchool, args.Args[9], unit.Name, entry);
                        }
                        // Divine Shield cast by some mobs providing temporary immunity... 
                        // .. ignore or blacklist for a few seconds
                        else if (!unit.Auras.ContainsKey("Divine Shield"))
                        {
                            Slog("### IMMUNE mob, adding {0} [#{1}] to {2} list", unit.Name, entry, spellSchool);
                            ImmunityMap[spellSchool].Add(entry);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Logging.Write(e.ToString());
            }
        }

        private void HandlePartyMembersChanged(object sender, LuaEventArgs args)
        {
            Dlog("HandlePartyMembersChanged:  event received");
            CheckGroupRoleAssignments();

            if (NeedToBuffRaid())
            {
                Slog("HandlePartyMembersChanged: buffing raid");
                BuffRaid();
            }
        }

        private void HandlePlayerTargetChanged(object sender, LuaEventArgs args)
        {
            if (!_me.GotTarget && GotAlivePet() && !IsPVP())
            {
                Dlog("HandlePlayerTargetChanged:  no current target, so finding one");
                FindAggroTarget();
            }
        }

        #region SAFE_ Functions

        private static string Right(string s, int c)
        {
            return s.Substring(c > s.Length ? 0 : s.Length - c);
        }
        /*
         * Safe_ Functions.  These were created to handle unexpected errors and
         * situations occurring in HonorBuddy.  try/catch handling is provided
         * where an exception is thrown by HB that shouldn't be. multiple
         * attempts at something (like dismounting) are done until the desired
         * state (!_me.Mounted) is achieved
         */
        private static string Safe_UnitID(WoWUnit unit)
        {
            if (unit == null)
                return "(null)";

            if (unit.IsMe)
                return "-ME-";

            if (unit.IsPlayer)
                return unit.Class.ToString() + "." + Right(String.Format("{0:X3}", unit.Guid), 4);

            return unit.Name + "." + Right(String.Format("{0:X3}", unit.Guid), 4);
        }

        private static string Safe_UnitName(WoWUnit unit)
        {
            if (unit == null)
                return "(null)";

#if HIDE_PLAYER_NAMES
            if (unit.IsMe)
                return "-ME-";
            else if (unit.IsPlayer) // && Safe_IsFriendly(unit)) // !unit.IsHostile ) // unit.IsFriendly)
            {
                if (GroupHealer == unit)
                    return "-HEALER-";
                if (GroupTank == unit)
                    return "-TANK-";
                return unit.Class.ToString() + "." + Right(String.Format("{0:X3}", unit.Guid), 4);
            }
#endif
            return unit.Name + "." + Right(String.Format("{0:X3}", unit.Guid), 4);
        }

        // replacement for WoWUnit.IsFriendly
        // to handle bug in HB 1.9.2.5 where .IsFriendly throws exception casting WoWUnit -> WoWPlayer
        private static bool Safe_IsFriendly(WoWUnit unit)
        {
            if (!unit.IsPlayer)
                return unit.IsFriendly;

            WoWPlayer p = unit.ToPlayer();
            return p.IsHorde == ObjectManager.Me.IsHorde;
        }

        // replacement for WoWUnit.IsNeutral
        // to handle bug in HB 1.9.2.5 where .IsHostile throws exception casting WoWUnit -> WoWPlayer
        private static bool Safe_IsNeutral(WoWUnit unit)
        {
            if (!unit.IsPlayer)
                return unit.IsNeutral;

            return false;
        }

        // replacement for WoWUnit.IsHostile
        // to handle bug in HB 1.9.2.5 where .IsHostile throws exception casting WoWUnit -> WoWPlayer
        private static bool Safe_IsHostile(WoWUnit unit)
        {
            if (!unit.IsPlayer)
                return unit.IsHostile || HasAggro(unit);

            WoWPlayer p = unit.ToPlayer();
            return p.IsHorde != ObjectManager.Me.IsHorde;
        }

        private static bool Safe_IsElite(WoWUnit unit)
        {
            if (unit != null)
            {
                if (unit.Elite && unit.MaxHealth > _me.MaxHealth && (unit.Level + 10) > _me.Level )
                    return true;

                if (unit.Level >= (_me.Level + cfg.PVE_LevelsAboveAsElite))
                    return true;
            }

            return false;
        }
        

        private static bool Safe_IsProfileMob(WoWUnit unit)
        {
            if (unit == null || unit.Faction == null || ProfileManager.CurrentProfile == null || ProfileManager.CurrentProfile.Factions == null)
                return false;
            return ProfileManager.CurrentProfile.Factions.Contains(unit.Faction.Id);
        }

        // replacement for WoWObject.IsValid
        private static bool Safe_IsValid(WoWUnit u)
        {
            try
            {
                // simply test access to a property to confirm valid or force exception
                return u != null && u.HealthPercent > -1;
            }
            catch (ThreadAbortException) { throw; }
            catch
            {
                return false;
            }
        }

        // replacement for WoWObject.IsValid
        private static bool Safe_IsValid(WoWObject o)
        {
#if DEBUG
			return o != null && ObjectManager.ObjectList.Contains(o);
#else
            return o != null;
#endif
        }

        private static bool MeImmobilized()
        {
            if (ObjectManager.Me.Stunned)
                Slog(Color.Orange, "You are stunned and unable to cast");
            else if (ObjectManager.Me.Possessed)
                Slog(Color.Orange, "You are possessed and unable to cast");
            else if (ObjectManager.Me.Fleeing)
                Slog(Color.Orange, "You are feared and unable to cast");
            else
                return false;

            return true;
        }

        private static bool MeSilenced()
        {
            if (MeImmobilized())
                ;
            else if (ObjectManager.Me.Silenced)
                Slog(Color.Orange, "You are silenced and unable to cast");
            else
                return false;

            return true;
        }

        private void UseItem(WoWItem item)
        {
            if (item == null || !item.Usable)
                return;

            if (item.Cooldown > 0)
                return;

            const bool forceUse = true;
            item.Use(forceUse);
            Slog(Color.DodgerBlue, "/Use: {0}", item.Name);
        }

        private void UseItem(List<WoWItem> list)
        {
            if (list == null || !list.Any())
                return;

            foreach (WoWItem item in list)
            {
                if (item.Cooldown == 0)
                {
                    const bool forceUse = true;
                    item.Use(forceUse);
                    Slog(Color.DodgerBlue, "/Use: {0}", item.Name);
                    return;
                }
            }
        }

        private void CheckGearForUsables(WoWItem item)
        {
            if (item == null || !item.Usable)
                return;

            // sometimes spellId isn't in first array element, so find first non-zero
            // uint spellId = (uint)item.ItemInfo.SpellId.FirstOrDefault(si => si != 0);

            foreach (int spellId in item.ItemInfo.SpellId)
            {
                if (_hashTrinkCombat.Contains(spellId))
                    trinkCombat.Add(item);

                if (_hashTrinkHealth.Contains(spellId))
                    trinkHealth.Add(item);

                if (_hashTrinkMana.Contains(spellId))
                    trinkMana.Add(item);

                if (_hashTrinkPVP.Contains(spellId))
                    trinkPVP.Add(item);
            }
        }

        public static void RunLUA(string sCmd)
        {
            WaitForCurrentSpell(null);
            Lua.DoString(sCmd, "shaman.lua");
        }

        public static List<string> CallLUA(string sCmd)
        {
            WaitForCurrentSpell(null);
            List<string> retList = Lua.GetReturnValues(sCmd, "shaman.lua");
            return retList;
        }


        /*
         * Only issues the Stop moving command if currently moving.  also
         * accounts for any lag which prevents immediate stop
         */
        public static void Safe_StopMoving()
        {
            if (IsMovementDisabled())
                return;

            if (!ObjectManager.Me.IsMoving)
                return;

            int countTries = 0;
            Stopwatch stopTimer = new Stopwatch();

            stopTimer.Start();
            while (!IsGameUnstable() && _me.IsAlive && ObjectManager.Me.IsMoving && stopTimer.ElapsedMilliseconds < 1000)
            {
                countTries++;
                WoWMovement.MoveStop();
                Thread.Sleep(100);          // increased from 50 to handle flag update lag better 
            }

            if (countTries > 1)
            {
                Dlog("Attempted to stop moving " + countTries);
            }

            if (!ObjectManager.Me.IsMoving)
                Dlog("Stopped Moving");
            else
            {
                if (_me.Fleeing )
                    Slog("Feared --- uggghhh");
                else if (MeImmobilized())
                    Slog("Immobilized is true but still moving and can't stop; am I Feared?");
                else
                    Slog("ERROR: " + countTries + " attempts to stop moving and failed; character Feared?");
            }
        }

        public static void Safe_FaceTarget()
        {
            if (IsMovementDisabled())
                return;

            if (_me.GotTarget && !Safe_IsFriendly(_me.CurrentTarget))
            {
                Dlog("Safe_FaceTarget:  facing current target {0}", Safe_UnitName(_me.CurrentTarget));
                // WoWMovement.Face();
                _me.CurrentTarget.Face();
            }
        }

        public static void Safe_StopFace()
        {
            if (IsMovementDisabled())
                return;
            WoWMovement.StopFace();
        }


        /*
         * Only issues the Stop moving command if currently moving.  also
         * accounts for any lag which prevents immediate stop
         */
        private void Safe_Dismount()
        {
            if (!_me.Mounted)
                return;

            int countTries = 0;
            Stopwatch stopTimer = new Stopwatch();

            stopTimer.Start();
            while (!IsGameUnstable() && _me.IsAlive && _me.Mounted && stopTimer.ElapsedMilliseconds < 1500)
            {
                countTries++;
                Mount.Dismount();
                Thread.Sleep(100);
            }


            if (_me.Mounted)
            {
                Slog("LAG!! still mounted after {0} dismount attempts - timed out after {1} ms", countTries, stopTimer.ElapsedMilliseconds);
            }
            else if (countTries > 1)
            {
                Dlog("Dismount needed {0} attempts - took {1} ms", countTries, stopTimer.ElapsedMilliseconds);
            }
        }

        /*
         * Only issues the Stop moving command if currently moving.  also
         * accounts for any lag which prevents immediate stop
         */
        private static bool Safe_SetCurrentTarget(WoWUnit target)
        {
            if (IsTargetingDisabled())
                return true;

            Stopwatch stopTimer = new Stopwatch();

            stopTimer.Start();
            if (target == null)
                _me.ClearTarget();
            else if (!_me.GotTarget || _me.CurrentTarget.Guid != target.Guid)
                target.Target();

            while (!IsGameUnstable() && _me.IsAlive && _me.CurrentTarget != target && stopTimer.ElapsedMilliseconds < 2000)
            {
                Thread.Sleep(60);
            }

            if (_me.CurrentTarget != target)
                Dlog("Timeout:  must have died, game state change, or serious lag - .CurrentTarget not updated after {0} ms", stopTimer.ElapsedMilliseconds);
            else if (target == null)
            {
                Slog("Cleared current target");
                Dlog("Safe_SetCurrentTarget() took {0} ms to .ClearTarget", stopTimer.ElapsedMilliseconds);
                return true;
            }
            else
            {
                Slog("Setting current target to: {0}[{1}]", Safe_UnitName(target), target == null ? 0 : target.Level);
                Dlog("Safe_SetCurrentTarget() took {0} ms to set .CurrentTarget to {0}[{1}]", stopTimer.ElapsedMilliseconds, Safe_UnitName(target), target == null ? 0 : target.Level);
                return true;
            }

            return false;
        }

        /*
         * MoveTo()
         * 
         * if the point to move to is less than PathPrecision, then the toon
         * will not move.  This function checks if we are moving a very small
         * distance and forces movement by changing the precision if needed
         */
        private static void MoveTo(WoWPoint newPoint)
        {
            if (IsMovementDisabled())
                return;

            float distToMove = _me.Location.Distance(newPoint);
            float prevPrec = Navigator.PathPrecision;

            if (distToMove <= prevPrec)
                Navigator.PathPrecision = distToMove - (float)0.1;

            Countdown stopCount = new Countdown(10000);
            while (!IsGameUnstable() && _me.IsAlive && _me.IsCasting)
            {
                if (stopCount.Done)
                {
                    Slog(Color.Red, "ERROR:  Waited 10+ secs for cast to finish-- moving anyway");
                    break;
                }
            }

            if (Navigator.GeneratePath(_me.Location, newPoint).Length <= 0)
                Slog(Color.Red, "Cannot generate navigation path to new position");
            else
            {
                Navigator.MoveTo(newPoint);
                // if ( IsRAF())
                //     Slog(Color.LightGray, "shamwow-move to point {0:F1} yds away from tank", newPoint.Distance(GroupTank.Location));
            }

            Navigator.PathPrecision = prevPrec;
        }

        private void MoveToCurrentTarget()
        {
            MoveToUnit(_me.CurrentTarget);
        }

        public static void MoveToUnit(WoWUnit unit)
        {
            MoveToUnit(unit, _offsetForMeleePull);
        }

        public static void MoveToUnit(WoWUnit unit, double dist )
        {
            if (unit == null)
                return;

            if (IsMovementDisabled())
                return;

            if (IsPVP() || IsRAF())
                Slog("MoveToUnit: moving to {0}:{1} thats {2:F0} yds away and {3}in line of sight", unit.IsPlayer ? "player" : "npc", Safe_UnitName(unit), unit.Distance, unit.InLineOfSightOCD ? "" : "NOT ");

            WoWPoint newPoint = WoWMovement.CalculatePointFrom(unit.Location, (float) dist);
            MoveTo(newPoint);      // WoWMovement.ClickToMove(newPoint);
        }

        private void MoveToHealTarget(WoWUnit unit, double distRange)
        {
            if (IsMovementDisabled())
                return;

            if (!IsUnitInRange(unit, distRange))
            {
                Slog("MoveToHealTarget:  moving within {0:F1} yds of Heal Target {1} that is {2:F1} yds away", distRange, Safe_UnitName(unit), unit.Distance);
                if (_me.IsCasting)
                    WaitForCurrentSpell(null);

                Stopwatch timerLastMove = new Stopwatch();
                Stopwatch timerStuckCheck = new Stopwatch();

                while (!IsGameUnstable() && _me.IsAlive && Safe_IsValid(unit) && unit.IsAlive && !IsUnitInRange(unit, distRange) && unit.Distance < 100)
                {
                    if (!_me.IsMoving || !timerLastMove.IsRunning || timerLastMove.ElapsedMilliseconds > 500)
                    {
                        MoveToUnit(unit);
                        timerLastMove.Reset();
                        timerLastMove.Start();
                    }

                    if (_me.IsMoving)
                    {
                        timerStuckCheck.Reset();
                        timerStuckCheck.Start();
                    }
                    else if (timerStuckCheck.ElapsedMilliseconds > 2000)
                    {
                        Dlog("MoveToHealTarget:  stuck? for {0} ms", timerStuckCheck.ElapsedMilliseconds);
                        break;
                    }

                    // while running, if someone else needs a heal throw a unleash elements on them
                    if (SpellManager.HasSpell("Unleash Elements") && SpellManager.CanCast("Unleash Elements"))
                    {
                        if (!IsWeaponImbueNeeded())
                        {
                            WoWPlayer otherTarget = ChooseNextHealTarget(unit, (double)hsm.NeedHeal);
                            if (otherTarget != null)
                            {
                                Slog("MoveToHealTarget:  healing {0} while moving to heal target {1}", Safe_UnitName(otherTarget), Safe_UnitName(unit));
                                Safe_CastSpell(otherTarget, "Unleash Elements", SpellRange.Check, SpellWait.NoWait);
                                StyxWoW.SleepForLagDuration();
                                continue;
                            }
                        }
                    }

                    // while running, if someone else needs a heal throw a riptide on them
                    if (SpellManager.HasSpell("Riptide") && SpellManager.CanCast("Riptide"))
                    {
                        WoWPlayer otherTarget = ChooseNextHealTarget(unit, (double)hsm.NeedHeal);
                        if (otherTarget != null)
                        {
                            Slog("MoveToHealTarget:  healing {0} while moving to heal target {1}", Safe_UnitName(otherTarget), Safe_UnitName(unit));
                            Safe_CastSpell(otherTarget, "Riptide", SpellRange.Check, SpellWait.NoWait);
                            StyxWoW.SleepForLagDuration();
                            continue;
                        }
                    }
                }

                if (_me.IsMoving)
                {
                    Dlog("MoveToHealTarget: stopping now that Heal Target is {0} yds away", unit.Distance);
                    Safe_StopMoving();
                }
            }
        }

        private bool FindBestTarget() { return FindBestTarget(_maxDistForRangeAttack); }
        private bool FindBestMeleeTarget() { return FindBestTarget(_maxDistForMeleeAttack); }

        private bool FindBestTarget(double withinDist)
        {
            // find mobs in melee distance
            List<WoWUnit> mobs = null;

            if (IsTargetingDisabled())
                return false;

#if DISABLE_TARGETING_FOR_INSTANCEBUDDY
			if (_me.IsInInstance && _isBotInstanceBuddy)
			{
				Dlog("InstanceBuddy: targeting disabled");
				return false;
			}
#endif
            if (IsRAF())
            {
                if (GroupTank == null)
                    return false;

                WoWUnit leaderTarget = GroupTank.CurrentTarget;

                if (!GroupTank.IsAlive)
                    Dlog("FindBestTarget-RAF:  RaF Leader is Dead!");
                else if (!GroupTank.GotTarget)
                    Dlog("FindBestTarget-RAF:  RaF Leader does not have a current target!");
#if DISABLE_TARGETING_FOR_INSTANCEBUDDY
                else if (TreeRoot.Current != null && _isBotInstanceBuddy)
                    Dlog( "Target search suppressed for InstanceBuddy");
#endif
                else if (leaderTarget.IsPlayer && _me.IsHorde == leaderTarget.ToPlayer().IsHorde)
                    Dlog("Ignore RaF Leader Target -- player is same faction");
                else if (Safe_IsFriendly(leaderTarget))
                    Dlog("Ignore RaF Leader Target -- unit is friendly");
                else if (!leaderTarget.Attackable)
                    Dlog("Ignore RaF Leader Target -- unit is not attackable");
                else if (!_me.GotTarget || leaderTarget.Guid != _me.CurrentTarget.Guid)
                {
                    Slog(">>> SET LEADERS TARGET:  {0}[{1}] at {2:F1} yds",
                            Safe_UnitName(leaderTarget),
                            leaderTarget.Level,
                            leaderTarget.Distance
                            );
                    Safe_SetCurrentTarget(leaderTarget);
                    return true;
                }

                return false;
            }

            // otherwise, build a list of mobs based upon whether in Battlegrounds or not
            string typeList = "";

            if (!IsPVP())
            {
                typeList = "PVE";
                mobs = (from o in ObjectManager.ObjectList
                        where o is WoWUnit && o.Distance <= withinDist
                        let unit = o.ToUnit()
                        where unit.Attackable
                            && unit.IsAlive
                            && unit.Combat
                            && !IsMeOrMyStuff(unit)
                            && (IsTargetingMeOrMyStuff(unit) || unit.CreatureType == WoWCreatureType.Totem)
                            && !Blacklist.Contains(unit.Guid)
                        orderby unit.CurrentHealth ascending
                        select unit
                            ).ToList();
            }
            else
            {
                typeList = "PVP";
                mobs = (from o in ObjectManager.ObjectList
                        where o is WoWUnit && o.Distance <= withinDist
                        let unit = o.ToUnit()
                        where unit.IsAlive && unit.IsPlayer && unit.ToPlayer().IsHorde != ObjectManager.Me.IsHorde
                            && unit.InLineOfSightOCD && !unit.IsPet
                            && !Blacklist.Contains(unit.Guid)
                        orderby unit.CurrentHealth ascending
                        select unit
                            ).ToList();
            }

            // now make best selection from list
            if (mobs != null && mobs.Any())
            {
                WoWUnit newTarget = mobs[0];

                if (newTarget == null)
                {
                    Dlog("FindBestTarget-{0}:  found null mob search list????", typeList);
                }
                else if (_me.GotTarget && newTarget.Guid == _me.CurrentTarget.Guid)
                {
                    Dlog("FindBestTarget-{0}:  already targeting found mob????", typeList);
                }
                else
                {
                    Slog(">>> BEST TARGET:  {0}-{1}[{2}] at {3:F1} yds",
                            newTarget.Class,
                            Safe_UnitName(newTarget),
                            newTarget.Level,
                            newTarget.Distance
                            );
                    Safe_SetCurrentTarget(newTarget);
                    return true;
                }
            }
            else
            {
                Dlog("FindBestTarget:  found 0 mobs within {0:F1} yds", withinDist);
            }

            return false;
        }

        // compares targets to find one with the lowest health (not lowest % of health)
        // .. to hopefully score a quick kill
        private class HealthSorter : IComparer<WoWUnit>
        {
            public int Compare(WoWUnit obj1, WoWUnit obj2)
            {
                return obj1.CurrentHealth.CompareTo(obj2.CurrentHealth);
            }
        }

        public static bool HasAggro(WoWUnit unit)
        {
            return
                unit.Combat
                && unit.Attackable
                && unit.CurrentHealth > 1
                && !Safe_IsFriendly(unit)
                && (unit.Aggro || unit.PetAggro || IsTargetingMeOrMyStuff(unit));
        }

        // find any units with aggro to us (keeping us in combat)
        // ... should work for totems also
        public static bool FindAggroTarget()
        {
            if (IsTargetingDisabled())
                return false;

            List<WoWUnit> mobs = (from o in ObjectManager.ObjectList
                                  where o is WoWUnit
                                  let unit = o.ToUnit()
                                  where HasAggro(unit)
                                  orderby unit.CurrentHealth ascending
                                  select !IsMeOrMyStuff(unit) ? unit : unit.CurrentTarget
                                ).ToList();

#if COMMMENT
                        unit.GotTarget && unit.Combat &&
                        (
                            (Safe_IsHostile(unit) && IsMeOrMyStuff( unit.CurrentTarget) && unit.IsAlive )
                         || (IsMeOrMyStuff(unit) && Safe_IsHostile( unit.CurrentTarget) && unit.CurrentTarget.IsAlive)
                        )
#endif

            if (mobs != null && mobs.Any())
            {
                WoWUnit newTarget = mobs.First();
                Slog(">>> AGGRO TARGET:  {0}-{1}[{2}] at {3:F1} yds",
                        newTarget.Class,
                        Safe_UnitName(newTarget),
                        newTarget.Level,
                        newTarget.Distance
                        );
                Safe_SetCurrentTarget(newTarget);
                if (newTarget.Aggro)
                    Dlog("FindAggroTarget: could also find using .Aggro");
                else if (newTarget.PetAggro)
                    Dlog("FindAggroTarget: could also find using .PetAggro");
                else
                    Dlog("FindAggroTarget:  could only find with IsMyStuff");

                return true;
            }

            Dlog("FindAggroTarget: no aggro mobs found");
            return false;
        }

        #endregion


        public static bool IsRAF()
        {
            return (ObjectManager.Me.IsInParty || ObjectManager.Me.IsInRaid) && !IsPVP(); // from Nesox
            // old test - return !IsPVP() && ObjectManager.Me.PartyMember1 != null;
        }

        public static bool IsRAFandTANK()
        {
            return IsRAF() && GroupTank != null && GroupTank.IsValid;
        }

        public static bool IsPVP()
        {
            return Battlegrounds.IsInsideBattleground;
        }

        public static bool IsHealer()
        {
            if (IsPVP())
                return cfg.PVP_CombatStyle != ConfigValues.PvpCombatStyle.CombatOnly || typeShaman == ShamanType.Resto;

            if (!IsRAF())
                return false;

            if (cfg.RAF_CombatStyle == ConfigValues.RafCombatStyle.Auto)
            {
                if (GroupHealer == null || !GroupHealer.IsAlive)
                    return true;

                // check if we need to temporarily switch to offhealing
                if (cfg.RAF_GroupOffHeal > minGroupHealth)
                    return true;

                return _myGroupRole == WoWPartyMember.GroupRole.Healer;
            }

            return cfg.RAF_CombatStyle != ConfigValues.RafCombatStyle.CombatOnly || typeShaman == ShamanType.Resto;
        }

        public static bool IsHealerOnly()
        {
            if (IsPVP())
                return cfg.PVP_CombatStyle == ConfigValues.PvpCombatStyle.HealingOnly;

            if (!IsRAF())
                return false;

            if (cfg.RAF_CombatStyle == ConfigValues.RafCombatStyle.Auto)
                return _myGroupRole == WoWPartyMember.GroupRole.Healer;

            return cfg.RAF_CombatStyle == ConfigValues.RafCombatStyle.HealingOnly;
        }

        public static bool IsCombatOnly()
        {
            return !IsHealer();
        }

        public static bool IsMeOrMyStuff(WoWUnit unit)
        {
            if (unit == null)
                return false;

            // find topmost unit in CreatedByUnit chain
            while (unit.CreatedByUnit != null)
                unit = unit.CreatedByUnit;

            // check if this unit was created by me
            return unit.IsMe;
        }

        public static bool IsTargetingMeOrMyStuff(WoWUnit unit)
        {
            return unit != null && IsMeOrMyStuff(unit.CurrentTarget);
        }

        public static bool IsMeOrMyGroup(WoWUnit unit)
        {
            if (unit != null)
            {
                // find topmost unit in CreatedByUnit chain
                while (unit.CreatedByUnit != null)
                    unit = unit.CreatedByUnit;

                if (unit.IsMe)
                    return true;

                if (unit.IsPlayer)
                {
                    WoWPlayer p = unit.ToPlayer();
                    if (p.IsHorde == _me.IsHorde && GroupMembers.Contains(unit.ToPlayer()))
                        return true;
                }
            }

            return false;
        }

        public static bool IsTargetingMeOrMyGroup(WoWUnit unit)
        {
            return unit != null && IsMeOrMyGroup(unit.CurrentTarget);
        }

        public static bool IsImmune(WoWUnit unit, WoWSpellSchool spellSchool)
        {
            return !IsImmunityCheckDisabled()
                && Safe_IsValid(unit)
                && ImmunityMap.ContainsKey(spellSchool)
                && ImmunityMap[spellSchool].Contains(unit.Entry);
        }

        public static bool IsImmunneToNature(WoWUnit unit)
        {
            return IsImmune(unit, WoWSpellSchool.Nature);
        }

        public static bool IsImmunneToFire(WoWUnit unit)
        {
            return IsImmune(unit, WoWSpellSchool.Fire);
        }

        public static bool IsImmunneToFrost(WoWUnit unit)
        {
            return IsImmune(unit, WoWSpellSchool.Frost);
        }
#if NOT_DEALT_WITH_BY_CC
        public static bool IsImmunneToArcane(WoWUnit unit)
        {
            return IsImmune(unit, WoWSpellSchool.Arcane);
        }

        public static bool IsImmunneToHoly(WoWUnit unit)
        {
            return IsImmune(unit, WoWSpellSchool.Holy);
        }

        public static bool IsImmunneToShadow(WoWUnit unit)
        {
            return IsImmune(unit, WoWSpellSchool.Shadow );
        }

        public static bool IsImmunneToPhysical(WoWUnit unit)
        {
            return IsImmune(unit, WoWSpellSchool.Physical);
        }
#endif
        public static bool IsFearMob(WoWUnit unit)
        {
            if (!Safe_IsValid(unit))
                return false;

            bool found = _hashTremorTotemMobs.Contains(unit.Entry);
            return found;
        }


        // used to cache the results of the last LUA call to check Weapon Imbues
        private bool _needMainhandImbue;
        private bool _needOffhandImbue;

        /*
         * Reports whether we to stop for any Weapon Buffs
         */
        private bool DoWeaponsHaveImbue()
        {
            bool doesIt = false;

            if (CanImbue(_me.Inventory.Equipped.MainHand))
                doesIt = doesIt || _me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != 0;
            if (CanImbue(_me.Inventory.Equipped.OffHand))
                doesIt = doesIt || _me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id != 0;

            return doesIt;
        }

        private bool IsWeaponImbuedWithDPS()
        {
            bool doesIt = false;

            if (CanImbue(_me.Inventory.Equipped.MainHand))
            {
                doesIt = _me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != 0
                      && _me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != 51730;
            }

            if (!doesIt && CanImbue(_me.Inventory.Equipped.OffHand))
            {
                doesIt = _me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id != 0
                      && _me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id != 51730;
            }

            return doesIt;
        }

        private bool IsWeaponImbuedWithEarthLiving()
        {
            bool doesIt = false;

            if (CanImbue(_me.Inventory.Equipped.MainHand))
                doesIt = _me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 51730;

            if (!doesIt && CanImbue(_me.Inventory.Equipped.OffHand))
                doesIt = _me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == 51730;

            return doesIt;
        }

        private bool IsWeaponImbueNeeded()
        {
            // due to lag between imbue spellcast and wow client updating buff aura, add delay before allowing subsequent imbues check
            if (!waitImbueCast.Done)
            {
                Dlog("IsWeaponImbueNeeded():  waiting {0} ms until next imbue check", waitImbueCast.Remaining);
                return false;
            }

            _needMainhandImbue = false;
            _needOffhandImbue = false;

            // now make sure we have a mainhand weapon we can imbue
            if (!CanImbue(_me.Inventory.Equipped.MainHand))
                return false;

            if (typeShaman == ShamanType.Unknown)
                return false;

            // see if we trained any weapon enchants yet... if not then don't need to imbue weapon
            string enchantMainhand;
            string enchantOffhand;
            GetBestWeaponImbues(out enchantMainhand, out enchantOffhand);

            if (string.IsNullOrEmpty(enchantMainhand))
                return false;

            // get the enchant info from LUA
#if USE_LUA_FOR_IMBUES
			List<string> weaponEnchants = CallLUA("return GetWeaponEnchantInfo()");
			if (Equals(null, weaponEnchants))
				return false;
			_needMainhandImbue = weaponEnchants[0] == "" || weaponEnchants[0] == "nil";
			if (IsOffhandWeaponEquipped())
				_needOffhandImbue = weaponEnchants[3] == "" || weaponEnchants[3] == "nil";

            if (_needMainhandImbue)
                Dlog("Mainhand weapon {0} needs imbue", _me.Inventory.Equipped.MainHand == null ? "(none)" : _me.Inventory.Equipped.MainHand.Name);
            else
            {
                //  Dlog("Mainhand weapon {0} imbued with {1} / {2}", _me.Inventory.Equipped.MainHand.Name, _me.Inventory.Equipped.MainHand.TemporaryEnchantment.Name, weaponEnchants[0]);
            }

            if (_needOffhandImbue)
                Dlog("Offhand  weapon {0} needs imbue", _me.Inventory.Equipped.OffHand == null ? "(none)" : _me.Inventory.Equipped.OffHand.Name);
            else
            {
                // Dlog("Offhand weapon {0} imbued with {1} / {2}", _me.Inventory.Equipped.OffHand.Name, _me.Inventory.Equipped.OffHand.TemporaryEnchantment.Name, weaponEnchants[3]);
            }
#else
            _needMainhandImbue = _me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 0;

            if (CanImbue(_me.Inventory.Equipped.OffHand))
                _needOffhandImbue = _me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == 0;
#endif

            return _needMainhandImbue || _needOffhandImbue;
        }

        private void GetBestWeaponImbues(out string enchantMainhand, out string enchantOffhand)
        {
            List<string> listMainhand = null;
            List<string> listOffhand = null;

            enchantMainhand = "";
            enchantOffhand = "";

            if (IsPVP())
            {
                enchantMainhand = cfg.PVP_MainhandImbue;
                enchantOffhand = cfg.PVP_OffhandImbue;
            }
            else
            {
                enchantMainhand = cfg.PVE_MainhandImbue;
                enchantOffhand = cfg.PVE_OffhandImbue;
            }

            // Dlog("gbwe1 --  mh:{0},  oh:{1}", enchantMainhand, enchantOffhand);
            switch (typeShaman)
            {
                case ShamanType.Unknown:
                    return;
                case ShamanType.Elemental:
                    // Dlog("Enchant - choosing Elemental Defaults for Auto");
                    listMainhand = _enchantElemental;
                    listOffhand = listMainhand;
                    break;
                case ShamanType.Resto:
                    // Dlog("Enchant - choosing Restoration Defaults for Auto");
                    listMainhand = _enchantResto;
                    listOffhand = _enchantResto;
                    break;
                case ShamanType.Enhance:
                    if (IsPVP())
                    {
                        // Dlog("Enchant - choosing PVP Enhancement Defaults for Auto");
                        listMainhand = _enchantEnhancementPVP_Mainhand;
                        listOffhand = _enchantEnhancementPVP_Offhand;
                    }
                    else
                    {
                        // Dlog("Enchant - choosing PVE Enhancement Defaults for Auto");
                        listMainhand = _enchantEnhancementPVE_Mainhand;
                        listOffhand = _enchantEnhancementPVE_Offhand;
                    }
                    break;
            }

            // Dlog("gbwe2 --  mh:{0},  oh:{1}", enchantMainhand, enchantOffhand);

            if ('A' == enchantMainhand.ToUpper()[0] && listMainhand != null)
            {
                enchantMainhand = listMainhand.Find(spellname => SpellManager.HasSpell(spellname));
                //Dlog("Enchant - Mainhand:  configured for AUTO so choosing '{0}'", enchantMainhand);
            }
            else
            {
                //Dlog("Enchant - Mainhand:  configured for '{0}'", enchantMainhand);
            }

            if ('A' == enchantOffhand.ToUpper()[0] && listOffhand != null)
            {
                enchantOffhand = listOffhand.Find(spellname => SpellManager.HasSpell(spellname));
                //Dlog("Enchant - Offhand:   configured for AUTO so choosing '{0}'", enchantOffhand);
            }
            else
            {
                //Dlog("Enchant - Offhand:   configured for '{0}'", enchantOffhand);
            }

            return;
        }

        private Countdown waitImbueCast = new Countdown();
        private const int IMBUE_CHECK_DELAY = 1000;

        // ImbueWeapons imbues the mainhand and offhand weapons with the best determined
        // or user selected weapon imbues.  It will imbue a maximum of 1 weapon per call,
        // and will require at least a minimum time to have passed since the last weapon 
        // imbue call (to ensure 
        private bool ImbueWeapons()
        {
            bool castSpell = false;

            try
            {
                if (!_needMainhandImbue && !_needOffhandImbue)
                    return false;

                string enchantMainhand;
                string enchantOffhand;

                GetBestWeaponImbues(out enchantMainhand, out enchantOffhand);

                if (_needMainhandImbue)
                {
                    if (!waitImbueCast.Done)
                    {
                        Dlog("ImbueWeapons(Mainhand):  need to wait {0} ms until next weapon imbue", waitImbueCast.Remaining);
                    }
                    else
                    {
                        Dlog("ImbueWeapons:  NeedOnMainhand={0}{1}", _needMainhandImbue, !_needMainhandImbue ? "" : "-" + enchantMainhand);
                        castSpell = Safe_CastSpell(enchantMainhand, SpellRange.NoCheck, SpellWait.Complete);
                        _needMainhandImbue = !castSpell;
                    }
                }

                if (_needOffhandImbue)
                {
                    if (!waitImbueCast.Done)
                    {
                        Dlog("ImbueWeapons(Offhand):  need to wait {0} ms until next weapon imbue", waitImbueCast.Remaining);
                    }
                    else
                    {
                        Dlog("ImbueWeapons:  NeedOnOffhand={0}{1}", _needOffhandImbue, !_needOffhandImbue ? "" : "-" + enchantOffhand);
                        castSpell = Safe_CastSpell(enchantOffhand, SpellRange.NoCheck, SpellWait.Complete);
                        _needOffhandImbue = !castSpell;
                    }
                }
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug("HB EXCEPTION in ImbueWeapons()");
                Logging.WriteException(e);
            }

            if (castSpell)
            {
                waitImbueCast.Remaining = IMBUE_CHECK_DELAY;
            }

            return castSpell;
        }

        /*
         * Checks to see if Off Hand slot currently has a weapon in it.
         * Uses a timer so that LUA call is not made more than once a minute
         */
        private bool CanImbue(WoWItem item)
        {
            if (item != null && item.ItemInfo.IsWeapon)
            {
                switch (item.ItemInfo.WeaponClass)
                {
                    case WoWItemWeaponClass.Axe:
                        return true;
                    case WoWItemWeaponClass.AxeTwoHand:
                        return true;
                    case WoWItemWeaponClass.Dagger:
                        return true;
                    case WoWItemWeaponClass.Fist:
                        return true;
                    case WoWItemWeaponClass.Mace:
                        return true;
                    case WoWItemWeaponClass.MaceTwoHand:
                        return true;
                    case WoWItemWeaponClass.Polearm:
                        return true;
                    case WoWItemWeaponClass.Staff:
                        return true;
                    case WoWItemWeaponClass.Sword:
                        return true;
                    case WoWItemWeaponClass.SwordTwoHand:
                        return true;
                }
            }

            return false;
        }

        public static bool IsAuraPresent(WoWUnit unit, string sAura)
        {
            uint stackCount;
            return IsAuraPresent(unit, sAura, out stackCount);
        }

        public static bool IsAuraPresent(WoWUnit unit, string sAura, out uint stackCount)
        {
            stackCount = 0;
            if (unit == null)
                return false;

            // HonorBuddy has a bug which mishandles stack count when a buff has the
            // .. same name as a talent.  maelstrom weapon and tidal waves are only ones for Shaman
            if (unit.IsMe && (sAura.ToLower() == "maelstrom weapon" || sAura.ToLower() == "tidal waves"))
            {
                List<string> myAuras = Lua.GetReturnValues("return UnitAura(\"player\",\"" + sAura + "\")");
                if (Equals(null, myAuras))
                    return false;

                stackCount = (uint)Convert.ToInt32(myAuras[3]);
                return true;
            }

            // otherwise, use more efficient aura retrieval
            WoWAura aura = GetAura(unit, sAura);
            if (aura == null)
                return false;

            stackCount = aura.StackCount;
            return true;
        }

        public static uint GetAuraStackCount(WoWUnit unit, string auraName)
        {
            uint stackCount = 0;
            bool isPresent = IsAuraPresent(unit, auraName, out stackCount);
            return stackCount;
        }

        public static WoWAura GetAura(WoWUnit unit, string auraName)
        {
            if (unit == null)
                return null;

            WoWAura aura = (from a in unit.Auras
                            where 0 == string.Compare(a.Value.Name, auraName, true)
                            select a.Value).FirstOrDefault();
            return aura;
        }

        private WoWAura GetAuraCreatedByMe(WoWUnit unit, string auraName)
        {
            if (unit == null)
                return null;

            WoWAura aura = (from a in unit.Auras
                            where a.Value.CreatorGuid == _me.Guid
                                  && 0 == string.Compare(a.Value.Name, auraName, true)
                            select a.Value).FirstOrDefault();
            return aura;
        }

        public static bool InVehicle()
        {
            int inVehicle = Lua.GetReturnVal<int>("return UnitInVehicle(\"player\")", 0);
            return inVehicle == 1;
        }

#if NOT_COMPLETED
        private void CancelAura(string auraName)
        {
            KeyValuePair<string,WoWAura> aura = ObjectManager.Me.Auras.FirstOrDefault(a => a.Key.ToLower() == auraName.ToLower());
            if (aura != null && aura.Value != null && aura.Value.Cancellable)
            {
                // SpellManager.
            }
        }
#endif

        public static Dictionary<int, Mob> _dictMob = new Dictionary<int, Mob>();


        private static WoWPartyMember.GroupRole GetGroupRoleAssigned(WoWPlayer p)
        {
            if (ObjectManager.Me.IsInParty || ObjectManager.Me.IsInRaid)
            {
                WoWPartyMember pm = GroupMemberInfos.FirstOrDefault(gm => gm.Guid == p.Guid);
                if (pm != null)
                {
                    // .Role is returning 1's bit set but no enum established... ?
                    return (WoWPartyMember.GroupRole)((int)pm.Role & 0x0FE);
                }
            }
         
            return WoWPartyMember.GroupRole.None;
        }


        private static WoWPlayer _prevLeader = null;

        private void CheckGroupRoleAssignments()
        {
            if (IsPVP() || (!_me.IsInParty && !_me.IsInRaid))
            {
                if (GroupTank != null || GroupHealer != null || _myGroupRole != WoWPartyMember.GroupRole.None)
                {
                    Slog("^EVENT:  Left Party/Raid ...");
                    GroupHealer = null;
                    _myGroupRole = WoWPartyMember.GroupRole.None;
                }

                return;
            }

            WoWPartyMember.GroupRole prevMyRole = _myGroupRole;
            WoWPlayer prevGroupHealer = GroupHealer;

            // get my current role
            _myGroupRole = GetGroupRoleAssigned(_me);

            // check healer is still good
            GroupHealer = null;
            foreach (WoWPartyMember p in GroupMemberInfos )
            {
                WoWPartyMember.GroupRole role = p.Role;
                if (role == WoWPartyMember.GroupRole.Healer && p.Guid != _me.Guid)
                    GroupHealer = p.ToPlayer();
#if COMMENT
                else if (role == GroupRole.Tank && (GroupTank == null || GroupTank.IsMe))
                {
                    Log("ShamWOW:  No tank set yet - setting to {0}", Safe_UnitID(p));
                    RaFHelper.SetLeader(p);
                }
#endif
            }

            if (prevMyRole != _myGroupRole || _prevLeader != GroupTank || prevGroupHealer != GroupHealer)
            {
                Slog("^EVENT:  Party/Raid Members Changed ...");
                Dlog("CheckGroupRoleAssignments:  my role changed={0}, tank changed={1}, healer changed={2}",
                    BoolToYN(prevMyRole != _myGroupRole),
                    BoolToYN(_prevLeader != GroupTank),
                    BoolToYN(prevGroupHealer != GroupHealer)
                    );

                if (_myGroupRole != WoWPartyMember.GroupRole.Tank)
                    Slog("RAF:  TANK = {0}", GroupTank == null ? "none currently" : Safe_UnitID(GroupTank));
                if (_myGroupRole != WoWPartyMember.GroupRole.Healer)
                    Slog("RAF:  HEALER = {0}", GroupHealer == null ? "none currently" : Safe_UnitID(GroupHealer));
                Slog("RAF:  {0} = {1}", _myGroupRole.ToString().ToUpper(), Safe_UnitName(_me));
            }
        }


        private bool HaveValidTarget()
        {
            return _me.GotTarget && _me.CurrentTarget.IsAlive;
            //                && !Blacklist.Contains( t.Guid );
        }

        /*
         * CurrentTargetInMeleeDistance()
         * 
         * Check to see if CurrentTarget is within melee range.  This allows
         * recognizing when a pulled mob is close enough to melee as well as 
         * as when a pulled mob moves out of melee
         */
        static ulong guidLastMob;
        static int meleeRangeCheck;
        const int STD_MELEE_RANGE = 5;

        private bool CurrentTargetInMeleeDistance()
        {
            if (!_me.GotTarget)
                return false;

            if (guidLastMob != _me.CurrentTargetGuid)
            {
                // set good defaults in case we hit an exception in mob check after
                guidLastMob = _me.CurrentTargetGuid;
                meleeRangeCheck = STD_MELEE_RANGE;

                // check if npc is setup with special handling / behavior
                if (!_me.CurrentTarget.IsPlayer)
                {
                    Mob mob = (from m in _dictMob where m.Key == _me.CurrentTarget.Entry select m.Value).FirstOrDefault();
                    if (mob != null)
                        meleeRangeCheck = mob.HitBox;
                }
            }

            return IsUnitInRange(_me.CurrentTarget, meleeRangeCheck);
        }

        /*
         * CurrentTargetInRangedDistance()
         * 
         * Check to see if CurrentTarget is within ranged attack distance and line of sight.  This allows
         * recognizing when a pulled mob is close enough to melee as well as 
         * as when a pulled mob moves out of melee
         */
        private bool CurrentTargetInRangedDistance()
        {
            return IsUnitInRange(_me.CurrentTarget, _maxDistForRangeAttack);
        }

        private bool CurrentTargetInRangedPullDistance()
        {
            return IsUnitInRange(_me.CurrentTarget, _offsetForRangedPull);
        }

        private static bool IsUnitInRange(WoWUnit unit, double range)
        {
            return (unit != null && unit.Distance < range && unit.InLineOfSightOCD);
        }

        /*
         * trys to determine if 'unit' points to a mob that is a Caster.  Currently
         * only see .Class as being able to help determine.  the big question marks
         * are Druids and Shamans for grinding purposes, so even though we try
         * to guess we still make routines able to adapt on pulls, etc. to fact
         * mob may not behave like we guessed
         */
        private static bool IsCaster(WoWUnit unit)
        {
            bool isCaster = false;

            switch (unit.Class.ToString().ToLower())
            {

                default:
                    Slog("UKNOWN MOB CLASS:  CONTACT CC DEVELOPER:  Please provide the class name '" + unit.Class + "' and name [" + Safe_UnitName(unit) + "]");
                    break;

                case "paladin":
                case "druid":
                case "rogue":
                case "warrior":
                case "death knight":
                    break;

                case "mage":
                case "warlock":
                case "shaman":
                case "priest":
                    isCaster = true;
                    break;
            }

            // following test added because of "Unyielding Sorcerer" in Hellfire
            // .. having a class of Paladin, yet they fight as ranged casters

            if (!isCaster)
            {
                // LOCALIZATION ISSUES
                if (unit.Name.ToLower().Contains("sorcerer"))
                    isCaster = true;
                else if (unit.Name.ToLower().Contains("shaman"))
                    isCaster = true;
                else if (unit.Name.ToLower().Contains("mage"))
                    isCaster = true;
                else if (unit.Name.ToLower().Contains("warlock"))
                    isCaster = true;
                else if (unit.Name.ToLower().Contains("priest"))
                    isCaster = true;
                else if (unit.Name.ToLower().Contains("wizard"))
                    isCaster = true;
                else if (unit.Name.ToLower().Contains("adept"))
                    isCaster = true;
            }

            return isCaster;
        }

        private static void AddToBlacklist(ulong guidMob)
        {
            AddToBlacklist(guidMob, System.TimeSpan.FromMinutes(5));
        }

        private static void AddToBlacklist(ulong guidMob, System.TimeSpan ts)
        {
            if (Blacklist.Contains(guidMob))
                Dlog("already blacklisted mob: " + guidMob);
            else
            {
                Blacklist.Add(guidMob, ts);
                Dlog("blacklisted mob: " + guidMob);
            }
        }

        /*
         * CheckForItem()
         * 
         * Lookup an item by its item # 
         * return null if not found
         */
        private static WoWItem CheckForItem(List<uint> listId)
        {
            WoWItem item = ObjectManager.GetObjectsOfType<WoWItem>(false).Find(unit => listId.Contains(unit.Entry));
            return item;
        }

        private static WoWItem CheckForItem(uint itemId)
        {
            WoWItem item = ObjectManager.GetObjectsOfType<WoWItem>(false).Find(unit => unit.Entry == itemId);
            return item;
        }

        private static WoWItem CheckForItem(string itemName)
        {
            WoWItem item;
            uint id;

            if (uint.TryParse(itemName, out id) && id > 0)
                item = ObjectManager.GetObjectsOfType<WoWItem>(false).Find(unit => unit.Entry == id);
            else
                item = ObjectManager.GetObjectsOfType<WoWItem>(false).Find(unit => 0 == string.Compare(unit.Name, itemName, true));

            return item;
        }

        #endregion


        #region REST

        private static bool _loadingScreen = false;

        public static bool IsGameUnstable()
        {
#if LOOP_WHILE_UNSTABLE
            for (; ; )
            {
                if (!Safe_IsValid(_me) )
                {
                    _loadingScreen = true;
                    Dlog("GameUnstable: HB or WOW initializing... not ready yet (Me == null)");
                    Thread.Sleep(1000);
                    continue;
                }

                if (!ObjectManager.IsInGame)
                {
                    _loadingScreen = true;
                    Dlog("GameUnstable: Detected Loading Screen... sleeping for 1 sec");
                    Thread.Sleep(1000);
                    continue;
                }

                if (_loadingScreen)
                {
                    _loadingScreen = false;
                    Dlog("GameUnstable: wait another 2 secs after Loading Screen goes away");
                    Thread.Sleep(2000);
                    continue;
                }

                if (IsPVP() && Battlegrounds.Finished)
                {
                    Dlog("GameUnstable: detected battlefield complete / scoreboard");
                    Thread.Sleep(2000);
                    continue;
                }

                // if we make it here, we are completed so can exit
                break;
            }
#else
            if (!Safe_IsValid(_me))
            {
                _loadingScreen = true;
                Dlog("GameUnstable: HB or WOW initializing... not ready yet (Me == null)");
                Thread.Sleep(1000);
                return true;
            }

            if (!ObjectManager.IsInGame)
            {
                _loadingScreen = true;
                Dlog("GameUnstable: Detected Loading Screen... sleeping for 1 sec");
                Thread.Sleep(1000);
                return true;
            }

            if (_loadingScreen)
            {
                _loadingScreen = false;
                Dlog("GameUnstable: wait another 2 seconds after Loading Screen goes away to be safe");
                Thread.Sleep(2000);
                return true;
            }

            if (IsPVP() && Battlegrounds.Finished)
            {
                Dlog("GameUnstable: detected battlefield complete / scoreboard");
                Thread.Sleep(1000);
                return true;
            }
#endif

            return false;
        }

        public override void Pulse()
        {
            // base.Pulse();    // does nothing
            TotemManagerUpdate();

            if (!_me.GotTarget && GotAlivePet() ) // && !IsPVP())
            {
                Dlog("Pulse:  no current target, but live pet");
                bool foundAggro = FindAggroTarget();
                if (foundAggro)
                    Dlog("Pulse:  found aggro");
            }
        }

        public override bool NeedRest
        {
            get
            {
                bool doWeNeedRest = false;
                try
                {
                    // Dlog("NEEDREST? START: " + _me.HealthPercent + "% health,  " + _me.ManaPercent + "% mana");
                    doWeNeedRest = NeedRestLogic();
                    // Dlog("NEEDREST? RETURN STATUS= {0}", doWeNeedRest  );
                }
                catch (ThreadAbortException) { throw; }
                catch (Exception e)
                {
                    Log(Color.Red, "An Exception occured. Check debug log for details.");
                    Logging.WriteDebug("HB EXCEPTION in NeedRest");
                    Logging.WriteException(e);
                }

                return doWeNeedRest;
            }
        }

        private bool NeedRestLogic()
        {
            if (IsGameUnstable())
                return false;

            if (_me.IsFlying || _me.OnTaxi || _me.IsOnTransport || InVehicle())
                return false;

            // following allows returning immediately when a plug-in or other
            // .. has casted a spell and returned to HB.  this improves the
            // .. bobber recognition of AutoAngler specifically
            if (HandleCurrentSpellCast())
            {
                Dlog("NeedRest:  aborted since casting");
                return false;
            }

            if (IsHealer())
            {
                if (NeedRestLogicResto())         // don't do anything else if we have to heal
                    return false;
            }                           // .. otherwise fall through to NeedRest tests

            ReportBodyCount();

            // don't allow NeedRest=true if we just started a pull
            if (!_pullTimer.Done)
                return false;

            if (Battlegrounds.Finished)
                return false;

            if (_me.Combat)
            {
                Dlog("NeedRest:  i am in Combat.... calling Combat() from NeedRest");
                Combat();
                return false;
            }

            if (IsRAFandTANK() && GroupTank.Combat)
            {
                Dlog("NeedRest:  RAF Leader in Combat... calling Combat from here");
                Combat();
                return false;
            }

            // if we switched modes ( grouped, battleground, or spec chg)
            if (DidWeSwitchModes())
            {
                Slog("^OPERATIONAL MODE CHANGED:  initializing...");
                Initialize();
                return true;
            }

            if (!IsHealerOnly())
            {
                if (IsPVP())
                {
                    if (_me.GotTarget && _me.CurrentTarget.IsPlayer && _me.CurrentTarget.ToPlayer().IsHorde != _me.IsHorde && _me.CurrentTarget.Distance < _maxDistForRangeAttack && !_me.CurrentTarget.Mounted)
                    {
                        Slog("BGCHK:  calling Combat() myself from NeedRest for CurrentTarget");
                        Combat();
                        return false;
                    }

                    if (FindBestTarget())
                    {
                        Dlog("BGCHK: calling Combat() myself from NeedRest for FindBestTarget()");
                        Combat();
                        return false;
                    }
                }
            }

            // check to be sure not in a travelling state before
            //.. setting switches that will cause a dismount or form change
            if (_me.Mounted || InGhostwolfForm())
            {
                // Dlog("Mounted or Ghostwolf - will wait to buff/enchant out of form");
            }
            else
            {
                if (IsRAF() && (_me.Combat || (GroupTank != null && GroupTank.Combat)))
                    ;   // suppress recall totem check
                else if (_WereTotemsSet && CheckForSafeDistance("Totem Recall", _ptTotems, CheckDistanceForRecall()))
                {
                    Dlog("Need rest: TotemsWereSet() and recall CheckForSafeDistance({0:F1})= true", CheckDistanceForRecall());
                    if (!_me.GotTarget)
                        Dlog("Need rest: no current target so looks good to recall");
                    else
                    {
                        WoWUnit unit = _me.CurrentTarget;
                        Dlog("Need rest: target:{0} atkable:{1} hostile:{2}, profile:{3} alive:{4}", unit.Distance,
                             unit.Attackable,
                             Safe_IsHostile(unit),
                             Safe_IsProfileMob(unit),
                             unit.IsAlive);
                    }
                    _RecallTotems = true;
                    return true;
                }

                if (IsWeaponImbueNeeded())
                {
                    Dlog("Need rest: true, IsWeaponImbueNeeded mh:{0} oh:{1}", _needMainhandImbue, _needOffhandImbue);
                    return true;
                }

                if (IsShieldBuffNeeded(true))
                {
                    Dlog("Need rest: true, ShieldBuffNeeded -- Mounted={0}, Flying={1}", _me.Mounted, _me.IsFlying);
                    return true;
                }

                if (IsCleanseNeeded(_me) != null)
                {
                    Dlog("Need rest: true, IsCleanseNeeded");
                    _castCleanse = true;
                    return true;
                }

                if (_me.IsSwimming)
                {
                    /*
                    if (!_me.HasAura("Water Walking") && SpellManager.CanCast("Water Walking"))
                    {
                        Dlog("Need rest: true, Swimming and need Water Walking");
                        return true;
                    }
                     */
                }
                else if (_me.HealthPercent <= cfg.RestHealthPercent)
                {
                    Dlog("Need rest: true, CurrentHealth {0:F1}% less than RestHealthPercent {1:F1}%", _me.HealthPercent, cfg.RestHealthPercent);
                    return true;
                }

                if (_me.ManaPercent <= cfg.RestManaPercent && !_me.IsSwimming)
                {
                    Dlog("Need rest: true, CurrentMana {0:F1}% less than RestManaPercent {1:F1}%", _me.ManaPercent, cfg.RestManaPercent);
                    return true;
                }

                if (_needClearWeaponEnchants)
                {
                    Dlog("Need rest: true, Need to Clear Weapon Enchants flag is set");
                    return true;
                }

                if (_needTotemBarSetup)
                {
                    Dlog("Need rest: true, Need to Setup Totem Bar flag is set");
                    return true;
                }
            }

            // ONLY set the _rezTarget after heals and mana taken care 
            //  ..  so ready in case attacked while rezzing
            _rezTarget = null;
            if (IsRAF() && SpellManager.HasSpell("Ancestral Spirit"))
            {
                // !p.IsAFKFlagged 
                _rezTarget = (from p in GroupMembers where Safe_IsValid(p) && p.Dead && !Blacklist.Contains(p) select p).FirstOrDefault();
                if (_rezTarget != null)
                {
                    Dlog("NeedRestLogic:  dead party member {0} @ {1:F1} yds", Safe_UnitName(_rezTarget), _rezTarget.Distance);
                    return true;
                }
            }

            if (CharacterSettings.Instance.UseMount && cfg.UseGhostWolfForm)
            {
                Wlog("warning:  UseMount takes precedence over UseGhostWolf:  the Ghost Wolf setting ignored this session");
            }
            else if (!_me.Mounted && !_me.IsFlying && !_me.OnTaxi && !IsRAF() && cfg.UseGhostWolfForm)
            {
                if (SpellManager.HasSpell("Ghost Wolf") && !InGhostwolfForm() && _me.IsOutdoors)
                {
                    if (CheckForSafeDistance("Ghost Wolf", _me.Location, cfg.DistanceForGhostWolf))
                    {
                        Dlog("Need rest: true, Ghost Wolf: closest enemy at least {0:F1} yds away", cfg.DistanceForGhostWolf);
                        if (SpellManager.CanCast("Ghost Wolf"))  // make sure we can so not stuck in loop
                            _castGhostWolfForm = true;
                        return true;
                    }
                }
            }

            if (NeedToBuffRaid())
            {
                Dlog("NeedRestLogic: raid members need buff");
                return true;
            }

            return false;
        }

        // bool inRest = false;
        public override void Rest()
        {
            try
            {
                ShowStatus("Enter REST");
                RestLogic();
                ShowStatus("Exit REST");
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug(">>> EXCEPTION: occurred in Rest()");
                Logging.WriteException(e);
            }
        }

        public void RestLogic()
        {
            if (_castCleanse && CleanseIfNeeded(_me))
            {
                _castCleanse = false;
                return;
            }

            if (_RecallTotems)
            {
                _RecallTotems = false;
                RecallTotemsForMana();
                return;
            }

            // try to use bandages, but only if we don't need Mana (if we need to drink, might as well eat too)
            if (_me.HealthPercent < cfg.RestHealthPercent && _me.ManaPercent > cfg.RestManaPercent)
            {
                if (cfg.UseBandages)
                {
                    if (UseBandageIfAvailable())
                        return;
                }
            }

            // try to heal several times (quicker than eating).  also heal to higher OOC level in PVP
            if (_me.HealthPercent < GetSelfHealThreshhold() && _me.ManaPercent >= cfg.RestManaPercent)
            {
                while (!IsGameUnstable() && _me.IsAlive && _me.HealthPercent < GetSelfHealThreshhold())
                {
                    if (_me.Combat || MeImmobilized())
                        return;

                    if (!HealMyself(GetSelfHealThreshhold()))
                        break;  // exit loop if we can't cast a heal for some reason
                }

                // already need to drink now, so may as well top-off health first
                while (!IsGameUnstable() && _me.IsAlive && _me.ManaPercent < cfg.RestManaPercent && _me.HealthPercent < 85)
                {
                    if (_me.Combat || MeImmobilized())
                        return;

                    if (!HealMyself(GetSelfHealThreshhold()))
                        break;  // exit loop if we can't cast a heal for some reason
                }
            }

            // 
            if (_needClearWeaponEnchants)
            {
                _needClearWeaponEnchants = false;
                RunLUA("CancelItemTempEnchantment(1)");
                RunLUA("CancelItemTempEnchantment(2)");
                IsWeaponImbueNeeded();
                return;
            }

            if (_needTotemBarSetup)
            {
                _needTotemBarSetup = false;
                TotemSetupBar();
                Slog("");
            }

            // ressurrect target set in NeedRest
            //------------------------------------
            if (!Safe_IsValid(_rezTarget))
            {
                Dlog("Ressurection:  dead ressurection target is invalid, resetting");
                _rezTarget = null;
            }
            else if (!_rezTarget.Dead)
            {
                Dlog("Ressurection:  dead ressurection target {0} is no longer dead", Safe_UnitName(_rezTarget));
                _rezTarget = null;
            }
            else if (!IsUnitInRange(_rezTarget, 28))
            {
                MoveToUnit(_rezTarget);
            }
            else
            {
                Safe_StopMoving();
                Log("^Ressurection:  dead target {0} is {1:F1} yds away", Safe_UnitName(_rezTarget), _rezTarget.Distance);

#if NOTRIGHTNOW
				if ( !SpellManager.Cast("Ancestral Spirit", _rezTarget ))
				{
					Dlog("Rez:  spell cast failed?  abort");
					return;
				}
#else
                if (!Safe_CastSpell(_rezTarget, "Ancestral Spirit", SpellRange.Check, SpellWait.NoWait))
                    return;
#endif
                StyxWoW.SleepForLagDuration();
                while (!IsGameUnstable() && _me.IsAlive && IsCasting())
                {
                    if (!Safe_IsValid(_rezTarget))
                    {
                        Dlog("Ressurection:  dead ressurrection target is invalid... Stop Casting...!!!");
                        SpellManager.StopCasting();
                    }
                    else if (_rezTarget.IsAlive)
                    {
                        Dlog("Ressurection:  {0} is alive... Stop Casting...!!!", Safe_UnitName(_rezTarget));
                        SpellManager.StopCasting();
                    }
                    StyxWoW.SleepForLagDuration();
                }

                Log("^Ressurection:  attempt completed, blacklisting {0} for 30 seconds", Safe_UnitName(_rezTarget));
                // blacklist so if they have a rez pending but haven't clicked yes,
                //  ..  we move onto rezzing someone else
                Blacklist.Add(_rezTarget, TimeSpan.FromSeconds(30));
                _rezTarget = null;
                return;
            }

            // Dlog("RestLogic:  before ShamanBuffs");
            ShamanBuffs(true);

            if (_castGhostWolfForm && SpellManager.CanCast("Ghost Wolf"))
            {
                GhostWolf();
                _castGhostWolfForm = false;
            }

            // now eat/drink if needed
            if ((_me.HealthPercent < cfg.RestHealthPercent || _me.ManaPercent < cfg.RestManaPercent) && !_me.IsSwimming)
            {
                bool noFood = false;
                bool noDrink = false;
                bool stoppedToEat = _me.HealthPercent < cfg.RestHealthPercent;
                bool stoppedToDrink = _me.ManaPercent < cfg.RestManaPercent;

                Safe_StopMoving();

                WaitForCurrentSpell(null);
                if (stoppedToEat)
                {
                    noFood = !UseConsumeable(CharacterSettings.Instance.FoodName);
                    if (noFood)
                        Log(Color.Red, "No food left, staying here waiting for health to regen to {0}%", cfg.RestHealthPercent);
                    else
                    {
                        _countFood++;
                        Dlog("Eating:  {0} total used, average {1:F0} per hour",
                            _countFood,
                            (60.0 * 60.0 * 1000.0 * _countFood) / timeStart.ElapsedMilliseconds
                            );
                    }
                }

                if (stoppedToDrink)
                {
                    // confirm the drink isn't the same as the food
                    if (stoppedToEat && 0 == String.Compare(CharacterSettings.Instance.FoodName, CharacterSettings.Instance.DrinkName, true))
                        ;
                    else
                    {
                        noDrink = !UseConsumeable(CharacterSettings.Instance.DrinkName);
                        if (noDrink)
                            Log(Color.Red, "No drinks left, staying here waiting for mana to regen to {0}%", cfg.RestManaPercent);
                        else
                        {
                            _countDrinks++;
                            Dlog("Drinking:  {0} total used, average {1:F0} per hour",
                                _countDrinks,
                                (60.0 * 60.0 * 1000.0 * _countDrinks) / timeStart.ElapsedMilliseconds
                                );
                        }
                    }
                }

                if (noFood == false && noDrink == false)
                {
                    Slog("Stopped to {0}{1}{2}",
                         stoppedToEat ? "Eat" : "",
                         stoppedToEat && stoppedToDrink ? " and " : "",
                         stoppedToDrink ? "Drink" : ""
                        );
                }

                // wait until food/drink buffs display
                Countdown waitForAuras = new Countdown(750);
                while (!IsGameUnstable())
                {
                    bool auraVisible = IsAuraPresent(_me, "Food");
                    auraVisible = auraVisible || IsAuraPresent(_me, "Drink");
                    auraVisible = auraVisible || IsAuraPresent(_me, "Nourishment");
                    if (auraVisible)
                        break;

                    if (_me.Combat || !_me.IsAlive)
                        break;

                    if (waitForAuras.Done)
                        break;

                    if (stoppedToEat && !noFood)
                        continue;

                    if (stoppedToDrink && !noDrink)
                        continue;

                    break;
                }

                // wait until we are done eating
                while (!IsGameUnstable())
                {
                    bool keepWaiting = IsAuraPresent(_me, "Food") && _me.HealthPercent < 99;
                    keepWaiting = keepWaiting || (IsAuraPresent(_me, "Drink") && _me.ManaPercent < 99);
                    keepWaiting = keepWaiting || (IsAuraPresent(_me, "Nourishment") && _me.HealthPercent < 99);
                    keepWaiting = keepWaiting || (IsAuraPresent(_me, "Nourishment") && _me.ManaPercent < 99);

                    Dlog("dbg waiting:  Eating:{0} Health:{1:F0}%  /  Drinking:{2} Mana:{3:F0}%", IsAuraPresent(_me, "Food"), _me.HealthPercent, IsAuraPresent(_me, "Drink"), _me.ManaPercent);
                    if (_me.Combat || !_me.IsAlive || !keepWaiting)
                    {
                        Dlog("RestLogic:  stopped eat/drink because of combat={0} alive={1} keepwaiting={2}", _me.Combat, _me.IsAlive, keepWaiting);
                        break;
                    }

                    if (IsRAFandTANK() && GroupTank.Combat && GroupTank.HealthPercent < 70)
                        break;

                    Thread.Sleep(100);
                }
            }

            if (IsAuraPresent(_me, "Herbouflage"))
            {
                Slog("Cancelling Herbouflage...");
                RunLUA("CancelUnitBuff(\"player\", \"Herbouflage\")");
            }

            if ( NeedToBuffRaid() )
            {
                BuffRaid();
            }
        }

        public static bool InGhostwolfForm()
        {
            return _me.Auras.ContainsKey("Ghost Wolf");
        }

        private static bool GhostWolf()
        {
            bool b = false;

            if ((!_me.IsOutdoors || _me.IsIndoors) && !IsPVP())
                ;
            else if (!SpellManager.HasSpell("Ghost Wolf"))
                ;
            else
            {
                if (!_hasTalentAncestralSwiftness)
                    Safe_StopMoving();

                b = Safe_CastSpell("Ghost Wolf", SpellRange.NoCheck, SpellWait.Complete);
            }

            return b;
        }

        #endregion

        #region HANDLE FALLING

        public void HandleFalling() { }

        #endregion

        #region Buffs

        /*
		 * Note:  the following are interface functions that need to be implemented by the class.  
		 * They are not used presently in the ShamWOW implementation.  Buffs are handled within the 
		 * flow of the current Pull() and Combat() event handlers
		 */
        public override bool NeedPreCombatBuffs 
        { 
            get 
            { 
                bool need = NeedToBuffRaid();
                if (need)
                    Slog("NeedPreCombatBuffs:  need to buff raid");
                return need;
            } 
        }

        public override void PreCombatBuff() 
        {
            BuffRaid();
        }

        public override bool NeedPullBuffs { get { return false; } }
        public override void PullBuff() { }

        public override bool NeedCombatBuffs { get { return false; } }
        public override void CombatBuff() { }


        public void ShamanBuffs(bool atRest)
        {
            if (!_me.IsAlive)
                return;

            // Shield Twisting:  Cast based upon amount of Mana available
            ShieldTwisting(atRest);

            Dlog("ShamanBuffs:  AllowNonHealSpells:{0}, atrest:{1}", AllowNonHealSpells(), atRest);
            //            if (AllowNonHealSpells() && atRest != false)
            if (atRest && (_needMainhandImbue || _needOffhandImbue))
            {
                ImbueWeapons();
            }
        }


        private List<ulong> RaidBuffTargets
        {
            get
            {
                if (!GroupMembers.Contains(_me))
                    GroupMembers.Add(_me);

                return (from p in GroupMembers 
                 where p.Distance < 27
                 where !Blacklist.Contains(p.Guid)
                    && ((_hasGlyphOfWaterWalking && !IsAuraPresent(p, "Water Walking") && cfg.PVP_PrepWaterWalking )
                        || (_hasGlyphOfWaterBreathing && !IsAuraPresent(p, "Water Breathing") && cfg.PVP_PrepWaterBreathing ))
                 orderby p.Distance ascending
                 select p.Guid 
                 ).ToList();
            }
        }

        private bool NeedToBuffRaid()
        {
            if (IsGameUnstable())
                return false; 

            if (!IsPVP() || _me.CurrentHealth <= 1 || GroupMemberInfos.Count() > 15)
                return false;

            if (!cfg.PVP_PrepWaterBreathing && !cfg.PVP_PrepWaterWalking)
                return false;

            if (!IsAuraPresent(_me, "Preparation") && !IsAuraPresent( _me, "Arena Preparation"))
                return false;

            return RaidBuffTargets.Any();
        }

        private void BuffRaid()
        {
            if (_me.CurrentHealth <= 1)
                return;

            foreach (ulong guid in RaidBuffTargets)
            {
                if (IsGameUnstable())
                    return;

                if (!IsAuraPresent(_me, "Preparation") && !IsAuraPresent(_me, "Arena Preparation"))
                {
                    Slog("Stopping Buffs... start area Preparation buff faded");
                    break;
                }

                WoWPlayer p = ObjectManager.GetObjectByGuid<WoWPlayer>(guid);
                if (p != null)
                {
                    if ( _hasGlyphOfWaterWalking && !IsAuraPresent( p, "Water Walking")) 
                        Safe_CastSpell( p, "Water Walking", SpellRange.Check, SpellWait.Complete);


                    if ( _hasGlyphOfWaterBreathing && !IsAuraPresent( p, "Water Breathing")) 
                        Safe_CastSpell( p, "Water Breathing", SpellRange.Check, SpellWait.Complete);

                    int duration = (Environment.TickCount & 1) == 0 ? 7 : 14;
                    AddToBlacklist( guid, TimeSpan.FromSeconds( duration ));
                }
            }
        }


        #endregion


        #region Heal

        /*
		 * NeedHeal
		 * 
		 * return a true/false indicating whether the Heal() event handler should be called by the
		 * HonorBuddy engine.
		 */
        public override bool NeedHeal
        {
            get
            {
                if (_me.IsFlying || _me.OnTaxi || _me.IsOnTransport)
                    return false;

                bool isHealNeeded = !_me.Combat && IsSelfHealNeeded();
                if (isHealNeeded)
                    ShowStatus("NeedHeal=YES!!!");
                return isHealNeeded;
            }
        }

        private bool IsSelfHealNeeded()
        {
            int threshhold = GetSelfHealThreshhold();

            if (_me.HealthPercent <= threshhold
                && countEnemy == 1
                && !IsFightStressful()
                && AllowNonHealSpells()
                && _me.GotTarget
                && _me.CurrentTarget.HealthPercent < 10.0
                && _me.CurrentTarget.IsAlive
                )
            {
                Log("^Enemy weak at {0:F0}%, skipping heal", _me.CurrentTarget.HealthPercent);
                return false;
            }

            return !MeSilenced() && _me.HealthPercent <= threshhold && SpellManager.HasSpell(HEALING_WAVE);
        }

        /*
         * Heal()
         * 
         * Called if a heal is needed.
         */
        public override void Heal()
        {
            ShowStatus("Enter HEAL");
            HealMyself();
            ShowStatus("Exit HEAL");
        }

        private int GetSelfHealThreshhold()
        {
            int threshhold;
            // for RAF, count on healer to heal and only self-heal in emergency
            if (IsRAF())
                threshhold = GroupHealer != null && GroupHealer.CurrentHealth > 1 ? cfg.EmergencyHealthPercent : cfg.NeedHealHealthPercent;
            // for Battlegounds, use NeedHeal in Combat, but top-off when Resting
            else if (IsPVP())
                threshhold = _me.Combat ? cfg.NeedHealHealthPercent : 85;
            // for Grinding/Questing  
            else if (!_me.Combat)
                threshhold = cfg.RestHealthPercent;
            else
                threshhold = cfg.NeedHealHealthPercent + (IsFightStressful() ? 10 : 0);

            return threshhold;
        }

        private bool AllowNonHealSpells()
        {
            return _me.ManaPercent > cfg.EmergencyManaPercent && _me.HealthPercent > cfg.EmergencyHealthPercent;
        }


        /*
         * IsCleanseNeeded()
         * 
         * Called cleanse if needed.
         */
        public WoWAura IsCleanseNeeded(WoWUnit unit)
        {
            // if we don't have any or cleansing disabled, exit quickly
            if (MeSilenced() || !unit.Debuffs.Any() || !cfg.GroupHeal.Cleanse)
                return null;

            bool knowCleanseSpirit = SpellManager.CanCast("Cleanse Spirit");
            bool canCleanMagic = knowCleanseSpirit && _hasTalentImprovedCleanseSpirit;
            bool canCleanCurse = knowCleanseSpirit;
            bool canStoneform = SpellManager.CanCast("Stoneform") && unit.IsMe;

            bool isBlacklisted = (from dbf in unit.Debuffs
                                  where _hashCleanseBlacklist.Contains(dbf.Value.SpellId)
                                  select dbf.Value
                                 ).Any();

            if (isBlacklisted)
                return null;

            WoWAura dispelDebuff = (
                                       from dbf in unit.Debuffs
                                       where
                                           (dbf.Value.Spell.DispelType == WoWDispelType.Curse && canCleanCurse)
                                           || (dbf.Value.Spell.DispelType == WoWDispelType.Magic && canCleanMagic)
                                           || (dbf.Value.Spell.DispelType == WoWDispelType.Magic && unit.IsMe && _hasGlyphOfShamanisticRage)
                                           || (dbf.Value.Spell.DispelType == WoWDispelType.Poison && canStoneform)
                                           || (dbf.Value.Spell.DispelType == WoWDispelType.Disease && canStoneform)
                                       select dbf.Value
                                   ).FirstOrDefault();

            return dispelDebuff;
        }

        public bool CleanseIfNeeded(WoWUnit unit)
        {
            return CleanseIfNeeded(unit, null);
        }

        public bool CleanseIfNeeded(WoWUnit unit, WoWAura dispelDebuff)
        {
            // if we don't have any, exit quickly
            if (unit == null || !unit.Debuffs.Any())
                return false;

            bool castSpell = false;

            if (dispelDebuff == null)
                dispelDebuff = IsCleanseNeeded(unit);

            if (dispelDebuff != null)
            {
                Log("^Dispel target {0}[{1}] at {2:F1} yds has debuf '{3}' with {4} secs remaining", Safe_UnitName(unit), unit.Level, unit.Distance, dispelDebuff.Name, dispelDebuff.TimeLeft.Seconds);
                if (dispelDebuff.Spell.DispelType == WoWDispelType.Poison || dispelDebuff.Spell.DispelType == WoWDispelType.Disease)
                    castSpell = Safe_CastSpell(unit, "Stoneform", SpellRange.NoCheck, SpellWait.Complete);
                else if (unit.IsMe && _hasGlyphOfShamanisticRage && dispelDebuff.Spell.DispelType == WoWDispelType.Magic)
                    castSpell = Safe_CastSpell(unit, "Shamanistic Rage", SpellRange.NoCheck, SpellWait.Complete);
                else if (_hasTalentImprovedCleanseSpirit || dispelDebuff.Spell.DispelType == WoWDispelType.Curse)
                    castSpell = Safe_CastSpell(unit, "Cleanse Spirit", SpellRange.Check, SpellWait.Complete);
            }

            return castSpell;
        }

        #endregion

        #region Pull

        /*
		 * Pull()
		 * 
		 * Currently always do a ranged pull from '_offsetForRangedPull' way
		 * If HB has given us a mob to pull that is further away, we will
		 * run towards him up to within '_offsetForRangedPull' 
		 * 
		 */

        public override void Pull()
        {
            if (IsGameUnstable())
                return;

            if (HandleCurrentSpellCast())
            {
                Dlog("Pull:  aborted since casting");
                return;
            }

            ShowStatus("Enter PULL");
            PullLogic();
            ShowStatus("Exit PULL");
        }

        public void PullInitialize()
        {
            _pullTargGuid = _me.CurrentTarget.Guid;
            // _pullTargHasBeenInMelee = false;
            _pullAttackCount = 0;
            // _pullStart = _me.Location;
            _pullTimer.Remaining = ConfigValues.PullTimeout;
        }

        public void PullLogic()
        {
            if (_me.IsFlying || _me.OnTaxi || _me.IsOnTransport)
                return;

            if (CombatResto())
                return;

            // don't pull in these... just jump into Combat behavior
            if (IsPVP() || IsRAF())
            {
                Combat();
                return;
            }

            if (!_me.GotTarget)
            {
                Dlog("HB gave (null) pull target");
                return;
            }

            if (_me.CurrentTarget.IsPet)
            {
                WoWUnit petOwner = _me.CurrentTarget.CreatedByUnit;
                if (petOwner != null)
                {
                    Dlog("Changing target from pet {0} to owner {1}", Safe_UnitName(_me.CurrentTarget), Safe_UnitName(petOwner));
                    Safe_SetCurrentTarget(petOwner);
                }
                else
                {
                    Dlog("Appears that pet {0} does not have an owner?  guess we'll fight a pet", Safe_UnitName(_me.CurrentTarget));
                }
            }

            if (!_me.CurrentTarget.IsAlive)
            {
                Dlog("HB gave a Dead pull target: " + Safe_UnitName(_me.CurrentTarget) + "[" + _me.CurrentTarget.Level + "]");
                Safe_SetCurrentTarget(null);
                return;
            }

            if (!IsPVP() && !IsRAF())
            {
                if (TreeRoot.Current != null && !TreeRoot.Current.Name.ToLower().Contains("duel") && !TreeRoot.Current.Name.ToLower().Contains("combat"))
                {
                    if (_me.CurrentTarget.TaggedByOther && !IsTargetingMeOrMyGroup(_me.CurrentTarget))
                    {
                        Slog("Combat Target is tagged by another player -- let them have it");
                        Safe_SetCurrentTarget(null);
                        return;
                    }
                }
            }

            if (Blacklist.Contains(_me.CurrentTargetGuid))
            {
                Slog("Skipping pull of blacklisted mob: " + Safe_UnitName(_me.CurrentTarget) + "[" + _me.CurrentTarget.Level + "]");
                Safe_SetCurrentTarget(null);
                return;
            }

            if (IsPVP())
            {
                if (_me.GotTarget && _me.CurrentTarget.IsPlayer && _me.CurrentTarget.Mounted && _me.IsHorde != _me.CurrentTarget.ToPlayer().IsHorde)
                {
                    Slog("Skipping mounted player: " + Safe_UnitName(_me.CurrentTarget) + "[" + _me.CurrentTarget.Level + "]");
                    Blacklist.Add(_me.CurrentTarget.Guid, System.TimeSpan.FromSeconds(2));
                    Safe_SetCurrentTarget(null);
                    return;
                }
            }

            CheckForAdds();

            // reset state values we use to determine what point we are at in 
            //  .. in transition from Pull() to Combat()
            //---------------------------------------------------------------------------
            if (_pullTargGuid != _me.CurrentTarget.Guid)
            {
                PullInitialize();
                Slog(">>> PULL: " + (_me.CurrentTarget.Elite ? "[ELITE] " : "") + Safe_UnitName(_me.CurrentTarget) + "[" + _me.CurrentTarget.Level + "] at " + _me.CurrentTarget.Distance.ToString("F1") + " yds");
                Dlog("pull started at {0:F1}% health, {1:F1}% mana", _me.HealthPercent, _me.ManaPercent);
            }

            if (IsPVP() || IsMovementDisabled())  // never timeout in PVP or when disable movement set
                ;
            else if (_pullTimer.Done && !(_me.CurrentTarget.TaggedByMe || IsTargetingMeOrMyGroup(_me.CurrentTarget)))
            {
                Blacklist.Add(_me.CurrentTarget.Guid, System.TimeSpan.FromSeconds(30));
                Slog("Pull TIMED OUT for: " + _me.CurrentTarget.Class + "-" + Safe_UnitName(_me.CurrentTarget) + "[" + _me.CurrentTarget.Level + "] after " + _pullTimer.ElapsedMilliseconds + " ms -- blacklisted for 30 secs");
                Safe_SetCurrentTarget(null);
                return;
            }

            if (!_me.IsSafelyFacing(_me.CurrentTarget))
            {
                Safe_FaceTarget();
            }

            Safe_StopFace();

            /*            if (typeShaman != ShamanType.Enhance)
                            PullRanged();
                        else 
            */
            if (typeShaman == ShamanType.Enhance && IsPVP())
                PullFast();
            else
            {
                switch (cfg.PVE_PullType)
                {
                    case ConfigValues.TypeOfPull.Auto:
                        PullAuto();
                        break;
                    case ConfigValues.TypeOfPull.Body:
                        PullBody();
                        break;
                    case ConfigValues.TypeOfPull.Fast:
                        PullFast();
                        break;
                    case ConfigValues.TypeOfPull.Ranged:
                        PullRanged();
                        break;
                }
            }


            if (!_me.GotTarget)
                Dlog("PullLogic:  leaving with no current target");
            else
                Dlog("distance after pull: {0:F2}", _me.CurrentTarget.Distance);
            // CheckForPlayers();
        }


        public void PullAuto()
        {
            Dlog("PullType Auto");
            if (typeShaman != ShamanType.Enhance || !IsCaster(_me.CurrentTarget))
                PullRanged();
            else
                PullFast();
        }

        public void PullBody()
        {
            Dlog("PullType Body");
            if (!CurrentTargetInMeleeDistance())
                MoveToCurrentTarget();
            else if (!ShockOpener())
            {
                if (_me.GotTarget && !_me.IsAutoAttacking && _me.CurrentTarget.IsAlive && CurrentTargetInMeleeDistance() && !IsPVP())
                {
                    Dlog("** Auto-Attack started in PullBody");
                    AutoAttack();
                }
            }
        }

        public void PullFast()
        {
            bool castSpell = false;

            Dlog("PullType Fast");
            if (!CurrentTargetInRangedDistance() || (typeShaman == ShamanType.Enhance && !CurrentTargetInMeleeDistance()))
                MoveToCurrentTarget();

            if (CurrentTargetInMeleeDistance() || (typeShaman != ShamanType.Enhance && CurrentTargetInRangedDistance()))
                Safe_StopMoving();

            if (!castSpell && typeShaman == ShamanType.Enhance)
                castSpell = MaelstromCheck();

            if (!castSpell)
                castSpell = UnleashElements();

            if (!castSpell && _me.CurrentTarget.Distance < 25)
                castSpell = ShockOpener();

            if (_me.CurrentTarget.Distance < 10 && _me.Mounted)
                Safe_Dismount();

            if (_me.GotTarget && !_me.IsAutoAttacking && _me.CurrentTarget.IsAlive && CurrentTargetInRangedDistance()) // && !IsPVP())
            {
                Dlog("** Auto-Attack started in PullFast");
                AutoAttack();
            }

            // mostly for Ele/Resto:  use LB stopped and haven't cast an instant yet
            if (!castSpell && typeShaman != ShamanType.Enhance && _me.GotTarget && (!_me.IsMoving || _hasGlyphOfUnleashedLightning))
            {
                Dlog("PullFast:  stopped and no cast yet, so using LB (others must be on CD)");
                castSpell = LightningBolt();
            }
        }


        public WoWUnit PullRangedAggroCheck()
        {
            WoWUnit add =
                (from o in ObjectManager.ObjectList
                 where o is WoWUnit && o.Distance <= Targeting.PullDistance
                 let unit = o.ToUnit()
                 where unit.Attackable
                     && unit.IsAlive
                     && unit.Combat
                     && !IsMeOrMyStuff(unit)
                     && IsTargetingMeOrMyStuff(unit)
                     && !Blacklist.Contains(unit.Guid)
                 orderby unit.Distance ascending
                 select unit
                ).FirstOrDefault();
            return add;
        }

        /// <summary>
        /// PullRanged() moves within ranged distance and attacks until tagged
        /// Tries to leverage following Lightning Bolt with an Instant attack
        /// to increase initial damage at range
        /// </summary>
        public void PullRanged()
        {
            Dlog("PullType Ranged");
            while (!IsGameUnstable() && !_pullTimer.Done && _me.IsAlive && _me.GotTarget && _me.CurrentTarget.IsAlive && !_me.CurrentTarget.Aggro)
            {
                if (_me.CurrentTarget.TaggedByOther && !IsTargetingMeOrMyGroup(_me.CurrentTarget))
                {
                    Slog("Combat Target is tagged by another player -- let them have it");
                    AddToBlacklist(_me.CurrentTargetGuid);
                    Safe_SetCurrentTarget(null);
                    return;
                }

                if (0 == _pullAttackCount && _me.Combat)
                {
                    WoWUnit aggro = PullRangedAggroCheck();
                    if (aggro != null)
                    {
                        Slog(Color.Orange, "Cancel pull to fight aggro {0}", Safe_UnitName(aggro));
                        Safe_StopMoving();
                        Safe_SetCurrentTarget(aggro);
                        return;
                    }
                }

                if (!CurrentTargetInRangedPullDistance())
                {
                    MoveToCurrentTarget();
                    continue;
                }

                if (_me.IsMoving)
                {
                    Dlog("PullRanged:  stopping at ranged attack distance");
                    Safe_StopMoving();
                }

                WaitForCurrentSpell(null);

                // set totems now for all specs except Enhancement (who does in Combat)
                if (typeShaman != ShamanType.Enhance && !IsPVP() && SetTotemsAsNeeded())
                {
                    continue;
                }

                // use LB if first attempt and don't already have aggro or close
                if (_pullAttackCount == 0 && (_me.CurrentTarget.Distance > 10 || !_me.CurrentTarget.Aggro) && LightningBolt())
                    _pullAttackCount++;
                else if (IsWeaponImbuedWithDPS() && UnleashElements())
                    _pullAttackCount++;
                else if (_me.CurrentTarget.Distance < 25 && ShockOpener())
                    _pullAttackCount++;
                else if (LightningBolt())
                    _pullAttackCount++;

                if (_me.GotTarget && _me.CurrentTarget.IsPlayer)
                    break;
            }
        }


        #endregion

        #region Combat

        /*
		 * Combat()
		 * 
		 */
        public override void Combat()
        {
            if (IsGameUnstable())
                return;

            List<int> l =
               (from o in ObjectManager.ObjectList
                where o is WoWPlayer && o.DistanceSqr <= 2500
                let p = o.ToPlayer()
                where
                    p.IsHorde != _me.IsHorde
                    && p.HealthPercent > 1
                from dbf in p.Buffs
                where
                    dbf.Value.Spell.DispelType == WoWDispelType.Magic
                    && !dictPurgeables.ContainsKey(dbf.Value.SpellId)
                    && !_hashPurgeWhitelist.Contains(dbf.Value.SpellId)
                select dbf.Value.SpellId
                ).ToList();

            foreach (int id in l)
            {
                dictPurgeables.Add(id, WoWSpell.FromId(id).Name);
            }

            if (_me.IsFlying || _me.OnTaxi || _me.IsOnTransport)
                return;

            if (HandleCurrentSpellCast())
            {
                Dlog("Combat:  aborted since casting");
                return;
            }

            ShowStatus("Enter COMBAT");
            try
            {
                CombatLogic();
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                if (!_me.GotTarget)
                    Logging.WriteDebug("Exception:  referencing 'null' target: mob expired or out of range? no big deal");
                else
                {
                    Log(Color.Red, "An Exception occured. Check debug log for details.");
                    Logging.WriteDebug("EXCEPTION in Combat() - HonorBuddy API or CC Error");
                }

                Logging.WriteException(e);
            }

            ShowStatus("Exit COMBAT");
        }

        private void CombatLogic()
        {
            // ListWowUnitsInRange();

            if (trinkPVP != null && MeImmobilized())
                UseItem(trinkPVP);
            if (_me.Combat)
                UseItem(trinkCombat);
            if (_me.HealthPercent < cfg.TrinkAtHealth)
                UseItem(trinkHealth);
            if (_me.ManaPercent < cfg.TrinkAtMana)
                UseItem(trinkMana);

            if (CombatResto())
                return;

            if (MeImmobilized())
            {
                // need to look at trinketting out here...
                // .. otherwise just return because we can't move and can't cast
                if (typeShaman == ShamanType.Enhance && IsAuraPresent(_me, "Feral Spirit") && SpellManager.CanCast("Spirit Walk"))
                {
                    Log("^Pet Ability - Spirit Walk (remove movement impairing effects)");
                    Safe_CastSpell("Spirit Walk");
                }

                return;
            }

            if (!combatChecks())
                return;

            if (_me.Fleeing && SpellManager.HasSpell((int)TotemId.TREMOR_TOTEM) && !TotemExist(TotemId.TREMOR_TOTEM))
            {
                if (SetTotemsAsNeeded())
                    return;
            }

            // targeting a friendly -- which we never need to do
            // ... since BG Bot sometimes insists on this we'll just wait it out
            if (IsPVP() && _me.GotTarget && Safe_IsFriendly(_me.CurrentTarget))
            {
                // Dlog("CombatLogic:  targeting a friendly player, so just wait for another target to come");
                return;
            }

            CheckForAdds();

            if (!combatChecks())
                return;

            if (_me.CurrentTarget.TaggedByOther && !_me.CurrentTarget.TaggedByMe && !IsTargetingMeOrMyGroup(_me.CurrentTarget) && TreeRoot.Current != null && !TreeRoot.Current.Name.ToLower().Contains("duel") && !TreeRoot.Current.Name.ToLower().Contains("combat") && !TreeRoot.Current.Name.ToLower().Contains("lazyraider"))
            {
                Slog("Combat Target is tagged by another player -- let them have it");
                Safe_SetCurrentTarget(null);
                return;
            }

            // check if we agroed a mob unintentionally
            if (_pullTargGuid != _me.CurrentTarget.Guid)
            {
                Dlog("CombatLogic:  STOP... combat target changed");
                Safe_StopMoving();
                Slog(">>> ADD: " + Safe_UnitName(_me.CurrentTarget) + "[" + _me.CurrentTarget.Level + "] at " + _me.CurrentTarget.Distance.ToString("F1") + " yds");
                PullInitialize();
            }

            // if (!WoWMovement.IsFacing)
            Safe_FaceTarget();             // hate to spam this, but appears .IsFacing=true doesn't mean that don't need to call .Face

            if (!_me.IsAutoAttacking && _me.GotTarget && _me.CurrentTarget.IsAlive && CurrentTargetInRangedDistance())
            {
                Dlog("** Auto-Attack started in Combat");
                AutoAttack();
            }

            // ok, reevaluate if still need healing in hopes we can attack
            if (IsSelfHealNeeded())
            {
                ShowStatus("COMBAT-HEAL=YES!!!!");
                HealMyself();
                // return;
            }

            InterruptEnemyCast();

            if (_me.HealthPercent < cfg.LifebloodPercent)
            {
                bool healCast = GiftOfTheNaaru();
                if (!healCast)
                    healCast = Lifeblood();
                if (healCast)
                    return;
            }


            if (!pvpChecks())
            {
                if (!FindBestTarget())
                    return;
            }

            if (typeShaman == ShamanType.Unknown)
                CombatUndefined();
            else if (typeShaman == ShamanType.Enhance)
                CombatMelee();
            else if (IsPVP())
                CombatElementalPVP();
            else
                CombatElemental();


            ShamanBuffs(false);
        }

        private readonly Stopwatch _timerCombatStats = new Stopwatch();


        #region Combat Styles

        /*
		 * CombatUndefined()
		 * 
		 * Rotation used prior to learning any talents.  Characteristics at this level are
		 * hi mana and health regen, with Lightning Bolt still having the best damage to mana
		 * ratio.  However, due to high regen will use instant attacks which aren't as 
		 * efficient but allow higher dps.
		 */
        private void CombatUndefined()
        {
            if (!CurrentTargetInRangedDistance() && _me.Rooted)
            {
                // options -- trinket, heal (anticipating being hit)
                if (!FindBestTarget())
                {
                    Slog("Rooted:  no target in range so waiting it out...");
                    return;
                }
            }

            if (!CurrentTargetInRangedDistance())
            {
                // chase until in ranged distance or for 8 seconds
                Dlog("running to mob " + _me.CurrentTarget.Distance.ToString("F2") + " away");
                MoveToCurrentTarget();
                BestInstantAttack();
                return;
            }

            if (_me.IsMoving)
            {
                if (!_me.GotTarget)
                    Dlog("Moving: and I don't have a target?  stopping to get my bearings");
                else
                    Dlog("Moving: target {0:F1} yds away, stopping now...", _me.Location.Distance(_me.CurrentTarget.Location));
                Safe_StopMoving();
            }

            if (!combatChecks())
                return;

            // #1 Set Strength of Earth Totem if no buff
            if (!_me.Auras.ContainsKey("Strength of Earth") && SetTotemsAsNeeded())
                return;

            // #2 Cast Racial dps enhance abilitiy
            CastCombatSpecials();

            // #3 Instant (Ranged): Earth Shock
            if (EarthShock())
                return;

            // #4 Instant (Melee):  Primal Strike
            if (PrimalStrike())
                return;

            // #5 Lightning Bolt filler
            if (combatChecks())
                LightningBolt();
        }


        /*
         * CombatMelee()
         * 
         * Rotation: Lightning Bolt, until the mob is near you, then spam Earth Shock while 
         * auto attacking.  Shield Twist between Water Shield and Lightning Shield.  Keep 
         * Windfury Weapon up, use Flametongue if not trained, and Rockbiter if no other choice
         * 
         * Looks redundant, but we use combatChecks() and AllowNonHealSpells() over and over.  They
         * are state checking functions and the conditions they check will change during the 
         * running of this function.  They need to be measured immediately before using an
         * ability.
         */
        private void CombatMelee()
        {
            if (IsPVP())
            {
                CombatMeleePVP();
                return;
            }

            if (!CurrentTargetInMeleeDistance())
            {
                if (!_me.Rooted)
                {
                    MoveToCurrentTarget();
                    BestInstantAttack();
                    return;
                }

                Slog("Rooted...");
                if (FeralSpirit())
                    return;

                if (FindBestMeleeTarget())      // check for weakest targets only first
                    ;
                else if (FindBestTarget())      // settle for weakest target within range
                {
                    CombatElemental();
                    return;
                }
                else
                {
                    Dlog("CombatMelee:  rooted and no target in range so waiting it out...");
                    return;
                }
            }

            if (_me.Fleeing)
            {
                Slog("^FEAR: casting Tremor Totem");
                SetTotemsAsNeeded();
            }


            if (_me.IsMoving)
            {
                if (!_me.GotTarget)
                    Dlog("CombatMelee: moving and I don't have a target???");
                else
                    Dlog("CombatMelee: target {0:F1} yds away, stopping now...", _me.Location.Distance(_me.CurrentTarget.Location));
                Safe_StopMoving();
            }

            if (!combatChecks())
                return;

            if (!IsMovementDisabled() && _me.GotTarget && _me.CurrentTarget.Distance < ConfigValues.TargetTooCloseDistance && !_me.CurrentTarget.IsPlayer)
            {
                double distFromMob = ConfigValues.TargetTooCloseDistance + ConfigValues.TargetTooCloseAdjust;
                WoWMovement.MovementDirection way = (Environment.TickCount & 1) == 0 ? WoWMovement.MovementDirection.StrafeLeft : WoWMovement.MovementDirection.StrafeRight;
                Log( "Close to target @ {0:F2} yds - {1} small distance away!!!", _me.CurrentTarget.Distance, way );

                Safe_StopFace();
                WoWMovement.Move(way);

                Dlog("CombatMelee:  strafing right for max of 333 ms");
                Countdown stopStrafe = new Countdown(333);
                while (!IsGameUnstable() && _me.IsAlive && _me.GotTarget && _me.CurrentTarget.Distance < distFromMob && !stopStrafe.Done)
                {
                    Thread.Sleep(25);   // give a small timeslice back
                }

                WoWPoint lastPos = new WoWPoint();
                do
                {
                    WoWMovement.MoveStop(way);
                    Thread.Sleep(50);
                    if (_me.Location.Distance2D(lastPos) == 0)
                        break;
                    lastPos = _me.Location;
                } while (!IsGameUnstable() && _me.IsAlive);

                WoWMovement.MoveStop();
                Safe_FaceTarget();
                Log("Adjusted distance is {0:F2} yds", _me.CurrentTarget.Distance);
            }

            WaitForCurrentSpell(null);

            if (priorityPurge == ConfigValues.SpellPriority.High && Purge())
            {
                return;
            }

            // use Shamanistic Rage when our mana is low and mob we are fighting has 
            // .. a lot of health left.  Since it gives mana back for 
            if (_me.ManaPercent < cfg.ShamanisticRagePercent)
            {
                if (IsFightStressful() || (countEnemy > 1) || (_me.GotTarget && _me.CurrentTarget.HealthPercent >= 75))
                {
                    ShamanisticRage();
                }
            }


            if (IsWeaponImbueNeeded())
            {
                ImbueWeapons();
                return;
            }

            // for Enhancement:  make first set of totems high priority
            if (!TotemsWereSet() && combatChecks() && AllowNonHealSpells() && SetTotemsAsNeeded())
                return;

            if (CallForReinforcements())
                return;

            CastCombatSpecials();

#if OLD_CATA_ROTATION
			if (FlameShock())
				return;

			if (SearingFlamesCheck())
				return;

			if (MaelstromCheck())
				return;

			if (FireNova())
				return;

			if ( IsStormstrikeNeeded()  && Stormstrike())
				return;

			if (EarthShock())
				return;

			if (Stormstrike())
				return;

			if (PrimalStrike())
				return;

            if (LavaLash())
                return;
#else
            if (LavaLash())
                return;

            if (UnleashFlameCheck())
                return;

            if (MaelstromCheck())
                return;

            if (UnleashElements())
                return;

            if (FireNova())
                return;

            if (IsStormstrikeNeeded() && Stormstrike())
                return;

            if ((_me.Level < 81 || !SpellManager.HasSpell("Unleash Elements")) && FlameShock())
                return;

            if (EarthShock())
                return;

            if (Stormstrike())
                return;

            if (PrimalStrike())
                return;

#endif

            if (CleanseIfNeeded(_me))
                return;

            // now check to see if any totems still needed
            if (combatChecks() && AllowNonHealSpells() && SetTotemsAsNeeded())
            {
                return;
            }
        }


        private void CombatMeleePVP()
        {
            // if I'm rooted then throw anything ranged at them
            if (_me.IsCasting)
            {
                Dlog("CombatMeleePVP:  cant move yet -- casting -- waiting to complete");
                return;
            }
            else if (_me.Fleeing)
            {
                Slog("^FEAR: casting Tremor Totem");
                SetTotemsAsNeeded();
            }
            else if (_me.Rooted || MeImmobilized())
            {
                Dlog("CombatMeleePVP:  cant move yet -- rooted or immobilized");
            }
            else if (!_me.GotTarget)
            {
                Dlog("CombatMeleePVP:  cant move yet -- no target");
                return;
            }
            else if (_me.CurrentTarget.Distance <= (_offsetForMeleePull + 1.5))
            {
                Dlog("CombatMeleePVP:  no need to move, close enough at {0} yds", _me.Distance);
            }
            else
            {
                MoveToCurrentTarget();
            }

            if (MaelstromCheckPVP())
                return;

            if (!CurrentTargetInMeleeDistance())
            {
                if (_me.GotTarget && _me.CurrentTarget.Distance < 50 && FeralSpirit())
                    return;

                if (!_me.Rooted)
                {
                    BestInstantAttack();
                    return;
                }

                Dlog("CombatMeleePVP:  I'm melee and rooted and out of melee range, do something!!!");
                if (IsAuraPresent(_me, "Feral Spirit"))
                {
                    if (!SpellManager.CanCast("Spirit Walk"))
                        Dlog("CombatMeleePVP:  Feral Spirit active but unable to cast Spirit Walk yet");
                    else if (Safe_CastSpell("Spirit Walk", SpellRange.NoCheck, SpellWait.Complete))
                        return;
                }

                if (FindBestMeleeTarget())      // check for weakest targets only first
                {
                    Dlog("... switched to nearby melee range target while rooted");
                    // so fall through to Melee PVP Combat logic
                }
                else if (FindBestTarget())      // settle for weakest target within range
                {
                    Dlog("... switched to lowest health ranged target while rooted");
                    CombatElementalPVP();
                    return;
                }
                else
                {
                    Slog("Rooted:  no target in range so waiting it out...");
                    return;
                }
            }

            if (_me.CurrentTarget.Distance < 1)
                Safe_StopMoving();

            if (!combatChecks())
                return;

            if (priorityPurge == ConfigValues.SpellPriority.High && Purge())
                return;

            ShamanisticRage();

            if (CallForReinforcements())
                return;

            // for Enhancement:  make first set of totems high priority
            if (!TotemsWereSet() && AllowNonHealSpells() && SetTotemsAsNeeded())
            {
                return;
            }

            CastCombatSpecials();

            if (FlameShock())
                return;

            if (IsStormstrikeNeeded() && Stormstrike())
                return;

            if (EarthShock())
                return;

            if (Stormstrike())
                return;

            if (PrimalStrike())
                return;

            if (LavaLash())
                return;

            if (FireNova())
                return;

            if (CleanseIfNeeded(_me))
                return;

            // now check to see if any totems still needed
            if (combatChecks() && AllowNonHealSpells() && SetTotemsAsNeeded())
            {
                return;
            }
        }


        /*
         * CombatElemental()
         * 
         */
        private void CombatElemental()
        {
            if (IsPVP())
            {
                CombatElementalPVP();
                return;
            }

            if (!CurrentTargetInRangedDistance() && _me.Rooted)
            {
                // options -- trinket, heal (anticipating being hit)
                if (!FindBestTarget())
                {
                    Slog("Rooted:  no target in range so waiting it out...");
                    return;
                }
            }

            if (!CurrentTargetInRangedDistance())
            {
                // chase until in ranged distance or for 8 seconds
                Dlog("running to mob " + _me.CurrentTarget.Distance.ToString("F2") + " away");
                MoveToCurrentTarget();
                BestInstantAttack();
                return;
            }

            if (_me.IsMoving)
            {
                Dlog("Stopping movement for Elemental combat");
                Safe_StopMoving();
            }

            if (!combatChecks())
            {
                Dlog("CombatElem: failed combat checks so no attack cast this pass");
                return;
            }

            // we already cast totems in pull, so this is just to see if we need to replace any if in crisis
            if (AllowNonHealSpells() && (countEnemy > 1 || (_me.GotTarget && _me.CurrentTarget.HealthPercent > 25)) && SetTotemsAsNeeded())
            {
                return;
            }

            if (CallForReinforcements())
            {
                Dlog("CombatElem: failed CallForReinforcements() so no attack cast this pass");
                return;
            }

            CastCombatSpecials();

            if ((_count10YardEnemy > 0 && IsFightStressful())
                || (_count10YardEnemy > 0 && _me.ManaPercent <= cfg.ThunderstormPercent)
                || (IsRAF() && _me.ManaPercent <= cfg.ThunderstormPercent)
               )
            {
                if (Thunderstorm())
                {
                    Dlog("CombatElem: Thunderstorm cast so no further attacks this pass");
                    return;
                }
            }

            int aoeCountNeeded = _hasGlyphOfChainLightning ? 7 : 5;
            if (AllowNonHealSpells() && _me.GotTarget && _me.ManaPercent > 60 && _countAoe8Enemy >= aoeCountNeeded && _me.CurrentTarget.Distance < 33 && SpellManager.HasSpell("Earthquake"))
            {
                if (Safe_CastSpell("Earthquake", SpellRange.Check, SpellWait.Complete))
                {
                    if (!LegacySpellManager.ClickRemoteLocation(_me.CurrentTarget.Location))
                    {
                        Dlog("^Ranged AoE Click FAILED:  cancelling Earthquake");
                        SpellManager.StopCasting();
                    }
                    else
                    {
                        Dlog("^Ranged AoE Click successful:  EARTHQUAKE on {0} targets", _countAoe8Enemy);
                        StyxWoW.SleepForLagDuration();
                    }

                    return;
                }
            }

            if (FlameShockRenew())
            {
                Dlog("CombatElem: flame shock, so no more attacks cast this pass");
                return;
            }

            ElementalMastery();
            if (LavaBurst())
            {
                Dlog("CombatElem: lavaburst, so no more attacks cast this pass");
                return;
            }

            if (FulminationCheck())
                return;

            if (_countAoe12Enemy > 1 && ChainLightning())
            {
                Dlog("CombatElem: chain lightning, so no more attacks cast this pass");
                return;
            }

            if (FireNova())
            {
                Dlog("CombatElem: firenova, so no more attacks cast this pass");
                return;
            }

            // earth shock now, but only if we don't have Fulmination talent and it won't interfere with Flame Shock DoT
            if (!_hasTalentFulmination && CanAnElemBuyAnEarthShock() && EarthShock())
            {
                Dlog("CombatElem: earth shock, so no more attacks cast this pass");
                return;
            }

            if (LightningBolt())
            {
                Dlog("CombatElem: lightningbolt, so no more attacks cast this pass");
                return;
            }

            if (CleanseIfNeeded(_me))
            {
                Dlog("CombatElem: cleanse, so no more attacks cast this pass");
                return;
            }

            Dlog("CombatElem: made it through entire pass without casting anything!!!!");
        }


        private void CombatElementalPVP()
        {
            if (!CurrentTargetInRangedDistance())
            {
                if (_me.Rooted)
                {
                    // options -- trinket, heal (anticipating being hit)
                    if (!FindBestTarget())
                    {
                        Slog("Rooted:  no target in range so waiting it out...");
                        return;
                    }
                }

                if (!CurrentTargetInRangedDistance())
                {
                    MoveToCurrentTarget();
                    BestInstantAttack();
                    return;
                }
            }

            if (_me.IsMoving)
            {
                Dlog("Stopping movement for Elemental combat");
                Safe_StopMoving();
            }

            if (!combatChecks())
            {
                Dlog("CombatElemPVP: failed combat checks so no attack cast this pass");
                return;
            }


#if NO_TOTEMS_IN_PVP
#else
            // we already cast totems in pull, so this is just to see if we need to replace any if in crisis
            if (AllowNonHealSpells() && (countEnemy > 1 || (_me.GotTarget && _me.CurrentTarget.HealthPercent > 35 && !_me.CurrentTarget.Mounted)))
            {
                SetTotemsAsNeeded();
            }
#endif
            if (_countMeleeEnemy > 0 && CallForReinforcements())
            {
                Dlog("CombatElemPVP: CallForReinforcements() so no attack cast this pass");
                return;
            }

            if (_count10YardEnemy > 1 || _me.ManaPercent < cfg.EmergencyManaPercent)
            {
                if (Thunderstorm())
                {
                    Dlog("CombatElemPVP: Thunderstorm cast so no further attacks this pass");
                    return;
                }
            }

            if (FulminationCheck())
                return;

            if (FlameShock())
            {
                Dlog("CombatElemPVP: flame shock, so no more attacks cast this pass");
                return;
            }

            if (_me.GotTarget && Hex(_me.CurrentTarget))
            {
                Dlog("CombatElemPVP: hex, so no more attacks cast this pass");
                return;
            }

            Dlog("CombatElemPVP:  target charmed:{0} Stunned:{1} Silenced:{2} Rooted:{3} Dazed:{4} Disarmed:{5} Fleeing:{6}",
                BoolToYN(_me.CurrentTarget.CharmedByUnit != null),
                BoolToYN(_me.CurrentTarget.Stunned),
                BoolToYN(_me.CurrentTarget.Silenced),
                BoolToYN(_me.CurrentTarget.Rooted),
                BoolToYN(_me.CurrentTarget.Dazed),
                BoolToYN(_me.CurrentTarget.Disarmed),
                BoolToYN(_me.CurrentTarget.Fleeing)
                );

            ElementalMastery();
            if (LavaBurst())
            {
                Dlog("CombatElemPVP: lavaburst, so no more attacks cast this pass");
                return;
            }

            if (ChainLightning())
            {
                Dlog("CombatElemPVP: chain lightning, so no more attacks cast this pass");
                return;
            }

            if (LightningBolt())
            {
                Dlog("CombatElemPVP: chain lightning, so no more attacks cast this pass");
                return;
            }

            Dlog("CombatElemPVP: made it through entire pass without casting anything!!!!");
        }


        private bool NeedRestLogicResto()
        {
            // handle low mana situation
            if (_me.ManaPercent <= cfg.ManaTidePercent && (_me.Combat || (IsRAFandTANK() && GroupTank.Combat)))
            {
                if (!TotemExist(TotemId.MANA_TIDE_TOTEM))
                {
                    if (_me.IsMoving)
                        Dlog("NeedRestLogicResto:  moving, so waiting to cast Mana Tide");
                    else
                    {
                        if (TotemCast(TotemId.MANA_TIDE_TOTEM))
                            return true;

                        if (!TotemExist(TotemId.MANA_SPRING_TOTEM) && TotemCast(TotemId.MANA_SPRING_TOTEM))
                            return true;
                    }

                    // use potion if a key person is in combat
                    if (UseManaPotionIfAvailable())
                        return true;
                }
            }

            // if we dispel someone, done for now
            if ( priorityCleanse == ConfigValues.SpellPriority.High )
            {
                if (DispelRaid())
                    return true;
            }

            // if we heal someone, done for now
            if (HealRaid())
                return true;

            // if we dispel someone, done for now
            if ( priorityCleanse == ConfigValues.SpellPriority.Low )
            {
                if (DispelRaid())
                    return true;
            }

            // healers need to stay within range of Tank,
            // .. so do best to stay in range while heals aren't desparately needed
            if (IsPVP() && !_me.IsMoving && _me.Combat)
            {
                if (SetTotemsAsNeeded())
                    return true;
            }
            else if (IsRAF())
            {
                // if not in range move there
                if (!IsUnitInRange(GroupTank, cfg.RAF_FollowAtRange))
                {
                    if (!cfg.RAF_FollowClosely)
                        ;
                    else if (GroupTank == null)
                        Dlog("NeedRestLogicResto:  no tank identified or in range, so nobody to follow");
                    else
                    {
                        Dlog("NeedRestLogicResto:  no heals needed and tank {0:F1} yds away so moving in range", GroupTank.Distance);
                        MoveToHealTarget(GroupTank, cfg.RAF_FollowAtRange * 0.88);
                    }
                }
                // else when in range see if fight started and position set
                else if (GroupTank.Combat && GroupTank.GotTarget && Safe_IsHostile(GroupTank.CurrentTarget))
                {
                    if (!GroupTank.IsMoving && 10 > GroupTank.Location.Distance(GroupTank.CurrentTarget.Location))
                    {
                        if (SetTotemsAsNeeded())
                            return true;
                    }
                }
            }

            // if in a Healer Only role (not trying to DPS for anything other than mana regen)
            //  ..  then check buffs and stuff then return True so it doesn't do Combat()
            if (IsHealerOnly() && InterruptEnemyCast())
                return true;

            return false;
        }



        private bool CombatResto()
        {
            // if we dispel someone, done for now
            if ( priorityCleanse == ConfigValues.SpellPriority.High )
            {
                if (DispelRaid())
                    return true;
            }

            // purely a group activity, so bail if not in one
            // check if offhealing is needed when you aren't a healer
            if (!IsHealer())
            {
                if (!IsRAF())
                    return false;

                WoWPlayer playerOffheal = GroupMembers.FirstOrDefault(p => p.CurrentHealth > 1 && cfg.RAF_GroupOffHeal > (int)p.HealthPercent && p.Distance < cfg.GroupHeal.SearchRange);
                if (playerOffheal == null)
                    return false;

                Slog("Temporarily switching to OffHeal:  {0} at {1:F1}%", Safe_UnitName(playerOffheal), playerOffheal.HealthPercent);
                minGroupHealth = (int)playerOffheal.HealthPercent;
            }

            // handle totems for battlegrounds
            if (IsPVP() && !_me.IsMoving && _me.Combat)
            {
                if (SetTotemsAsNeeded())
                    return true;
            }
            // handle totems for RAF
            else if (IsRAF())
            {
                // if not in range move there
                if (!IsUnitInRange(GroupTank, cfg.RAF_FollowAtRange))
                {
                    if (!cfg.RAF_FollowClosely)
                        ;
                    else if (GroupTank == null)
                        Dlog("CombatResto:  no tank exists or in range, so nobody to move towards");
                    else
                    {
                        Dlog("CombatResto:  no heals needed and tank {0:F1} yds away so moving in range", GroupTank.Distance);
                        MoveToHealTarget(GroupTank, cfg.RAF_FollowAtRange * 0.88);
                    }
                }
                // else when in range see if fight started and position set
                else if (GroupTank.Combat && GroupTank.GotTarget && Safe_IsHostile(GroupTank.CurrentTarget))
                {
                    if (!_me.IsMoving && !GroupTank.IsMoving && 10 > GroupTank.Location.Distance(GroupTank.CurrentTarget.Location))
                    {
                        if (SetTotemsAsNeeded())
                            return true;
                    }
                }
            }

            // handle low mana situation
            if (_me.ManaPercent < cfg.EmergencyManaPercent && (_me.Combat || (IsRAFandTANK() && GroupTank.Combat)))
            {
                if (!TotemExist(TotemId.MANA_TIDE_TOTEM))
                {
                    if (_me.IsMoving)
                        Dlog("CombatResto:  moving, so waiting for stop to cast Mana Tide");
                    else
                    {
                        if (TotemCast(TotemId.MANA_TIDE_TOTEM))
                            return true;

                        if (!TotemExist(TotemId.MANA_SPRING_TOTEM) && TotemCast(TotemId.MANA_SPRING_TOTEM))
                            return true;
                    }

                    // use potion if a key person is in combat
                    if (UseManaPotionIfAvailable())
                        return true;
                }
            }

            ShieldTwisting(false);      // try to keep shields up at all times

            // if we heal someone, done with processing for now
            if (HealRaid())
                return true;

            // if we dispel someone, done for now
            if ( priorityCleanse == ConfigValues.SpellPriority.Low )
            {
                if (DispelRaid())
                    return true;
            }

            // if in a Healer Only role (not trying to DPS for anything other than mana regen)
            //  ..  then check buffs and stuff then return True so it doesn't do Combat()
            if (IsHealerOnly())
            {
                if (combatChecks()) // makes sure we have hostile target and are facing
                {
#if HEALER_DONT_WINDSHEAR
#else
                    InterruptEnemyCast();
#endif
                    if (priorityPurge == ConfigValues.SpellPriority.Low && Purge())
                        return true;

                    if (!_me.IsSafelyFacing(_me.CurrentTarget))
                        Safe_FaceTarget();

                    if (_me.GotTarget && _me.CurrentTarget.IsAlive && Safe_IsHostile(_me.CurrentTarget) && _me.CurrentTarget.InLineOfSightOCD)
                    {
                        bool castAttack = false;
#if HEALER_IGNORE_FOCUSED_INSIGHT
#else
                        if (_me.ManaPercent > 60 && _hasTalentFocusedInsight && SpellManager.CanCast("Earth Shock", true))
                            castAttack = Safe_CastSpell("Earth Shock", SpellRange.Check, SpellWait.NoWait);
#endif
#if HEALER_IGNORE_TELLURIC_CURRENTS
#else
                        if (!castAttack && _hasTalentTelluricCurrents && SpellManager.CanCast("Lightning Bolt", true))
                            castAttack = Safe_CastSpell("Lightning Bolt", SpellRange.Check, SpellWait.NoWait);
#endif
                        if (castAttack)
                            return true;
                    }
                }
            }

            return IsHealerOnly();
        }

        public static string BoolToYN(bool b)
        {
            return b ? "Y" : "N";
        }

        private void ShowStatus(string s)
        {
            if (IsRAF())
            {
                Dlog("RAFSTAT {0}: H={1:F1}% M={2:F1}% melee:{3},range:{4},mecombat:{5},memoving:{6},metarg:{7} at {8:F1} yds",
                    s,
                    _me.HealthPercent,
                    _me.ManaPercent,
                    _countMeleeEnemy,
                    _countRangedEnemy,
                    BoolToYN(_me.Combat),
                    BoolToYN(_me.IsMoving),
                    !_me.GotTarget ? "(null)" : Safe_UnitName(_me.CurrentTarget),
                    !_me.GotTarget ? "(null)" : _me.CurrentTarget.Distance.ToString("F1")
                    );
                Dlog("RAFSTAT {0}: tnkH={1:F1}% tnkcombat:{2} tnkmoving:{3} at {4:F1} yds,tktarg:{5} at {6:F1} yds",
                    s,
                    GroupTank == null ? 0.0 : GroupTank.HealthPercent,
                    GroupTank == null ? "(null)" : BoolToYN(GroupTank.Combat),
                    GroupTank == null ? "(null)" : BoolToYN(GroupTank.IsMoving),
                    GroupTank == null ? "(null)" : GroupTank.Distance.ToString("F1"),
                    GroupTank == null || !GroupTank.GotTarget ? "(null)" : Safe_UnitName(GroupTank.CurrentTarget),
                    GroupTank == null || !GroupTank.GotTarget ? "(null)" : GroupTank.CurrentTarget.Distance.ToString("F1")
                    );
            }
            else if (IsPVP())
            {
                Dlog("PVPSTAT {0} [me]:  h/m:{1:F1}%/{2:F1}%, combat:{3}, melee:{4}, range:{5}, rooted:{6}, immobile:{7}, silenced:{8}",
                    s,
                    _me.HealthPercent,
                    _me.ManaPercent,
                    _me.Combat,
                    _countMeleeEnemy,
                    _countRangedEnemy,
                    _me.Rooted,
                    MeImmobilized(),
                    _me.Silenced
                    );
            }
            else
            {
                Dlog("GRDSTAT {0} [-me-]: h/m={1:F1}%/{2:F1}%, combat={3}, facing={4}, melee={5}, range={6}, rooted={7}, immobile={8}, silenced={9}",
                    s,
                    _me.HealthPercent,
                    _me.ManaPercent,
                    BoolToYN(_me.Combat),
                    BoolToYN(!_me.GotTarget ? false : _me.IsSafelyFacing(_me.CurrentTarget)),
                    _countMeleeEnemy,
                    _countRangedEnemy,
                    BoolToYN(_me.Rooted),
                    BoolToYN(MeImmobilized()),
                    BoolToYN(_me.Silenced)
                    );
            }

            if (!_me.GotTarget || IsHealer())
                ;
            else
                Dlog("        {0} [target]: {1} th={2:F1}%, tdist={3:F1} tlos={4} tlosocd={5} tcombat={6} ttarget={7} taggro={8} tpetaggro={9}",
                    s,
                    Safe_UnitName(_me.CurrentTarget),
                    _me.CurrentTarget.HealthPercent,
                    _me.CurrentTarget.Distance,
                    BoolToYN(_me.CurrentTarget.InLineOfSight),
                    BoolToYN(_me.CurrentTarget.InLineOfSightOCD),
                    BoolToYN(_me.CurrentTarget.Combat),
                    !_me.CurrentTarget.GotTarget ? "(null)" : Safe_UnitName(_me.CurrentTarget.CurrentTarget),
                    BoolToYN(_me.CurrentTarget.Aggro),
                    BoolToYN(_me.CurrentTarget.PetAggro)
                    );
        }

        private WoWPoint _pursuitStart;
        private readonly Stopwatch _pursuitTimer = new Stopwatch();

        public void PursuitBegin()
        {
            _pursuitTimer.Reset();
            _pursuitTimer.Start();
            _pursuitStart = _me.Location;
        }

        public bool InPursuit()
        {
            return _pursuitTimer.IsRunning;
        }

        public long PursuitTime
        { get { return _pursuitTimer.ElapsedMilliseconds; } }

        public WoWPoint PursuitOrigin
        { get { return _pursuitStart; } }


        #endregion

        #region Spells

        /// <summary>
        /// combatChecks() verifies the minimum necessary elements for combat between 
        /// _me and _me.CurrentTarget.  This verifies:
        ///     _me is alive
        ///     != null
        ///     .CurrentTarget is alive
        ///     .CurrentTarget is not self
        ///     .CurrentTarget is not my pet/totems/etc
        ///     _me is facing the .CurrentTarget
        ///     
        /// if no current target OR if .CurrentTarget is dead and still in combat or in a battleground
        ///     switch to the best available target
        ///     
        /// </summary>
        /// <returns>true - combat can continue
        /// false - unable to fight current target</returns>
        private bool combatChecks()
        {
            WoWUnit add = null;

            if (IsGameUnstable())
                return false;

            // if I am dead
            if (!_me.IsAlive)
            {
                ReportBodyCount();
                return false;
            }

            // if my target is dead
            if (_me.GotTarget && !_me.CurrentTarget.IsAlive)
            {
                ReportBodyCount();
                if (!_me.Combat && !IsPVP())
                    return false;
            }

            // if no target, or target is dead, or its a friendly target
            if (!_me.GotTarget || !_me.CurrentTarget.IsAlive || Safe_IsFriendly(_me.CurrentTarget))
            {
                Dlog("combatChecks:  in combat but not a valid hostile target, finding another");
                if (_me.Combat || IsPVP() || IsRAF())
                {
                    if (_me.GotAlivePet && _me.Pet.GotTarget && _me.Pet.CurrentTarget.IsAlive && !Safe_IsFriendly(_me.Pet.CurrentTarget))
                    {
                        add = _me.Pet.CurrentTarget;
                        Slog(">>> SET PETS TARGET: {0}-{1}[{2}]", add.Class, Safe_UnitName(add), add.Level);
                    }
                    else if (FindBestTarget())
                    {
                        add = _me.CurrentTarget;
                    }
                    else if (!IsPVP() && FindAggroTarget())
                    {
                        add = _me.CurrentTarget;
                    }

                    if (add == null && !_me.IsInInstance)
                    {
                        // target an enemy totem (this just cleans up in some PVE fights)
                        List<WoWUnit> addList
                            = (from o in ObjectManager.ObjectList
                               where o is WoWUnit && o.Distance <= _maxDistForRangeAttack
                               let unit = o.ToUnit()
                               where unit.Attackable
                                && unit.IsAlive
                                && !Safe_IsFriendly(unit)
                                && unit.InLineOfSightOCD
                                && unit.CreatedByUnitGuid != _me.Guid   // guard against my own totems being selected
                                && unit.CreatureType == WoWCreatureType.Totem
                               select unit
                                    ).ToList();

                        if (addList != null && addList.Any())
                        {
                            add = addList.First();
                            Slog("Setting to enemy totem: {0}-{1}[{2}]", add.Class, add.Name, add.Level);
                        }
                    }

                    if (add != null)
                    {
                        if (!Safe_SetCurrentTarget(add))
                            return false;
                    }
                }

                if (!_me.GotTarget)
                {
                    Dlog("No Current Target and can't find adds -- why still in Combat()");
                    return false;
                }
            }

            if (_me.GotTarget)
            {
                if (_me.CurrentTarget.IsMe)
                {
                    Dlog("Targeting myself -- clearing and bailing out of Combat()");
                    Safe_SetCurrentTarget(null);
                    return false;
                }

                if (_me.CurrentTarget.CreatedByUnitGuid == _me.Guid)
                {
                    Slog("? HB targeted my own: {0}, blacklisting ?", Safe_UnitName(_me.CurrentTarget));
                    Dlog("combatCheck:   targeted item   me.guid={0:X}   tgt:createdby={1:X}", _me.Guid, _me.CurrentTarget.CreatedByUnitGuid);
                    AddToBlacklist(_me.CurrentTarget.Guid);
                    Safe_SetCurrentTarget(null);
                    return false;
                }

                //				if (!WoWMovement.IsFacing)
                //					Safe_FaceTarget();
            }

            return true;
        }


        /// <summary>
        /// pvpChecks() 
        /// Inspects the current target and handles certain PvP specific issues
        ///     blacklists pet
        ///     purges player defensive ability (iceblock, divine shield)
        /// </summary>
        /// <returns>
        /// true - continue with fighting
        /// false- can't fight, find new target if needed
        /// </returns>
        private bool pvpChecks()
        {
            if (!_me.GotTarget)
                return false;

            if (IsPVP() || (_me.CurrentTarget.IsPlayer && _me.IsHorde != _me.CurrentTarget.ToPlayer().IsHorde))
            {
                // check for things we can't fight and should blacklist
                if (_me.CurrentTarget.IsPet)
                {
                    Slog("PVP: Blacklisting pet " + Safe_UnitName(_me.CurrentTarget));
                    Blacklist.Add(_me.CurrentTarget.Guid, TimeSpan.FromMinutes(5));
                    Safe_SetCurrentTarget(null);
                    return false;
                }

                // test, if in battleground and someone is out of line of sight, blacklist for 5 seconds
#if COMMENT
				if (!_me.CurrentTarget.InLineOfSight)
				{
					Slog("PVP: Target not in LoS, blacklisting for 2 seconds");
					Blacklist.Add(_me.CurrentTarget.Guid, TimeSpan.FromSeconds(2));
                    Safe_SetCurrentTarget(null);
                    return false;
				}
#endif
                if (typeShaman == ShamanType.Enhance && _me.CurrentTarget.Distance > _maxDistForRangeAttack)
                {
                    if (_hasTalentAncestralSwiftness && !InGhostwolfForm() && GhostWolf())
                        return true;

                    Slog("PVP: Target out of range (" + _me.CurrentTarget.Distance + " yds), blacklisting for 3 seconds");
                    Blacklist.Add(_me.CurrentTarget.Guid, TimeSpan.FromSeconds(3));
                    Safe_SetCurrentTarget(null);
                    return false;
                }

                // _me.CurrentTarget.GetBuffs(true);   // refresh buffs for checking need for blacklist or purge

                if (_me.CurrentTarget.Auras.ContainsKey("Divine Shield"))
                {
                    Slog("PVP: Palidan popped Divine Shield, blacklisted 10 secs");
                    Blacklist.Add(_me.CurrentTarget.Guid, TimeSpan.FromSeconds(10));
                    Safe_SetCurrentTarget(null);
                    return false;
                }

                if (_me.CurrentTarget.Auras.ContainsKey("Ice Block"))
                {
                    Slog("PVP: Mage popped Iceblock, blacklisted 10 secs");
                    Blacklist.Add(_me.CurrentTarget.Guid, TimeSpan.FromSeconds(10));
                    Safe_SetCurrentTarget(null);
                    return false;
                }

#if PURGE_IS_DIFFERENT_NOW
                if (SpellManager.HasSpell("Purge") && SpellManager.Spells["Purge"].Cooldown)
                {
                    Dlog("PVP:  Purge on cooldown, skipping buff tests");
                }
                else if (_me.CurrentTarget.Auras.ContainsKey("Presence of Mind") && Purge())
                {
                    Slog("PVP: mage had Presence of Mind, purging");
                }
                else if (_me.CurrentTarget.Auras.ContainsKey("Blessing of Protection") && Purge())
                {
                    Slog("PVP: target has Blessing of Protection, purging");
                }
                else if (_me.CurrentTarget.Auras.ContainsKey("Avenging Wrath") && Purge())
                {
                    Slog("PVP: paladin used Avenging Wrath, purging");
                }

                else if (_me.CurrentTarget.Auras.ContainsKey("Power Word: Shield") && Purge())
                {
                    Slog("PVP: priest used Power Word: Shield, purging");
                }
                else if (_me.CurrentTarget.Auras.ContainsKey("Fear Ward") && Purge())
                {
                    Slog("PVP: target has Fear Ward, purging");
                }
#endif
            }

            return true;
        }

        #region ATTACK SPELLS

        private void AutoAttack()
        {
            Log(Color.DodgerBlue, "*Auto-Attack");
            // RunLUA("StartAttack()");
            Lua.DoString("StartAttack()");
        }

        private void StopAutoAttack()
        {
            Log(Color.DodgerBlue, "*Stop Auto-Attack");
            // RunLUA("StopAttack()");
            Lua.DoString("StopAttack()");
        }

        private bool CastCombatSpecials()
        {
            bool cast = false;

            if (combatChecks() && AllowNonHealSpells() && !cfg.FarmingLowLevel)
            {
                if (IsPVP() || IsFightStressful() || (_me.GotTarget && _me.CurrentTarget.HealthPercent > 75))
                {
                    /*
                                        if (_me.Auras.ContainsKey("Elemental Mastery"))
                                            ;
                                        else 
                     */
                    if (_me.Auras.ContainsKey("Berserking"))
                        ;
                    else if (_me.Auras.ContainsKey("Blood Fury"))
                        ;
                    else if (_me.Auras.ContainsKey("Bloodlust"))
                        ;
                    else if (_me.Auras.ContainsKey("Heroism"))
                        ;
                    else if (_me.Auras.ContainsKey("Elemental Mastery"))
                        ;
                    else
                    {
                        // Elemental Mastery is cast a part of rotation
                        cast = Berserking();  // trolls
                        if (!cast)
                            cast = BloodFury();   // orcs
                        if (!cast)
                            cast = BloodlustHeroism();   // horde and alliance shaman
                    }

                    if (!cast)
                        cast = Stoneform();
                }
            }

            return cast;
        }

        private bool LightningBolt()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() && !AllowNonHealSpells())
                ;
            else if (IsImmunneToNature(_me.CurrentTarget))
                Dlog("skipping Lightning Bolt since {0}[{1}] is immune to Nature damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else
            {
                Safe_StopMoving();
                return Safe_CastSpell("Lightning Bolt", SpellRange.Check, SpellWait.NoWait);
            }

            return false;
        }

        private static int EnemyCountInAOE(WoWUnit target, double distRadius)
        {
            int enemyCount = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();

            if (target != null)
            {
                try
                {
                    if (!IsPVP())
                    {
                        enemyCount = (from o in ObjectManager.ObjectList
                                      where o is WoWUnit && o.Location.Distance(target.Location) <= distRadius
                                      let unit = o.ToUnit()
                                      where unit != target
                                            && unit.Attackable
                                            && unit.IsAlive
                                            && unit.Combat
                                            && !IsMeOrMyGroup(unit)
                                            && IsTargetingMeOrMyGroup(unit)
                                            && !Blacklist.Contains(unit.Guid)
                                      orderby unit.CurrentHealth ascending
                                      select unit
                                     ).Count();
                    }
                    else
                    {
                        enemyCount = (from o in ObjectManager.ObjectList
                                      where o is WoWUnit && o.Location.Distance(target.Location) <= distRadius
                                      let unit = o.ToUnit()
                                      where unit != target
                                            && unit.IsAlive
                                            && unit.IsPlayer && unit.ToPlayer().IsHorde != ObjectManager.Me.IsHorde
                                            && !unit.IsPet
                                            && !Blacklist.Contains(unit.Guid)
                                      orderby unit.CurrentHealth ascending
                                      select unit
                                     ).Count();
                    }

                }
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    Log(Color.Red, "An Exception occured. Check debug log for details.");
                    Logging.WriteDebug("HB EXCEPTION in EnemyCountInAOE()");
                    Logging.WriteException(e);
                }
            }

            Dlog("EnemyCountInAOE(): took {0} ms", timer.ElapsedMilliseconds);
            return enemyCount;
        }

#if NOT_RIGHT_NOW
		private static bool WillChainLightningHop(WoWUnit target)
		{
			return 2 >= EnemyCountInAOE(target, 12);
		}

		private static bool UseEarthquake(WoWUnit target)
		{
			return 3 >= EnemyCountInAOE(target, 8);
		}
#endif
        private bool ChainLightning()
        {
            if (!combatChecks())
                ;
            else if (!SpellManager.HasSpell("Chain Lightning"))
                ;
            else if (!HaveValidTarget() || !AllowNonHealSpells())
                ;
            else
            {
                Safe_StopMoving();
                return Safe_CastSpellWithRangeCheck("Chain Lightning");
            }

            return false;
        }

        private bool ShamanisticRage()
        {
            if (!combatChecks())
                ;
            else if (!SpellManager.HasSpell("Shamanistic Rage"))
                ;
            else
            {
                return Safe_CastSpell("Shamanistic Rage");
            }

            return false;
        }

        private bool EarthShock()
        {
            // NOTE:  with patch 3.3 of WoW using Shock spells is not as much of a concern
            // ... since they drastically increased the mana regen
            // --- SO, force it to ignore Water Shield for now
            if (!combatChecks())
                Dlog("EarthShock:  failed combat check");
            else if (!HaveValidTarget() || !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Earth Shock"))
                ;
            else if (!_me.CurrentTarget.IsPlayer && IsImmunneToNature(_me.CurrentTarget))
                Dlog("skipping Earth Shock since {0}[{1}] is immune to Nature damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else
                return Safe_CastSpell("Earth Shock", SpellRange.Check, SpellWait.NoWait);

            return false;
        }

        private bool CanAnElemBuyAnEarthShock()
        {
            if (!_me.GotTarget)
                return false;

            bool permitEarthShock = false;
            WoWAura fsa = GetAuraCreatedByMe(_me.CurrentTarget, "Flame Shock");
            if (fsa == null)
                permitEarthShock = !SpellManager.HasSpell("Flame Shock");
            else
            {
                WoWSpell fss = SpellManager.Spells["Flame Shock"];
                permitEarthShock = fsa.TimeLeft.TotalMilliseconds > 6000;
                Dlog("CanBuyEarthShock:  flame shock DoT left={0}", fsa.TimeLeft.TotalMilliseconds);
            }

            return permitEarthShock;
        }

        private bool UnleashFlameCheck()
        {
            // NOTE:  with patch 3.3 of WoW using Shock spells is not as much of a concern
            // ... since they drastically increased the mana regen
            // --- SO, force it to ignore Water Shield for now
            if (!combatChecks())
                Dlog("UnleashFlameCheck:  failed combat check");
            else if (!HaveValidTarget() || !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Flame Shock"))
                ;
            else if (!_me.CurrentTarget.IsPlayer && IsImmunneToFire(_me.CurrentTarget))
                Dlog("UnleashFlameCheck:  skipping Flame Shock since {0}[{1}] is immune to Fire damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else if (!_me.Auras.ContainsKey("Unleash Flame"))
                Dlog("UnleashFlameCheck:  missing Unleash Flame debuff");
            else
                return Safe_CastSpell("Flame Shock", SpellRange.Check, SpellWait.NoWait);

            return false;
        }

        private bool UnleashElements()
        {
            if (!combatChecks())
                Dlog("UnleashElements:  failed combat check");
            else if (!AllowNonHealSpells() || !SpellManager.HasSpell("Unleash Elements"))
                ;
            else if (!DoWeaponsHaveImbue())
                Dlog("UnleashElements:  skipping cast until weapons imbued");
            else
                return Safe_CastSpell("Unleash Elements", SpellRange.Check, SpellWait.NoWait);

            return false;
        }

        private bool FlameShock()
        {
            // NOTE:  with patch 3.3 of WoW using Shock spells is not as much of a concern
            // ... since they drastically increased the mana regen
            // --- SO, force it to ignore Water Shield for now
            if (!combatChecks())
                Dlog("FlameShock:  failed combat check");
            else if (!HaveValidTarget() || !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Flame Shock"))
                ;
            else if (!_me.CurrentTarget.IsPlayer && IsImmunneToFire(_me.CurrentTarget))
                Dlog("skipping Flame Shock since {0}[{1}] is immune to Fire damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else if (_me.CurrentTarget.Auras.ContainsKey("Flame Shock"))
                Dlog("FlameShock:  target already has DoT");
            else
                return Safe_CastSpell("Flame Shock", SpellRange.Check, SpellWait.NoWait);

            return false;
        }

        private bool FlameShockRenew()
        {
            // NOTE:  with patch 3.3 of WoW using Shock spells is not as much of a concern
            // ... since they drastically increased the mana regen
            // --- SO, force it to ignore Water Shield for now
            if (!combatChecks())
                Dlog("FlameShock:  failed combat check");
            else if (!HaveValidTarget() || !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Flame Shock"))
                ;
            else if (!_me.CurrentTarget.IsPlayer && IsImmunneToFire(_me.CurrentTarget))
                Dlog("skipping Flame Shock since {0}[{1}] is immune to Fire damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else
            {
                // following code checks to make sure that DoT won't 
                // ... fall off before Lava Burst cast completes
                WoWAura fs = GetAuraCreatedByMe(_me.CurrentTarget, "Flame Shock");
                if (fs != null)
                {
                    if (!SpellManager.HasSpell("Lava Burst"))
                        return false;

                    WoWSpell lvb = SpellManager.Spells["Lava Burst"];
                    if ((200 + lvb.CastTime) < fs.TimeLeft.TotalMilliseconds)
                        return false;

                    Dlog("FlameShock:  DoT only has {0} ms left, so renewing", fs.TimeLeft.TotalMilliseconds);
                }

                return Safe_CastSpell("Flame Shock", SpellRange.Check, SpellWait.NoWait);
            }
            return false;
        }


        private bool FrostShock()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() && !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Frost Shock"))
                ;
            else if (!_me.CurrentTarget.IsPlayer && IsImmunneToFrost(_me.CurrentTarget))
                Dlog("skipping Frost Shock since {0}[{1}] is immune to Frost damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else if (_me.CurrentTarget.Auras.ContainsKey("Frost Shock"))
                ;
            else
                return Safe_CastSpell("Frost Shock", SpellRange.Check, SpellWait.NoWait);

            return false;
        }

        /*
         * Summary:  determines the best Shock spell opener to
         * use.  This is for use during pull only.  
         */
        private bool ShockOpener()
        {
            bool bCast = false;

            if (_me.GotTarget && !IsHealerOnly())
            {
                if (!bCast)
                    bCast = UnleashElements();

                if (!bCast && (_me.CurrentTarget.IsPlayer && _me.CurrentTarget.Class != WoWClass.Rogue))
                    bCast = FrostShock();

                if (!bCast)
                    bCast = FlameShock();

                if (!bCast)
                    bCast = EarthShock();

                if (!bCast)
                    bCast = FrostShock();
            }

            return bCast;
        }

        private bool BestInstantAttack()
        {
            bool knowFrost = SpellManager.HasSpell("Frost Shock");
            bool knowEarth = SpellManager.HasSpell("Earth Shock");
            bool knowFire = SpellManager.HasSpell("Flame Shock");

            if (!combatChecks())
                return false;

            if (priorityPurge == ConfigValues.SpellPriority.High && Purge())
                return true;

            if (knowFire && _me.CurrentTarget.Class == WoWClass.Rogue && FlameShock())
                return true;

            if (knowFrost && (_me.CurrentTarget.Fleeing || _me.CurrentTarget.IsPlayer) && FrostShock())
                return true;

            if (knowEarth && FulminationCheck())
                return true;

            if (knowFire && FlameShock())
                return true;

            if (knowEarth && EarthShock())
                return true;

            if (MaelstromCheck())
                return true;

            if (IsWeaponImbuedWithDPS() && UnleashElements())
                return true;

            //      if (knowFrost && FrostShock())
            //      return true;

            return false;
        }

        // note:  unlike other offensive spells, this one works on a provided
        // target rather than .CurrentTarget so we can CC someone other than
        // person directly engaged in combat with
        private bool Hex(WoWUnit u)
        {
            //            if (!AllowNonHealSpells())        don't care since very low mana cost
            //                ;
            if (!SpellManager.HasSpell("Hex"))
                ;
            else if (!u.IsPlayer && u.CreatureType != WoWCreatureType.Humanoid && u.CreatureType != WoWCreatureType.Beast)
                Dlog("Hex:  cannot hex, target {0} is type={1}; must be a player, humanoid, or beast", Safe_UnitName(u), u.CreatureType);
            else if (IsAuraPresent(u, "Hex"))
                Dlog("Hex:  Player is already a Frog");
            else
                return Safe_CastSpell(u, "Hex", SpellRange.Check, SpellWait.NoWait);

            return false;
        }

        private bool PrimalStrike()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() || !AllowNonHealSpells())
                ;
            else if (SpellManager.HasSpell("Stormstrike"))      // never use if we know Stormstrike
                ;                                               // .. since they share a cooldown
            else if (!SpellManager.HasSpell("Primal Strike"))
                ;
            else if (!CurrentTargetInMeleeDistance())
                ;
            else
                return Safe_CastSpell("Primal Strike", SpellRange.NoCheck, SpellWait.NoWait);

            return false;
        }

        private bool Stormstrike()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() || !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Stormstrike"))
                ;
            else if (!CurrentTargetInMeleeDistance())
                ;
            else
                return Safe_CastSpell("Stormstrike", SpellRange.NoCheck, SpellWait.NoWait);

            return false;
        }

        private bool IsStormstrikeNeeded()
        {
            if (SpellManager.HasSpell("Stormstrike"))
            {
                if (HaveValidTarget() && !_me.CurrentTarget.Auras.ContainsKey("Stormstrike"))
                {
                    if (CurrentTargetInMeleeDistance())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool LavaLash()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() && !AllowNonHealSpells())
                ;
            else if (!CurrentTargetInMeleeDistance())
                ;
            else if (!SpellManager.HasSpell("Lava Lash"))
                ;
            else if (IsImmunneToFire(_me.CurrentTarget))
                Dlog("skipping Lava Lash since {0}[{1}] is immune to Fire damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else
            {
                /*
                                // do a test to see if searing flames are likley
                                bool isSearingFlamesLikely = _hasTalentImprovedLavaLash 
                                    && (TotemExist(TotemId.SEARING_TOTEM) || (!TotemExist(TOTEM_FIRE) && _totemBar[TOTEM_FIRE] == TotemId.SEARING_TOTEM ))
                                    && (_me.CurrentTarget.HealthPercent > 15 || _me.CurrentTarget.Elite);

                                if (isSearingFlamesLikely && !_me.CurrentTarget.IsPlayer)
                                    ;
                                else
                 */
                return Safe_CastSpell("Lava Lash", SpellRange.NoCheck, SpellWait.NoWait);
            }

            return false;
        }

        private bool LavaBurst()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() && !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Lava Burst"))
                ;
            else if (IsImmunneToFire(_me.CurrentTarget))
                Dlog("skipping Lava Burst since {0}[{1}] is immune to Fire damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else
            {
                return Safe_CastSpell("Lava Burst", SpellRange.Check, SpellWait.NoWait);
            }

            return false;
        }

        private bool ElementalMastery()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() && !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Elemental Mastery"))
                ;
            else
                return Safe_CastSpell("Elemental Mastery", SpellRange.NoCheck, SpellWait.NoWait);

            return false;
        }

        private bool Thunderstorm()
        {
            if (!combatChecks())
                ;
            else if (IsRAF() && !cfg.RAF_UseThunderstorm)
                ;
            else if (!SpellManager.HasSpell("Thunderstorm"))
                ;
            else
                return Safe_CastSpell("Thunderstorm", SpellRange.NoCheck, SpellWait.NoWait);

            return false;
        }

        private bool FireNova()
        {
            if (!combatChecks())
                ;
            else if (!HaveValidTarget() && !AllowNonHealSpells())
                ;
            else if (!SpellManager.HasSpell("Fire Nova"))
                ;
#if PRE_410_METHOD
            else if (!(TotemExist(TotemId.MAGMA_TOTEM) || TotemExist(TotemId.FLAMETONGUE_TOTEM) || TotemExist(TotemId.FIRE_ELEMENTAL_TOTEM)))
                Dlog("Magma/Flametongue/Fire elemental totem doesn't exist, Fire Nova not cast");
#else
            else if ( null != GetAuraCreatedByMe( _me.CurrentTarget, "Flame Shock"))
                Dlog("FireNova:  current target missing Flame Shock");
#endif
            else if (_countFireNovaEnemy < 3)
                Dlog("FireNova:  not cast, only {0} enemies within range of current target", _countFireNovaEnemy );
            else if (IsImmunneToFire(_me.CurrentTarget))
                Dlog("skipping Fire Nova since {0}[{1}] is immune to Fire damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else
                return Safe_CastSpell("Fire Nova", SpellRange.NoCheck, SpellWait.NoWait);

            return false;
        }

        private bool MaelstromCheck()
        {
            bool castSpell = false;

            if (!_hasTalentMaelstromWeapon)
                ;
            else if (!combatChecks())
                ;
            else if (!HaveValidTarget())
                ;
            else if (!IsPVP() && IsImmunneToNature(_me.CurrentTarget))
                Dlog("MaelstromCheck:  skipping since {0}[{1}] is immune to Nature damage", Safe_UnitName(_me.CurrentTarget), _me.CurrentTarget.Entry);
            else
            {
                uint stackCount;
                if (!IsAuraPresent(_me, "Maelstrom Weapon", out stackCount))
                    ;
                else if (stackCount < 5)
                    ;   // ignore... we are looking for 4 for emergency heal or 5 for attack
                else if ( IsPVP() || IsSelfHealNeeded() )
                {
                    if ( !IsPVP() || (IsPVP() && _me.HealthPercent < 70))
                    {
                        if (SpellManager.HasSpell("Greater Healing Wave"))
                            return Safe_CastSpell(_me, "Greater Healing Wave", SpellRange.Check, SpellWait.NoWait);
                        if (SpellManager.HasSpell("Healing Surge"))
                            return Safe_CastSpell(_me, "Healing Surge    ", SpellRange.Check, SpellWait.NoWait);
                        return Safe_CastSpell(_me, "Healing Wave", SpellRange.Check, SpellWait.NoWait);
                    }
                }
                else
                {
                    Log("^Maelstrom Attack @ " + stackCount + " stks");
                    if (CurrentTargetInMeleeDistance() && IsStormstrikeNeeded())
                        Stormstrike();  // throw a Stormstrike if debuff not up

                    while (!IsGameUnstable() && _me.IsAlive && _me.ManaPercent > cfg.EmergencyManaPercent && !SpellManager.CanCast("Lightning Bolt"))
                    {
                        Dlog("MaelstromCheck:  waiting to cast some Lightning");
                        Thread.Sleep(25);
                    }

                    if (_countAoe12Enemy > 1 && SpellManager.HasSpell("Chain Lightning"))
                        castSpell = Safe_CastSpell("Chain Lightning", SpellRange.Check, SpellWait.NoWait);
                    // castSpell = SpellManager.Cast("Chain Lightning");

                    if (!castSpell)
                        castSpell = Safe_CastSpell("Lightning Bolt", SpellRange.Check, SpellWait.NoWait);
                    // castSpell = SpellManager.Cast("Lightning Bolt");

                    // if (castSpell)
                    //     Log("*Maelstrom Attack");
                }
            }

            return castSpell;
        }

        private bool MaelstromCheckPVP()
        {
            bool castSpell = false;

            if (!_hasTalentMaelstromWeapon)
                return false;

            if ( !_me.ActiveAuras.ContainsKey( "Maelstrom Weapon") )
                return false;

            WoWAura mael = _me.ActiveAuras["Maelstrom Weapon"];
            if (mael.StackCount < 5)
                return false;

            WoWPlayer heal;

            if ( _me.HealthPercent < 75 )
                heal = _me.ToPlayer();
            else
            {
                heal = 
                    (from p in GroupMembers 
                     where p.Distance < 38 && p.HealthPercent < 50
                     orderby p.HealthPercent ascending
                     select p 
                     ).FirstOrDefault();
            }

            if ( heal != null )
            {
                Log("^Maelstrom Heal @ 5 stacks");
                if (SpellManager.HasSpell( "Greater Healing Wave"))
                    return Safe_CastSpell( heal, "Greater Healing Wave", SpellRange.Check, SpellWait.NoWait);
                if (SpellManager.HasSpell("Healing Surge"))
                    return Safe_CastSpell(heal, "Healing Surge    ", SpellRange.Check, SpellWait.NoWait);
                return Safe_CastSpell(heal, "Healing Wave", SpellRange.Check, SpellWait.NoWait);
            }

                    
            if ( mael.TimeLeft.TotalMilliseconds > 2500 )
                return false;

            Log("^Maelstrom Attack @ 5 stks with only {0} ms left", mael.TimeLeft.TotalMilliseconds);
            if (CurrentTargetInMeleeDistance() && IsStormstrikeNeeded())
                Stormstrike();  // throw a Stormstrike if debuff not up

            while (!IsGameUnstable() && _me.IsAlive && _me.ManaPercent > cfg.EmergencyManaPercent && !SpellManager.CanCast("Lightning Bolt"))
            {
                Dlog("MaelstromCheck:  waiting to cast some Lightning");
                Thread.Sleep(25);
            }

            if (SpellManager.HasSpell("Chain Lightning"))
                castSpell = Safe_CastSpell("Chain Lightning", SpellRange.Check, SpellWait.NoWait);

            if (!castSpell)
                castSpell = Safe_CastSpell("Lightning Bolt", SpellRange.Check, SpellWait.NoWait);

            return castSpell;
        }

#if WAIT_FOR_SEARING_FLAMES
		private bool SearingFlamesCheck()
		{
			if (!_hasTalentImprovedLavaLash)
				;
			else if (!combatChecks())
				;
			else if (!HaveValidTarget())
				;
			else if (!SpellManager.HasSpell("Lava Lash"))
				;
			else if (!CurrentTargetInMeleeDistance())
				;
			else if (IsImmunneToFire(_me.CurrentTarget))
				Slog("SearingFlames: skipping Lava Lash because mob is Fire immune");
			else
			{
				uint stackCount = GetAuraStackCount( _me.CurrentTarget, "Searing Flames");
				Dlog( "SearingFlameCheck: found {0} stacks", stackCount);
				if (stackCount >= 5)
				{
					Slog("^Searing Flames @ " + stackCount + " stks");
					if (Safe_CastSpell("Lava Lash", SpellRange.NoCheck, SpellWait.NoWait))
					{
						Dlog("CombatElem: Lava Lash cast so no further attacks this pass");
						return true;
					}
				}
			}

			return false;
		}
#endif

        private bool FulminationCheck()
        {
            if (!_hasTalentFulmination)
                ;
            else if (!combatChecks())
                ;
            else if (!HaveValidTarget())
                ;
            else if (SpellManager.HasSpell("Earth Shock"))
            {
                uint stackCount = GetAuraStackCount(_me, "Lightning Shield");
                Dlog("FulminationCheck:  Lightning Shields stack count is {0}", stackCount);
                if (stackCount >= 7)
                {
                    if (IsImmunneToNature(_me.CurrentTarget))
                        Slog("FulminationCheck: skipping Earth Shock because mob is Nature immune");
                    else if (!SpellManager.CanCast("Earth Shock"))
                        Slog("Earth Shock on Cooldown... waiting on Fulmination cast", stackCount);
                    else
                    {
                        Slog("^Fulmination at {0} stacks", stackCount);
                        if (Safe_CastSpell("Earth Shock", SpellRange.Check, SpellWait.NoWait))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool InterruptEnemyCast()
        {
            int EnemyCastDistance = 0;
            WoWUnit target = null;
            WoWSpell windShear = null;
            WoWSpell warStomp = null;

            if (cfg.InterruptStyle == ConfigValues.SpellInterruptStyle.None)
                return false;

            if (SpellManager.HasSpell("Wind Shear"))
            {
                windShear = SpellManager.Spells["Wind Shear"];
                if (!windShear.Cooldown)
                    EnemyCastDistance = Math.Max(EnemyCastDistance, (int)windShear.MaxRange);
                else
                    windShear = null;
            }

            if (SpellManager.HasSpell("War Stomp"))
            {
                warStomp = SpellManager.Spells["War Stomp"];
                if (!warStomp.Cooldown)
                    EnemyCastDistance = Math.Max(EnemyCastDistance, 8);
                else
                    warStomp = null;
            }

            if (EnemyCastDistance == 0)
            {
                Dlog("InterruptEnemyCast:  both Wind Shear and War Stomp on cooldown, cannot cast");
                return false;
            }

            if (cfg.InterruptStyle == ConfigValues.SpellInterruptStyle.CurrentTarget)
            {
                if (IsTargetCastInterruptible(_me.CurrentTarget) && _me.CurrentTarget.Distance <= EnemyCastDistance)
                {
                    target = _me.CurrentTarget;
                }
            }
            else // ConfigValues.SpellInterruptStyle.All
            {

                target = (from o in ObjectManager.ObjectList
                          where o is WoWUnit && o.Distance <= EnemyCastDistance
                          let unit = o.ToUnit()
                          where unit.Attackable
                                && Safe_IsHostile(unit)
                                && !unit.IsPet
                                && unit.HealthPercent > 5
                                && ((unit.Combat && IsTargetingMeOrMyGroup(unit)) || IsPVP())
                                && IsTargetCastInterruptible(unit)
                          orderby unit.CurrentHealth ascending
                          select unit
                         ).FirstOrDefault();
            }

            if (target != null)
            {
                Dlog("InterruptEnemyCast:  interrupting enemy {0} @ {1:F1} yds casting [{2}]", Safe_UnitName(target), target.Distance, target.CastingSpell == null ? "(null)" : target.CastingSpell.Name);
                if (windShear != null && Safe_CastSpell(target, windShear, SpellRange.Check, SpellWait.NoWait))
                {
                    return false;       // return false because Wind Shear doesn't have GCD
                }

                if (warStomp != null && Safe_CastSpell(null, warStomp, SpellRange.NoCheck, SpellWait.Complete))
                {
                    return true;
                }

                Dlog("InterruptEnemyCast:  failed to interrupt", Safe_UnitName(target), target.Distance, target.CastingSpell == null ? "(null)" : target.CastingSpell.Name);
            }
            return false;
        }


        private bool Purge()
        {
            int EnemyCastDistance = 0;
            WoWUnit target = null;
            WoWSpell purge = null;

            if (IsCasting())
            {
                Dlog("Purge: cast in progress - purge not cast");
                return false;
            }

            if (!AllowNonHealSpells())
            {
                Dlog("Purge: non-healing spells blocked - purge not cast");
                return false;
            }

            if (SpellManager.HasSpell("Purge"))
                purge = SpellManager.Spells["Purge"];

            if (purge == null)
                return false;

            target=(from o in ObjectManager.ObjectList
                    where o.Distance <= (EnemyCastDistance - 1)
                    let unit = o.ToUnit()
                    where unit.Attackable
                        && Safe_IsHostile(unit)
                        && !unit.IsPet
                        && unit.HealthPercent > 1
                        && !Blacklist.Contains(unit)
                        && (IsPVP() || (unit.Combat && IsTargetingMeOrMyGroup(unit)))
                        && (from dbf in unit.Buffs
                            where
                                dbf.Value.Spell.DispelType == WoWDispelType.Magic
                                && _hashPurgeWhitelist.Contains(dbf.Value.SpellId)
                            select dbf.Value
                            ).Any()
                    select unit
                    ).FirstOrDefault();           

            if (target != null)
            {
                Dlog("Purge:  purging enemy {0} @ {1:F1} yds", Safe_UnitName(target), target.Distance );
                if (Safe_CastSpell(target, purge, SpellRange.Check, SpellWait.NoWait  ))
                    return true;

                Dlog("Purge:  failed to purge {0} @ {1:F1} yds", Safe_UnitName(target), target.Distance );
            }

            return false;
        }


        private bool FeralSpirit()
        {
            bool castGood = false;

            if (cfg.FarmingLowLevel)
                return false;

            if (IsRAF() && cfg.RAF_SaveFeralSpiritForBosses && (!_me.GotTarget || !_me.CurrentTarget.IsAlive || _me.CurrentTarget.CreatureRank != WoWUnitClassificationType.WorldBoss))
            {
                // Dlog("Feral Spirit:  not cast because not currently targeting a boss");
            }
            else if (!(IsPVP() || IsFightStressful() || !cfg.PVE_SaveForStress_FeralSpirit))
            {
                // Dlog("Feral Spirit:  not cast because  InBattleground={0}, IsFightStressful()={1}, and SaveForStress={2}", IsPVP(), IsFightStressful(), cfg.PVE_SaveForStress_FeralSpirit );
            }
            else if (SpellManager.HasSpell("Feral Spirit"))
            {
                castGood = Safe_CastSpell("Feral Spirit", SpellRange.NoCheck, SpellWait.Complete);
                if (castGood)
                {
                    Log("^Pet Defensive Mode");
                    RunLUA("PetDefensiveMode()");     // turn on defensive mode

                    Log("^Pet Attack");
                    RunLUA("PetAttack()");     // turn on defensive mode

                    if (!IsPVP())
                    {
                        Log("^Pet Ability - Twin Howl");
                        RunLUA("CastPetAction(5)");       // throw a twin howl immediately
                    }
                    else if (_me.Rooted || MeImmobilized())
                    {
                        Log("^Pet Ability - Spirit Walk (remove movement impairing effects)");
                        Safe_CastSpell("Spirit Walk");
                    }
                }
            }

            return castGood;
        }


        private bool EarthElementalTotem()
        {
            bool castGood = false;

            if (!HasTotemSpell(TotemId.EARTH_ELEMENTAL_TOTEM))
                ;
            else if (IsRAF() && cfg.RAF_SaveElementalTotemsForBosses && (!_me.GotTarget || !_me.CurrentTarget.IsAlive || _me.CurrentTarget.CreatureRank != WoWUnitClassificationType.WorldBoss))
            {
                Dlog("Earth Elemental Totem:  not cast because not currently targeting a boss");
            }
            else if (!IsRAF() && !IsPVP() && !IsFightStressful() && cfg.PVE_SaveForStress_ElementalTotems)
            {
                Dlog("Earth Elemental Totem:  not cast because not a stressful PVE situation");
            }
            else
            {
                castGood = TotemCast(TotemId.EARTH_ELEMENTAL_TOTEM);
            }

            return castGood;
        }

        private bool FireElementalTotem()
        {
            bool castGood = false;

            if (!HasTotemSpell(TotemId.FIRE_ELEMENTAL_TOTEM))
                ;
            else if (IsRAF() && cfg.RAF_SaveElementalTotemsForBosses && (!_me.GotTarget || !_me.CurrentTarget.IsAlive || _me.CurrentTarget.CreatureRank != WoWUnitClassificationType.WorldBoss))
            {
                Dlog("Fire Elemental Totem:  not cast because not currently targeting a boss");
            }
            else if (!IsRAF() && !IsPVP() && !IsFightStressful() && cfg.PVE_SaveForStress_ElementalTotems)
            {
                Dlog("Fire Elemental Totem:  not cast because  InBattleground={0}, IsFightStressful()={1}, and SaveForStress={2}",
                    IsPVP(),
                    IsFightStressful(),
                    cfg.PVE_SaveForStress_ElementalTotems
                    );
            }
            else
            {
                castGood = TotemCast(TotemId.FIRE_ELEMENTAL_TOTEM);
            }

            return castGood;
        }

        private bool CallForReinforcements()
        {
            if (cfg.FarmingLowLevel)
                return false;

            if (_me.GotAlivePet)
                return false;

            if (TotemExist(TotemId.EARTH_ELEMENTAL_TOTEM))
                return false;

            if (TotemExist(TotemId.FIRE_ELEMENTAL_TOTEM))
                return false;

            if (FeralSpirit())
                return true;

            if (IsPVP() && !IsHealerOnly() && FireElementalTotem())
                return true;
            else if (IsRAF())
            {
                // in instances, use Fire Elemental on bosses if DPS
                if (!IsHealerOnly() && FireElementalTotem())
                    return true;

                // in instances, use Earth Elemental if tank in trouble
                if (GroupTank != null && GroupTank.HealthPercent < 20)
                {
                    // if any Earth Elemental Totem exists, don't cast
                    if (ObjectManager.ObjectList.Any(o => o.Entry == 15430))
                        return false;

                    return EarthElementalTotem();
                }
            }
            else // grinding and questing
            {
                if (EarthElementalTotem())
                    return true;

                if (FireElementalTotem())
                    return true;
            }

            return false;
        }

        private bool BloodlustHeroism()
        {
            if (cfg.FarmingLowLevel)
                return false;

            if (IsPVP())
            {
                ; // okay to have a trigger finger here
            }
            else if (IsRAF())
            {
                if (!cfg.RAF_UseBloodlustOnBosses)
                    return false;
                if (GroupTank == null || !GroupTank.Combat)
                    return false;
                WoWUnit target = GroupTank.CurrentTarget;
                if (target == null || !target.Combat)
                    return false;
                if (target.CreatureRank != WoWUnitClassificationType.WorldBoss)
                    return false;
            }
            else
            {
                if (cfg.PVE_SaveForStress_Bloodlust && !IsFightStressful())
                    return false;
            }

            bool knowBloodlust = SpellManager.HasSpell("Bloodlust");
            bool knowHeroism = SpellManager.HasSpell("Heroism");
            if (!knowBloodlust && !knowHeroism)
                ;
            else if (_me.Debuffs.ContainsKey("Temporal Displacement"))
                ;
            else if (knowBloodlust && !_me.Debuffs.ContainsKey("Sated") && Safe_CastSpell("Bloodlust", SpellRange.NoCheck, SpellWait.NoWait))
            {
                Slog("Bloodlust: just broke out a major can of whoop a$$!");
                return true;
            }
            else if (knowHeroism && !_me.Debuffs.ContainsKey("Exhaustion") && Safe_CastSpell("Heroism", SpellRange.NoCheck, SpellWait.NoWait))
            {
                Slog("Heroism: just broke out a major can of whoop a$$!");
                return true;
            }

            return false;
        }

        #endregion

        #region HEALING

        private bool HealingWave()
        {
            return Safe_CastSpell("Healing Wave");
        }

        private bool HealingSurge()
        {
            if (!SpellManager.HasSpell("Healing Surge"))
                return false;
            return Safe_CastSpell("Healing Surge");
        }

        private bool CanUsePotion()
        {
            return !_potionTimer.IsRunning || _potionTimer.ElapsedMilliseconds > 60 * 1000;
        }

        private bool UseManaPotionIfAvailable()
        {
            return UsePotion(CheckForItem(_potionManaEID));
        }

        private bool UseHealthPotionIfAvailable()
        {
            return UsePotion(CheckForItem(_potionHealthEID));
        }

        private bool UsePotion(WoWItem potion)
        {
            if (potion != null)
            {
                if (MeImmobilized())
                    Slog("Immobilized -- unable to use potion now");
                else
                {
                    if (CanUsePotion())
                    {
                        Slog("POTION:  Using '" + potion.Name + "'");
                        Dlog("{0} has a cooldown of {1}", potion.Name, potion.Cooldown);
                        RunLUA("UseItemByName(\"" + potion.Name + "\")");
                        StyxWoW.SleepForLagDuration();
                        _potionTimer.Reset();
                        _potionTimer.Start();
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool UseConsumeable(string sItem)
        {
            WoWItem item = CheckForItem(sItem);
            if (item != null)
            {
                item.Use();
                StyxWoW.SleepForLagDuration();
            }

            return item != null;
        }

        private bool UseBandageIfAvailable()
        {
            WoWItem bandage = CheckForItem(_bandageEID);
            if (cfg.UseBandages && !SpellManager.Spells.ContainsKey("First Aid"))
            {
                Wlog("Use Bandages ignored : your Shaman has not trained First Aid");
            }
            else if (bandage == null)
                Wlog("FIRST-AID:  no bandages in inventory");
            else if (_me.Debuffs.ContainsKey("Recently Bandaged"))
                Dlog("FIRST-AID:  can't bandage -- currently under 'Recently Bandaged' debuff");
            else if (!MeImmobilized())
            {
                foreach (KeyValuePair<string, WoWAura> dbf in _me.Debuffs)
                {
                    if (!dbf.Value.IsHarmful)
                        continue;
                    Dlog("FIRST-AID:  can't bandage -- harmful debuff '{0}' active", dbf.Key);
                    return false;
                }

                Safe_StopMoving();

                double healthStart = _me.HealthPercent;
                Stopwatch timeBandaging = new Stopwatch();
                Slog("FIRST-AID:  using '{0}' at {1:F0}%", bandage.Name, _me.HealthPercent);
                timeBandaging.Start();

                try
                {
                    bandage.Use();
                    // RunLUA("UseItemByName(\"" + bandage.Name + "\", \"player\")");
                    do
                    {
                        Thread.Sleep(100);
                        Dlog("dbg firstaid:  buff-present:{0}, casting:{1}, channeled:{2}",
                            IsAuraPresent(_me, "First Aid"),
                            _me.IsCasting,
                            _me.ChanneledCastingSpellId != 0);
                    } while (!IsGameUnstable() && _me.IsAlive && (IsAuraPresent(_me, "First Aid") || timeBandaging.ElapsedMilliseconds < 1000) && _me.HealthPercent < 100.0);
                }
                catch (ThreadAbortException) { throw; }
                catch (Exception e)
                {
                    Log(Color.Red, "An Exception occured. Check debug log for details.");
                    Logging.WriteDebug("WOW LUA Call to UseItemByName() failed");
                    Logging.WriteException(e);
                }

                Dlog("FIRST-AID:  used {0} for {1:F1} secs ending at {2:F0}%", bandage.Name, timeBandaging.Elapsed.TotalSeconds, _me.HealthPercent);
                if (healthStart < _me.HealthPercent)
                    return true;
            }

            return false;
        }


        #endregion

        private bool IsShieldBuffNeeded(bool atRest)
        {
            return ShieldType.None != WhichShieldTypeNeeded(atRest);
        }

        private ShieldType WhichShieldTypeNeeded(bool atRest)
        {
            ShieldType shield = ShieldType.None;

            if (cfg.FarmingLowLevel)
                return shield;

            // RAF for any SHAMAN HEALER (can be any spec)
            //  :   Water Shield on self, Earth Shield (if available) on leader
            if (cfg.FarmingLowLevel || (IsRAF() && IsHealer()))
            {
                uint uWaterStacks = GetAuraStackCount(_me, "Water Shield");
                if ((uWaterStacks == 0 || (uWaterStacks < 3 && atRest)) && SpellManager.HasSpell("Water Shield"))
                {
                    shield = ShieldType.Water;
                    Dlog("WhichShieldTypeNeeded:  {0} Shield on self due to RAF Healer or Lowbie farming", shield.ToString());
                }
                else if (IsRAFandTANK() && GroupTank.IsAlive && SpellManager.HasSpell("Earth Shield"))
                {
                    // check if tank needs Earth Shield and is within range
                    if (GetAuraStackCount(GroupTank, "Earth Shield") < (atRest ? 4 : 1) && IsUnitInRange(GroupTank, 39))
                    {
                        shield = ShieldType.Earth;
                        // dont set last shield used here since it was on someone else
                        Dlog("WhichShieldTypeNeeded:  {0} Shield on Tank due to RAF Healer", shield.ToString());
                    }
                }
            }
            // Battlegrounds/Arenas for RESTO SHAMAN ONLY
            //  :   Support Shield Twisting between Water Shield and Earth Shield
            else if (typeShaman == ShamanType.Resto && IsPVP() && SpellManager.HasSpell("Earth Shield"))
            {
                bool trainedWaterShield = SpellManager.HasSpell("Water Shield");
                bool trainedEarthShield = SpellManager.HasSpell("Earth Shield");
                if (!trainedEarthShield && !trainedWaterShield)
                    return shield;

                uint uWaterStacks = GetAuraStackCount(_me, "Water Shield");
                uint uEarthStacks = GetAuraStackCount(_me, "Earth Shield");

                if (trainedWaterShield && _me.ManaPercent <= cfg.TwistManaPercent && (uWaterStacks == 0 || (atRest && uWaterStacks < 3)))
                {
                    shield = ShieldType.Water;
                    Dlog("WhichShieldTypeNeeded:  {0} Shield on self due to PvP Resto and Mana at {1:F1}%", shield.ToString(), _me.ManaPercent);
                }
                // check if Earth shield required
                else if (trainedEarthShield && _me.ManaPercent > cfg.TwistDamagePercent && (uEarthStacks == 0 || (atRest && uEarthStacks < 3)))
                {
                    shield = ShieldType.Earth;
                    Dlog("WhichShieldTypeNeeded:  {0} Shield on self due to PvP Resto and Mana at {1:F1}%", shield.ToString(), _me.ManaPercent);
                }
                // now check if missing a shield and need to Twist
                else if ((uWaterStacks + uEarthStacks) == 0 || (atRest && (uWaterStacks + uEarthStacks) < 3))
                {
                    if (_lastShieldUsed == ShieldType.Water && trainedWaterShield)
                        shield = ShieldType.Water;
                    else
                        shield = ShieldType.Earth;

                    Dlog("WhichShieldTypeNeeded:  {0} Shield on self due to PvP Resto and Twist with Mana at {1:F1}%", shield.ToString(), _me.ManaPercent);
                }
            }
            // Everything Else
            //  :   Shield Twist between Lightning Shield and Water Shield
            else
            {
                bool trainedWaterShield = SpellManager.HasSpell("Water Shield");
                bool trainedLightningShield = SpellManager.HasSpell("Lightning Shield");
                if (!trainedLightningShield && !trainedWaterShield)
                    return shield;

                uint uWaterStacks = GetAuraStackCount(_me, "Water Shield");
                uint uLightningStacks = GetAuraStackCount(_me, "Lightning Shield");

                if (trainedWaterShield && _me.ManaPercent <= cfg.TwistManaPercent && (uWaterStacks == 0 || (atRest && uWaterStacks < 3)))
                {
                    shield = ShieldType.Water;
                    Dlog("WhichShieldTypeNeeded:  {0} Shield on self with Mana at {1:F1}%", shield.ToString(), _me.ManaPercent);
                }
                // check if Lightning shield required
                else if (trainedLightningShield && _me.ManaPercent > cfg.TwistDamagePercent && (uLightningStacks == 0 || (atRest && uLightningStacks < 3)))
                {
                    shield = ShieldType.Lightning;
                    Dlog("WhichShieldTypeNeeded:  {0} Shield on self with Mana at {1:F1}%", shield.ToString(), _me.ManaPercent);
                }
                // now check if missing a shield and need to Twist
                else if ((uWaterStacks + uLightningStacks) == 0 || (atRest && (uWaterStacks + uLightningStacks) < 3))
                {
                    if (_lastShieldUsed != ShieldType.Water && trainedWaterShield)
                        shield = ShieldType.Water;
                    else
                        shield = ShieldType.Lightning;

                    Dlog("WhichShieldTypeNeeded:  {0} Shield on self due to Twist with Mana at {1:F1}%", shield.ToString(), _me.ManaPercent);
                }
            }

            return shield;
        }

        /*
         * ShieldTwisting()
         * 
         * Implement a technique known as shield twisting where
         * you alternate between a damage shield and a mana restoration 
         * shield.  basically make sure one or the other is active.
         * 
         * Here is the general approach in priority order:
         * 
         * If Mana is low, then force Mana restoration
         * If Mana is full, then force Damage shield
         * Otherwise alterante between shield types
         * 
         * This is a Level 20 and Higher Technique
         */
        private bool ShieldTwisting(bool atRest)
        {
            ShieldType shield = WhichShieldTypeNeeded(atRest);
            SpellWait sw = atRest ? SpellWait.Complete : SpellWait.NoWait;
            bool castShield = false;

            if (shield == ShieldType.None)
                return castShield;

            // if the shield type to use is Earth Shield and we are in RAF, it's meant for the tank
            if (IsRAF() && shield == ShieldType.Earth)
            {
                if (IsUnitInRange(GroupTank, 39))
                    castShield = Safe_CastSpell(GroupTank, shield.ToString() + " Shield", SpellRange.NoCheck, sw);

                return castShield;
            }

            // otherwise, cast shield on me
            castShield = Safe_CastSpell(shield.ToString() + " Shield", SpellRange.NoCheck, sw);
            if (castShield)
                _lastShieldUsed = shield;

            return castShield;
        }


        /*
         * Summary: inspects the list of WoWUnits for one that is within the
         *      maximum distance provided of the pt given.
         *          
         * Returns: true if clear for atleast that distance
         */
        private static bool CheckForSafeDistance(string reason, WoWPoint pt, double dist)
        {
            WoWUnit unitClose = null;
            Stopwatch timer = new Stopwatch();

            timer.Start();
            try
            {
                if (!IsPVP())
                    unitClose = (from o in ObjectManager.ObjectList
                                 where o is WoWUnit
                                 let unit = o.ToUnit()
                                 where pt.Distance(o.Location) < dist
                                     && unit.Attackable
                                     && (Safe_IsHostile(unit) || Safe_IsProfileMob(unit))
                                     && unit.IsAlive
                                 orderby unit.CurrentHealth ascending
                                 select unit
                                ).FirstOrDefault();
                else
                    unitClose = (from o in ObjectManager.ObjectList
                                 where o is WoWUnit
                                 let unit = o.ToUnit()
                                 where pt.Distance(o.Location) < dist
                                     && unit.IsPlayer
                                     && Safe_IsHostile(unit)
                                     && unit.IsAlive
                                 orderby unit.CurrentHealth ascending
                                 select unit
                                ).FirstOrDefault();

                if (unitClose == null)
                    Dlog("{0} CheckForSafeDistance({1:F1}): no hostiles/profile mobs in range - took {2} ms", reason, dist, timer.ElapsedMilliseconds);
                else
                    Dlog("{0} CheckForSafeDistance({1:F1}): saw {2}{3} - {4}[{5}] around {6:F0} yds away", // - took {6} ms",
                        reason,
                        dist,
                        (unitClose.IsTargetingMeOrPet ? "*" : ""),
                        unitClose.Class,
                        Safe_UnitName(unitClose),
                        unitClose.Level,
                        5 * Math.Round(pt.Distance(unitClose.Location) / 5)
                        //          , timer.ElapsedMilliseconds
                        );
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug("HB EXCEPTION in CheckForSafeDistance()");
                Logging.WriteException(e);
            }

            return unitClose == null;
        }


        private static Countdown timerNextList = new Countdown(4000);

        static Dictionary<int, string> dictPurgeables = new Dictionary<int, string>();

        public static void ListWowUnitsInRange()
        {
            Slog("\nID  DEBUFF-NAME\n-----------------------");
            foreach( KeyValuePair<int, string> dbf in dictPurgeables)
            {
                Slog("    <Spell Id=\"{0}\" Name=\"{1}\" />", dbf.Key, dbf.Value);
            }

            Slog(" ");
            return;

            // save this code for debugging later if needed
            // ----------------------------------------------
            if (!timerNextList.Done)
                return;

            timerNextList.Remaining = 4000;

            List<WoWUnit> adds = (from o in ObjectManager.ObjectList
                                  where o is WoWUnit
                                  let unit = o.ToUnit()
                                  where unit.Distance < 50
                                  select unit
                                        ).ToList();

            Slog("ADDLST  -- CURRENT WOWUNIT LIST -- {0} ENTRIES", adds.Count());
            foreach (WoWUnit unit in adds)
            {
                LogWowUnit(unit);
            }

            adds = (from o in ObjectManager.ObjectList
                    where o is WoWUnit
                    let unit = o.ToUnit()
                    where unit.Distance < 50
                    select unit
                        ).ToList();
            Slog("STUFF   --GUID           Target           SummonedBy       CreatedBy        CharmedBy         RootOwner         Owner             Name [Type] -------");
            foreach (WoWUnit unit in adds)
            {
                DumpWowUnit(unit);
            }

            adds = (from o in ObjectManager.ObjectList
                    where o != null && o is WoWUnit
                    let unit = o.ToUnit()
                    where unit.Distance < 50
                    select unit
                        ).ToList();
            Slog("ALL<50  --GUID           Target           SummonedBy       CreatedBy        CharmedBy         RootOwner         Owner             Name [Type] -------");
            // foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
            foreach (WoWUnit unit in adds)
            {
                // if ( unit != null && (IsMeOrMyStuff(unit) || IsTargetingMeOrMyStuff(unit)))
                DumpWowUnit(unit);
            }

        }

        public static void LogWowUnit(WoWUnit unit)
        {
            try
            {
                Slog("        {0}{1:F2} yds hostile:{2} attackable:{3}  {4}-{5} {6}[{7}] -->{8} | {9} | {10}| {11}| {12}",
                    IsTargetingMeOrMyStuff(unit) ? "X " : "  ",
                    unit.Distance,
                    Safe_IsHostile(unit),
                    unit.Attackable,
                    unit.CreatureType,
                    unit.Class,
                    unit.Name,
                    unit.Level,
                    !unit.GotTarget ? "has no target" : unit.CurrentTargetGuid == ObjectManager.Me.Guid ? "TARGETTING ME" : unit.CurrentTarget.Name,
                    ObjectManager.Me.CurrentTarget.Guid == unit.Guid ? "MY TARGET<---" : " ",
                    unit.Aggro ? "AGGRO<---" : "         ",
                    ObjectManager.Me.GotAlivePet && ObjectManager.Me.Pet.GotTarget && ObjectManager.Me.Pet.CurrentTargetGuid == unit.Guid ? "PET TARGET<---" : " ",
                    unit.PetAggro ? "PETAGGRO!" : "         "
                    );
            }
            catch
            {
            }
        }


        public static void DumpWowUnit(WoWUnit unit)
        {
            try
            {
                Slog("        {0:X16} {1:X16} {2:X16} {3:X16} {4:X16} {5:X16}  {6} [{7}]",
                    unit.Guid,
                    unit.GotTarget ? unit.CurrentTargetGuid : 0,
                    unit.SummonedByUnitGuid,
                    unit.CreatedByUnitGuid,
                    unit.CharmedByUnitGuid,
                    unit.SummonedUnitGuid,
                    unit.Name,
                    unit.CreatureType
                    );
            }
            catch { }
        }

        /*
         * Summary: inspects the objects within ranged to check if they are targeting me
         *          and hostile.  breaks down counts between ranged and melee targets
         *          
         * Returns: total number of hostiles fighting me
         */
        private readonly Stopwatch _addsTimer = new Stopwatch();
        public static List<WoWUnit> mobList = new List<WoWUnit>();

        private void CheckForAdds()
        {
            // ListWowUnitsInRange();

            Stopwatch timerCFA = new Stopwatch();
            timerCFA.Start();

            _countMeleeEnemy = 0;
            _count8YardEnemy = 0;
            _count10YardEnemy = 0;
            _countRangedEnemy = 0;
            _countAoe8Enemy = 0;
            _countAoe12Enemy = 0;
            _countFireNovaEnemy = 0;
            _countMobs = 0;

            // _distClosestEnemy = 9999.99;
            _OpposingPlayerGanking = false;
            _BigScaryGuyHittingMe = false;

            try
            {
                // List<WoWObject> longList = ObjectManager.ObjectList;
                // List<WoWUnit> mobList = ObjectManager.GetObjectsOfType<WoWUnit>(false);
                // if (_mobList == null || (_addsTimer.ElapsedMilliseconds > 5000 && !_me.Combat ) ) 
                {
                    if (IsPVP())
                    {
                        mobList = (from o in ObjectManager.ObjectList
                                   where o is WoWUnit && o.Distance <= _maxDistForRangeAttack
                                   let unit = o.ToUnit()
                                   where unit.IsAlive && unit.IsPlayer && unit.ToPlayer().IsHorde != ObjectManager.Me.IsHorde && !unit.IsPet
                                   // orderby o.Distance ascending
                                   select unit
                                    ).ToList();
                        Dlog("CheckForAdds():  PvP list built has {0} entries within {1:F1} yds", mobList.Count, _maxDistForRangeAttack);
                    }
                    else
                    {
                        mobList = (from o in ObjectManager.ObjectList
                                   where o is WoWUnit && o.Distance <= _maxDistForRangeAttack
                                   let unit = o.ToUnit()
                                   where unit.Attackable
                                       && unit.IsAlive
                                       && unit.Combat
                                       && (!unit.IsPlayer || unit.ToPlayer().IsHorde != _me.IsHorde)
                                       && !IsMeOrMyGroup(unit)
                                       && (IsTargetingMeOrMyGroup(unit) || unit.CreatureType == WoWCreatureType.Totem)
                                   select unit
                                    ).ToList();
                        Dlog("CheckForAdds():  PVE list built has {0} entries within {1:F1} yds", mobList.Count, _maxDistForRangeAttack);
                    }
                }

                if (mobList != null && mobList.Any())
                {
                    Dlog("CheckForAdds() can see:");
                    try
                    {
                        foreach (WoWUnit unit in mobList)
                        {
                            if (unit == null || !unit.IsAlive)  // check again incase one died since making list
                                continue;

                            if (unit.Distance < 5)
                                _countMeleeEnemy++;
                            else
                                _countRangedEnemy++;

                            if (unit.Distance < 8)     // special case for 8 yard checks
                                _count8YardEnemy++;

                            if (unit.Distance < 10)     // special case for 10 yard checks
                                _count10YardEnemy++;

                            if (_me.GotTarget)
                            {
                                if (unit.Location.Distance(_me.CurrentTarget.Location) <= 12)
                                {
                                    _countAoe12Enemy++;
                                    if (unit.Location.Distance(_me.CurrentTarget.Location) <= 8)
                                        _countAoe8Enemy++;
                                }
                            }

#if PRE_410_METHOD
                            if (TotemExist(TOTEM_FIRE) && _totem[TOTEM_FIRE].Location.Distance(unit.Location) < 10)
                                _countFireNovaEnemy++;
#else
                            if (null != GetAuraCreatedByMe(_me.CurrentTarget, "Flame Shock"))
                            {
                                int firenovaRange = (_hasGlyphOfFireNova ? 15*15 : 10*10 );
                                if ( _me.CurrentTarget.Location.DistanceSqr( unit.Location) < firenovaRange )
                                    _countFireNovaEnemy++;
                            }

#endif
                            if (unit.IsPlayer)
                            {
                                Dlog("  "
                                    + (!unit.GotTarget ? " " : unit.CurrentTarget.IsMe ? "*" : unit.IsTargetingPet ? "+" : IsTargetingMeOrMyStuff(unit) ? "@" : " ")
                                    + "PLAYER: (" + (unit.ToPlayer().IsHorde ? "H" : "A") + ") " + unit.Race + " " + unit.Class + " - " + Safe_UnitName(unit) + "[" + unit.Level + "]  dist: " + _me.Location.Distance(unit.Location).ToString("F2"));
                                _OpposingPlayerGanking = !IsPVP();
                                if (_OpposingPlayerGanking && (!_me.GotTarget || !_me.CurrentTarget.IsPlayer))
                                {
                                    Safe_SetCurrentTarget(unit);
                                }
                            }
                            else
                            {
                                string sType = "NPC";
                                if (Safe_IsProfileMob(unit))
                                {
                                    _countMobs++;
                                    sType = "MOB";
                                }

                                Dlog("  "
                                    + (!unit.GotTarget ? " " : unit.CurrentTarget.IsMe ? "*" : unit.IsTargetingPet ? "+" : IsTargetingMeOrMyStuff(unit) ? "@" : " ")
                                    + sType + ": " + unit.Class + " - " + Safe_UnitName(unit) + "[" + unit.Level + "]  dist: " + _me.Location.Distance(unit.Location).ToString("F2"));

                                if (Safe_IsElite( unit))
                                    _BigScaryGuyHittingMe = true;
                            }
                        }
                    }
                    catch (ThreadAbortException) { throw; }
                    catch (Exception e)
                    {
                        Log(Color.Red, "An Exception occured. Check debug log for details.");
                        Logging.WriteDebug("EXCEPTION in code doing CheckForAdds(1)");
                        Logging.WriteException(e);
                    }
                }
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug("HB EXCEPTION in CheckForAdds(2)");
                Logging.WriteException(e);
            }

            Dlog("   ## Total  {0}/{1} melee/ranged in Combat - CheckForAdds took {2} ms", _countMeleeEnemy, _countRangedEnemy, timerCFA.ElapsedMilliseconds);
            if (!IsRAF())
            {
                if (countEnemy > 1)
                    Slog(">>> MULTIPLE TARGETS:  " + _countMeleeEnemy + " melee,  " + _countRangedEnemy + " ranged");
                if (_BigScaryGuyHittingMe)
                    Slog(">>> BIG Scary Guy Hitting Me (elite or {0}+ levels)", cfg.PVE_LevelsAboveAsElite);
                if (_OpposingPlayerGanking)
                    Slog(">>> Opposing PLAYER is Attacking!!!!");
            }

            return;
        }

        private WoWUnit CheckForTotems()
        {
            Stopwatch timerCFA = new Stopwatch();
            timerCFA.Start();
            WoWUnit totem = null;

            totem = ObjectManager.GetObjectsOfType<WoWUnit>(false).Find(
            unit => unit != null
                && Safe_IsHostile(unit)
                && unit.IsAlive
                && unit.Distance <= Targeting.PullDistance
                && unit.CreatureType == WoWCreatureType.Totem
                );

            return totem;
        }



        #endregion

        #region RACIALS

        private bool Warstomp()
        {
            if (!SpellManager.HasSpell("War Stomp"))
                ;
            else if (_count8YardEnemy < 1)
                ;
            else if (Safe_CastSpell("War Stomp", SpellRange.NoCheck, SpellWait.NoWait))
            {
                Slog("War Stomp: BOOM!");
                return true;
            }

            return false;
        }

        private bool Stoneform()
        {

            if (IsPVP() || IsFightStressful())
            {
                if (!SpellManager.HasSpell("Stoneform"))
                    ;
                else if (Safe_CastSpell("Stoneform", SpellRange.NoCheck, SpellWait.NoWait))
                {
                    Slog("Stoneform: just put on some body armor!");
                    return true;
                }
            }

            return false;
        }

        private bool Berserking()
        {

            if (IsPVP() || IsFightStressful() || !cfg.PVE_SaveForStress_DPS_Racials)
            {
                if (!SpellManager.HasSpell("Berserking"))
                    ;
                else if (Safe_CastSpell("Berserking", SpellRange.NoCheck, SpellWait.NoWait))
                {
                    Slog("Berserking: just broke out a can of whoop a$$!");
                    return true;
                }
            }

            return false;
        }

        private bool BloodFury()
        {
            if (IsPVP() || IsFightStressful() || !cfg.PVE_SaveForStress_DPS_Racials)
            {
                if (!SpellManager.HasSpell("Blood Fury"))
                    ;
                else if (Safe_CastSpell("Blood Fury", SpellRange.NoCheck, SpellWait.NoWait))
                {
                    Slog("Blood Fury: just broke out a can of whoop a$$!");
                    return true;
                }
            }

            return false;
        }

        private bool GiftOfTheNaaru()
        {
            if (!SpellManager.HasSpell("Gift of the Naaru"))
                ;
            else if (Safe_CastSpell("Gift of the Naaru", SpellRange.NoCheck, SpellWait.NoWait))
            {
                Slog("Gift of the Naaru: it's good to be Draenei!");
                return true;
            }

            return false;
        }

        private bool Lifeblood()
        {
            if (!SpellManager.HasSpell("Lifeblood"))
                ;
            else if (Safe_CastSpell("Lifeblood", SpellRange.NoCheck, SpellWait.NoWait))
            {
                Slog("Lifeblood: the benefit of being a flower picker!");
                return true;
            }

            return false;
        }

        #endregion

        /*
		 * Totem Manager Declarations -- slot numbers correlate to LUA usage
		 */
        #endregion



        #region Item / Spell ID Lists

        /*
		 * Following list of id's come from:
		 * 
		 *      http://www.wowhead.com
		 * 
		 */

        // Potion EntryId's taken from WoWHead
        public static readonly List<uint> _potionHealthEID = new List<uint>()
		{
			//=== RESTORATION POTIONS (HEALTH AND MANA)
			40077,  // Crazy Alchemist's Potion 3500 (Alchemist)
			34440,  // Mad Alchemist's Potion 2750   (Alchemist)
			40087,  // Powerful Rejuvenation Potion 4125
			22850,  // Super Rejuvenation Potion 2300
			18253,  // Major Rejuvenation Potion 1760
			9144,   // Wildvine Potion 1500
			2456,   // Minor Rejuvenation Potion 150

			//=== HEALTH POTIONS 
			33447,  // Runic Healing Potion 4500

			43569,  // Endless Healing Potion  2500
			43531,  // Argent Healing Potion  2500
			32947,  // Auchenai Healing Potion  2500
			39671,  // Resurgent Healing Potion 2500
			22829,  // Super Healing Potion 2500
			33934,  // Crystal Healing Potion 2500
			23822,  // Healing Potion Injector 2500
			33092,  // Healing Potion Injector 2500

			31852,  // Major Combat Healing Potion 1750
			31853,  // Major Combat Healing Potion 1750
			31839,  // Major Combat Healing Potion 1750
			31838,  // Major Combat Healing Potion 1750
			13446,  // Major Healing Potion 1750
			28100,  // Volatile Healing Potion 1750 

			18839,  // Combat Healing Potion 900 
			3928,   // Superior Healing Potion 900

			1710,   // Greater Healing Potion  585

			929,    // Healing Potion  360

			4596,   // Discolored Healing Potion 180
			858,    // Lesser Healing Potion 180

			118     // Minor Healing Potion 90
			
		};

        // Mana Potion EntryId's taken from WoWHead
        public static readonly List<uint> _potionManaEID = new List<uint>()
		{
			//=== RESTORATION POTIONS (HEALTH AND MANA)
			40077,  // Crazy Alchemist's Potion 4400 (Alchemist)
			34440,  // Mad Alchemist's Potion 2750   (Alchemist)
			40087,  // Powerful Rejuvenation Potion 4125
			22850,  // Super Rejuvenation Potion 2300
			18253,  // Major Rejuvenation Potion 1760
			9144,   // Wildvine Potion 1500
			2456,   // Minor Rejuvenation Potion 150

			//=== MANA POTIONS 
		43570, // 3000 Endless Mana Potion 
		33448, // 4400 Runic Mana Potion
		40067, // 3000 Icy Mana Potion
		31677, // 3200 Fel Mana Potion
		33093, // 3000 Mana Potion Injector
		43530, // 3000 Argent Mana Potion
		32948, // 3000 Auchenai Mana Potion
		22832, // 3000 Super Mana Potion
		28101, // 2250 Unstable Mana Potion
		13444, // 2250 Major Mana Potion
		13443, // 1500 Superior Mana Potion
		 6149, // 900 Greater Mana Potion
		 3827, // 585 Mana Potion
		 3385, // 360 Lesser Mana Potion
		 2455, // 180 Minor Mana Potion
	};

        // Bandage EntryId's taken from WoWHead
        public static readonly List<uint> _bandageEID = new List<uint>()
		{
			// ID,  BANDAGE NAME,         (Level, Healing)
		34722,  // Heavy Frostweave,    (400, 5800)
		34721,  // Frostweave,      (350, 4800)
		21991,  // Heavy Netherweave,   (325, 3400)
		21990,  // Netherweave,     (300, 2800)
		14530,  // Heavy Runecloth, (225, 2000)
		14529,  // Runecloth,       (200, 1360)
		8545,   // Heavy Mageweave, (175, 1104)
		8544,   // Mageweave        (150, 800)
		6451,   // Heavy Silk,      (125, 640)
		6450,   // Silk         (100, 400)
		3531,   // Heavy Wool,      ( 75, 301)
			3530,   // Wool         ( 50, 161)
		2581,   // Heavy Linen,     ( 20, 114)
		1251,   // Linen        (  1, 66)
			
		};

        public static readonly HashSet<uint> _listFrostImmune = new HashSet<uint>()
	{
		24601,  // Steam Rager (65-71)
		17358,  // Fouled Water Spirit (18-19)
		3950,  // Minor Water Guardian (25)
		14269,  // Seeker Aqualon (21)
		3917,  // Befouled Water Elemental (23-25)
		10757,  // Boiling Elemental (27-28)
		10756,  // Scalding Elemental (28-29)
		2761,  // Cresting Exile (38-39)
		691,  // Lesser Water Elemental (35-37)
		5461,  // Sea Elemental (46-49)
		5462,  // Sea Spray (45-48)
		8837,  // Muck Splash (47-49))
		7132,  // Toxic Horror(53-54)
		14458,  // Watery Invader (56-58)
		20792,  // Bloodscale Elemental (62-63) 
		20090,  // Bloodscale Sentry (62-63)
		20079,  // Darkcrest Sentry (61-62)
		17153,  // Lake Spirit (64-65)
		17155,  // Lake Surger (64-66)
		17154,  // Muck Spawn (63-66)
		21059,  // Enraged Water Spirit (68-69)
		25419,  // Boiling Spirit (68-70)
		25715,  // Frozen Elemental (65-71)
		23919,  // Ice Elemental (64-69)
		24228,  // Iceshard Elemental (70-71)   
		26316,  // Crystalline Ice Elemental (73-74)
		16570,  // Crazed Water Spirit (71-76)
		28411,  // Frozen Earth (76-77)
		29436,  // Icetouched Earthrager (69-75)
		29844,  // Icebound Revenant (78-80)
		30633,  // Water Terror (77-78)
	};

        public static readonly HashSet<uint> _listFireImmune = new HashSet<uint>()
	{
		6073, // Searing Infernal
		4038, // Burning Destroyer
		4037, // Burning Ravager
		4036, // Rogue Flame Spirit
		2760, // Burning Exile
		5850, // Blazing Elemental
		5852, // Inferno Elemental
		5855, // Magma Elemental
		9878, // Entropic Beast
		9879, // Entropic Horror
		14460, // Blazing Invader
		6521, // Living Blaze
		6520, // SScorching Elemental       
		20514, // Searing Elemental
		21061, // Enraged Fire Spirit   
		29504, // Seething Revenant 
		6073, // Searing Infernal
		7136, // Infernal Sentry
		7135, // Infernal Bodyguard
		21419, // Infernal Attacker

		19261, // Infernal Warbringer
        25417, // Raging Boiler
	};

        public static readonly HashSet<uint> _listNatureImmune = new HashSet<uint>()
	{
		18062, // Enraged Crusher
		11577, // Whirlwind Stormwalker
		11578, // Whirlwind Shredder
		11576, // Whirlwind Ripper
		4661, // Gelkis Rumbler
		832, // Dust Devil      
		4034, // Enraged Stone Spirit
		4035, // Furious Stone Spirit
		4499, // Rok'Alim the Pounder
		9377, // Swirling Vortex
		4120, // Thundering Boulderkin
		2258, // Stone Fury
		2592, // Rumbling Exile
		2762, // Thundering Exile
		2791, // Enraged Rock Elemental
		2919, // Fam'retor Guardian
		2736, // GGreater Rock Elemental    
		2735, // Lesser Rock Elemental
		92, // Rock Elemental   
		8667, // Gusting Vortex
		9396, // Ground Pounder
		5465, // Land Rager
		9397, // Living Storm
		14462, // Thundering Invader
		11745, // Cyclone Warrior   
		11746, // Desert Rumbler
		11744, // Dust Stormer
		14455, // Whirling Invader  
		17158, // Dust Howler   
		18062, // Enraged Crusher
		17160, // Living Cyclone    
		17157, // Shattered Rumbler
		17159, // Storm Rager
		17156, // Tortured Earth Spirit
		18882, // Sundered Thunderer
		20498, // Sundered Shard
		21060, // Enraged Air Spirit
		22115, // Enraged Earth Shard
		21050, // Enraged Earth Spirit      
		25415, // Enraged Tempest
		24229, // Howling Cyclone   
		24340, // Rampaging Earth Elemental     
		26407, // Lightning Sentry
		28784, // Altar Warden
		29124, // Lifeblood Elemental   
		28858, // Storm Revenant
	};

        public static readonly HashSet<uint> _hashTremorTotemMobs = new HashSet<uint>()
	{
		31402,  // TEST ONLY!!!!
		31403,  // test only 
		31228,  // testonly

		// NPC Abilities Fear Abilities:  http://www.wowhead.com/spells=-8?filter=me=5;dt=1

		30284,  //  Ahn'kahet: Old Kingdom,   Bonegrinder
		2256,   //  Alterac Mountains,     Crushridge Enforcer
		19906,  //  Alterac Mountains,    Usha Eyegouge
		11947,  //  Alterac Valley,   Captain Galvangar
		30231,  //  Arathi Highlands,     Radulf Leder
		19905,  //  Arathi Highlands,     The Black Bride
		19908,  //  Ashenvale,    Su'ura Swiftarrow
		6116,   //  Azshara,      Highborne Apparition
		22855,  //  Black Temple,     Illidari Nightlord
        39700,  //  Blackrock Caverns, Beauty
		9018,   //  Blackrock Depths,      High Interrogator Gerstahn <Twilight's Hammer Interrogator>
		16059,  //  Blackrock Depths,     Theldren
		10162,  //  Blackrock Spire/Blackwing Lair,   Lord Victor Nefarius
		11583,  //  Blackrock Spire/Blackwing Lair,   Nefarian
		23353,  //  Blade's Edge Mountains,    Braxxus
		20735,  //  Blade's Edge Mountains,   Dorgok
		20889,  //  Blade's Edge Mountains,   Ethereum Prisoner (Group Energy Ball)
		22204,  //  Blade's Edge Mountains,   Fear Fiend
		23055,  //  Blade's Edge Mountains,   Felguard Degrader
		8716,   //  Blasted Lands,    Dreadlord
		17664,  //  Bloodmyst Isle,   Matis the Cruel <Herald�of�Sironas>
		32322,  //  Dalaran,      Gold Warrior
		32321,  //  Dalaran,      Green Warrior
		34988,  //  Darnassus,    Landuen Moonclaw
		34989,  //  Darnassus,    Rissa Shadeleaf
		14325,  //  Dire Maul,    Captain Kromcrush
		11455,  //  Dire Maul,    Wildspawn Felsworn
		14324,  //  Dire Maul North,      Cho'Rush the Observer
		27483,  //  Drak'Tharon Keep,     King Dred
		26830,  //  Drak'Tharon Keep,     Risen Drakkari Death Knight
		40195,  //  Durotar,      Mindless Troll
		1200,   //  Duskwood,      Morbent Fel
		202,    //  Duskwood,      Skeletal Horror
		12339,  //  Eastern Plaguelands,       Demetria <The Scarlet Oracle>
		8521,   //  Eastern Plaguelands,      Blighted Horror
		8542,   //  Eastern Plaguelands,      Death Singer
		8528,   //  Eastern Plaguelands,      Dread Wearer
		8600,   //  Eastern Plaguelands,      Plaguebat
		10938,  //  Eastern Plaguelands,      Redpath the Corrupted
		113,    //  Elwynn Forest,    Stonetusk Boar
		16329,  //  Ghostlands,   Dar'khan Drathir
		11445,  //  Gordok Captain,   
		21350,  //  Gruul's Lair,     Gronn-Priest
		28961,  //  Halls of Lightning,   Titanium Siegebreaker
		17000,  //  Hellfire Peninsula,   Aggonis
		17478,  //  Hellfire Peninsula,   Bleeding Hollow Scryer
		19424,  //  Hellfire Peninsula,   Bleeding Hollow Tormenter
		17014,  //  Hellfire Peninsula,   Collapsing Voidwalker
		2215,   //  Hillsbrad Foothills,      High Executor Darthalia
		17968,  //  Hyjal Summit,     Archimonde
		32278,  //  Icecrown,     Harbinger of Horror
		31222,  //  Icecrown,     Khit'rix the Dark Master
		31775,  //  Icecrown,     Thexal Deathchill
		37955,  //  Icecrown Citidel,      Blood-Queen Lana'thel
		34991,  //  Ironforge,    Borim Goldhammer
		17521,  //  Karazhan,     The Big Bad Wolf
		24558,  //  Magister's Terrace,   Ellrys Duskhallow
		24559,  //  Magister's Terrace,   Warlord Salaris
		11982,  //  Molten Core,       Magmadar
		17152,  //  Nagrand,       Felguard Legionnaire
		18870,  //  Netherstorm�,   Voidshrieker
		17833,  //  Old Hillsbrad Foothills,      Durnholde Warden
		34955,  //  Orgrimmar,    Karg Skullgore
		30610,  //  Orgrimmar,    War-Hunter Molog
		10508,  //  Ras Frostwhisperer,   Scholomance
		15391,  //  Ruins of Ahn'Qiraj,   Captain Qeez
		6490,   //  Scarlet Monestary,     Azshir the Sleepless
		4542,   //  Scarlet Monestary,    High Inquisitor Fairbanks
		10502,  //  Scholomance,      Lady Illucia Barov
		10470,  //  Scholomance,      Scholomance Neophyte
		8280,   //  Searing Gorge,     Shleipnarr
		18325,  //  Sethekk Halls,    Sethekk Prophet
		18796,  //  Shadow Labryinth,     Fel Overseer
		18731,  //  Shadow Labyrinth,     Ambassador Hellmaw
		19826,  //  Shadowmoon Valley,    Dark Conclave Shadowmancer
		21166,  //  Shadowmoon Valley,    Illidari Dreadlord
		22074,  //  Shadowmoon Valley,    Illidari Mind Breaker <The�Crimson�Sigil>
		22006,  //  Shadowmoon Valley,    Shadlowlord Deathwill
		21314,  //  Shadowmoon Valley,    Terrormaster
		15200,  //  Silithus�,      Twilight Keeper Mayna <Twilight's�Hammer>
		15308,  //  Silithus�,      Twilight Prophet <Twilight's�Hammer>
		40413,  //  Silvermoon City,      Alenjon Sunblade
		34998,  //  Stormwind City,   Alison Devay
		30578,  //  Stormwind City,   Bethany Aldire
		34997,  //  Stormwind City,   Devin Fardale
		20381,  //  Stormwind City,   Jovil
		1559,   //  Stranglethorn Vale,    King Mukla
		680,    //  Stranglethorn Vale,    Mosh'Ogg Lord
		2464,   //  Stranglethorn Vale,   Commander Aggro'gosh
		469,    //  Stranglethorn Vale,   Lieutenant Doren
		10812,  //  Stratholme,   Grand Crusader Dathrohan
		11143,  //  Stratholme,   Postmaster Malown
		16102,  //  Stratholme,   Sothos
		5271,   //  Sunken Temple,    Atal'ai Deathwalker
		25370,  //  Sunwell Plateau,      Sunblade Dusk Priest
		15311,  //  Temple of Ahn'Qiraj,      Anubisath Warder
		15543,  //  Temple of Ahn'Qiraj,      Princess Yaui
		15252,  //  Temple of Ahn'Qiraj,      Qiraji Champion
		23067,  //  Terokkar Forest,      Talonpriest Skizzik
		21200,  //  Terokkar Forrest,     Screeching Spirit
		18686,  //  Terrokar Forest,      Doomsayer Jurim
		20912,  //  The Arcatraz,     Harbinger Skyriss
		20875,  //  The Arcatraz,     Negaton Screamer
		3393,   //  The Barrens,      Captain Fairmount
		14781,  //  The Barrens,      Captain Shatterskull
		3338,   //  The Barrens,      Sergra Darkthorn
		21104,  //  The Black Morass,     Rift Keeper
		642,    //  The Deadmines,     Sneed's Shredder <Lumbermaster>
		30581,  //  The Exodar,   Buhurda
		34987,  //  The Exodar,   Hunara
		20118,  //  The Exodar,   Jihi
		34986,  //  The Exodar,   Liedel the Just
		20119,  //  The Exodar,   Mahul
		20382,  //  The Exodar,   Mitia
		35027,  //  The Exotar,   Erutor
		36497,  //  The Forge of Souls,   Bronjahm
		12496,  //  The Hinterlands,      Dreamtracker
		26798,  //  The Nexus,    Commander Kolurg
		26796,  //  The Nexus,    Commander Stoutbeard
		17694,  //  The Shattered Halls,      Shadowmoon Darkcaster
		16809,  //  The Shattered Halls,      Warbringer O'mrogg
		17957,  //  The Slave Pens,   Coilfang Champion
		17801,  //  The Steamvault,   Coilfang Siren
		1663,   //  The Stockade,     Dextren Ward
		34466,  //  Trial of the Crusader,    Anthar Forgemender <Priest>
		34473,  //  Trial of the Crusader,    Brienna Nightfell <Priest>
		34447,  //  Trial of the Crusader,    Caiphus the Stern <Priest>
		34450,  //  Trial of the Crusader,    Harkzog
		34474,  //  Trial of the Crusader,    Serissa Grimdabbler
		34441,  //  Trial of the Crusader,    Vivienne Blackwhisper <Priest>
		33515,  //  Ulduar,   Auriaya
		33818,  //  Ulduar,   Twilight Adherent
		34983,  //  Undercity,    Deathstalker Fane
		347,    //  Undercity,    Grizzle Halfmane
		2804,   //  Undercity,    Kurden Bloodclaw
		20386,  //  Undercity,    Lyrlia Blackshield
		35021,  //  Undercity,    Marog
		31531,  //  Undercity,    Perfidious Dreadlord
		32391,  //  Undercity,    Perfidious Dreadlord
		30583,  //  Undercity,    Sarah Forthright
		9167,   //  Un'Goro Crater,    Frenzied Pterrordax
		9166,   //  Un'Goro Crater,   Pterrordax
		26696,  //  Utgarde Pinnacle,      Ymirjar Berserker
		5056,   //  Wailing Caverns,       Deviate Dreadfang
		3654,   //  Wailing Caverns,       Mutanus the Devourer
		1785,   //  Western Plaguelands,      Skeletal Terror
		10200,  //  Winterspring,      Rak'shiri
		24246,  //  Zul'Aman,     Darkheart
		24239,  //  Zul'Aman,     Hex Lord Malacrass
		7275,   //  Zul'Farrak,    Shadowpriest Sezz'ziz
		11830,  //  Zul'Gurub,    Hakkari Priest
		14517,  //  Zul'Gurub,    High Priestess Jeklik
		11359,  //  Zul'Gurub,    Soulflayer
	};

        public static readonly HashSet<int> _hashTrinkCombat = new HashSet<int>()
        {
            92226	, // 	call of conquest
            84969	, // 	call of conquest
            92225	, // 	Call of Dominance
            84968	, // 	Call of Dominance
            92224	, // 	call of victory
            84966	, // 	call of victory
            67683	, // 	celerity 
            91173	, // 	celerity 
            91136	, // 	Leviathan
            91135	, // 	Leviathan
            92071	, // 	Nimble
            91352	, // 	Polarization
            91351	, // 	Polarization
            91828	, // 	thrill of victory
            91340	, // 	Typhoon
            91341	, // 	Typhoon
            67684   , //    Hospitality
            60521   , //    Winged Talisman
            60305   , //    Heart of a Dragon
            73551   , //    Figurine Jewel Serpent
            46567   , //    Goblin Rocket Launcher
            55039   , //    Gnomish Lightning Generator
            82645   , //    Elementium Dragonling
            92357   , //    Memory of Invincibility
            92200   , //    Blademaster
            92199   , //    Blademaster

            92123   , //    Enigma
            73549   , //    Demon Panther Figurine
            73550   , //    Earthen Guardian - Figurine
            73522   , //    King of Boars
            91019   , //    Soul Power

            91374   , //    Battle Prowess
            91376   , //    Battle Prowess
            92098   , //    Speed of Thought
            92099   , //    Speed of Thought
            92336   , //    Summon Fallen Footman
            92337   , //    Summon Fallen Grunt
            90900   , //    Focus
            92188   , //    Master Tactician
            95875   , //    Heartsparked
            90889   , //    fury of the earthen
            91345   , //    favored
            95874   , //    searing words
            95870   , //    lightning in a bottle
            95877   , //    la-la's song
            91344   , //    battle!

        };

        public static readonly HashSet<int> _hashTrinkPVP = new HashSet<int>()
        {
            42292	, // 	PVP Trinket
        };

        public static readonly HashSet<int> _hashTrinkMana = new HashSet<int>()
        {
            91155	, // 	Expansive Soul
            73552   , //    Figurine Jewel Owl
            92272   , //    Collecting Mana - Tyrande's Favorite Doll
            92601   , //    Detonate Mana - Tyrande's Favorite Doll
            95872   , //    undying flames
        };

        public static readonly HashSet<int> _hashTrinkHealth = new HashSet<int>()
        {
            55915	, // 	tremendous fortitude
            84960	, // 	tremendous fortitude
            44055	, // 	tremendous fortitude
            55917	, // 	tremendous fortitude
            67596   , //    tremendous fortitude
            92223	, // 	tremendous fortitude
            89181	, // 	mighty earthquake
            92186   , //    amazing fortitude
            92187   , //    amazing fortitude
            92172   , //    great fortitude
            33828   , //    talisman of the alliance
            32140   , //    Talisman of the Horde
        };

        public static readonly HashSet<string> _hashTinkerCombat = new HashSet<string>()
        {
            "Hyperspeed Accelerators",
            "Hand-Mounted Pyro Rocket",
            "Reticulated Armor Webbing",
            "Quickflip Deflection Plates",
            "Synapse Springs",
            "Tazik Shocker",
            "Grounded Plasma Shield"
        };

        public static readonly List<string> _enchantElemental = new List<string>
	    {
		    "Flametongue Weapon",
		    "Windfury Weapon",
		    "Rockbiter Weapon",
		    "Frostbrand Weapon",
	    };

        public static readonly List<string> _enchantEnhancementPVE_Mainhand = new List<string>
	    {
		    "Windfury Weapon",
		    "Flametongue Weapon",
		    "Rockbiter Weapon",
		    "Frostbrand Weapon"
	    };

        public static readonly List<string> _enchantEnhancementPVE_Offhand = new List<string>
	    {
		    "Flametongue Weapon",
		    "Windfury Weapon",
		    "Rockbiter Weapon",
		    "Frostbrand Weapon"
	    };

        public static readonly List<string> _enchantEnhancementPVP_Mainhand = new List<string>
	    {
		    "Windfury Weapon",
		    "Flametongue Weapon",
		    "Rockbiter Weapon",
		    "Frostbrand Weapon"
	    };

        public static readonly List<string> _enchantEnhancementPVP_Offhand = new List<string>
	    {
		    "Frostbrand Weapon",
		    "Flametongue Weapon",
		    "Windfury Weapon",
		    "Rockbiter Weapon"
	    };

        public static readonly List<string> _enchantResto = new List<string>
	    {
		    "Earthliving Weapon",
		    "Flametongue Weapon",
		    "Windfury Weapon",
		    "Rockbiter Weapon",
		    "Frostbrand Weapon"
	    };

        public static readonly HashSet<string> _hashCC = new HashSet<string>()
        {
            "Polymorph",
            "Mind Control",
            "Shackle Undead",
            "Repentance", 
            "Enslave Demon",
            "Banish",
            "Hex",
            "Bind Elemental",
            "Sap",
            "Hibernate"
        };

        public static HashSet<int> _hashCleanseBlacklist = new HashSet<int>();
        public static HashSet<int> _hashPurgeWhitelist = new HashSet<int>();

        #endregion


        // public override bool WantButton { get { return true; } }
        public override bool WantButton
        { get { return true; } }

#if    BUNDLED_WITH_HONORBUDDY

		public override void OnButtonPress()
		{
            DialogResult rc = MessageBox.Show(
                "This CC does not have a configuration"     +Environment.NewLine +
                "window, but you can modify the .config"      +Environment.NewLine +
                "file to change settings."                  +Environment.NewLine +
                ""                                           +Environment.NewLine +
                "Click OK and Windows will open the file"   +Environment.NewLine +
                "containing the settings for this Shaman"    +Environment.NewLine +
                "using the program setup as the default"  + Environment.NewLine +
                "for .config files on your system."       + Environment.NewLine +
                ""   + Environment.NewLine +
                "If none is setup, Windows will display"   + Environment.NewLine +
                "a Window letting you choose one.  Just"   + Environment.NewLine +
                "select Notepad.exe or a text editor.",
                Name, 
                MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Information
                );

			if ( rc == DialogResult.OK )
                Process.Start("\"" + ConfigFilename + "\"");
		}
  
#else
        private ConfigForm _frm;

        public override void OnButtonPress()
        {
            if (_frm == null)
                _frm = new ConfigForm();

            Dlog(" About to show dialog");
            System.Windows.Forms.DialogResult rc = _frm.ShowDialog();
            if (rc == System.Windows.Forms.DialogResult.OK)
            {
                lastCheckConfig = true;
                cfg.DebugDump();
                try
                {
                    cfg.Save(ConfigFilename);
                    Log("Options saved to ShamWOW-realm-char.config");
                    hsm = new HealSpellManager();
                    hsm.Dump();
                }
                catch (ThreadAbortException) { throw; }
                catch (Exception e)
                {
                    Log(Color.Red, "An Exception occured. Check debug log for details.");
                    Logging.WriteDebug("EXCEPTION Saving to ShamWOW-realm-char.config");
                    Logging.WriteException(e);
                }

                // TotemSetupBar(); // just for debug atm
                _needTotemBarSetup = true;
                _needClearWeaponEnchants = true;
            }
        }
#endif

    }

    /// <summary>
    ///  class Countdown
    ///  
    ///  provides a Countdown timer for easily checking whether a specified number of
    ///  milliseconds has elapsed.
    /// </summary>
    public class Countdown
    {
        private Stopwatch s = new Stopwatch();
        private int timeExpire;

        public Countdown()
        {
            timeExpire = 0;
        }
        public Countdown(int ms)
        {
            StartTimer(ms);
        }

        public bool Done
        {
            get { return timeExpire <= s.ElapsedMilliseconds; }
        }

        public int Remaining
        {
            get
            {
                return Math.Max(0, timeExpire - (int)s.ElapsedMilliseconds);
            }
            set
            {
                StartTimer(value);
            }
        }

        public void StartTimer(int ms)
        {
            timeExpire = ms;
            s.Reset();
            s.Start();
        }

        public int ElapsedMilliseconds
        {
            get { return (int)s.ElapsedMilliseconds; }
        }
    }

    class Mob
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int HitBox { get; set; }

        public Mob(int id, string name, int melee)
        {
            Id = id;
            Name = name;
            HitBox = melee;       
        }
    }
}
