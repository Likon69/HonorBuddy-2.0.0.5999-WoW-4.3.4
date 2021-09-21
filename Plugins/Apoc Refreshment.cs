using System;

using Styx.Helpers;
using Styx.Logic.Inventory;
using Styx.Plugins.PluginClass;

namespace QuickPlugins
{
    public class RefreshmentDetector : HBPlugin
    {
        private readonly WaitTimer _updateTimer = WaitTimer.TenSeconds;
        public override void Pulse()
        {
            if (_updateTimer.IsFinished)
            {
                _updateTimer.Reset();
                var drink = Consumable.GetBestDrink(false);
                var food = Consumable.GetBestFood(false);

                if (drink != null)
                    CharacterSettings.Instance.DrinkName = drink.Entry.ToString();
                if (food != null)
                    CharacterSettings.Instance.FoodName = food.Entry.ToString();
            }
        }

        public override string Name { get { return "Refreshment Detection"; } }

        public override string Author { get { return "Apoc"; } }

        public override Version Version { get { return new Version(1,0,0,0);} }
    }
}