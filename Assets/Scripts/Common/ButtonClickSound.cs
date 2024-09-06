using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ClickButton);
    }

    void ClickButton()
    {
        AudioManager.Instance.PlayButtonClick();
    }
}
