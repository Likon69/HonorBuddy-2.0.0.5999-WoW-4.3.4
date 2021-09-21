#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: highvoltz $
// $Date: 2012-05-16 21:54:19 +0300 (Çar, 16 May 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/PriestSettings.cs $
// $LastChangedBy: highvoltz $
// $LastChangedDate: 2012-05-16 21:54:19 +0300 (Çar, 16 May 2012) $
// $LastChangedRevision: 631 $
// $Revision: 631 $

#endregion

using System;
using System.ComponentModel;

using Styx.Helpers;

using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace Singular.Settings
{
    internal class PriestSettings : Styx.Helpers.Settings
    {
        public PriestSettings()
            : base(SingularSettings.SettingsPath + "_Priest.xml")
        {
        }

        #region Shadow

        [Setting]
        [DefaultValue(30)]
        [Category("Shadow")]
        [DisplayName("Flash Heal Health")]
        [Description("Health for Flash Heal for shadow spec")]
        public double ShadowFlashHealHealth { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Shadow")]
        [DisplayName("Mind Flay Mana")]
        [Description("Will only use Mind Flay while manapercent is above this value")]
        public double MindFlayMana { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Shadow")]
        [DisplayName("Shield Health Percent")]
        [Description("Use PW:Shield on set HP. This will only be used for Mind Spike rotation")]
        public double ShieldHealthPercent { get; set; }

        [Setting]
        [DefaultValue(15)]
        [Category("Shadow")]
        [DisplayName("Mind Blast Timer")]
        [Description("Casts mind blast anyway after this many seconds if we haven't got 3 shadow orbs")]
        public int MindBlastTimer { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Shadow")]
        [DisplayName("Mind Blast Orbs")]
        [Description("Casts mind blast once we get (at least) this many shadow orbs")]
        public int MindBlastOrbs { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Shadow")]
        [DisplayName("Devouring Plague First")]
        [Description("Casts devouring plague before anything else, useful for farming low hp mobs")]
        public bool DevouringPlagueFirst { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Shadow")]
        [DisplayName("Archangel on 5")]
        [Description("Always archangel on 5 dark evangelism, ignoring mana %")]
        public bool AlwaysArchangel5 { get; set; }

        [Setting]
        [DefaultValue(20)]
        [Category("Shadow")]
        [DisplayName("Dispersion Mana")]
        [Description("Dispersion will be used when mana percentage is less than this value")]
        public int DispersionMana { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Shadow")]
        [DisplayName("Healing Spells Health")]
        [Description("Won't attempt to use healing spells unless below this health percent")]
        public int DontHealPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Shadow")]
        [DisplayName("No Shadowform Below Heal")]
        [Description("Won't attempt to re-enter shadowform while below healing spells health threshold")]
        public bool DontShadowFormHealth { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Shadow")]
        [DisplayName("Psychic Horror Adds")]
        [Description("Attempt to psychic horror adds")]
        public bool UsePsychicHorrorAdds { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Shadow")]
        [DisplayName("Psychic Horror Interrupt")]
        [Description("Attempt to psychic horror target as interrupt (on top of silence)")]
        public bool UsePsychicHorrorInterrupt { get; set; }

        #endregion

        #region Common

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Shield Pre-Pull")]
        [Description("Use PW:Shield pre-pull. This setting is not used in Mind Spike rotation")]
        public bool UseShieldPrePull { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Shadow Protection")]
        [Description("Use Shadow Protection buff")]
        public bool UseShadowProtection { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Psychic Scream")]
        [Description("Use Psychic Scream")]
        public bool UsePsychicScream { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Common")]
        [DisplayName("Use Psychic Scream Count")]
        [Description("Uses Psychic Scream when there's >= these number of adds (not including current target)")]
        public int PsychicScreamAddCount { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Fear Ward")]
        [Description("Use Fear Ward buff")]
        public bool UseFearWard { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Inner Fire")]
        [Description("Use Inner Fire, otherwise uses Inner Will")]
        public bool UseInnerFire { get; set; }

        [Setting]
        [DefaultValue(75)]
        [Category("Common")]
        [DisplayName("Archangel Mana")]
        [Description("Archangel will be used at this value")]
        public int ArchangelMana { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Common")]
        [DisplayName("Shadowfiend Mana")]
        [Description("Shadowfiend will be used at this value")]
        public int ShadowfiendMana { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Common")]
        [DisplayName("Hymn of Hope Mana")]
        [Description("Hymn of Hope will be used at this value")]
        public int HymnofHopeMana { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Wand")]
        [Description("Uses wand if we're oom")]
        public bool UseWand { get; set; }

        #endregion

        #region Discipline

        [Setting]
        [DefaultValue(30)]
        [Category("Discipline")]
        [DisplayName("Penance Health")]
        [Description("Penance will be used at this value")]
        public int Penance { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Discipline")]
        [DisplayName("Flash Heal Health")]
        [Description("Flash Heal will be used at this value")]
        public int FlashHeal { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Discipline")]
        [DisplayName("Greater Heal Health")]
        [Description("Greater Heal will be used at this value")]
        public int GreaterHeal { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Discipline")]
        [DisplayName("Heal Health")]
        [Description("Heal will be used at this value")]
        public int Heal { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Discipline")]
        [DisplayName("Renew Health")]
        [Description("Renew will be used at this value")]
        public int Renew { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Discipline")]
        [DisplayName("Pain Suppression Health")]
        [Description("Pain Suppression will be used at this value")]
        public int PainSuppression { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Discipline")]
        [DisplayName("Binding Heal Self Health")]
        [Description("Binding Heal will be used when your health is below this value")]
        public int BindingHealMe { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Discipline")]
        [DisplayName("Binding Heal Other Health")]
        [Description("Binding Heal will be used when someone elses health is below this value")]
        public int BindingHealThem { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Discipline")]
        [DisplayName("Prayer of Healing Health")]
        [Description("Prayer of Healing will be used at this value")]
        public int PrayerOfHealing { get; set; }


        [Setting]
        [DefaultValue(80)]
        [Category("Discipline")]
        [DisplayName("Power Word: Shield Health")]
        [Description("Power Word: Shield will be used at this value")]
        public int PowerWordShield { get; set; }

        [Setting]
        [DefaultValue(3)]
        [Category("Discipline")]
        [DisplayName("Prayer of Healing Count")]
        [Description("Prayer of Healing will be used when count of players whom health is below PoH Health setting mets this value")]
        public int PrayerOfHealingCount { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Discipline")]
        [DisplayName("Dps Mana")]
        [Description("Dps while mana is above this value (Used while in a party)")]
        public int DpsMana { get; set; }

        #endregion

        #region Holy

        [Setting]
        [DefaultValue(95)]
        [Category("Holy")]
        [DisplayName("Heal Health")]
        [Description("Heal will be used at this value")]
        public int HolyHeal { get; set; }

        [Setting]
        [DefaultValue(50)]
        [Category("Holy")]
        [DisplayName("Greater Heal Health")]
        [Description("Greater Heal will be used at this value")]
        public int HolyGreaterHeal { get; set; }

        [Setting]
        [DefaultValue(25)]
        [Category("Holy")]
        [DisplayName("Flash Heal Health")]
        [Description("Flash Heal will be used at this value")]
        public int HolyFlashHeal { get; set; }

        [Setting]
        [DefaultValue(40)]
        [Category("Holy")]
        [DisplayName("Divine Hymn Health")]
        [Description("Divine Hymn will be used at this value")]
        public int DivineHymnHealth { get; set; }

        [Setting]
        [DefaultValue(6)]
        [Category("Holy")]
        [DisplayName("Divine Hymn Count")]
        [Description("Divine Hymn will be used when this many heal targets below the Divine Hymn Health percent")]
        public int DivineHymnCount { get; set; }

        [Setting]
        [DefaultValue(80)]
        [Category("Holy")]
        [DisplayName("Prayer of Healing with Serendipity Health")]
        [Description("Prayer of Healing with Serendipity will be used at this value")]
        public int PrayerOfHealingSerendipityHealth { get; set; }

        [Setting]
        [DefaultValue(4)]
        [Category("Holy")]
        [DisplayName("Prayer of Healing with Serendipity Count")]
        [Description("Prayer of Healing with Serendipity will be used when this many heal targets below the Prayer of Healing with Serendipity Health percent")]
        public int PrayerOfHealingSerendipityCount { get; set; }

        [Setting]
        [DefaultValue(95)]
        [Category("Holy")]
        [DisplayName("Circle of Healing Health")]
        [Description("Circle of Healing will be used at this value")]
        public int CircleOfHealingHealth { get; set; }

        [Setting]
        [DefaultValue(5)]
        [Category("Holy")]
        [DisplayName("Circle of Healing Count")]
        [Description("Circle of Healing will be used when this many heal targets below the Circle of Healing Health percent")]
        public int CircleOfHealingCount { get; set; }

        #endregion
    }
}