using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPanel : MonoBehaviour
{
    public void ClickRoll()
    {
        GameController.Instance.StartSpin();
    }

    public void ClickAutoPlay()
    {
        GameController.Instance.SwitchAutoPlay();
    }
}
