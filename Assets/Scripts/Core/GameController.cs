using System.Collections.Generic;
using UnityEngine;
using CuteAliens.Data;

namespace CuteAliens.Core
{
    public class GameController : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private DeckDefinition starterDeck;
        [SerializeField] private GameSetupSettings setupSettings;

        [Header("UI")]
        [SerializeField] private GameUIView gameUIView;

        [Header("Audio")]
        [SerializeField] private SoundManager soundManager;

        public List<PlayerState> Players { get; private set; }
        public List<CardInstance> Deck { get; private set; }
        public int ActivePlayerIndex { get; private set; }
        public bool IsRoundOver { get; private set; }
        public int CurrentRound { get; private set; }
        public int MaxRounds { get; private set; }
        public bool IsGameOver { get; private set; }

        private int selectionsThisCycle;

        private void Start()
        {
            //
        }

        public void StartGameFromMenu()
        {
            StartGame();

            if (gameUIView != null)
            {
                gameUIView.HideMainMenu();
            }
        }

        public void StartGame()
        {
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

            MaxRounds = setupSettings.roundCount;
            CurrentRound = 1;
            IsGameOver = false;

            Players = CreatePlayers(setupSettings.playerCount);

            StartRound();

            Debug.Log("Game started.");
            Debug.Log($"Active player: {GetActivePlayer().Name}");

            PrintGameState();
            RefreshUI();
        }

        private void StartRound()
        {
            Deck = DeckBuilder.BuildDeck(starterDeck);
            DeckBuilder.Shuffle(Deck);

            foreach (PlayerState player in Players)
            {
                player.PrepareForNextRound();
            }

            DealStartingHands(Deck, Players, setupSettings.startingHandSize);

            ActivePlayerIndex = 0;
            IsRoundOver = false;
            selectionsThisCycle = 0;

            Debug.Log($"Round {CurrentRound} started.");
            Debug.Log($"Active player: {GetActivePlayer().Name}");

            PrintGameState();
            RefreshUI();
        }

        public PlayerState GetActivePlayer()
        {
            return Players[ActivePlayerIndex];
        }

        public void PlayCardForActivePlayer(CardInstance card)
        {
            if (IsRoundOver)
            {
                Debug.LogWarning("Round is already over.");
                return;
            }

            PlayerState activePlayer = GetActivePlayer();

            bool success = activePlayer.PlayCard(card);

            if (!success)
            {
                Debug.LogWarning($"{activePlayer.Name} could not play card.");
                return;
            }

            Debug.Log($"{activePlayer.Name} played {card.DisplayName}.");

            if (soundManager != null)
            {
                soundManager.PlayCardPlay();
            }

            selectionsThisCycle++;

            if (CheckRoundOver())
            {
                EndRound();
                return;
            }

            if (selectionsThisCycle >= Players.Count)
            {
                PassHands();
                selectionsThisCycle = 0;
            }

            AdvanceTurn();
            PrintGameState();
            RefreshUI();
        }

        private void PassHands()
        {
            Debug.Log("Passing hands...");

            List<List<CardInstance>> oldHands = new List<List<CardInstance>>();

            foreach (PlayerState player in Players)
            {
                oldHands.Add(new List<CardInstance>(player.Hand));
            }

            for (int i = 0; i < Players.Count; i++)
            {
                int previousPlayerIndex = i - 1;

                if (previousPlayerIndex < 0)
                {
                    previousPlayerIndex = Players.Count - 1;
                }

                Players[i].Hand.Clear();
                Players[i].Hand.AddRange(oldHands[previousPlayerIndex]);
            }

            Debug.Log("Hands passed.");
        }

        private void AdvanceTurn()
        {
            ActivePlayerIndex++;

            if (ActivePlayerIndex >= Players.Count)
            {
                ActivePlayerIndex = 0;
            }

            Debug.Log($"Next active player: {GetActivePlayer().Name}");
        }

        private bool CheckRoundOver()
        {
            foreach (PlayerState player in Players)
            {
                if (player.Hand.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void EndRound()
        {
            IsRoundOver = true;

            ScoringSystem.CalculateScores(Players);
            AddRoundScoresToTotalScores();

            if (CurrentRound >= MaxRounds)
            {
                IsGameOver = true;
                Debug.Log("Game ended.");
            }

            Debug.Log($"Round {CurrentRound} ended.");
            PrintGameState();

            if (IsGameOver)
            {
                PrintWinner();
            }

            RefreshUI();
        }

        private void AddRoundScoresToTotalScores()
        {
            foreach (PlayerState player in Players)
            {
                player.TotalScore += player.Score;
            }
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

        public void PrintGameState()
        {
            foreach (PlayerState player in Players)
            {
                Debug.Log(
                $"{player.Name} hand: {player.Hand.Count}, " +
                $"played: {player.PlayedCards.Count}, " +
                $"round score: {player.Score}, " +
                $"total score: {player.TotalScore}"
);
            }
        }

        private void PrintWinner()
        {
            int highestScore = int.MinValue;

            foreach (PlayerState player in Players)
            {
                if (player.Score > highestScore)
                {
                    highestScore = player.Score;
                }
            }

            List<PlayerState> winners = new List<PlayerState>();

            foreach (PlayerState player in Players)
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

        public string GetRoundResultText()
        {
            string text = $"Round {CurrentRound} Over\n";

            foreach (PlayerState player in Players)
            {
                text += $"{player.Name}: +{player.Score} this round | Total: {player.TotalScore}\n";
            }

            if (IsGameOver)
            {
                text += GetWinnerText();
            }
            else
            {
                text += "Ready for next round?";
            }

            return text;
        }

        public string GetWinnerText()
        {
            int highestScore = int.MinValue;

            foreach (PlayerState player in Players)
            {
                if (player.TotalScore > highestScore)
                {
                    highestScore = player.TotalScore;
                }
            }

            List<PlayerState> winners = new List<PlayerState>();

            foreach (PlayerState player in Players)
            {
                if (player.TotalScore == highestScore)
                {
                    winners.Add(player);
                }
            }

            if (winners.Count == 1)
            {
                return $"Final Winner: {winners[0].Name}\nTotal Score: {highestScore}";
            }

            string winnerNames = "";

            for (int i = 0; i < winners.Count; i++)
            {
                winnerNames += winners[i].Name;

                if (i < winners.Count - 1)
                {
                    winnerNames += ", ";
                }
            }

            return $"Final Tie!\nWinners: {winnerNames}\nTotal Score: {highestScore}";
        }

        public void RestartGame()
        {
            StartGame();
        }
        private void RefreshUI()
        {
            if (gameUIView != null)
            {
                gameUIView.Refresh();
            }
        }

        public void ContinueAfterRound()
        {
            if (!IsRoundOver)
            {
                return;
            }

            if (IsGameOver)
            {
                ReturnToMainMenu();
                return;
            }

            CurrentRound++;
            StartRound();
        }

        public void ReturnToMainMenu()
        {
            IsRoundOver = false;
            IsGameOver = false;
            CurrentRound = 0;
            ActivePlayerIndex = 0;
            selectionsThisCycle = 0;

            if (Players != null)
            {
                Players.Clear();
            }

            if (Deck != null)
            {
                Deck.Clear();
            }

            if (gameUIView != null)
            {
                gameUIView.ShowMainMenu();
                gameUIView.Refresh();
            }
        }

        public void QuitGame()
    {
        Debug.Log("Quit Game requested.");

        Application.Quit();
    }
    }
}