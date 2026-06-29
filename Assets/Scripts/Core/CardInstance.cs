using CuteAliens.Data;

namespace CuteAliens.Core
{
    public class CardInstance
    {
        public CardDefinition Definition { get; private set; }

        public CardInstance(CardDefinition definition)
        {
            Definition = definition;
        }

        public string DisplayName
        {
            get { return Definition.displayName; }
        }

        public CardScoringType ScoringType
        {
            get { return Definition.scoringType; }
        }

        public int PointValue
        {
            get { return Definition.pointValue; }
        }

        public int RequiredCount
        {
            get { return Definition.requiredCount; }
        }
    }
}