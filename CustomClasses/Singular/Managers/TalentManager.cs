#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: bobby53 $
// $Date: 2012-08-21 16:49:41 +0300 (Sal, 21 Ağu 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Managers/TalentManager.cs $
// $LastChangedBy: bobby53 $
// $LastChangedDate: 2012-08-21 16:49:41 +0300 (Sal, 21 Ağu 2012) $
// $LastChangedRevision: 652 $
// $Revision: 652 $

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.WoWInternals;

namespace Singular.Managers
{
    public enum TalentSpec
    {
        // Just a 'spec' for low levels
        Lowbie = 0,
        // A value representing any spec
        Any = int.MaxValue,
        // Here's how this works
        // In the 2nd byte we store the class for the spec
        // Low byte stores the index of the spec. (0-2 for a total of 3)
        // We can retrieve the class easily by doing spec & 0xFF00 to get the high-byte (or you can just shift right 8bits)
        // And the spec can be retrieved via spec & 0xFF - Simple enough
        // Extra flags are stored in the 3rd byte

        BloodDeathKnight = ((int)WoWClass.DeathKnight << 8) + 0,
        FrostDeathKnight = ((int)WoWClass.DeathKnight << 8) + 1,
        UnholyDeathKnight = ((int)WoWClass.DeathKnight << 8) + 2,

        BalanceDruid = ((int)WoWClass.Druid << 8) + 0,
        FeralDruid = ((int)WoWClass.Druid << 8) + 1,
        //FeralTankDruid = TalentManager.TALENT_FLAG_ISEXTRASPEC + ((int)WoWClass.Druid << 8) + 1,
        RestorationDruid = ((int)WoWClass.Druid << 8) + 2,

        BeastMasteryHunter = ((int)WoWClass.Hunter << 8) + 0,
        MarksmanshipHunter = ((int)WoWClass.Hunter << 8) + 1,
        SurvivalHunter = ((int)WoWClass.Hunter << 8) + 2,

        ArcaneMage = ((int)WoWClass.Mage << 8) + 0,
        FireMage = ((int)WoWClass.Mage << 8) + 1,
        FrostMage = ((int)WoWClass.Mage << 8) + 2,

        HolyPaladin = ((int)WoWClass.Paladin << 8) + 0,
        ProtectionPaladin = ((int)WoWClass.Paladin << 8) + 1,
        RetributionPaladin = ((int)WoWClass.Paladin << 8) + 2,

        DisciplinePriest = ((int)WoWClass.Priest << 8) + 0,
        //DisciplineHealingPriest = TalentManager.TALENT_FLAG_ISEXTRASPEC + ((int)WoWClass.Priest << 8) + 0,
        HolyPriest = ((int)WoWClass.Priest << 8) + 1,
        ShadowPriest = ((int)WoWClass.Priest << 8) + 2,

        AssasinationRogue = ((int)WoWClass.Rogue << 8) + 0,
        CombatRogue = ((int)WoWClass.Rogue << 8) + 1,
        SubtletyRogue = ((int)WoWClass.Rogue << 8) + 2,

        ElementalShaman = ((int)WoWClass.Shaman << 8) + 0,
        EnhancementShaman = ((int)WoWClass.Shaman << 8) + 1,
        RestorationShaman = ((int)WoWClass.Shaman << 8) + 2,

        AfflictionWarlock = ((int)WoWClass.Warlock << 8) + 0,
        DemonologyWarlock = ((int)WoWClass.Warlock << 8) + 1,
        DestructionWarlock = ((int)WoWClass.Warlock << 8) + 2,

        ArmsWarrior = ((int)WoWClass.Warrior << 8) + 0,
        FuryWarrior = ((int)WoWClass.Warrior << 8) + 1,
        ProtectionWarrior = ((int)WoWClass.Warrior << 8) + 2,

        // BrewmasterMonk = ((int)WoWClass.Monk << 8) + 0,
        // MistweaverMonk = ((int)WoWClass.Monk << 8) + 1,
        // WindwalkerMonk = ((int)WoWClass.Monk << 8) + 2,
    }

    internal static class TalentManager
    {
        //public const int TALENT_FLAG_ISEXTRASPEC = 0x10000;

        static TalentManager()
        {
            Talents = new List<Talent>();
            Glyphs = new HashSet<string>();
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("GLYPH_UPDATED", UpdateTalentManager);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", UpdateTalentManager);
        }

        public static TalentSpec CurrentSpec { get; private set; }

        public static List<Talent> Talents { get; private set; }

        public static HashSet<string> Glyphs { get; private set; }

        public static int GetCount(int tab, int index)
        {
            return Talents.FirstOrDefault(t => t.Tab == tab && t.Index == index).Count;
        }

        /// <summary>
        ///   Checks if we have a glyph or not
        /// </summary>
        /// <param name = "glyphName">Name of the glyph without "Glyph of". i.e. HasGlyph("Aquatic Form")</param>
        /// <returns></returns>
        public static bool HasGlyph(string glyphName)
        {
            return Glyphs.Count > 0 && Glyphs.Contains(glyphName);
        }

        private static void UpdateTalentManager(object sender, LuaEventArgs args)
        {
            TalentSpec oldSpec = CurrentSpec;

            Update();

            if (CurrentSpec != oldSpec)
            {
                Logger.Write("Your spec has been changed. Rebuilding behaviors");
                SingularRoutine.Instance.CreateBehaviors();
            }
        }

        public static void Update()
        {
            // Don't bother if we're < 10
            if (StyxWoW.Me.Level < 10)
            {
                CurrentSpec = TalentSpec.Lowbie;
                return;
            }
            WoWClass myClass = StyxWoW.Me.Class;
            int treeOne = 0, treeTwo = 0, treeThree = 0;
            //bool isExtraSpec = false;

            // Keep the frame stuck so we can do a bunch of injecting at once.
            using (new FrameLock())
            {
                Talents.Clear();
                for (int tab = 1; tab <= 3; tab++)
                {
                    var numTalents = Lua.GetReturnVal<int>("return GetNumTalents(" + tab + ")", 0);
                    for (int index = 1; index <= numTalents; index++)
                    {
                        var rank = Lua.GetReturnVal<int>(string.Format("return GetTalentInfo({0}, {1})", tab, index), 4);
                        var t = new Talent { Tab = tab, Index = index, Count = rank };
                        Talents.Add(t);

                        //// Thick Hide - Only used by tanking druids
                        //if (myClass == WoWClass.Druid && tab == 2 && index == 11 && rank != 0)
                        //{
                        //    isExtraSpec = true;
                        //}

                        //// Renewed Hope
                        //if (myClass == WoWClass.Priest && tab == 1 && index == 8 && rank != 0)
                        //{
                        //    isExtraSpec = true;
                        //}

                        switch (tab)
                        {
                            case 1:
                                treeOne += rank;
                                break;
                            case 2:
                                treeTwo += rank;
                                break;
                            case 3:
                                treeThree += rank;
                                break;
                        }
                    }
                }

                Glyphs.Clear();

                var glyphCount = Lua.GetReturnVal<int>("return GetNumGlyphSockets()", 0);

                if (glyphCount != 0)
                {
                    for (int i = 1; i <= glyphCount; i++)
                    {
                        List<string> glyphInfo = Lua.GetReturnValues(String.Format("return GetGlyphSocketInfo({0})", i));

                        if (glyphInfo != null && glyphInfo[3] != "nil" && !string.IsNullOrEmpty(glyphInfo[3]))
                        {
                            Glyphs.Add(WoWSpell.FromId(int.Parse(glyphInfo[3])).Name.Replace("Glyph of ", ""));
                        }
                    }
                }
            }

            if (treeOne == 0 && treeTwo == 0 && treeThree == 0)
            {
                CurrentSpec = TalentSpec.Lowbie;
                return;
            }

            int max = Math.Max(Math.Max(treeOne, treeTwo), treeThree);
            Logger.WriteDebug("[Talents] Best Tree: " + max);
            //Logger.WriteDebug("[Talents] Is Special Spec: " + isExtraSpec);
            int specMask = ((int)StyxWoW.Me.Class << 8);

            //// Bear tanks, healing disc priests, etc.
            //if (isExtraSpec)
            //{
            //    specMask += TALENT_FLAG_ISEXTRASPEC;
            //}

            if (max == treeOne)
            {
                CurrentSpec = (TalentSpec)(specMask + 0);
            }
            else if (max == treeTwo)
            {
                CurrentSpec = (TalentSpec)(specMask + 1);
            }
            else
            {
                CurrentSpec = (TalentSpec)(specMask + 2);
            }
        }

        #region Nested type: Talent

        public struct Talent
        {
            public int Count;

            public int Index;

            public int Tab;
        }

        #endregion
    }
}