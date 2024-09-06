using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerLinePanel : MonoBehaviour
{
    [SerializeField] private Button increaseButton, decreaseButton;
    [SerializeField] private TextMeshProUGUI perLineText;

    void Start()
    {
        SetPerLineText();
        SetButtonsActive();

        increaseButton.onClick.AddListener(Increase);
        decreaseButton.onClick.AddListener(Decrease);
    }

    void SetPerLineText()
    {
        perLineText.text = BetController.Instance.PerLine.ToString();
    }

    private void Increase()
    {
        BetController.Instance.IncreasePerLine();
        SetPerLineText();
        SetButtonsActive();
    }

    private void Decrease()
    {
        BetController.Instance.DecreasePerLine();
        SetPerLineText();
        SetButtonsActive();
    }

    private void SetButtonsActive()
    {
        increaseButton.gameObject.SetActive(BetController.Instance.CanIncreasePerLine());
        decreaseButton.gameObject.SetActive(BetController.Instance.CanDecreasePerLine());
    }
}
