using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int CardIndex => cardIndex;
    public Image Image => _image;
    public RectTransform RectTransform => transform as RectTransform;

    [SerializeField] private Image _image;
    [SerializeField] private GameObject _cover;

    private int cardIndex;

    public void SetRandomCard()
    {
        cardIndex = GameController.Instance.SlotsCombinationsStorage.GetRandomCardIndex();
        _image.sprite = GameController.Instance.SlotsCombinationsStorage.GetCardSprite(cardIndex);
    }

    public void SetCoverActive(bool coverActive)
    {
        _cover.SetActive(coverActive);
    }
}
