using System;
using System.ComponentModel;
using System.IO;
using Styx.Helpers;
using Styx;

namespace EzRet
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
           string[] Words = M.Split('-');
           Spell = Words[0];
           Trigger = float.Parse(Words[1]);
        }

        public override string ToString()
        {
            return Spell + "-" + Trigger;
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
           string[] Words = M.Split('-');
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
            return TrinketName+"-"+StackName+"-"+StackNumber+"-"+Trigger;
        }
    }

    public class EzRetSettings : Settings
    {
        public static readonly EzRetSettings Instance = new EzRetSettings();

        public EzRetSettings(): base(Path.Combine(Logging.ApplicationPath, string.Format(@"EzPVP/EzRet/{0}.xml", StyxWoW.Me.Name)))
        {

            if (HealRotation == null || HealRotation.Length == 0)
            {
                //HealRotation = new Act[] { new Act("Divine Protection", 60.0f), new Act("Lay on Hands", 20.0f), new Act("Word of Glory", 10.0f) };
                HealRotation = new String[] { new Act("Divine Protection", 60.0f).ToString(), 
                    new Act("Lay on Hands", 20.0f).ToString(), 
                    new Act("Word of Glory", 10.0f).ToString() };
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
        [DisplayName("SealName")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [Styx.Helpers.DefaultValue("Seal of Truth")]
        public string DPSSeal { get; set; }


        [Category("Tweaks")]
        [DisplayName("Disable Inquisition")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        public bool DisableInq { get; set; }

        [Category("Tweaks")]
        [DisplayName("Inquisition refresh time")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [Styx.Helpers.DefaultValue(6)]
        public int InqRefreshDuration { get; set; }


        //[Category("Item Types")]
        [DisplayName("DPS Rotation")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [Styx.Helpers.DefaultValue(new string[] { "Hammer of Justice", "Crusader Strike", "Holy Wrath", "Judgement" })]
        public string[] Rotation { get; set; }

        //[Category("Item Types")]
        [DisplayName("Kick Rotation")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [Styx.Helpers.DefaultValue(new string[] { "Rebuke", "Repentance", "Arcane Torrent" })]
        public string[] KickRotation { get; set; }

        //[Category("Item Types")]
        [DisplayName("DPS Cooldowns")]
        //[Description("Toggles if AutoEquip should equip purple items.")]
        [Setting]
        [Styx.Helpers.DefaultValue(new string[] { "Avenging Wrath", "Guardian of Ancient Kings" })]
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