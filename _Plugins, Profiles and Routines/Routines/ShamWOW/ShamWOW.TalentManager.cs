/*
 * NOTE:    DO NOT POST ANY MODIFIED VERSIONS OF THIS TO THE FORUMS.
 * 
 *          DO NOT UTILIZE ANY PORTION OF THIS COMBAT CLASS WITHOUT
 *          THE PRIOR PERMISSION OF AUTHOR.  PERMITTED USE MUST BE
 *          ACCOMPANIED BY CREDIT/ACKNOWLEDGEMENT TO ORIGINAL AUTHOR.
 * 
 * ShamWOW Shaman CC
 * 
 * Author:  Bobby53
 * 
 * See the ShamWOW.chm file for Help
 *
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Bobby53
{
    public class TalentManager
    {
        public TalentManager()
        {
        }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public readonly string[] TabNames = new string[4];
        public int[] TabPoints = new int[4];
        public int GroupIndex;

        public Dictionary<uint, string> _glyphs;

		public int Spec
		{
			get
			{
				int s1 = TabPoints[1], s2 = TabPoints[2], s3 = TabPoints[3];

				int nSpec;
				if (s1 == 0 && s2 == 0 && s3 == 0)
                    nSpec = 0;
				else if (s1 >= s2 && s1 >= s3)
                    nSpec = 1;
				else if (s2 >= s1 && s2 >= s3)
                    nSpec = 2;
				else
                    nSpec = 3;

                return nSpec;
			}
		}

        public void Load()
        {
            Load(GetActiveGroup());
        }

        public void Load( int grp )
        {
            if (!ObjectManager.IsInGame)
                return;

            GroupIndex = grp;

            int nTab;
            for (nTab = 1; nTab <= 3; nTab++)
            {
                try
                {
                    List<string> tabInfo = Lua.GetReturnValues("return GetTalentTabInfo(" + nTab + ",false,false," + GroupIndex + ")", "hawker.lua");

                    if (Equals(null, tabInfo))
                        return;

                    TabNames[nTab] = tabInfo[1];
                    TabPoints[nTab] = Convert.ToInt32(tabInfo[4]);
                }
                catch (Exception ex)
                {
                    Logging.WriteException(ex);
                }
            }

            LoadGlyphs();
        }

        public bool IsActiveGroup() { return GetActiveGroup() == GroupIndex; }

        public static int GetActiveGroup()
        {
            if (!ObjectManager.IsInGame)
                return 0;

            return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false,false)", 0);
        }

        public void ActivateGroup()
        {
            if (!ObjectManager.IsInGame)
                return;

            Lua.DoString("SetActiveTalentGroup('{0}')", GroupIndex);
        }

        public int GetNumGroups()
        {
            if (!ObjectManager.IsInGame)
                return 0;

            return Lua.GetReturnVal<int>("return GetNumTalentGroups(false,false)", 0);
        }

        public int TotalPoints
        {
            get
            {
                int nPoints = 0;
                for (int iTab = 1; iTab <= 3; iTab++)
                    nPoints += TabPoints[iTab];

                return nPoints;
            }
        }

        public int UnspentPoints
        {
            get
            {
                return Lua.GetReturnVal<int>("return GetUnspentTalentPoints(false,false," + GroupIndex + ")", 0);
            }
        }

        public int GetTalentInfo(int idxTab, int idxTalent)
        {
            string sLuaCmd = String.Format("return GetTalentInfo( {0}, {1});", idxTab, idxTalent);
            List<string> retList = Lua.GetReturnValues(sLuaCmd, "hawker.lua");
            if (Equals(null, retList))
            {
                Shaman.Slog("ERROR:  Talent {0}, {1} does not exist -- Notify CC developer", idxTab, idxTalent);
                return 0;
            }

            return Convert.ToInt32(retList[4]);
        }

        private void LoadGlyphs()
        {
            int iSocket;
            uint countSockets = Lua.GetReturnVal<uint>("return GetNumGlyphSockets()", 0);

            _glyphs = new Dictionary<uint, string>();
            for (iSocket = 1; iSocket <= countSockets; iSocket++)
            {
                List<string> ret = Lua.GetReturnValues("return GetGlyphSocketInfo(" + iSocket + ", " + GroupIndex + ")");
                if (ret != null && ret.Count > 0)
                {
                    if (ret[0] == "1" && !string.IsNullOrEmpty(ret[3]) && ret[3] != "nil")
                    {
                        uint glyphId = Convert.ToUInt32(ret[3]);
                        string glyphName = Lua.GetReturnVal<string>("return GetSpellInfo(" + glyphId + ")", 0);
                        if (!string.IsNullOrEmpty(glyphName) && glyphName != "nil")
                        {
                            _glyphs.Add(glyphId, glyphName);
                        }
                    }
                }
            }
        }
    }
}
