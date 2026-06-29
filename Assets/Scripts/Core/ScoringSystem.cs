using System.Collections.Generic;
using System.Linq;
using CuteAliens.Data;

namespace CuteAliens.Core
{
    public static class ScoringSystem
    {
        public static void CalculateScores(List<PlayerState> players)
        {
            foreach (PlayerState player in players)
            {
                player.Score = 0;
            }

            foreach (PlayerState player in players)
            {
                player.Score += CalculateIndividualScore(player);
            }

            ApplyMajorityScores(players);
        }

        private static int CalculateIndividualScore(PlayerState player)
        {
            int score = 0;

            var groups = player.PlayedCards
                .GroupBy(card => card.Definition.cardId);

            foreach (var group in groups)
            {
                CardDefinition definition = group.First().Definition;
                int count = group.Count();

                switch (definition.scoringType)
                {
                    case CardScoringType.SimplePoint:
                        score += ScoreSimplePoint(definition, count);
                        break;

                    case CardScoringType.PairBonus:
                        score += ScoreBySets(definition, count);
                        break;

                    case CardScoringType.SetComplete:
                        score += ScoreBySets(definition, count);
                        break;

                    case CardScoringType.Majority:
                        break;
                }
            }

            return score;
        }

        private static int ScoreSimplePoint(CardDefinition definition, int count)
        {
            return count * definition.pointValue;
        }

        private static int ScoreBySets(CardDefinition definition, int count)
        {
            if (definition.requiredCount <= 0)
            {
                return 0;
            }

            int completedSets = count / definition.requiredCount;
            return completedSets * definition.pointValue;
        }

        private static void ApplyMajorityScores(List<PlayerState> players)
        {
            var majorityDefinitions = players
                .SelectMany(player => player.PlayedCards)
                .Select(card => card.Definition)
                .Where(definition => definition.scoringType == CardScoringType.Majority)
                .GroupBy(definition => definition.cardId)
                .Select(group => group.First());

            foreach (CardDefinition majorityDefinition in majorityDefinitions)
            {
                ApplyMajorityScoreForCard(players, majorityDefinition);
            }
        }

        private static void ApplyMajorityScoreForCard(
            List<PlayerState> players,
            CardDefinition majorityDefinition
        )
        {
            int highestCount = 0;

            foreach (PlayerState player in players)
            {
                int count = CountCard(player, majorityDefinition.cardId);

                if (count > highestCount)
                {
                    highestCount = count;
                }
            }

            if (highestCount == 0)
            {
                return;
            }

            foreach (PlayerState player in players)
            {
                int count = CountCard(player, majorityDefinition.cardId);

                if (count == highestCount)
                {
                    player.Score += majorityDefinition.pointValue;
                }
            }
        }

        private static int CountCard(PlayerState player, string cardId)
        {
            return player.PlayedCards.Count(card => card.Definition.cardId == cardId);
        }
    }
}