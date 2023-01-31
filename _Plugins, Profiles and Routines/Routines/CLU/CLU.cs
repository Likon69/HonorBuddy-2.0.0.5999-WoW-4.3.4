using System;
using System.Drawing;
using System.Linq;

using CLU.Classes;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

// Credits
//-------------
// * cowdude for his initial work with Felmaster and giving me inspiration to create this CC.
// * All the Singular Developers For there tiresome work and some supporting code
// * bobby53 For his Disclaimer (Couldn't have said it better)
// * Kickazz006 for his BlankProfile.xml
// * Shaddar & Mentally for thier initial detection code of HourofTwilight, FadingLight and Shrapnel
// * bennyquest for his continued efforts in reporting issues!
// * Jamjar0207 for his Protection Warrior Rotation and Fire Mage
// * gniegsch for his Arms Warrior Rotation
// * Stormchasing for his help with warlocks and patchs
// * Obliv For his Boomkin, Frost Mage & Assassination rotations and Arms warrior improvement
// * ShinobiAoshi for his initial Affliction warlock rotation
// * fluffyhusky for his initial Enhancement Shaman rotation
// * Digitalmocking for his initial Elemental Shaman rotation

namespace CLU
{
    using System.Timers;

    using global::CLU.GUI;
    using global::CLU.Helpers;

    public class CLU : CombatRoutine
    {
        private static readonly CLU CLUInstance = new CLU();

        /// <summary>
        /// An instance of the CLU class
        /// </summary>
        public static CLU Instance { get { return CLUInstance; } }

        private Timer keybindTimer; // A timer for keybinds

        /// <summary>
        /// Write debug messages to the log
        /// </summary>
        private static bool debugOn;

        private static RotationBase rotationBase;

        public static readonly Version Version = new Version(2, 5, 3);

        public override string Name { get { return "CLU (Codified Likeness Utility) " + Version; } }

        public override WoWClass Class { get { return StyxWoW.Me.Class; } }

        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        public override void OnButtonPress()
        {
            Configuration.Display(this);
        }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// writes messages to the log file and the client UI
        /// </summary>
        /// <param name="msg">the message to write to the log</param>
        /// <param name="args">the arguments that accompany the message</param>
        public void Log(string msg, params object[] args)
        {
            if (msg != null)
            {
                Logging.Write(Color.Yellow, "[CLU] " + Version + ": " + msg, args);
            }
        }

        /// <summary>
        /// writes debug messages to the log file (false by default)
        /// </summary>
        /// <param name="msg">the message to write to the log</param>
        /// <param name="args">the arguments that accompany the message</param>
        public static void DebugLog(string msg, params object[] args)
        {
            if (msg != null && debugOn)
            {
                Logging.WriteDebug(String.Format("[CLU] " + Version + ": " + msg, args));
            }
        }

        /// <summary>
        /// Returns the string "Player" if the unit name is equal to our name.
        /// </summary>
        /// <param name="unit">the unit to check</param>
        /// <returns>a safe name for the log</returns>
        public static string SafeName(WoWUnit unit)
        {
            return (unit.Name == Me.Name) ? "Flynn Lives" : "some random!";
        }

        public override void Initialize()
        {
               // Sound.InitializeSound();

                keybindTimer = new Timer(2000); // Set
                keybindTimer.Elapsed += KeybindTimerElapsed; // Attatch
                keybindTimer.Enabled = true; // Enable

                // Check for LazyRaider and Disable movement
                if (BotChecker.Instance.BotBaseInUse("LazyRaider"))
                {
                    SettingsFile.Instance.HandleMovement = false;
                    this.Log(" [BotChecker] LazyRaider Detected. *MOVEMENT DISABLED*");
                }

                // Check for Raid Bot and Disable movement
                if (BotChecker.Instance.BotBaseInUse("Raid Bot"))
                {
                    SettingsFile.Instance.HandleMovement = false;
                    this.Log(" [BotChecker] Raid Bot Detected. *MOVEMENT DISABLED*");
                }

                // Check for Combat Bot and Disable movement
                if (BotChecker.Instance.BotBaseInUse("Combat Bot"))
                {
                    SettingsFile.Instance.HandleMovement = false;
                    this.Log(" [BotChecker] Combat Bot Detected. *MOVEMENT DISABLED*");
                }

                // Check for BGBuddy and MultiDotting
                if (BotChecker.Instance.BotBaseInUse("BGBuddy"))
                {
                    SettingsFile.Instance.HandleMultiDotting = false;
                    this.Log(" [BotChecker] BGBuddy Bot Detected. *MULTI-DOTTING DISABLED*");
                }

                ///////////////////////////////////////////////////////////////////
                //// PLEASE SET debugOn = true;
                //// TO UPLOAD YOUR LOG
                ///////////////////////////////////////////////////////////////////
                debugOn = false;

                foreach (WoWSpell racial in Spell.CurrentRacials)
                {
                    this.Log(" [Racial Abilitie] {0} ", racial.Name);
                }

               // If required HandleCombatLogEvent attach LUA events here
               BotEvents.OnBotStarted += WoWStats.Instance.WoWStatsOnStarted;
               BotEvents.OnBotStopped += WoWStats.Instance.WoWStatsOnStopped;
            
                // For debug purposes.
                // CLU.DebugLog("======================DUMPING SPELLS===========================");
                // Helpers.Spell.DumpSpells();
                // CLU.DebugLog("======================DUMPING AURAS===========================");
                // Helpers.Buff.DumpAuras();
                // CLU.DebugLog("===============================================================");
        }

        private static void KeybindTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Keybinds.Pulse();
        }

        public override void Pulse()
        {
            PetManager.Pulse();
            
            // var locks = CombatLogEvents.Instance.DumpSpellLocks();
            // foreach (var x in locks)
            // {
            //    this.Log("Key: {0} Value: {1} HasDebuff: {2}", x.Key, x.Value.ToString(), Buff.TargetHasDebuff(x.Key) ? "Up" : "Down");
            // }
        }

        /// <summary>
        /// Determine which group we are in if any.
        /// </summary>
        private string Group
        {
            get
            {
                if (Me.IsInParty)
                    return "PARTY";
                if (Me.IsInRaid)
                    return "RAID";
                return "SOLO";
            }
        }

        /// <summary>
        /// Depending on if we are grouped or solo, and if we are in a dungeon or battleground, return the rotation we will use.
        /// </summary>
        private Composite Rotation
        {
            get
            {
                Composite rotation;
                switch (this.Group)
                {
                    case "PARTY":
                    case "RAID":
                        rotation = Battlegrounds.IsInsideBattleground
                                       ? this.ActiveRotation.PVPRotation
                                       : this.ActiveRotation.PVERotation;
                        break;

                    default:
                        rotation = this.ActiveRotation.SingleRotation;
                        break;
                }

                return new Sequence(
                    new DecoratorContinue(x => SettingsFile.Instance.HandleMovement, Movement.MovingFacingBehavior()),
                    new DecoratorContinue(x => Me.CurrentTarget != null, rotation));
            }
        }

        /// <summary>
        /// Combat heal/buffs Behavior
        /// </summary>
        private Composite Medic
        {
            get
            {
                return this.ActiveRotation.Medic;
            }
        }

        /// <summary>
        /// Pre Combat buffs Behavior
        /// </summary>
        private Composite PreCombat
        {
            get
            {
                return this.ActiveRotation.PreCombat;
            }
        }

        /// <summary>
        ///  Setting the HB base behaviours.
        /// </summary>
        public override Composite PullBehavior
        {
            get
            {
                return this.Rotation;
            }
        }

        public override double? PullDistance
        {
            get
            {
                return this.ActiveRotation.CombatMaxDistance;
            }
        }

        public override Composite CombatBehavior
        {
            get
            {
                return this.Rotation;
            }
        }

        public override Composite CombatBuffBehavior
        {
            get
            {
                return this.Medic;
            }
        }

        public override Composite PreCombatBuffBehavior
        {
            get
            {
                return this.PreCombat;
            }
        }

        public override Composite RestBehavior
        {
            get
            {
                return this.PreCombat;
            }
        }

        /// <summary>
        /// If we havnt loaded a rotation for our character then do so by querying our character's class tree based on a Keyspell.
        /// </summary>
        public RotationBase ActiveRotation
        {
            get
            {
                if (rotationBase == null)
                {
                    this.QueryClassTree();
                }

                return rotationBase;
            }
        }

        /// <summary>This will: loop assemblies,
        /// loop types,
        /// filter types that are a subclass of RotationBase and not the abstract,
        /// create an instance of a RotationBase subclass so we can interigate KeySpell within the RotationBase subclass,
        /// Check if the character has the Keyspell,
        /// Set the active rotation to the matching RotationBase subclass.</summary>
        private void QueryClassTree()
        {
            var type = typeof (RotationBase);
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsSubclassOf(type) && !p.IsAbstract);

            foreach (var x in types)
            {
                var constructorInfo = x.GetConstructor(new Type[] { });
                if (constructorInfo != null)
                {
                    var rb = constructorInfo.Invoke(new object[] { }) as RotationBase;
                    if (rb != null && SpellManager.HasSpell(rb.KeySpell))
                    {
                        DebugLog(" Using " + rb.Name + " rotation. Character has " + rb.KeySpell);
                        this.SetActiveRotation(rb);
                    }
                    else
                    {
                        if (rb != null)
                            DebugLog(" Skipping " + rb.Name + " rotation. Character is missing " + rb.KeySpell);
                    }
                }
            }
        }

        /// <summary>
        /// sets the character's active rotation.
        /// Print the help located in the specific rotation
        /// </summary>
        /// <param name="rb">an instance of the rotationbase</param>
        private void SetActiveRotation(RotationBase rb)
        {
            this.Log(" Greetings, level {0} user!", Me.Level);
            this.Log(" I am CLU.");
            this.Log(" I will create the perfect system for you.");
            this.Log(" I suggest we use the " + rb.Name + " rotation.");
            this.Log(" as I know you have " + rb.KeySpell);
            this.Log(" BotBase: {0}  ({1})", BotManager.Current.Name, BotChecker.Instance.bots.Contains(BotManager.Current.Name) ? "Supported" : "Currently Not Supported");

            Logging.Write(Color.Red, " ========= ATTENTION USER ===================");
            Logging.Write(Color.Red, " If my internal algorithms are not computing");
            Logging.Write(Color.Red, " please contact my creator - wulf");
            Logging.Write(Color.Red, " Please be sure to attatch my log");
            Logging.Write(Color.Red, " as I record information to help resolve my issues.");
            Logging.Write(Color.Red, " NO LOG = NO SUPPORT");
            Logging.Write(Color.Red, " ========================================");

            this.Log(rb.Help);
            this.Log(" Let's execute the plan!");
            this.Log(" Checkout the keybinds tab in the GUI");
            rotationBase = rb;
        }
    }
}
