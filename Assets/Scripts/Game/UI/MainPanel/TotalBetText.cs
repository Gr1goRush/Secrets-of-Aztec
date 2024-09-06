using System.Collections;
using TMPro;
using UnityEngine;

public class TotalBetText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {
        SetTotalBet(BetController.Instance.TotalBet);

        BetController.Instance.OnTotalBetChanged += SetTotalBet;
    }

    void SetTotalBet(int amount)
    {
        _text.text = amount.ToString();
    }

    private void OnDestroy()
    {
        if (BetController.Instance != null)
        {
            BetController.Instance.OnTotalBetChanged -= SetTotalBet;
        }
    }
}