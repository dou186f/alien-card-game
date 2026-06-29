using System.Collections.Generic;
using UnityEngine;
using CuteAliens.Data;
using CuteAliens.Core;

public class MVPBootstrap : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private DeckDefinition starterDeck;
    [SerializeField] private GameSetupSettings setupSettings;

    private void Start()
    {
        Debug.Log("MVP Bootstrap started.");

        if (starterDeck == null)
        {
            Debug.LogWarning("Starter Deck is not assigned.");
            return;
        }

        if (setupSettings == null)
        {
            Debug.LogWarning("Setup Settings is not assigned.");
            return;
        }

        List<CardInstance> deck = DeckBuilder.BuildDeck(starterDeck);
        DeckBuilder.Shuffle(deck);

        Debug.Log($"Deck created and shuffled. Card count: {deck.Count}");

        List<PlayerState> players = CreatePlayers(setupSettings.playerCount);

        DealStartingHands(deck, players, setupSettings.startingHandSize);

        Debug.Log("=== BEFORE PLAYING CARDS ===");
        PrintPlayers(players);

        PlayFullRound(players);

        Debug.Log("=== AFTER ROUND END, BEFORE SCORING ===");
        PrintPlayers(players);

        ScoringSystem.CalculateScores(players);

        Debug.Log("=== AFTER SCORING ===");
        PrintPlayers(players);

        PrintWinner(players);

        Debug.Log($"Cards remaining in deck: {deck.Count}");
    }

    private List<PlayerState> CreatePlayers(int playerCount)
    {
        List<PlayerState> players = new List<PlayerState>();

        for (int i = 0; i < playerCount; i++)
        {
            PlayerState player = new PlayerState($"Player {i + 1}");
            players.Add(player);
        }

        return players;
    }

    private void DealStartingHands(
        List<CardInstance> deck,
        List<PlayerState> players,
        int handSize
    )
    {
        for (int cardIndex = 0; cardIndex < handSize; cardIndex++)
        {
            foreach (PlayerState player in players)
            {
                if (deck.Count == 0)
                {
                    Debug.LogWarning("Deck ran out of cards while dealing.");
                    return;
                }

                CardInstance topCard = deck[0];
                deck.RemoveAt(0);

                player.AddCardToHand(topCard);
            }
        }
    }

    private void PlayFullRound(List<PlayerState> players)
    {
        int turnNumber = 1;

        while (AnyPlayerHasCardsInHand(players))
        {
            Debug.Log($"=== TURN {turnNumber} ===");

            foreach (PlayerState player in players)
            {
                PlayFirstCardForPlayer(player);
            }

            turnNumber++;
        }

        Debug.Log("Round finished. All hands are empty.");
    }

    private bool AnyPlayerHasCardsInHand(List<PlayerState> players)
    {
        foreach (PlayerState player in players)
        {
            if (player.Hand.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    private void PlayFirstCardForPlayer(PlayerState player)
    {
        if (player.Hand.Count == 0)
        {
            Debug.Log($"{player.Name} has no cards left.");
            return;
        }

        CardInstance selectedCard = player.Hand[0];

        bool success = player.PlayCard(selectedCard);

        if (success)
        {
            Debug.Log($"{player.Name} played {selectedCard.DisplayName}.");
        }
        else
        {
            Debug.LogWarning($"{player.Name} could not play {selectedCard.DisplayName}.");
        }
    }

    private void PrintPlayers(List<PlayerState> players)
    {
        foreach (PlayerState player in players)
        {
            Debug.Log($"{player.Name} hand ({player.Hand.Count} cards):");

            for (int i = 0; i < player.Hand.Count; i++)
            {
                Debug.Log($"  Hand {i + 1}: {player.Hand[i].DisplayName}");
            }

            Debug.Log($"{player.Name} played cards ({player.PlayedCards.Count} cards):");

            for (int i = 0; i < player.PlayedCards.Count; i++)
            {
                Debug.Log($"  Played {i + 1}: {player.PlayedCards[i].DisplayName}");
            }

            Debug.Log($"{player.Name} score: {player.Score}");
        }
    }

    private void PrintWinner(List<PlayerState> players)
    {
        int highestScore = int.MinValue;

        foreach (PlayerState player in players)
        {
            if (player.Score > highestScore)
            {
                highestScore = player.Score;
            }
        }

        List<PlayerState> winners = new List<PlayerState>();

        foreach (PlayerState player in players)
        {
            if (player.Score == highestScore)
            {
                winners.Add(player);
            }
        }

        if (winners.Count == 1)
        {
            Debug.Log($"Winner: {winners[0].Name} with {highestScore} points!");
        }
        else
        {
            string winnerNames = "";

            for (int i = 0; i < winners.Count; i++)
            {
                winnerNames += winners[i].Name;

                if (i < winners.Count - 1)
                {
                    winnerNames += ", ";
                }
            }

            Debug.Log($"Tie! Winners: {winnerNames} with {highestScore} points!");
        }
    }
}