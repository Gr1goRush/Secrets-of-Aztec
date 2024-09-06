using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OverlappedCombinationReward
{
    public int combinationIndex, reward;
    public SlotsColumnOverlappedCombination[] columnOverlappedCombinations;
}

[CreateAssetMenu(fileName = "SlotsCombinations", menuName = "Game Data/ Slots Combinations")]
public class SlotsCombinationsStorage : ScriptableObject
{
    [SerializeField] private int superGameCardIndex = 2;

    [SerializeField] private SlotCard[] cards;
    [SerializeField] private SlotsCombination[] combinations;

    public int GetRandomCardIndex()
    {
        return Random.Range(0, cards.Length);
    }

    public Sprite GetCardSprite(int index)
    {
        return cards[index].Sprite;
    }

    public List<OverlappedCombinationReward> GetOverlappedCombinations(List<int> cardsIndexes, SlotsColumnState[] columnStates)
    {
        List<OverlappedCombinationReward> overlappedCombinations = new List<OverlappedCombinationReward> ();

        for (int combinationIndex = 0; combinationIndex < combinations.Length; combinationIndex++)
        {
            List<SlotsCardOverlapCombination> slotsCardCombinations = combinations[combinationIndex].FindOverlapCombinations(columnStates, cardsIndexes);
            int maxReward = 0;
            int overlapCombinationIndex = -1;
            for (int j = 0; j < slotsCardCombinations.Count; j++)
            {
                SlotsCardOverlapCombination slotsCardCombination = slotsCardCombinations[j];

                int overlapsCount = slotsCardCombination.overlapsCount;
                int cardIndex = slotsCardCombination.cardIndex;

                SlotCard slotCard = cards[cardIndex];
                int _reward = slotCard.FindReward(overlapsCount);
                if (_reward > 0 && _reward > maxReward)
                {
                    maxReward = _reward;
                    overlapCombinationIndex = j;
                }
            }

            if (maxReward > 0 && overlapCombinationIndex >= 0)
            {
                OverlappedCombinationReward overlappedCombinationReward = new OverlappedCombinationReward
                {
                    combinationIndex = combinationIndex,
                    reward = maxReward,
                    columnOverlappedCombinations = slotsCardCombinations[overlapCombinationIndex].overlappedCombinations
                };

                overlappedCombinations.Add(overlappedCombinationReward);
            }
        }

        return overlappedCombinations;
    }

    public bool IsSuperGame(SlotsColumnState[] columnStates)
    {
        int count = 0;

        for (int columnIndex = 0; columnIndex < columnStates.Length; columnIndex++)
        {
            SlotsColumnState columnState = columnStates[columnIndex];
            for (int i = 0; i < columnState.slotsCardsIndexes.Length; i++)
            {
                if (columnState.slotsCardsIndexes[i] == superGameCardIndex)
                {
                    count++;
                    if(count >= 2)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
