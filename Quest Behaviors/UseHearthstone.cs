using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Styx.Database;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Styx.Bot.Quest_Behaviors
{
    //
    // Class: UseHearthstone
    // Author: Azaril
    // Version 1.0.0

    //
    // Example usage:
    //
    // <CustomBehavior File="UseHearthstone" HearthLocation="123" X="3523.637" Y="5015.019" Z="-0.9931576" OutsideDistance="1000"/>  
    //
    // OutsideDistance - Optional parameter to indicate only to use hearthstone if outside range. If not present, then the bot will hearth from any distance.
    //

    public class UseHearthstone : CustomForcedBehavior
    {
        public UseHearthstone(Dictionary<string, string> args)
            : base(args)
        {
            m_Complete = false;

            try
            {
                if (Args.ContainsKey("OutsideDistance"))
                {
                    float Distance = 0.0f;

                    if (!float.TryParse(Args["OutsideDistance"], out Distance))
                    {
                        throw new ArgumentException("Parsing attribute 'OutsideDistance' in SetHearthstone behavior failed! Please check your profile!");
                    }

                    m_OutsideDistance = Distance;
                }
                else
                {
                    m_OutsideDistance = null;
                }

                float X;
                float Y;
                float Z;

                if (!float.TryParse(Args["X"], out X))
                {
                    throw new ArgumentException("Parsing attribute 'X' in SetHearthstone behavior failed! Please check your profile!");
                }

                if (!float.TryParse(Args["Y"], out Y))
                {
                    throw new ArgumentException("Parsing attribute 'Y' in SetHearthstone behavior failed! Please check your profile!");
                }

                if (!float.TryParse(Args["Z"], out Z))
                {
                    throw new ArgumentException("Parsing attribute 'Z' in SetHearthstone behavior failed! Please check your profile!");
                }

                m_Location = new WoWPoint(X, Y, Z);

                if (!uint.TryParse(Args["HearthLocation"], out m_HearthLocation))
                {
                    throw new ArgumentException("Parsing attribute 'HearthLocation' in SetHearthstone behavior failed! Please check your profile!");
                }
            }
            catch (Exception Ex)
            {
                Logging.Write(string.Format("Failed to create UseHearthstone behavior.\nException:\n{0}", Ex.ToString()));

                Thread.CurrentThread.Abort();
            }
        }

        protected override Composite CreateBehavior()
        {
            return m_Root ?? (m_Root = InternalCreateBehavior());
        }

        static uint[] HearthItemIDs = 
        {
            // Hearthstone
            6948,
            // Innkeeper's Daughter
            64488
        };

        IEnumerable<WoWItem> GetHearthItems()
        {
            LocalPlayer Player = ObjectManager.Me;

            if (Player != null)
            {
                return from WoWItem Item in Player.BagItems
                       where HearthItemIDs.Contains(Item.Entry)
                       select Item;
            }

            return null;
        }

        Composite InternalCreateBehavior()
        {
            //
            // Check if the behavior is not yet complete.
            //
            return new Decorator(TreeContext =>
            {
                return !m_Complete;
            },
                //
                // Check the player is not currently casting.
                //
                new Decorator(TreeContext =>
                {
                    LocalPlayer Player = ObjectManager.Me;

                    if(Player != null)
                    {
                        return !Player.IsCasting;
                    }

                    return false;
                },
                    //
                    // Attempt to hearth.
                    //
                    new Action(TreeContext =>
                    {
                        IEnumerable<WoWItem> HearthItems = GetHearthItems();

                        if(HearthItems == null || HearthItems.Count() == 0)
                        {
                            Logging.Write("No hearth items on player, aborting hearth action.");

                            m_Complete = true;

                            return RunStatus.Success;
                        }

                        WoWItem Hearth = HearthItems.FirstOrDefault(Item => Item.Cooldown <= 20);

                        if(Hearth == null)
                        {
                            Logging.Write("Hearthstone cooldown is longer than 20 seconds, done hearthing.");

                            m_Complete = true;

                            return RunStatus.Success;
                        }

                        if(Hearth.Cooldown == 0)
                        {
                            Logging.Write("Using hearthstone.");

                            WoWMovement.MoveStop();

                            Hearth.Use();

                            Thread.Sleep(1000);
                        }

                        return m_Complete ? RunStatus.Success : RunStatus.Failure;
                    })
                )
            );
        }

        string GetBindLocation()
        {
            return Lua.GetReturnVal<string>("return GetBindLocation();", 0);
        }

        bool CheckOutsideDistanceToTarget()
        {
            if (m_OutsideDistance == null)
            {
                return true;
            }

            LocalPlayer Player = ObjectManager.Me;

            if (Player != null)
            {
                return Player.Location.Distance(m_Location) >= m_OutsideDistance;
            }

            return true;
        }

        public override bool IsDone
        {
            get
            {
                return m_Complete;
            }
        }

        public override void OnStart()
        {
            if (!CheckOutsideDistanceToTarget())
            {
                Logging.Write("Inside of range to hearth location, skipping action.");

                m_Complete = true;
            }

            LocalPlayer Player = ObjectManager.Me;

            if (Player != null)
            {
                Logging.Write(string.Format("Current bind location - Name: [ {0} ] - ID: [ {1} ]", GetBindLocation(), Player.HearthstoneAreaId));

                if(Player.HearthstoneAreaId != m_HearthLocation)
                {
                    Logging.Write("Hearth is not set to target area, skipping action.");

                    m_Complete = true;
                }
            }
            else
            {
                Logging.Write("Unable to get local player for hearth action, skipping.");

                m_Complete = true;
            }
        }

        Nullable<float> m_OutsideDistance;
        WoWPoint m_Location;
        uint m_HearthLocation;
        Composite m_Root;
        bool m_Complete;
    }
}