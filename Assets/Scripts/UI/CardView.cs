using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CuteAliens.Core;

public class CardView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button button;
    [SerializeField] private Image artworkImage;
    [SerializeField] private Sprite cardBackSprite;

    private CardInstance card;
    private Action<CardInstance> onClicked;

public void Setup(
    CardInstance cardInstance,
    Action<CardInstance> clickCallback,
    bool interactable,
    bool hidden
)
{
    card = cardInstance;
    onClicked = clickCallback;

    if (hidden)
    {
        if (nameText != null)
        {
            nameText.text = "";
        }

        if (artworkImage != null)
        {
            artworkImage.sprite = cardBackSprite;
            artworkImage.enabled = cardBackSprite != null;
        }

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }

        return;
    }

    if (nameText != null)
    {
        nameText.text = "";
    }

    if (artworkImage != null)
    {
        artworkImage.sprite = card.Definition.artwork;
        artworkImage.enabled = card.Definition.artwork != null;
    }

    if (button != null)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
        button.interactable = interactable;
    }
}

    private void OnClicked()
    {
        if (card == null)
        {
            Debug.LogWarning("Clicked CardView has no card assigned.");
            return;
        }

        onClicked?.Invoke(card);
    }
}