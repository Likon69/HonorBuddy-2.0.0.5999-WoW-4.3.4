#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: raphus $
// $Date: 2012-02-06 08:18:55 +0200 (Pzt, 06 Şub 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/DeathKnightSettings.cs $
// $LastChangedBy: raphus $
// $LastChangedDate: 2012-02-06 08:18:55 +0200 (Pzt, 06 Şub 2012) $
// $LastChangedRevision: 587 $
// $Revision: 587 $

#endregion

using System;
using System.ComponentModel;

using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;

using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace Singular.Settings
{
    internal class DeathKnightSettings : Styx.Helpers.Settings
    {
        public DeathKnightSettings()
            : base(SingularSettings.SettingsPath + "_DeathKnight.xml")
        {
        }

        #region Common

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use Death and Decay")]
        public bool UseDeathAndDecay { get; set; }

        [Setting]
        [DefaultValue(2)]
        [Category("Common")]
        [DisplayName("Death and Decay Add Count")]
        [Description("Will use Death and Decay when agro mob count is equal to or higher then this value. This basicly determines AoE rotation")]
        public int DeathAndDecayCount { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Icebound Fortitude")]
        public bool UseIceboundFortitude { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Common")]
        [DisplayName("Icebound Fortitude Percent")]
        public int IceboundFortitudePercent { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Common")]
        [DisplayName("Death Strike Emergency Percent")]
        public int DeathStrikeEmergencyPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Lichborne")]
        public bool UseLichborne { get; set; }

        #endregion

        #region Category: Blood
        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Anti-Magic Shell")]
        public bool UseAntiMagicShell { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Blood")]
        [DisplayName("Army of the Dead")]
        public bool UseArmyOfTheDead { get; set; }

        [Setting]
        [DefaultValue(20)]
        [Category("Blood")]
        [DisplayName("Army of the Dead Percent")]
        public int ArmyOfTheDeadPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Bone Shield")]
        public bool UseBoneShield { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Bone Shield Exclusive")]
        public bool BoneShieldExclusive { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Dancing Rune Weapon")]
        public bool UseDancingRuneWeapon { get; set; }
        
        [Setting]
        [DefaultValue(30)]
        [Category("Blood")]
        [DisplayName("Empower Rune Weapon Percent")]
        public int EmpowerRuneWeaponPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Icebound Fortitude Exclusive")]
        public bool IceboundFortitudeExclusive { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Lichborne Exclusive")]
        public bool LichborneExclusive { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("Lichborne Percent")]
        public int LichbornePercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Pet Sacrifice")]
        public bool UsePetSacrifice { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Pet Sacrifice Exclusive")]
        public bool PetSacrificeExclusive { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("Pet Sacrifice Summon Percent")]
        public int PetSacrificeSummonPercent { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("Pet Sacrifice Percent")]
        public int PetSacrificePercent { get; set; }
        
        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Vampiric Blood")]
        public bool UseVampiricBlood { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Blood")]
        [DisplayName("Vampiric Blood Exclusive")]
        public bool VampiricBloodExclusive { get; set; }

        [Setting]
        [DefaultValue(60)]
        [Category("Blood")]
        [DisplayName("Vampiric Blood Percent")]
        public int VampiricBloodPercent { get; set; }
        #endregion

        #region Category: Frost

        [Setting]
        [DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Pillar of Frost")]
        public bool UsePillarOfFrost { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Raise Dead")]
        public bool UseRaiseDead { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Frost")]
        [DisplayName("Empower Rune Weapon")]
        public bool UseEmpowerRuneWeapon { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Frost")]
        [DisplayName("Use Necrotic Strike - Frost")]
        public bool UseNecroticStrike { get; set; }

        #endregion

        #region Category: Unholy

        [Setting]
        [DefaultValue(true)]
        [Category("Unholy")]
        [DisplayName("Summon Gargoyle")]
        public bool UseSummonGargoyle { get; set; }

        #endregion

    }
}