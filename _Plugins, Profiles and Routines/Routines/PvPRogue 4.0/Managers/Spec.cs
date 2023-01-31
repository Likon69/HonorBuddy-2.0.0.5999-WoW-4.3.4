using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.WoWInternals;

namespace PvPRogue.Managers
{
        public enum eSpec
        {
            Assasination,
            Combat,
            Subtlety,
            Lowbie
        }

    public static class Spec
    {
        /// <summary>
        /// Holds our current spec
        /// </summary>
        public static eSpec CurrentSpec = eSpec.Lowbie;


        /// <summary>
        /// Updates CurrentSpec with current spec;
        /// </summary>
        public static void Update()
        {
            // Our var counts
            int AssasinationPoints = 0;
            int CombatPoints = 0;
            int SubtletyPoints = 0;

            // Since we calling a couple of lua commands, lock the frame
            using (new FrameLock())
            {
                // Go threw each of the Spec tabs
                for (int Tab = 1; Tab <= 3; Tab++)
                {
                    // We could really just detect by this what is the best one
                    int TalentCount = Lua.GetReturnVal<int>("return GetNumTalents(" + Tab + ")", 0);

                    // Go threw each of the tallents
                    for (int i = 0; i < TalentCount; i++)
                    {
                        int level = Lua.GetReturnVal<int>(string.Format("return GetTalentInfo({0}, {1})", Tab, i), 4);

                        if (Tab == 1) AssasinationPoints += level;
                        if (Tab == 2) CombatPoints += level;
                        if (Tab == 3) SubtletyPoints += level;
                    }
                }
            }

            // Now we need to check who has the highest count
            int Max = Math.Max(Math.Max(AssasinationPoints, CombatPoints), SubtletyPoints);

            // Error trap lowbies
            if (Max == 0)
            {
                Log.Write("Spec Detected: Lowbie");
                return;
            }

            if (Max == AssasinationPoints)
            {
                CurrentSpec = eSpec.Assasination;
                Log.Write("Spec Detected: Assasination");
                return;
            }

            if (Max == CombatPoints)
            {
                CurrentSpec = eSpec.Combat;
                Log.Write("Spec Detected: Combat");
                return;
            }

            if (Max == SubtletyPoints)
            {
                CurrentSpec = eSpec.Subtlety;
                Log.Write("Spec Detected: Subtlety");
                return;
            }

        }

        /// <summary>
        /// Returns if is Assasination Spec
        /// </summary>
        public static bool IsAssasination
        {
            get
            {
                return (CurrentSpec == eSpec.Assasination);
            }
        }

        /// <summary>
        /// Returns if is combat Spec
        /// </summary>
        public static bool IsCombat
        {
            get
            {
                return (CurrentSpec == eSpec.Combat);
            }
        }

        /// <summary>
        /// Returns if is Subtlety Spec
        /// </summary>
        public static bool IsSubtlety
        {
            get
            {
                return (CurrentSpec == eSpec.Subtlety);
            }
        }

        /// <summary>
        /// Returns if player has no spec
        /// </summary>
        public static bool IsLobie
        {
            get
            {
                return (CurrentSpec == eSpec.Lowbie);
            }
        }

    }
}
