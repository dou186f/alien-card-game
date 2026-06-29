using System.Collections.Generic;
using UnityEngine;

namespace CuteAliens.Data
{
    [CreateAssetMenu(
        fileName = "NewDeckDefinition",
        menuName = "Cute Aliens/Deck Definition"
    )]
    public class DeckDefinition : ScriptableObject
    {
        public List<DeckCardEntry> cards = new List<DeckCardEntry>();
    }
}