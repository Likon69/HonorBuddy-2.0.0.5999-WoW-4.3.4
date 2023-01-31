using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Zerfall.Talents
{

    public class TalentManager
    {
        public TalentManager()
        {
            Load(ActiveGroup);
        }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private readonly string[] _tabNames = new string[4];
        public int[] TabPoints = new int[4];
        public int IdxGroup;

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

        public WarlockTalentSpec Spec
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

                return (WarlockTalentSpec)nSpec;
            }
        }

        private void Load(int nGroup)
        {
            if (!ObjectManager.IsInGame)
                return;

            int nTab;

            IdxGroup = nGroup;

            for (nTab = 1; nTab <= 3; nTab++)
            {
                try
                {
                    List<string> tabInfo = Lua.GetReturnValues("return GetTalentTabInfo(" + nTab + ",false,false," + IdxGroup + ")", "hawker.lua");

                    if (Equals(null, tabInfo))
                        return;
                    //Changed for 4.0.1 - Thx Raphus
                    _tabNames[nTab] = tabInfo[2];
                    TabPoints[nTab] = Convert.ToInt32(tabInfo[4]);
                }
                catch (Exception ex)
                {
                    Logging.WriteException(ex);
                }
            }
        }

        public bool IsActiveGroup() { return ActiveGroup == IdxGroup; }

        public int ActiveGroup
        {
            get
            {
                if (!ObjectManager.IsInGame)
                    return 0;

                return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false,false)", 0);
            }
        }

        public void ActivateGroup()
        {
            if (!ObjectManager.IsInGame)
                return;

            Lua.DoString("SetActiveTalentGroup('{0}')", IdxGroup);
        }

        public int GetNumGroups()
        {
            if (!ObjectManager.IsInGame)
                return 0;

            return Lua.GetReturnVal<int>("return GetNumTalentGroups(false,false)", 0);
        }
    }
}
