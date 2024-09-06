using System.Collections;
using UnityEngine;

public delegate void GameActionInt(int value);

public class BalanceManager : Singleton<BalanceManager>
{

    public int Balance { get; private set; }

    public event GameActionInt OnBalanceChanged;

    protected override void Awake()
    {
        base.Awake();

        Balance = PlayerSaves.GetInt("Balance", 5000);
    }

    public void AddBalance(int amount)
    {
        Balance += amount;
        PlayerSaves.SetInt("Balance", Balance);

        OnBalanceChanged?.Invoke(Balance);
    }

    public void SubtractBalance(int amount)
    {
        AddBalance(-amount);
    }
}