using Styx;

namespace CLU.Helpers
{

    public class BotChecker
    {
        /* putting all the BotBase logic here */

        private static readonly BotChecker UnitInstance = new BotChecker();

        /// <summary>
        /// An instance of the BotBase Class
        /// </summary>
        public static BotChecker Instance { get { return UnitInstance; } }

        // "ArchaeologyBuddy", "BGBuddy", "Combat Bot", "Gatherbuddy2", "Grind Bot", "Instancebuddy", "LazyRaider",
        //    "Mixed Mode", "PartyBot", "ProfessionBuddy", "Questing", "Raid Bot"

        public readonly string[] bots = new[]
        {
            "BGBuddy", "Combat Bot", "LazyRaider", "Questing", "Raid Bot"
        };

        public bool BotBaseInUse(string botname)
        {
            return BotManager.Current.Name == botname;
        }
    }
}
