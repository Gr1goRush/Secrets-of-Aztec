using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : Singleton<TutorialController>
{
    [SerializeField] private GameObject tutorialPanel;

    void Start()
    {
        if(PlayerPrefs.GetInt("TutorialShowed", 0) != 1)
        {
            tutorialPanel.SetActive(true);
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void Stop()
    {
        if (tutorialPanel.activeSelf)
        {
            tutorialPanel.SetActive(false);
            PlayerPrefs.SetInt("TutorialShowed", 1);
        }
    }
}
