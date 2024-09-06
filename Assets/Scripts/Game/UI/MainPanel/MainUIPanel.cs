using System.Collections;
using TMPro;
using UnityEngine;

public class MainUIPanel : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI winAmountText;

    public void SetWinAmount(int amount)
    {
        winAmountText.text = amount.ToString();
    }
}