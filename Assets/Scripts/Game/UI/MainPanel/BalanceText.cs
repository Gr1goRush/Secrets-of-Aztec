using System.Collections;
using TMPro;
using UnityEngine;

public class BalanceText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {
        SetBalance(BalanceManager.Instance.Balance);

        BalanceManager.Instance.OnBalanceChanged += SetBalance;
    }

    void SetBalance(int amount)
    {
        _text.text = amount.ToString();
    }

    private void OnDestroy()
    {
        if (BalanceManager.Instance != null)
        {
            BalanceManager.Instance.OnBalanceChanged -= SetBalance;
        }
    }
}