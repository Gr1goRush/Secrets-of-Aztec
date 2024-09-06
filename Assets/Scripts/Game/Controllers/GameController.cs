using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public SlotsCombinationsStorage SlotsCombinationsStorage => slotsCombinationsStorage;

    [SerializeField] private float highlightTime = 1.5f, rewardShowTime = 2f;

    [SerializeField] private Playground playground;
    [SerializeField] private SlotsCombinationsStorage slotsCombinationsStorage;

    private bool isSpining = false, superGame = false, autoPlay = false;

    private int totalWinAmount = 0, lastRollWinAmount = 0;

    void Start()
    {
        playground.SlotsGrid.Spawn();

        UpdateWinAmountText();
    }

    private void UpdateWinAmountText()
    {
        UIController.Instance.MainPanel.SetWinAmount(totalWinAmount);
    }

    public void SwitchAutoPlay()
    {
        autoPlay = !autoPlay;
        if(autoPlay && !isSpining)
        {
            StartSpin();
        }
    }

    public void StartSpin()
    {
        if (isSpining)
        {
            return;
        }    

        if(BetController.Instance.PerLine > BalanceManager.Instance.Balance)
        {
            return;
        }

        TutorialController.Instance.Stop();
        GameSoundsController.Instance.PlayOneShot("spin_start");

        Invoke(nameof(PlaySpin), 0.5f);

        BetController.Instance.UpdateTotalBet();
        BalanceManager.Instance.SubtractBalance(BetController.Instance.PerLine);

        isSpining = true;
        superGame = false;

        playground.SlotsGrid.StartRoll();
    }

    private void PlaySpin()
    {
        GameSoundsController.Instance.PlayOneShot("spin");
    }

    public void SpinStopped(SlotsColumnState[] columnStates)
    {
        GameSoundsController.Instance.PlayOneShot("spin_stop");

        superGame = slotsCombinationsStorage.IsSuperGame(columnStates);

        List<int> cardIndexes = new List<int>();
        for (int columnStateIndex = 0; columnStateIndex < columnStates.Length; columnStateIndex++)
        {
            SlotsColumnState columnState = columnStates[columnStateIndex];

            for (int slotIndex = 0; slotIndex < columnState.slotsCardsIndexes.Length; slotIndex++)
            {
                int slotCardIndex = columnState.slotsCardsIndexes[slotIndex];
                if (cardIndexes.Contains(slotCardIndex))
                {
                    continue;
                }

                cardIndexes.Add(slotCardIndex);
            }
        }

        List<OverlappedCombinationReward> rewards = slotsCombinationsStorage.GetOverlappedCombinations(cardIndexes, columnStates);
        StartCoroutine(RewardsShow(rewards));
    }

    private IEnumerator RewardsShow(List<OverlappedCombinationReward> rewards)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            OverlappedCombinationReward overlappedCombinationReward = rewards[i];
            playground.SlotsGrid.HighlighCombination(overlappedCombinationReward.columnOverlappedCombinations);

            yield return new WaitForSeconds(highlightTime);

            UIController.Instance.WinPanel.Show(overlappedCombinationReward.reward);

            yield return new WaitForSeconds(rewardShowTime);

            UIController.Instance.WinPanel.Hide();

            lastRollWinAmount += overlappedCombinationReward.reward;
            UpdateWinAmountText();
        }

        if(lastRollWinAmount > 0)
        {
            GameSoundsController.Instance.PlayOneShot("win");
        }

        playground.SlotsGrid.UnhighlightAll();

        if (superGame && !autoPlay)
        {
            UIController.Instance.SuperGamePanel.Show();
        }
        else
        {
            FinishRollCycle();
        }
    }

    private void FinishRollCycle()
    {
        totalWinAmount += lastRollWinAmount;
        lastRollWinAmount = 0;
        BalanceManager.Instance.AddBalance(totalWinAmount);

        UpdateWinAmountText();

        isSpining = false;

        if (autoPlay)
        {
            StartSpin();
        }
    }

    public void SuperGameCompleted(bool nullifier, float multiplier)
    {
        if(nullifier)
        {
            lastRollWinAmount = 0;
        }
        else if(multiplier > 0)
        {
            lastRollWinAmount = Mathf.RoundToInt(lastRollWinAmount * multiplier);
        }

        FinishRollCycle();
    }
}
