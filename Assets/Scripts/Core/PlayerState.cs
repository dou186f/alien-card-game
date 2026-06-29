using System.Collections.Generic;

namespace CuteAliens.Core
{
    public class PlayerState
    {
        public string Name { get; private set; }

        public List<CardInstance> Hand { get; private set; }
        public List<CardInstance> PlayedCards { get; private set; }

        public int Score { get; set; }
        public int TotalScore { get; set; }

        public PlayerState(string name)
        {
            Name = name;
            Hand = new List<CardInstance>();
            PlayedCards = new List<CardInstance>();
            Score = 0;
            TotalScore = 0;
        }

        public void AddCardToHand(CardInstance card)
        {
            Hand.Add(card);
        }

        public bool PlayCard(CardInstance card)
        {
            if (card == null)
            {
                return false;
            }

            if (!Hand.Contains(card))
            {
                return false;
            }

            Hand.Remove(card);
            PlayedCards.Add(card);

            return true;
        }

        public void PrepareForNextRound()
        {
            Hand.Clear();
            PlayedCards.Clear();
            Score = 0;
        }
    }
}