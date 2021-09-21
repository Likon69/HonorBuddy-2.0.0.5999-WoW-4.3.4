using System;
using Styx.Helpers;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Styx
{
    public class AntiDrown : HBPlugin
    {
        public override string Name { get { return "Anti Drown"; } }
        public override string Author { get { return "Nesox"; } }
        public override Version Version { get { return _version; } }
        private readonly Version _version = new Version(1, 0, 0, 0);

        private Composite _root;

        public override void Pulse()
        {
            var me = StyxWoW.Me;
            var value = ObjectManager.Me.GetMirrorTimerInfo(MirrorTimerType.Breath).CurrentTime;
            if (value < 60000 && me.IsAlive && me.IsSwimming && (value != 0) || (value > 900001))
            {
                if (_root == null)
                    _root = new Action(ctx => DoCheck(ctx));

                Tick(_root);
            }
        }

        private static RunStatus DoCheck(object context)
        {
            if (!StyxWoW.Me.IsSwimming)
                return RunStatus.Success;

            Logging.Write("[Anti Drown]: Going for a nibble of air!");
            WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromMilliseconds(5000));
            return RunStatus.Running;
        }

        private static void Tick(Composite tree)
        {
            if (tree.LastStatus != RunStatus.Running)
                tree.Start(null);

            if (tree.Tick(null) != RunStatus.Running)
                tree.Stop(null);
        }
    }
}
