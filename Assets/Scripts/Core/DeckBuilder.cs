using System.Collections.Generic;
using UnityEngine;
using CuteAliens.Data;

namespace CuteAliens.Core
{
    public static class DeckBuilder
    {
        public static List<CardInstance> BuildDeck(DeckDefinition deckDefinition)
        {
            List<CardInstance> deck = new List<CardInstance>();

            if (deckDefinition == null)
            {
                Debug.LogError("DeckDefinition is null.");
                return deck;
            }

            foreach (DeckCardEntry entry in deckDefinition.cards)
            {
                if (entry.card == null)
                {
                    Debug.LogWarning("Deck has an empty card entry.");
                    continue;
                }

                for (int i = 0; i < entry.count; i++)
                {
                    deck.Add(new CardInstance(entry.card));
                }
            }

            return deck;
        }

        public static void Shuffle(List<CardInstance> deck)
        {
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);

                CardInstance temp = deck[i];
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }
    }
}