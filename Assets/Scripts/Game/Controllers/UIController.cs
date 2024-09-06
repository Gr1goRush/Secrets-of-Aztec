using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public MainUIPanel MainPanel => mainPanel;
    [SerializeField] private MainUIPanel mainPanel;

    public WinPanel WinPanel => winPanel;
    [SerializeField] private WinPanel winPanel;

    public SuperGamePanel SuperGamePanel => superGamePanel;
    [SerializeField] private SuperGamePanel superGamePanel;
}
