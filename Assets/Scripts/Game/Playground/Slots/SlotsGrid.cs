using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SlotsGrid : MonoBehaviour
{
    [SerializeField] private int columnSpawnSlotsCount = 10;
    [SerializeField] private float rollSpeed = 200f, slowDownSpeed = 100f, minRollSpeed = 50f, rollTime = 3f, slowDownInterval = 0.5f;

    [SerializeField] private SlotsColumn[] columns;

    public void Spawn()
    {
        SpawnSlots();
    }

    public void StartRoll()
    {
        foreach (var item in columns)
        {
            item.StartRoll(rollSpeed, minRollSpeed);
        }

        StartCoroutine(Roll());
    }

    private void SpawnSlots()
    {
        for (int i = 0; i < columns.Length; i++)
        {
            SlotsColumn slotsColumn = columns[i];
            slotsColumn.SpawnSlots(columnSpawnSlotsCount);
        }
    }

    private IEnumerator Roll()
    {
        yield return new WaitForSeconds(rollTime);

        for (int i = 0; i < columns.Length; i++)
        {
            SlotsColumn slotsColumn = columns[i];
            slotsColumn.SlowDownAndBring(slowDownSpeed);

            yield return new WaitForSeconds(slowDownInterval);
        }

        while (!AllColumnsStopped())
        {
            yield return null;
        }

        SlotsColumnState[] columnStates = new SlotsColumnState[columns.Length];
        for (int i = 0; i < columnStates.Length; i++)
        {
            columnStates[i] = columns[i].GetState();
        }

        GameController.Instance.SpinStopped(columnStates);
    }

    private bool AllColumnsStopped()
    {
        for (int i = 0; i < columns.Length; i++)
        {
            if (!columns[i].RollStopped)
            {
                return false;
            }
        }

        return true;
    }

    public void HighlighCombination(SlotsColumnOverlappedCombination[] overlappedCombinations)
    {
        for (int columnIndex = 0; columnIndex < overlappedCombinations.Length; columnIndex++)
        {
            SlotsColumnOverlappedCombination overlappedCombination = overlappedCombinations[columnIndex];
            for (int slotIndex = 0; slotIndex < SlotsCombination.columnSlotsCount; slotIndex++)
            {
                columns[columnIndex].GetSlot(slotIndex).SetCoverActive(!overlappedCombination.overlapIndexes.Contains(slotIndex));
            }
        }
    }

    public void UnhighlightAll()
    {
        for (int columnIndex = 0; columnIndex < SlotsCombination.columnsCount; columnIndex++)
        {
            for (int slotIndex = 0; slotIndex < SlotsCombination.columnSlotsCount; slotIndex++)
            {
                columns[columnIndex].GetSlot(slotIndex).SetCoverActive(false);
            }
        }
    }
}
