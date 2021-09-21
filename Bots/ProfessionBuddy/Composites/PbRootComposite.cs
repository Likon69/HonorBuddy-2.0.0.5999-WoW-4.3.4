using Styx;
using TreeSharp;

namespace HighVoltz.Composites
{
    public class PbRootComposite : PrioritySelector
    {
        private BotBase _secondaryBot;

        public PbRootComposite(PbDecorator pbBotBase, BotBase secondaryBot)
            : base(pbBotBase, secondaryBot == null ? new PrioritySelector() : secondaryBot.Root)
        {
            _secondaryBot = secondaryBot;
        }

        public PbDecorator PbBotBase
        {
            get { return Children[0] as PbDecorator; }
            set { Children[0] = value; }
        }

        public BotBase SecondaryBot
        {
            get { return _secondaryBot; }
            set
            {
                _secondaryBot = value;
                Children[1] = value.Root;
            }
        }
    }
}