using System;
using System.Collections.Generic;
using Styx.WoWInternals;

namespace RogueAssassin.Talents
{
	internal static class SpecManager
	{
		#region SpecList enum

		public enum SpecList
		{
			None = 0,
			Assassination,
			Combat,
			Subtlety
		}

		#endregion

		public static SpecList Spec
		{
			get
			{
				int currentSpec = CurrentSpecValue();
				return (SpecList) currentSpec;
			}
		}

		private static int CurrentSpecGroup()
		{
			return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false, false)", 0);
		}

		private static int CurrentSpecValue()
		{
			int tab;
			int group = CurrentSpecGroup();

			var pointsSpent = new int[3];

			for (tab = 1; tab <= 3; tab++)
			{
				List<string> talentTabInfo = Lua.GetReturnValues("return GetTalentTabInfo(" + tab + ", false, false, " + group + ")");
				pointsSpent[tab - 1] = Convert.ToInt32(talentTabInfo[4]);
			}

			if (pointsSpent[0] > (pointsSpent[1] + pointsSpent[2]))
			{
			    return 1;
			}

		    if (pointsSpent[1] > (pointsSpent[0] + pointsSpent[2]))
		    {
		        return 2;
		    }

		    return pointsSpent[2] > (pointsSpent[0] + pointsSpent[1]) ? 3 : 0;
		}
	}
}