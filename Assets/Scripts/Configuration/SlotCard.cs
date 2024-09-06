using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SlotCardReward
{
    public int overlapsCount;
    public int amount;
}

[System.Serializable]
public struct SlotCard
{
    public Sprite Sprite => sprite;
    [SerializeField] private Sprite sprite;

    [SerializeField] private SlotCardReward[] rewards;

    public int FindReward(int overlapsCount)
    {
        foreach (SlotCardReward item in rewards)
        {
            if(item.overlapsCount == overlapsCount)
            {
                return item.amount * item.overlapsCount;
            }
        }

        return 0;
    }
}
