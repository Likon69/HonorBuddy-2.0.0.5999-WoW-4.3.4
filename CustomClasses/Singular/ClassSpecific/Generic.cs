using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.Logic.Inventory;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Singular.ClassSpecific
{
    public static class Generic
    {
        //[Spec(TalentSpec.Any)]
        //[Behavior(BehaviorType.All)]
        //[Class(WoWClass.None)]
        //[Priority(999)]
        //[Context(WoWContext.All)]
        //[IgnoreBehaviorCount(BehaviorType.Combat), IgnoreBehaviorCount(BehaviorType.Rest)]
        //public static Composite CreateFlasksBehaviour()
        //{
        //    return new Decorator(
        //        ret => SingularSettings.Instance.UseAlchemyFlasks && !Unit.HasAnyAura(StyxWoW.Me, "Enhanced Agility", "Enhanced Intellect", "Enhanced Strength"),
        //        new PrioritySelector(
        //            Item.UseItem(58149),
        //            Item.UseItem(47499)));
        //}

        //[Spec(TalentSpec.Any)]
        //[Behavior(BehaviorType.All)]
        //[Class(WoWClass.None)]
        //[Priority(999)]
        //[Context(WoWContext.All)]
        //[IgnoreBehaviorCount(BehaviorType.Combat), IgnoreBehaviorCount(BehaviorType.Rest)]
        //public static Composite CreateTrinketBehaviour()
        //{
        //    return new PrioritySelector(
        //        new Decorator(
        //            ret => SingularSettings.Instance.Trinket1,
        //            Item.UseEquippedItem((uint)InventorySlot.Trinket0Slot)),
        //        new Decorator(
        //            ret => SingularSettings.Instance.Trinket2,
        //            Item.UseEquippedItem((uint)InventorySlot.Trinket1Slot)));
        //}

        //[Spec(TalentSpec.Any)]
        //[Behavior(BehaviorType.All)]
        //[Class(WoWClass.None)]
        //[Priority(999)]
        //[Context(WoWContext.All)]
        //[IgnoreBehaviorCount(BehaviorType.Combat), IgnoreBehaviorCount(BehaviorType.Rest)]
        //public static Composite CreateRacialBehaviour()
        //{
        //    return new Decorator(
        //        ret => SingularSettings.Instance.UseRacials,
        //        new PrioritySelector(
        //            new Decorator(
        //                ret => SpellManager.CanCast("Stoneform") && StyxWoW.Me.GetAllAuras().Any(a => a.Spell.Mechanic == WoWSpellMechanic.Bleeding ||
        //                    a.Spell.DispelType == WoWDispelType.Disease ||
        //                    a.Spell.DispelType == WoWDispelType.Poison),
        //                Spell.Cast("Stoneform")),
        //            new Decorator(
        //                ret => SpellManager.CanCast("Escape Artist") && Unit.HasAuraWithMechanic(StyxWoW.Me, WoWSpellMechanic.Rooted, WoWSpellMechanic.Snared),
        //                Spell.Cast("Escape Artist")),
        //            new Decorator(
        //                ret => SpellManager.CanCast("Every Man for Himself") && PVP.IsCrowdControlled(StyxWoW.Me),
        //                Spell.Cast("Every Man for Himself")),
        //            new Decorator(
        //                ret => SpellManager.CanCast("Gift of the Naaru") && StyxWoW.Me.HealthPercent < SingularSettings.Instance.GiftNaaruHP,
        //                Spell.Cast("Gift of the Naaru")),
        //            new Decorator(
        //                ret => SingularSettings.Instance.ShadowmeldThreatDrop && SpellManager.CanCast("Shadowmeld") && (StyxWoW.Me.IsInParty || StyxWoW.Me.IsInRaid) &&
        //                    !StyxWoW.Me.PartyMemberInfos.Any(pm => pm.Guid == StyxWoW.Me.Guid && pm.Role == WoWPartyMember.GroupRole.Tank) &&
        //                    ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Any(unit => unit.CurrentTargetGuid == StyxWoW.Me.Guid),
        //                Spell.Cast("Shadowmeld"))
        //            ));
        //}
    }
}
