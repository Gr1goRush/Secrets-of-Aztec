using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggleSwitchSound : MonoBehaviour
{
    void Awake()
    {
        GetComponent<CustomToggle>().onSwitched.AddListener(ToggleSwitch);
    }

    void ToggleSwitch(bool v)
    {
        AudioManager.Instance.PlayButtonClick();
    }
}
