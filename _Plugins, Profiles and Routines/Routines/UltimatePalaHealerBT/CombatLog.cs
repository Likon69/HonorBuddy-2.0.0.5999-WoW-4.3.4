/*Taken from Singular V1 CC big thanks to Apoc for his help with th BehaviourTree :)*/

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace UltimatePaladinHealerBT
{
    internal class CombatLogEventArgs : LuaEventArgs
    {
        public CombatLogEventArgs(string eventName, uint fireTimeStamp, object[] args)
            : base(eventName, fireTimeStamp, args)
        {
        }

        public double Timestamp { get { return (double)Args[0]; } }

        public string Event { get { return Args[1].ToString(); } }

        public ulong SourceGuid { get { return ulong.Parse(Args[3].ToString().Replace("0x", ""), NumberStyles.HexNumber); } }

        public WoWUnit SourceUnit
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, true).FirstOrDefault(
                        o => o.IsValid && (o.Guid == SourceGuid || o.DescriptorGuid == SourceGuid));
            }
        }

        public string SourceName { get { return Args[4].ToString(); } }

        public int SourceFlags { get { return (int)(double)Args[5]; } }

        public ulong DestGuid { get { return ulong.Parse(Args[6].ToString().Replace("0x", ""), NumberStyles.HexNumber); } }

        public WoWUnit DestUnit
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, true).FirstOrDefault(
                        o => o.IsValid && (o.Guid == DestGuid || o.DescriptorGuid == DestGuid));
            }
        }

        public string DestName { get { return Args[7].ToString(); } }

        public int DestFlags { get { return (int)(double)Args[8]; } }

        public int SpellId { get { return (int)(double)Args[9]; } }

        public WoWSpell Spell { get { return WoWSpell.FromId(SpellId); } }

        public string SpellName { get { return Args[10].ToString(); } }

        public WoWSpellSchool SpellSchool { get { return (WoWSpellSchool)(int)(double)Args[11]; } }

        public object[] SuffixParams
        {
            get
            {
                var args = new List<object>();
                for (int i = 10; i < Args.Length; i++)
                {
                    if (Args[i] != null)
                    {
                        args.Add(args[i]);
                    }
                }
                return args.ToArray();
            }
        }
    }
}
