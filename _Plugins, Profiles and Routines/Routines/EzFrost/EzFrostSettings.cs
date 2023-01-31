using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Styx.Helpers;
using Styx.Logic.Inventory;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx;
namespace EzFrost
{



    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Act
    {
        [Description("Name of the ability to use.")]
        public string Spell
        {
            get;
            set;
        }
        [Description("Health to use the ability at.")]
        public float Trigger
        {
            get;
            set;
        }

        public Act()
        {
        }
        public Act(String s, float t)
        {
            Spell = s;
            Trigger = t;
        }
        public Act(String M)
        {
            string[] Words = M.Split('|');
            Spell = Words[0];
            Trigger = float.Parse(Words[1]);
        }

        public override string ToString()
        {
            return Spell + "|" + Trigger;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Trinket
    {
        [Description("Name of the trinket to use.")]
        public string TrinketName
        {
            get;
            set;
        }
        [Description("Name of the stacking buff you get while using this trinket.")]
        public string StackName
        {
            get;
            set;
        }
        [Description("How many stacks you should use the trinket at.")]
        public int StackNumber
        {
            get;
            set;
        }
        [Description("Use this trinket when enemy is at what health.")]
        public float Trigger
        {
            get;
            set;
        }
        public Trinket(String s, String sn, int snn, float t)
        {
            TrinketName = s;
            StackName = sn;
            StackNumber = snn;
            Trigger = t;
        }

        public Trinket(String M)
        {
            string[] Words = M.Split('|');
            TrinketName = Words[0];
            StackName = Words[1];
            if (StackName == "")
            {
                StackName = null;
            }
            StackNumber = Int32.Parse(Words[2]);
            Trigger = float.Parse(Words[3]);

        }

        public Trinket()
        {
        }
        public override string ToString()
        {
            return TrinketName + "|" + StackName + "|" + StackNumber + "|" + Trigger;
        }
    }

    public class EzFrostSettings : Settings
    {
        public static readonly EzFrostSettings Instance = new EzFrostSettings();

        public EzFrostSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"EzPVP/EzFrost/{0}.xml", StyxWoW.Me.Name)))
        {

            if (HealRotation == null || HealRotation.Length == 0)
            {
                //HealRotation = new Act[] { new Act("Divine Protection", 60.0f), new Act("Lay on Hands", 20.0f), new Act("Word of Glory", 10.0f) };
                HealRotation = new String[] { new Act("Lichborne", 30.0f).ToString()
                    , new Act("Icebound Fortitude", 50.0f).ToString(),
                    new Act("Anti-Magic Shell", 60.0f).ToString() };
            }

            if (Trinkets == null || Trinkets.Length == 0)
            {
                //Trinkets = new Trinket[] { new Trinket("Vicious Gladiator's Badge of Victory", null, 0, 100.0f), new Trinket("Essence of the Eternal Flame", null, 0, 100.0f), new Trinket("Apparatus of Khaz'goroth", "Titanic Power", 5, 60.0f), new Trinket("Fury of Angerforge", "Raw Fury", 5, 60.0f) };
                Trinkets = new String[] { new Trinket("Badge of Victory", null, 0, 100.0f).ToString(),
                    new Trinket("Essence of the Eternal Flame", null, 0, 100.0f).ToString(),
                    new Trinket("Apparatus of Khaz'goroth", "Titanic Power", 5, 60.0f).ToString(),
                    new Trinket("Fury of Angerforge", "Raw Fury", 5, 60.0f).ToString() };

            }



        }







        [Category("Tweaks")]
        [Description("At what percent should we stop using lichborne to heal ourself.")]
        [DisplayName("Lichborne Heal")]
        [Setting]
        [DefaultValue(50.0f)]
        public float LichborneHealStop { get; set; }

        [Category("Tweaks")]
        [Description("At what percent should we use a healthstone? Set to 0 for manual control for arena.")]
        [DisplayName("Healthstone")]
        [Setting]
        [DefaultValue(35.0f)]
        public float HealthStone { get; set; }

        [Category("Tweaks")]
        [Description("Use Unholy Presence? If set to false, bot will not change what presence your in.")]
        [DisplayName("Unholy Presence")]
        [Setting]
        [DefaultValue(true)]
        public bool UnholyPresence { get; set; }


        [Category("Tweaks")]
        [Description("Use Empower Rune Weapon?")]
        [DisplayName("Empower Rune Weapon")]
        [Setting]
        [DefaultValue(true)]
        public bool EmpowerRuneWeapon { get; set; }

        //[Category("Item Types")]
        [DisplayName("DPS Rotation")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [DefaultValue(new string[] { "Necrotic Strike", "Howling Blast", "Frost Strike" })]
        public string[] Rotation { get; set; }

        //[Category("Item Types")]
        [DisplayName("Ranged Rotation")]
        [Description("Remove deathgrip if you dont want it to use it.")]
        [Setting]
        [DefaultValue(new string[] { "Death Grip", "Howling Blast", "Death Coil" })]
        public string[] RangedRotation { get; set; }


        //[Category("Item Types")]
        [DisplayName("Kick Rotation")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [DefaultValue(new string[] { "Mind Freeze", "Strangulate", "Arcane Torrent" })]
        public string[] KickRotation { get; set; }

        //[Category("Item Types")]
        [DisplayName("DPS Cooldowns")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [DefaultValue(new string[] { "Horn of Winter", "Blood Tap", "Outbreak", "Pillar of Frost" })]
        public string[] Cooldowns { get; set; }


        //[Category("Item Types")]
        [DisplayName("Don't Touch")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [ReadOnly(true)]
        public String[] Trinkets { get; set; }

        [DisplayName("DPS Trinkets")]
        public Trinket[] _Trinkets { get; set; }



        //[Category("Item Types")]
        [DisplayName("Don't Touch")]
        [Setting]
        [ReadOnly(true)]
        public String[] HealRotation { get; set; }

        //[Category("Item Types")]
        [DisplayName("Heal Rotation")]
        public Act[] _HealRotation { get; set; }

    }

}