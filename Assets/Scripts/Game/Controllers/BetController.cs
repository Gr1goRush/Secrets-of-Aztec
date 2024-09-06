using System.Collections;
using UnityEngine;

public class BetController : Singleton<BetController>
{

   public int PerLine {  get; private set; }
    public int TotalBet {  get; private set; }

    public event GameActionInt OnTotalBetChanged;

    private const int perLineChangeAmount = 10;

    protected override void Awake()
    {
        base.Awake();

        PerLine = 10;
        TotalBet = 0;
    }

    public void UpdateTotalBet()
    {
        TotalBet += PerLine;

        OnTotalBetChanged?.Invoke(TotalBet);
    }

    public void IncreasePerLine()
    {
        PerLine += perLineChangeAmount;
        if(PerLine > BalanceManager.Instance.Balance)
        {
            PerLine = BalanceManager.Instance.Balance;
        }
    }

    public bool CanIncreasePerLine()
    {
        return PerLine < BalanceManager.Instance.Balance;
    }

    public void DecreasePerLine()
    {
        PerLine -= perLineChangeAmount;
    }

    public bool CanDecreasePerLine()
    {
        return PerLine > 0;
    }
}