using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Styx;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Logic.Pathing;

namespace PvPRogue.Managers
{
    public static class Poisons
    {
        // Poisen ID's
        private static readonly uint[] InstantID = { 6947, 43231 };
        private static readonly uint[] CripplingID = { 3775 };
        private static readonly uint[] MindNumbingID = { 5237 };
        private static readonly uint[] DeadlyID = { 2892, 43233 };
        private static readonly uint[] WoundID = { 10918, 43235 };


        public static void Pulse()
        {
            if (StyxWoW.Me.IsCasting) return;

            switch (Managers.Spec.CurrentSpec)
            {
                case eSpec.Subtlety:
                    Sub();
                    break;
            }
        }


        private static void Sub()
        {
            WoWItem Main = GetPoison(ClassSettings._Instance.SubtletyMainPoison);
            WoWItem OffHand = GetPoison(ClassSettings._Instance.SubtletyOffHandPoison);
            WoWItem Thrown = GetPoison(ePoison.Crippling);

            // Main
            if ((!HasMainHandPoison) && (Main != null))
            {
                Log.Write("Applying [{0}] to main hand", Main.Name);
                Navigator.PlayerMover.MoveStop();
                StyxWoW.SleepForLagDuration();
                Main.UseContainerItem();
                StyxWoW.SleepForLagDuration();
                Lua.DoString("UseInventoryItem(16)");
                StyxWoW.SleepForLagDuration();
                Thread.Sleep(1000);                              // Ugly <---- 
                while (StyxWoW.Me.IsCasting) { Thread.Sleep(100); }
                StyxWoW.SleepForLagDuration();
                Thread.Sleep(1000); 
                
                return;
            }

            // Offhand
            if ((!HasOffHandPoison) && (OffHand != null))
            {
                Log.Write("Applying [{0}] to off hand", OffHand.Name);
                Navigator.PlayerMover.MoveStop();
                StyxWoW.SleepForLagDuration();
                OffHand.UseContainerItem();
                StyxWoW.SleepForLagDuration();
                Lua.DoString("UseInventoryItem(17)");
                StyxWoW.SleepForLagDuration();
                Thread.Sleep(1000);                              // Ugly <---- 
                while (StyxWoW.Me.IsCasting) { Thread.Sleep(100); }
                StyxWoW.SleepForLagDuration();
                Thread.Sleep(1000);

                return;
            }

            // Thrown
            if ((!HasThrownPoison) && (Thrown != null) && (ClassSettings._Instance.GeneralThrownPoison))
            {
                Log.Write("Applying [{0}] to thrown", Thrown.Name);
                Navigator.PlayerMover.MoveStop();
                StyxWoW.SleepForLagDuration();
                Thrown.UseContainerItem();
                StyxWoW.SleepForLagDuration();
                Lua.DoString("UseInventoryItem(18)");
                StyxWoW.SleepForLagDuration();
                Thread.Sleep(1000);                              // Ugly <---- 
                while (StyxWoW.Me.IsCasting) { Thread.Sleep(100); }
                StyxWoW.SleepForLagDuration();
                Thread.Sleep(1000);

                return;
            }
        }


        /// <summary>
        /// Returns a item WoWItem from PList
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static WoWItem GetPoison(ePoison Poison)
        {
            switch (Poison)
            {
                case ePoison.Instant:
                    return StyxWoW.Me.CarriedItems.Where(i => InstantID.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                case ePoison.Crippling:
                    return StyxWoW.Me.CarriedItems.Where(i => CripplingID.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                case ePoison.MindNumbing:
                    return StyxWoW.Me.CarriedItems.Where(i => MindNumbingID.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                case ePoison.Deadly:
                    return StyxWoW.Me.CarriedItems.Where(i => DeadlyID.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                case ePoison.Wound:
                    return StyxWoW.Me.CarriedItems.Where(i => WoundID.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
            }

            return null;
        }


        /// <summary>
        /// Returns if it has a MainHand Poison
        /// </summary>
        public static bool HasMainHandPoison
        {
            get
            {
                if ((StyxWoW.Me.Inventory.Equipped.MainHand != null) 
                    && (StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 0)) return false;
                return true;
            }
        }

        /// <summary>
        /// Returns if it has a Offhand Poison
        /// </summary>
        public static bool HasOffHandPoison
        {
            get
            {
                if ((StyxWoW.Me.Inventory.Equipped.OffHand != null) 
                    && (StyxWoW.Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == 0)) return false;
                return true;
            }
        }

        /// <summary>
        /// Returns if it has a Ranged/Thrown Posion
        /// </summary>
        public static bool HasThrownPoison
        {
            get
            {
                if ((StyxWoW.Me.Inventory.Equipped.Ranged != null) 
                    && (StyxWoW.Me.Inventory.Equipped.Ranged.IsThrownWeapon)
                    && (StyxWoW.Me.Inventory.Equipped.Ranged.TemporaryEnchantment.Id == 0)) return false;
                return true;
            }
        }

    }
}
