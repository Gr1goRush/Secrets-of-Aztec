using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct SuperGameCardData
{
    public AnimatorOverrideController controller;
    public float multiplier;
    public bool nullifier;
}

public class SuperGamePanel : MonoBehaviour
{
    [SerializeField] private GameObject header, winPanel, content;
    [SerializeField] private TextMeshProUGUI winText;

    [SerializeField] private SuperGameCardData[] cardsData;
    [SerializeField] private SuperGameCard[] cardsObjects;

    private int selectedObjectIndex = -1, lastSelectedCardIndex = -1, sameCardSelectsCount = 0;

    private int[] cardsIndexes;

    private const int sameCardSelectsCountCondition = 3;

    void Start()
    {
        for (int i = 0; i < cardsObjects.Length; i++)
        {
            int index = i;
            cardsObjects[i].AddClickListener(() => SelectCard(index));
            cardsObjects[i].OnSelectAnimationShowedEvent += OnSelectAnimationShowed;
        }
    }

    public void Show()
    {
        sameCardSelectsCount = 0;
        selectedObjectIndex = -1;
        lastSelectedCardIndex = -1;

        gameObject.SetActive(true);

        content.SetActive(false);
        winPanel.SetActive(false);
        header.SetActive(true);

        Invoke(nameof(ShowContent), 1.5f);
    }

    private void ShowContent()
    {
        content.SetActive(true);
        header.SetActive(false);
        winPanel.SetActive(false);

        cardsIndexes = Utility.GetRandomEnumerable(Enumerable.Range(0, cardsData.Length)).ToArray();

        for (int i = 0; i < cardsObjects.Length; i++)
        {
            SuperGameCard cardObject = cardsObjects[i];
            cardObject.SetInteractable(true);

            int cardIndex = cardsIndexes[i];
            cardObject.SetAnimatorController(cardsData[cardIndex].controller);

            cardObject.SetDefault();
        }
    }

    private void SelectCard(int index)
    {
       selectedObjectIndex = index;
        cardsObjects[index].ShowSelectAnimation();
    }

    private void OnSelectAnimationShowed()
    {
        SetAllCardsInteractable(false);

        int cardIndex = cardsIndexes[selectedObjectIndex];
        if(lastSelectedCardIndex >= 0 && cardIndex != lastSelectedCardIndex)
        {
            Miss();
            return;
        }

        lastSelectedCardIndex = cardIndex;

        sameCardSelectsCount++;
        if(sameCardSelectsCount >= sameCardSelectsCountCondition)
        {
            Win();
            return;
        }

        Invoke(nameof(ShowContent), 1.5f);
    }

    private void Miss()
    {
        GameController.Instance.SuperGameCompleted(false, -1);

        gameObject.SetActive(false);
    }

    private void Win()
    {
        int cardIndex = cardsIndexes[selectedObjectIndex];
        SuperGameCardData superGameCardData = cardsData[cardIndex];

        if (!superGameCardData.nullifier)
        {
            winText.text = superGameCardData.multiplier.ToString();
            winPanel.SetActive(true);

            content.SetActive(false);

            GameSoundsController.Instance.PlayOneShot("super_game_win");

            Invoke(nameof(SuperGameCompleted), 1.5f);
        }

        SuperGameCompleted();
    }

    private void SuperGameCompleted()
    {
        int cardIndex = cardsIndexes[selectedObjectIndex];
        SuperGameCardData superGameCardData = cardsData[cardIndex];

        GameController.Instance.SuperGameCompleted(superGameCardData.nullifier, superGameCardData.multiplier);

        gameObject.SetActive(false);
    }

    private void SetAllCardsInteractable(bool interactable)
    {
        for (int i = 0; i < cardsObjects.Length; i++)
        {
            cardsObjects[i].SetInteractable(interactable);
        }
    }
}