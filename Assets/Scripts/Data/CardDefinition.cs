using UnityEngine;

namespace CuteAliens.Data
{
    [CreateAssetMenu(
        fileName = "NewCardDefinition",
        menuName = "Cute Aliens/Card Definition"
    )]
    public class CardDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string cardId;
        public string displayName;

        [TextArea]
        public string description;

        [Header("Visual")]
        public Sprite artwork;

        [Header("Scoring")]
        public CardScoringType scoringType;

        public int pointValue = 1;

        public int requiredCount = 1;
    }
}