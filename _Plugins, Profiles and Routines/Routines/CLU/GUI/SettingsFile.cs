using Styx.Helpers;
using Styx.WoWInternals;

namespace CLU.GUI
{
    using global::CLU.Helpers;

    public class SettingsFile : Settings
    {
        private static readonly SettingsFile MySettings = new SettingsFile();

        private SettingsFile()
            : base("./Settings/CLU_" + ObjectManager.Me.Name + ".xml")
        {
            this.Load();
        }

        public static SettingsFile Instance
        {
            get
            {
                return MySettings;
            }
        }

        [Setting]
        [DefaultValue(true)]
        public bool HandleCooldowns { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Handleextraactionbutton { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleSpriestRotSelector { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleAoE { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleRaidPartyBuff { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleInterrupts { get; set; }
        
        [Setting]
        [DefaultValue(true)]
        public bool HandleHealing { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleBaneOfHavoc { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleMultiDotting { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleWarriorShout { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleClientLag { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool HandleTotems { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool PauseRotation { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool HandleMovement { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindCooldownManagement { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindDsExtraButtonClick { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindAoEManagement { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindRaidPartyBuffManagement { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindHealDefensiveManagement { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindMultiDottingManagement { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindInteruptManagement { get; set; }

        [Setting]
        [DefaultValue("")]
        public string KeybindPauseRotation { get; set; }
        
        [Setting]
        [DefaultValue(PoisonType.Instant)] // TODO: make a GUI selection
        public PoisonType MHPoison { get; set; }

        [Setting]
        [DefaultValue(PoisonType.Deadly)] // TODO: make a GUI selection
        public PoisonType OHPoison { get; set; }

        [Setting]
        [DefaultValue(PoisonType.Wound)] // TODO: make a GUI selection
        public PoisonType ThrownPoison { get; set; }


        /// <summary>
        /// GUI Setting for Automaticly handling Cooldowns (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageCooldowns { get { return HandleCooldowns; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Dragon soul Extra button clicks (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoExtraActionButton { get { return Handleextraactionbutton; } }

        /// <summary>
        /// GUI Setting for Selecting the default rotation or the T13 MindSpike rotation (Default=True/MindSpike=False)
        /// </summary>
        public bool SpriestRotationSelector { get { return HandleSpriestRotSelector; } }

        /// <summary>
        /// GUI Setting for Automaticly handling AoE such as DnD, Multishot, MindSear, HellFire, etc (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageAoE { get { return HandleAoE; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Raid and Party Buffing such as Kings, MoTW, Fortitude, etc (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageRaidPartyBuff { get { return HandleRaidPartyBuff; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Interrupts such as Kick, Rebuke, Mind Freeze, etc (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageInterrupts { get { return HandleInterrupts; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Healing such as Vampiric Blood, Lay on Hands, Healthstones, etc (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageHealing { get { return HandleHealing; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Bane of Havoc Target (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageBaneOfHavoc { get { return HandleBaneOfHavoc; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Multi-Dotting Targets (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageMultiDotting { get { return HandleMultiDotting; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Warrior Shout (Commanding Shout=True/Battle Shout=False)
        /// </summary>
        public bool AutoManageWarriorShout { get { return HandleWarriorShout; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Client Lag (Latency) (Default=True/Detect=False)
        /// </summary>
        public bool AutoManageClientLag { get { return HandleClientLag; } }

        /// <summary>
        /// GUI Setting for Automaticly handling Totems (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManageTotems { get { return HandleTotems; } }

        /// <summary>
        /// setting for stopping main rotations (Automatic=True/Manual=False)
        /// </summary>
        public bool AutoManagePauseRotation { get { return PauseRotation; } }

        /// <summary>
        /// Toggles Movement on and Off (Yes=True/No=False)
        /// </summary>
        public bool AutoHandleMovement { get { return HandleMovement; } }

        // Keybinds -----------------------------------------------------------------------------

        /// <summary>
        /// GUI Setting for Keybind Cooldown Management
        /// </summary>
        public string KBindCooldownManagement { get { return KeybindCooldownManagement; } }

        /// <summary>
        /// GUI Setting for Keybind  Ds Extra Button Click
        /// </summary>
        public string KBindDsExtraButtonClick { get { return KeybindDsExtraButtonClick; } }

        /// <summary>
        /// GUI Setting for Keybind AoE Management
        /// </summary>
        public string KBindAoEManagement { get { return KeybindAoEManagement; } }

        /// <summary>
        /// GUI Setting for Keybind Raid Party Buff Management
        /// </summary>
        public string KBindRaidPartyBuffManagement { get { return KeybindRaidPartyBuffManagement; } }

        /// <summary>
        /// GUI Setting for Keybind Heal Defensive Management
        /// </summary>
        public string KBindHealDefensiveManagement { get { return KeybindHealDefensiveManagement; } }

        /// <summary>
        /// GUI Setting for Keybind MultiDotting Management
        /// </summary>
        public string KBindMultiDottingManagement { get { return KeybindMultiDottingManagement; } }

        /// <summary>
        /// GUI Setting for Keybind Interupt Management
        /// </summary>
        public string KBindInteruptManagement { get { return KeybindInteruptManagement; } }

        /// <summary>
        /// GUI Setting for Keybind Pause Rotation
        /// </summary>
        public string KBindPauseRotation { get { return KeybindPauseRotation; } }
    }
}
