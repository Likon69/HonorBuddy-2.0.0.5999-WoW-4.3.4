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
    // Class: SetHearthstone
    // Author: Azaril
    // Version 1.0.0

    //
    // Example usage:
    //
    // <CustomBehavior File="SetHearthstone" MobId="25841" X="3523.637" Y="5015.019" Z="-0.9931576" HearthLocation="123" WithinDistance="1000"/>  
    //
    // WithinDistance - Optional parameter to indicate only to get hearthstone if within range. If not present, then the bot will move from any distance
    //                  to the hearthstone provider.
    //

    public class SetHearthstone : CustomForcedBehavior
    {
        public SetHearthstone(Dictionary<string, string> args)
            : base(args)
        {
            m_Complete = false;

            try
            {
                if (!uint.TryParse(Args["MobId"], out m_MobEntryID))
                {
                    throw new ArgumentException("Parsing attribute 'MobId' in SetHearthstone behavior failed! Please check your profile!");
                }

                if (Args.ContainsKey("WithinDistance"))
                {
                    float Distance = 0.0f;

                    if (!float.TryParse(Args["WithinDistance"], out Distance))
                    {
                        throw new ArgumentException("Parsing attribute 'WithinDistance' in SetHearthstone behavior failed! Please check your profile!");
                    }

                    m_WithinDistance = Distance;
                }
                else
                {
                    m_WithinDistance = null;
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
                Logging.Write(string.Format("Failed to create SetHearthstone behavior.\nException:\n{0}", Ex.ToString()));

                Thread.CurrentThread.Abort();
            }
        }

        protected override Composite CreateBehavior()
        {
            return m_Root ?? (m_Root = InternalCreateBehavior());
        }

        WoWUnit GetTargetUnit()
        {
            WoWUnit Unit = (from WoWUnit SearchUnit in ObjectManager.GetObjectsOfType<WoWUnit>()
                            where SearchUnit.Entry == m_MobEntryID
                            select SearchUnit).FirstOrDefault();

            return Unit;
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
                new PrioritySelector(
                    //
                    // While not in interact range, move to the target using the mob location.
                    //
                    new Decorator(TreeContext =>
                    {
                        WoWUnit Unit = GetTargetUnit();

                        return (Unit != null) && !Unit.WithinInteractRange;
                    },
                        //
                        // Move to the unit.
                        //
                        new Action(TreeContext =>
                        {
                            WoWUnit Unit = GetTargetUnit();

                            return Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(Unit.Location));
                        })
                    ),
                    //
                    // While the unit is not found, move to the target location specified.
                    //
                    new Decorator(TreeContext =>
                    {
                        WoWUnit Unit = GetTargetUnit();

                        return (Unit == null);
                    },
                        //
                        // Move to the location.
                        //
                        new Action(TreeContext =>
                        {
                            return Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(m_Location));
                        })
                    ),
                    //
                    // Check if the unit is found and within interact range.
                    //
                    new Decorator(TreeContext =>
                    {
                        WoWUnit Unit = GetTargetUnit();

                        return (Unit != null) && Unit.WithinInteractRange;
                    },
                        new PrioritySelector(
                            //
                            // Check that the player is not currently talking to the target.
                            //
                            new Decorator(TreeContext =>
                            {
                                GossipFrame Frame = GossipFrame.Instance;

                                return (Frame == null || !Frame.IsVisible);
                            },
                                new Action(TreeContext =>
                                {
                                    WoWUnit Unit = GetTargetUnit();

                                    Unit.Interact();
                                })
                            ),
                            //
                            // Select the bind hearthstone gossip option.
                            //
                            new Action(TreeContext =>
                            {
                                GossipFrame Frame = GossipFrame.Instance;

                                if (Frame != null && Frame.IsVisible)
                                {
                                    if (Frame.GossipOptionEntries != null)
                                    {
                                        foreach (GossipEntry Entry in Frame.GossipOptionEntries)
                                        {
                                            if (Entry.Type == GossipEntry.GossipEntryType.Binder)
                                            {
                                                Frame.SelectGossipOption(Entry.Index);

                                                return RunStatus.Success;
                                            }
                                        }
                                    }
                                }

                                return RunStatus.Failure;
                            })
                        )
                    )
                )
            );
        }

        bool CheckWithinDistanceToTarget()
        {
            if (m_WithinDistance == null)
            {
                return true;
            }

            LocalPlayer Player = ObjectManager.Me;

            if (Player != null)
            {
                return Player.Location.Distance(m_Location) <= m_WithinDistance;
            }

            return false;
        }

        string GetBindLocation()
        {
            return Lua.GetReturnVal<string>("return GetBindLocation();", 0);
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
            if (!CheckWithinDistanceToTarget())
            {
                Logging.Write("Outside of range to set hearth location, skipping action.");

                m_Complete = true;
            }

            LocalPlayer Player = ObjectManager.Me;

            if (Player != null)
            {
                Logging.Write(string.Format("Current bind location - Name: [ {0} ] - ID: [ {1} ]", GetBindLocation(), Player.HearthstoneAreaId));

                if(Player.HearthstoneAreaId == m_HearthLocation)
                {
                    Logging.Write("Hearth is already set to target area, skipping action.");

                    m_Complete = true;
                }
            }

            if (!m_Complete)
            {
                Lua.Events.AttachEvent("CONFIRM_BINDER", OnConfirmBinder);
            }
        }

        void OnConfirmBinder(object Sender, LuaEventArgs Args)
        {
            Lua.DoString("ConfirmBinder();");

            Lua.DoString("StaticPopup_Hide(\"CONFIRM_BINDER\");");

            Lua.Events.DetachEvent("CONFIRM_BINDER", OnConfirmBinder);

            m_Complete = true;

            LocalPlayer Player = ObjectManager.Me;

            if (Player != null)
            {
                Logging.Write(string.Format("New bind location - Name: [ {0} ] - ID: [ {1} ]", GetBindLocation(), Player.HearthstoneAreaId));
            }
        }

        uint m_MobEntryID;
        Nullable<float> m_WithinDistance;
        uint m_HearthLocation;
        WoWPoint m_Location;
        Composite m_Root;
        bool m_Complete;
    }
}