using System;

namespace Singular
{
    // stop pollution of the namespace in random classes

    public enum TrinketUsage
    {
        Never,
        OnCooldown,
        OnCooldownInCombat,
        LowPower,
        LowHealth,
        CrowdControlled,
        CrowdControlledSilenced
    }

    public enum ClusterType
    {
        Radius,
        Chained,
        Cone
    }

    public enum CastOn
    {
        Never,
        Bosses,
        Players,
        All,
    }

    [Flags]
    public enum WoWContext
    {
        None = 0,
        Normal = 0x1,
        Instances = 0x2,
        Battlegrounds = 0x4,

        All = Normal | Instances | Battlegrounds,
    }

    [Flags]
    public enum BehaviorType
    {
        Rest = 0x1,
        PreCombatBuffs = 0x2,
        PullBuffs = 0x4,
        Pull = 0x8,
        Heal = 0x10,
        CombatBuffs = 0x20,
        Combat = 0x40,

        // this is no guarantee that the bot is in combat
        InCombat = Heal | CombatBuffs | Combat,
        // this is no guarantee that the bot is out of combat
        OutOfCombat = Rest | PreCombatBuffs | PullBuffs | PreCombatBuffs,

        All = Rest | PreCombatBuffs | PullBuffs | Pull | Heal | CombatBuffs | Combat,
    }
}
