using System;
using System.Windows.Forms;
using Styx.WoWInternals;

namespace CLU.GUI
{
    using System.Drawing;

    using Styx;

    using global::CLU.Helpers;

    public partial class TargetInfo : Form
    {
        private static TargetInfo instance = new TargetInfo();

        public static void Display()
        {
            if (instance == null || instance.IsDisposed)
                instance = new TargetInfo();
            if (!instance.Visible)
                instance.Show();
        }

        private TargetInfo()
        {
            InitializeComponent();
        }

        private void update()
        {

            var target = ObjectManager.Me.CurrentTarget;
            var color = StyxWoW.Me.IsSafelyBehind(target) ? Color.DarkGreen : Color.Red;


            tname.Text = target == null ? "<No target>" : target.Name;
            tguid.Text = target == null ? string.Empty : target.Guid.ToString();
            distance.Text = target == null ? string.Empty : Math.Round(target.Distance, 3).ToString();
            realDistance.Text = target == null ? string.Empty : Math.Round(Spell.DistanceToTargetBoundingBox(target), 3).ToString();
            issafelybehindtarget.ForeColor = color;
            issafelybehindtarget.Text = (target != null && StyxWoW.Me.IsSafelyBehind(target)).ToString();

            if (target != null)
            {
                // facing
                var me = ObjectManager.Me.Location;
                var ta = target.Location;
                facing.Text = Math.Round(Spell.FacingTowardsUnitDegrees(me, ta), 2) + "°";
            }
        }

        void Timer1Tick(object sender, EventArgs e)
        {
            try
            {
                update();
            }
            catch { }

        }
    }
}
