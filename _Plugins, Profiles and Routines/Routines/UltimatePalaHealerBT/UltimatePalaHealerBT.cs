//Inspired by Sm0k3d CC for paladin healer
//Inspired by Sm0k3d and Gilderoy UpaCC
//UPaCCBT the BehaviourTree Ultimate Paladin Healer Custom Class
//A bilion thanks go to Sm0k3d for his exellent work on the paladin healing class

//aggiungere lifeblood
//addon trinket?
//mount up dopo buff in pvp
//FIXME se compagno muore in arena arrendersi o passare a solo mode
//FIXME non si possono passare argomenti, solo usare variabili globali!
//FIXME Cambio del tank in arena
//FIXME per il PVP potrei fare se sono in combattimento e non ho nessuna bless casta la king (contro i dispell)
//FIXME riaggiungere Hand of Freedom anche per slow oltre che per snare
//FIXME se sto in Solo e me.inparty || me.inraid richiamare create behaviour, stessacosa al contrartio
//dal nome ottenere la posizione nell'array interno dei raid memeber di wow
//Quando uso un trinket devo controllare: che non sia passivo, che sia utilizzabile, che non sia in cooldown, che sia del tipo giusto

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using UltimatePaladinHealerBT.Talents;
using TreeSharp;
using Action = TreeSharp.Action;
namespace UltimatePaladinHealerBT
{
    public partial class UltimatePalaHealerBT : CombatRoutine
    {
        public static UltimatePalaHealerBT Instance = new UltimatePalaHealerBT();
        private WoWUnit lastCast = null;
        private WoWPlayer fallbacktank = null;
        private WoWPlayer tank;
        private WoWUnit Enemy;
        private Random rng;
        private WoWPlayer x;
        private WoWPlayer tar;
        private WoWPlayer mtank;
        private WoWUnit Epet;
        private WoWPlayer BlessTarget;
        private WoWPlayer RessTarget;
        private WoWPlayer CleanseTarget;
        private WoWPlayer UrgentCleanseTarget;
        private WoWPlayer UrgentHoFTarget;
        private WoWPlayer luatank;
        private WoWPlayer focustank;
        private WoWPlayer focus;
        public List<string> GUIDlist;
        public string FormattedGUID;
        public ulong FinalGuid;

        private string lastBehaviour = null;
        private string actualBehaviour = null;
        private string usedBehaviour = null;
        private double maxAOEhealth = 85;
        private double dontHealAbove = 95;

        private string lastbless = null;
        private bool Global_chimaeron_p1 = false;
        private bool Global_chimaeron = false;
        private bool Global_SoD = false;
        private int Global_Judgment_range = 0; //for me..
        private bool Global_debug
        { get 
            {
                switch (myclass)
                { 
                    case 0:
                        return UPaHBTSetting.Instance.WriteLog;
                    case 1:
                        return UPrHBTSetting.Instance.WriteLog;
                    default:
                        return true;
                }
            } 
        }
        private int Talent_last_word = 1;                      //0 1 or 2 point in the talent Last Word
        private bool _should_king;
        private string fallbacktankname;
        private string urgentdebuff;
        private int tryes;
        private bool specialhealing_warning=false;
        int i,j;
        private bool selective_resetted = false;
        public string[] NameorRM = new string[41];
        public string[] OrganizedNames = new string[41];
        public string[] WoWnames = new string[41];
        public int[] Raidsbugroup = new int[41];
        public int[] Raidorder = new int[41];
        public bool[] healornot = new bool[41];
        public int[] check_aoe = new int[41];
        public int SB1C;
        public int SB2C;
        public int SB3C;
        public int SB4C;
        public int SB5C;
        public int myclass{
            get
            {
                if (Me.Class == WoWClass.Priest)
                {
                    return 1;
                }
                else if (Me.Class == WoWClass.Paladin)
                {
                    return 0;
                }
                else
                {
                    slog("my class is {0}", Me.Class);
                    return -1;
                }
            }
        }
        public static double allcasttaked=0;
        public static int how_many_cast=0;
        public static double max_allcasted = 0;
        public static double this_casted;
        public static double medium_time_to_decide;

        public WoWUnit CastingSpellTarget { get; set; }
        public string LastSpellCast { get; set; }
        public string LastSpell { get; set; }

        public WoWItem Trinket1;
        public WoWItem Trinket2;

        public Composite _combatBehavior;
        private Composite _pullBehavior;
        public Composite _restBehavior;
        public uint Latency { get { return StyxWoW.WoWClient.Latency; } }
        public override Composite CombatBehavior { get { return _combatBehavior; } }
        public override Composite CombatBuffBehavior { get { return _combatBehavior;/*_combatBuffsBehavior;*/ } }
        public override Composite HealBehavior { get { return _combatBehavior;/*_healBehavior;*/ } }
        public override Composite PreCombatBuffBehavior { get { return _combatBehavior;/*_preCombatBuffsBehavior;*/ } }
        public override Composite PullBehavior { get { return _pullBehavior; } }
        public override Composite PullBuffBehavior { get { return _combatBehavior;/*_pullBuffsBehavior*/ } }
        public override Composite RestBehavior { get { return _restBehavior; } }

        public string[] RaidNames = new string[41];
        public bool[] Raidrole = new bool[41];
        public int[] Subgroup = new int[41];

        public List<WoWPlayer> NearbyFriendlyPlayers { get { return ObjectManager.GetObjectsOfType<WoWPlayer>(true, true).Where(p => p.DistanceSqr <= 40 * 40 && p.IsFriendly).ToList(); } }
        public List<WoWPlayer> NearbyFarFriendlyPlayers { get { return ObjectManager.GetObjectsOfType<WoWPlayer>(true, true).Where(p => p.DistanceSqr <= 70 * 70 && p.IsFriendly && p.IsInMyPartyOrRaid).ToList(); } }
        public List<WoWPlayer> NearbyUnFriendlyPlayers { get { return ObjectManager.GetObjectsOfType<WoWPlayer>(false, false).Where(p => p.DistanceSqr <= 40 * 40 && !p.IsInMyPartyOrRaid && !p.Dead).ToList(); } }
        
        public List<WoWUnit> NearbyUnfriendlyUnits
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => /*p.IsHostile && */!p.IsFriendly && !p.Dead && /*!p.IsPet &&*/ p.DistanceSqr <= 40 * 40).
                        ToList();
            }
        }
        public List<WoWPlayer> PartyorRaid { get { if (InParty()) { return Me.PartyMembers; } else if (InRaid()) { return Me.RaidMembers; } else { return null; } } }
        
        private static Stopwatch sw = new Stopwatch();
        private static Stopwatch Trinket1_sw = new Stopwatch();
        private static Stopwatch Trinket2_sw = new Stopwatch();
        private static Stopwatch select_heal_watch = new Stopwatch();
        private static Stopwatch combatfrom = new Stopwatch();
        private static Stopwatch noncombatfrom = new Stopwatch();
        private static Stopwatch subgroupSW = new Stopwatch();
        private static Stopwatch performance = new Stopwatch();

        private string version = "1.4";
        private string revision = "242";

        public override sealed string Name
        {
            get
            {
                if (Me.Class == WoWClass.Priest)
                {
                    return "UltimateHolyPriestHealerBT v " + version + " revision " + revision;
                }
                else
                {
                    return "UltimatePalaHealerBT v " + version + " revision " + revision;
                }

            }
        }

        public override WoWClass Class
        {
            get
            {
                if (Me.Class == WoWClass.Priest)
                {
                    return WoWClass.Priest;
                }
                else
                {
                    return WoWClass.Paladin;
                }
            }
        }

        public static LocalPlayer Me { get { return ObjectManager.Me; } }
        private readonly SpecManager Talent = new SpecManager();
        public override void Initialize()
        {
            tryes2 = 0;
            Instance = this;
            while (myclass < 0)
            {
                slog("Your class is not pala nor priest, insteas is {0} tryes {1} \n I'm retyng to undersand your class but if you are not a pala or priest you should not be here", Me.Class,tryes2);
                tryes2++;
                Thread.Sleep(1000);
                ObjectManager.Update();
            }
            if (myclass == 0)
            {
                slog(Color.Orange, "Hello Executor!\n I\'m UPaHCCBT and i\'m here to assist you keeping your friend alive\n You are using UPaHCCBT version {0} revision {1}", version, revision);
                Global_Judgment_range = (int)SpellManager.Spells["Judgement"].MaxRange;
                slog(Color.Orange, "Your Judgment range is {0} yard! will use this value", Global_Judgment_range);
                _can_dispel_disease = true;
                _can_dispel_magic = true;
                _can_dispel_poison = true;
            }
            else if (myclass == 1)
            {
                slog(Color.Orange, "Hello Executor!\n I\'m UPrHCCBT and i\'m here to assist you keeping your friend alive\n You are using UPrHCCBT version {0} revision {1}", version, revision);
                _can_dispel_disease = false;
                _can_dispel_magic = true;
                _can_dispel_poison = false;
            }

            AttachEventHandlers();
            if (!CreateBehaviors())
            {
                return;
            }
        }

        public bool CreateBehaviors()
        {
            tryes = 0;
            while (Me==null || !Me.IsValid || Me.Dead)
            {
                slog("i'm not valid, still on loading schreen {0}", tryes);
                tryes++;
                Thread.Sleep(1000);
                ObjectManager.Update();
            }
            if (!unitcheck(Me) && !Me.IsGhost)
            {
                if (unitcheck(ObjectManager.Me))
                {
                    slog("Me is not valid but OBJM.Me yes, we have a problem here..");
                }
                else
                {
                    slog("nor me nor OBJ.Me are valid, i shouldn't be here..");
                }
            }
            else if(!Me.IsGhost)
            {
                slog("All green! building behaviours now!");
            } else if (Me.IsGhost)
            {
                slog("So, i'm a Ghost, let's wait..");
            }
            Beahviour();
            tryes = 0;
            while (usedBehaviour == "WTF are you doing?")
            {
                tryes++;
                slog("No Valid Behaviour found, tryand again! that's try number {0}", tryes);
                if (unitcheck(Me))
                {
                    slog("party {0} raid {1} instance {2} pvpstatus {3} valid {4} behaviour {5}", Me.IsInParty, Me.IsInRaid, Me.IsInInstance, actualBehaviour, Me.IsValid, usedBehaviour);
                }
                else
                {
                    slog("i'm not valid, still on loading schreen and behaviour=WTF");
                }
                Thread.Sleep(1000);
                ObjectManager.Update();
                Beahviour();
            }
            slog(Color.HotPink, "{0}", usedBehaviour);
            if (myclass == 0)
            {
                UPaHBTSetting.Instance.Load();
                if (!selective_resetted) { UPaHBTSetting.Instance.Selective_Healing = false; selective_resetted = true; slog("Starting up, resetting selective healing to false"); }
                UPaHBTSetting.Instance.General_Stop_Healing = false;
                Load_Trinket();
                Inizialize_Trinket();
                Variable_inizializer();
                _combatBehavior = Composite_Selector();
                _restBehavior = Composite_Rest_Selector();
                _pullBehavior = Composite_Pull_Selector();
                Variable_Printer();
                slog(Color.HotPink, "{0}", usedBehaviour);
                UPaHBTSetting.Instance.Save();
            }
            else if (myclass == 1)
            {
                UPrHBTSetting.Instance.Load();
                if (!selective_resetted) { UPrHBTSetting.Instance.Selective_Healing = false; selective_resetted = true; slog("Starting up, resetting selective healing to false"); }
                Priest_Variable_inizializer();                              //priest
                switch (Talent.Spec)
                { 
                    case SpecManager.SpecList.None: //Lowbie Rotation, let'S take originals
                        _combatBehavior = Composite_Priest_Selector();
                        _restBehavior = Composite_Priest_Rest_Selector();
                        _pullBehavior = Composite_Priest_Pull_Selector();
                        break;
                    case SpecManager.SpecList.TreeOne: //DiscPriest, new behaviors
                        _combatBehavior = Composite_Priest_Selector(); //replace later
                        _restBehavior = Composite_Priest_Rest_Selector(); //replace later
                        _pullBehavior = Composite_Priest_Pull_Selector(); //replace later
                        break;
                    case SpecManager.SpecList.TreeTwo: //HolyPriest, originals
                        _combatBehavior = Composite_Priest_Selector();
                        _restBehavior = Composite_Priest_Rest_Selector();
                        _pullBehavior = Composite_Priest_Pull_Selector();
                       break;
                    case SpecManager.SpecList.TreeThree: //Shadow, originals, remember ... this is a HEALING CC
                        _combatBehavior = Composite_Priest_Selector();
                        _restBehavior = Composite_Priest_Rest_Selector();
                        _pullBehavior = Composite_Priest_Pull_Selector();
                        break;
                }

                Priest_Variable_Printer();
                slog(Color.HotPink, "{0}", usedBehaviour);
                UPrHBTSetting.Instance.Save();
            }
            lastBehaviour = usedBehaviour;
            return true;
        }
        public override bool WantButton { get { return true; } }

        private Form _configForm;
        public override void OnButtonPress()
        {
            Inizialize_variable_for_GUI();
            if (usedBehaviour == "Raid" && _selective_healing) { BuildSubGroupArray(); }
            if (_configForm == null || _configForm.IsDisposed || _configForm.Disposing)
            {
                if (Me.Class == WoWClass.Paladin)
                {
                    _configForm = new UPaHCCBTConfigForm();
                }
                else if (Me.Class == WoWClass.Priest)
                {
                    _configForm = new UPrHCCBTConfigForm();
                }
            }
            if (_configForm != null)
            {
                try
                {
                    _configForm.ShowDialog();
                }
                catch(Exception e)
                {
                    slog(Color.Red,"Exception Thrown (Calling ConfigForm): {0}",e);
                }
            }
        }

    }
}
