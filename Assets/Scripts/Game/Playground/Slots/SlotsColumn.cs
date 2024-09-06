using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SlotsColumnState
{
    public int[] slotsCardsIndexes;
}

public class SlotsColumn : MonoBehaviour
{
    public bool RollStopped { get; private set; }

    [SerializeField] private float spacing = 10f;

    [SerializeField] private Slot slotOriginal;
    [SerializeField] private RectTransform slotsParent;

    private bool rolling = false, slowingDown = false, bringing = false;
    private float rollSpeed = 1f, minRollSpeed = 1f, slowDownSpeed = 1f;
    private float bringDirection = 0;

    private Vector3 topEdgeSlotPosition, topFirstSlotPosition;
    private Vector2 slotSize;

    private List<Slot> slots;

    public void SpawnSlots(int count)
    {
        slots = new List<Slot>();

        slotSize = slotOriginal.RectTransform.rect.size;

        Vector3 columnSize = new Vector3(slotSize.x, slotSize.y * count + (spacing * (count - 1)), 0f);
        slotsParent.sizeDelta = columnSize;

        topFirstSlotPosition = new Vector3(0, (-slotSize.y / 2), 0);

        for (int i = 0; i < count; i++)
        {
            Slot slot = i == 0 ? slotOriginal : Instantiate(slotOriginal, slotsParent);
            slot.name = "Slot " + i;
            RectTransform rectTransform = slot.RectTransform;

            slot.SetRandomCard();

            rectTransform.anchoredPosition = GetSlotPosition(i);

            slots.Add(slot);
        }

        topEdgeSlotPosition = slots[0].RectTransform.anchoredPosition + new Vector2(0f, slotSize.y);
    }

    private Vector3 GetSlotPosition(int index)
    {
        return new Vector3(0, (-slotSize.y / 2) - ((slotSize.y + spacing) * index), 0);
    }

    public void StartRoll(float speed, float minSpeed)
    {
        rollSpeed = speed;
        minRollSpeed = minSpeed;

        StartCoroutine(Roll());
    }

    public void SlowDownAndBring(float speed)
    {
        slowDownSpeed = speed;
        slowingDown = true;
    }

    IEnumerator Roll()
    {
        rolling = true;
        slowingDown = false;
        bringing = false;
        RollStopped = false;

        while (rolling)
        {
            if (slowingDown && !bringing)
            {
                rollSpeed -= Time.deltaTime * slowDownSpeed;
                if (rollSpeed <= minRollSpeed)
                {
                    bringing = true;
                    rollSpeed = minRollSpeed;

                    Vector3 pos = slots[0].RectTransform.anchoredPosition;
                    bringDirection = Mathf.Sign(topFirstSlotPosition.y - pos.y);
                }
            }

            Slot topRollSlot = null;

            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                RectTransform rectTransform = slot.RectTransform;

                Vector3 currentPosition = rectTransform.anchoredPosition;

                float offset = Time.deltaTime * rollSpeed;

                if (bringing)
                {
                    currentPosition.y += (offset * bringDirection);

                    if (rolling && Mathf.Abs(topFirstSlotPosition.y - currentPosition.y) <= offset)
                    {
                        rolling = false;
                        slowingDown = false;
                        bringing = false;
                    }
                }
                else
                {
                    if (currentPosition.y >= topEdgeSlotPosition.y)
                    {
                        currentPosition.y = slots[slots.Count - 1].RectTransform.anchoredPosition.y - slotSize.y - spacing;
                        topRollSlot = slot;
                    }

                    currentPosition.y += offset;
                }

                rectTransform.anchoredPosition = currentPosition;
            }

            if (topRollSlot != null)
            {
                slots.RemoveAt(0);
                slots.Add(topRollSlot);

                topRollSlot.SetRandomCard();
            }

            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i];
            RectTransform rectTransform = slot.RectTransform;

            rectTransform.anchoredPosition = GetSlotPosition(i);
        }

        OnRollStop();
    }

    private void OnRollStop()
    {
        RollStopped = true;
    }

    public SlotsColumnState GetState()
    {
        int[] _cardIndexes = new int[SlotsCombination.columnSlotsCount];
        for (int i = 0; i < _cardIndexes.Length; i++)
        {
            _cardIndexes[i] = slots[i].CardIndex;
        }

        return new SlotsColumnState { slotsCardsIndexes = _cardIndexes };
    }

    public Slot GetSlot(int slotIndex)
    {
        return slots[slotIndex];
    }
}
