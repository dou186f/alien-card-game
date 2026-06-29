using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CuteAliens.Core;

public class GameUIView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameController gameController;

    [Header("Texts")]
    [SerializeField] private TMP_Text activePlayerText;
    [SerializeField] private TMP_Text player1StatusText;
    [SerializeField] private TMP_Text player2StatusText;

    [Header("Card UI")]
    [SerializeField] private CardView cardViewPrefab;

    [SerializeField] private Transform player1HandContainer;
    [SerializeField] private Transform player2HandContainer;

    [SerializeField] private Transform player1PlayedContainer;
    [SerializeField] private Transform player2PlayedContainer;

    [Header("Result UI")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private TMP_Text resultButtonText;

    [Header("Menu UI")]
    [SerializeField] private GameObject mainMenuPanel;

    private void Start()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        Refresh();
    }

    public void Refresh()
    {
        if (gameController == null)
        {
            Debug.LogWarning("GameController reference is missing.");
            return;
        }

        List<PlayerState> players = gameController.Players;

        if (players == null || players.Count == 0)
        {
            activePlayerText.text = "Game not started.";
            player1StatusText.text = "";
            player2StatusText.text = "";

            ClearAllCardContainers();

            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }
            return;
        }

        if (gameController.IsRoundOver)
        {
            if (gameController.IsGameOver)
            {
                activePlayerText.text = "GAME OVER";
            }
            else
            {
                activePlayerText.text = $"ROUND {gameController.CurrentRound} OVER";
            }
        }
        else
        {
            activePlayerText.text =
                $"Round {gameController.CurrentRound}/{gameController.MaxRounds} - " +
                $"{gameController.GetActivePlayer().Name}'s Turn";
        }

        player1StatusText.text = BuildPlayerHeaderText(players[0]);

        if (players.Count > 1)
        {
            player2StatusText.text = BuildPlayerHeaderText(players[1]);
        }
        else
        {
            player2StatusText.text = "";
        }

        RefreshCards();
        RefreshResultPanel();
    }

    private string BuildPlayerHeaderText(PlayerState player)
    {
        bool isActive =
            !gameController.IsRoundOver &&
            gameController.GetActivePlayer() == player;

        string turnText = isActive ? "YOUR TURN" : "Waiting...";

        return
            $"{player.Name.ToUpper()}    Total: {player.TotalScore}\n" +
            $"Round: {player.Score}    {turnText}";
    }

    private void RefreshCards()
    {
        ClearContainer(player1HandContainer);
        ClearContainer(player2HandContainer);
        ClearContainer(player1PlayedContainer);
        ClearContainer(player2PlayedContainer);

        if (gameController == null || gameController.Players == null)
        {
            return;
        }

        List<PlayerState> players = gameController.Players;

        if (players.Count < 2)
        {
            return;
        }

        PlayerState player1 = players[0];
        PlayerState player2 = players[1];

        bool player1IsActive = !gameController.IsRoundOver && gameController.GetActivePlayer() == player1;
        bool player2IsActive = !gameController.IsRoundOver && gameController.GetActivePlayer() == player2;

        CreateCardViews(player1.Hand, player1HandContainer, player1IsActive, !player1IsActive);
        CreateCardViews(player2.Hand, player2HandContainer, player2IsActive, !player2IsActive);

        CreateCardViews(player1.PlayedCards, player1PlayedContainer, false, false);
        CreateCardViews(player2.PlayedCards, player2PlayedContainer, false, false);
    }

    private void CreateCardViews(
        List<CardInstance> cards,
        Transform container,
        bool interactable,
        bool hidden
    )
    {
        foreach (CardInstance card in cards)
        {
            CardView cardView = Instantiate(cardViewPrefab, container);
            cardView.Setup(card, OnCardClicked, interactable, hidden);
        }
    }

    private void ClearContainer(Transform container)
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    private void OnCardClicked(CardInstance card)
    {
        if (gameController == null)
        {
            Debug.LogWarning("Cannot play card because GameController is missing.");
            return;
        }

        gameController.PlayCardForActivePlayer(card);
    }

    private void RefreshResultPanel()
    {
        if (resultPanel == null || winnerText == null)
        {
            return;
        }

        if (gameController == null || gameController.Players == null)
        {
            resultPanel.SetActive(false);
            return;
        }

        if (gameController.IsRoundOver)
        {
            resultPanel.SetActive(true);
            winnerText.text = gameController.GetRoundResultText();

            if (resultButtonText != null)
            {
                if (gameController.IsGameOver)
                {
                    resultButtonText.text = "Main Menu";
                }
                else
                {
                    resultButtonText.text = "Next Round";
                }
            }
        }
        else
        {
            resultPanel.SetActive(false);
        }
    }

    public void HideMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    private void ClearAllCardContainers()
    {
        ClearContainer(player1HandContainer);
        ClearContainer(player2HandContainer);
        ClearContainer(player1PlayedContainer);
        ClearContainer(player2PlayedContainer);
    }
}