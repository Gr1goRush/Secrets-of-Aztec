using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public struct SlotsCardOverlapCombination
{
    public int cardIndex, overlapsCount;
    public SlotsColumnOverlappedCombination[] overlappedCombinations;
}

[System.Serializable]
public struct SlotsColumnOverlappedCombination
{
    public int[] overlapIndexes;
}

[System.Serializable]
public struct SlotsCombination
{
    public SlotsColumnOverlappedCombination[] columns;

    public const int columnsCount = 5, columnSlotsCount = 5;

    public List<SlotsCardOverlapCombination> FindOverlapCombinations(SlotsColumnState[] states, List<int> cardsIndexes)
    {
        List<SlotsCardOverlapCombination> cardCombinations = new List<SlotsCardOverlapCombination>();

        foreach (int _cardIndex in cardsIndexes)
        {
            SlotsCardOverlapCombination slotsCardCombination = GetCombinationOverlap(_cardIndex, states);
            if (slotsCardCombination.overlapsCount > 0)
            {
                cardCombinations.Add(slotsCardCombination);
            }
        }

        return cardCombinations;
    }

    private SlotsCardOverlapCombination GetCombinationOverlap(int _cardIndex, SlotsColumnState[] states)
    {
        SlotsColumnOverlappedCombination[] slotsColumnOverlappedCombinations = new SlotsColumnOverlappedCombination[columnsCount];
        int _overlapsCount = 0;

        for (int columnStateIndex = 0; columnStateIndex < states.Length; columnStateIndex++)
        {
            SlotsColumnState columnState = states[columnStateIndex];

            List<int> overlapsIndexes = new List<int>();

            for (int slotIndex = 0; slotIndex < columnState.slotsCardsIndexes.Length; slotIndex++)
            {
                int slotCardIndex = columnState.slotsCardsIndexes[slotIndex];
                if (slotCardIndex != _cardIndex)
                {
                    continue;
                }

                if (columns[columnStateIndex].overlapIndexes.Contains(slotIndex))
                {
                    overlapsIndexes.Add(slotIndex);
                    _overlapsCount++;
                }
            }

            slotsColumnOverlappedCombinations[columnStateIndex] = new SlotsColumnOverlappedCombination
            {
                overlapIndexes = overlapsIndexes.ToArray()
            };
        }

        return new SlotsCardOverlapCombination
        {
            cardIndex = _cardIndex,
            overlapsCount = _overlapsCount,
            overlappedCombinations = slotsColumnOverlappedCombinations
        };
    }
}
