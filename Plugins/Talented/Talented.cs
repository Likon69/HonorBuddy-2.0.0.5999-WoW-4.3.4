using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Helpers;
using Talented.Forms;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;

namespace Talented
{
    public class Talented : HBPlugin
    {
        #region Non-HB Specific

        private static bool HasLearnedMajorTree { get { return Lua.GetReturnVal<int>("return GetPrimaryTalentTree()", 0) > 0; } }

        public void SelectMajorTalentTree(int index)
        {
            Lua.DoString("SetPreviewPrimaryTalentTree({0},GetActiveTalentGroup())", index);
        }

        private static IEnumerable<TalentPlacement> BuildLearnedTalentDictionary()
        {
            var ret = new List<TalentPlacement>();

            using (new FrameLock())
            {
                for (int tabIndex = 1; tabIndex <= 3; tabIndex++)
                {
                    int numTalents = Lua.GetReturnVal<int>("return GetNumTalents(" + tabIndex + ", false, false)", 0);
                    for (int talentIndex = 1; talentIndex <= numTalents; talentIndex++)
                    {
                        var vals = Lua.GetReturnValues("return GetTalentInfo(" + tabIndex + ", " + talentIndex + ")");
                        var name = vals[0];
                        var rank = int.Parse(vals[4]);
                        if (rank != 0)
                            ret.Add(new TalentPlacement(tabIndex, talentIndex, rank, name));
                    }
                }
            }

            return ret;
        }

        private static void LearnTalent(int tab, int index)
        {
            Lua.DoString(string.Format("AddPreviewTalentPoints({0}, {1}, 1)", tab, index));
        }

        private IEnumerable<TalentPlacement> GetCurrentSpecTalentPlacements()
        {
            var talentPoints = new List<int>(
                new[]
                    {
                        Lua.GetReturnVal<int>("return GetTalentTabInfo(1)", 4),
                        Lua.GetReturnVal<int>("return GetTalentTabInfo(2)", 4),
                        Lua.GetReturnVal<int>("return GetTalentTabInfo(3)", 4)
                    });

            var bestTab = talentPoints.IndexOf(talentPoints.Max()) + 1;
            if (talentPoints.All(i => i == 0))
                bestTab = _talentBuild.Specialization;

            var placement = _talentBuild.TalentPlacements.Where(ret => ret.Tab == bestTab).ToList();

            return placement;
        }

        private void InitializeHooks()
        {
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", HandleTalentPointsChanged);
        }

        private void HandleTalentPointsChanged(object sender, LuaEventArgs args)
        {
            using (new FrameLock())
            {
                if (!HasLearnedMajorTree)
                    SelectMajorTalentTree(_talentBuild.Specialization);

                var learned = BuildLearnedTalentDictionary();
                var wanted = _talentBuild.TalentPlacements;
                //var wanted = GetCurrentSpecTalentPlacements();

                int numAvailable = Lua.GetReturnVal<int>("return GetUnspentTalentPoints()", 0);

                foreach (var tp in wanted)
                {
                    if (numAvailable == 0)
                        continue;

                    var lt = learned.FirstOrDefault(t => (t.Name == tp.Name) || (t.Index == tp.Index && t.Tab == tp.Tab));

                    int numLearned = 0;

                    if (lt != null)
                        numLearned = lt.Count;

                    while (numAvailable != 0 && numLearned < tp.Count)
                    {
                        LearnTalent(tp.Tab, tp.Index);
                        numLearned++;
                        numAvailable--;
                    }
                }

                Lua.DoString("LearnPreviewTalents()");
            }
        }

        #endregion

        private TalentTree _talentBuild;
        private bool _initialized;
        public override void Pulse()
        {
            if (TalentedSettings.Instance.FirstUseAfterChange)
            {
                Logging.Write(Color.Red, "[Talented] There has been some structure changes in talented. You need to (re)set a talent build from settings.");
                return;
            }

            if (TalentedSettings.Instance.ChoosenTalentBuild == null)
            {
                Logging.Write(Color.Red, "[Talented] You have not yet selected a talent build! Please open the form and select one");
                return;
            }

            if (_talentBuild == null || (_talentBuild != null && _talentBuild.BuildName != TalentedSettings.Instance.ChoosenTalentBuildName))
                _talentBuild = TalentedSettings.Instance.ChoosenTalentBuild;

            if (_talentBuild != null && _talentBuild.Class != StyxWoW.Me.Class && StyxWoW.Me.Level > 0)
            {
                Logging.Write(Color.Red, "[Talented] This build is not meant for you class! Please select another talent build");
                return;
            }

            if (!_initialized)
            {
                InitializeHooks();
                HandleTalentPointsChanged(null, null);
                _initialized = true;
            }
        }

        public override string Name { get { return "Talented"; } }

        public override string Author { get { return "Apoc/Nesox"; } }

        public override Version Version { get { return new Version(1, 0, 0, 1); } }

        public override bool WantButton { get { return true; } }

        public override string ButtonText { get { return "Select Talent Build"; } }

        public override void OnButtonPress()
        {
            var config = new FormConfig();
            config.ShowDialog();
        }
    }
}
