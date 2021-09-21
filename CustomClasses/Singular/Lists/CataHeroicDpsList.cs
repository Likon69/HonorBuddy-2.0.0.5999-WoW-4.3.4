using System.Collections.Generic;

namespace Singular.Lists
{
    // This just contains a list for Catacylsm dungeon adds that NEED to be killed immediately.
    class CataHeroicDpsList
    {
        public static HashSet<uint> KillList = new HashSet<uint>
            {
                39984, // Malignant Trogg - Grim Batol (Purple Trogg)
                40357, // Invoked Flaming Spirit - Grim Batol (Dragha Flame Elementals)
                39392, // Faceless Corruptor - Grim Batol (Erudax spawns)

                49267, // Crystal Shard - Stonecore
                43286, // Crystal Shard - Stonecore

                40447, // Chains of Woe - Blackrock Caverns (Rom'ogg's chains, before he goes HULK SMASH on anybody in range)

                48906, // Blaze of the Heavens - Lost City of Tol'vir (Prophet's phoenix he summons)
                43927, // Harbinger of Darkness - Same as above, but the one summoned in the 'spririt realm'

                40716, // Seedling Pod - HOO - Ammunae fight (these turn into Bloodpetal Blossom)
                40620, // Bloodpetal Blossom - HOO - Ammunae fight (these should be dead already!)
                40585, // Spore - HOO - Ammunae fight (randomly summoned, leaves behind a Spore Cloud)
                41055, // Chaos Portal - HOO - Setesh Fight (close these or suffer endless waves of mobs)
            };
    }
}
