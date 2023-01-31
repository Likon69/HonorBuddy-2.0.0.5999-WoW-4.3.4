using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;


namespace PvPRogue.Managers
{
    class PlayerObjects
    {
        /// <summary>
        /// returns number of enemys focused on me
        /// </summary>
        public static int EnemysFocusedOnMe
        {
            get
            {
                return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false)
                        where Unit.IsAlive  &&
                        Unit.Distance < 35  &&
                        !Unit.IsFriendly  &&
                        Unit.InLineOfSight  &&
                        Unit.CurrentTarget != null  &&
                        Unit.CurrentTarget.Guid == StyxWoW.Me.Guid
                        select Unit).Count();
            }
        }

        /// <summary>
        /// returns number of enemys around
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public static int EnemysAround(float Distance)
        {
            return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false)
                    where Unit.IsAlive &&
                    Unit.Distance <= Distance &&
                    !Unit.IsFriendly
                    select Unit).Count();
        }

        /// <summary>
        /// Returns number of team mates around
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public static int TeamAround(int Distance)
        {
            return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false, true)
                    where Unit.IsAlive &&
                    Unit.Distance < Distance &&
                    Unit.IsFriendly
                    select Unit).Count();
        }

        public static int MeleeTargeting
        {
            get
            {
                return (from Unit in ObjectManager.GetObjectsOfType<WoWUnit>(false)
                        where Unit.IsAlive  &&
                        Unit.IsWithinMeleeRange &&
                        Unit.Class != WoWClass.Priest  &&
                        Unit.Class != WoWClass.Hunter &&
                        Unit.Class != WoWClass.Warlock &&
                        Unit.Class != WoWClass.Mage &&
                        !Unit.IsFriendly
                        select Unit).Count();
            }
        }
    }
}
