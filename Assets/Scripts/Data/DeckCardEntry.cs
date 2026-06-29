using System;
using UnityEngine;

namespace CuteAliens.Data
{
    [Serializable]
    public class DeckCardEntry
    {
        public CardDefinition card;
        [Min(1)] public int count = 1;
    }
}