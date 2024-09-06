using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private CustomToggle musicToggle, vibrationToggle;

    private void Start()
    {
        musicToggle.IsOn = !AudioManager.Instance.SoundsMuted;
        musicToggle.onSwitched.AddListener(OnMusicSwitched);

        vibrationToggle.IsOn = VibrationManager.Instance.VibrationEnabled;
        vibrationToggle.onSwitched.AddListener(OnVibrationSwitched);
    }

    private void OnMusicSwitched(bool v)
    {
        AudioManager.Instance.SetSoundsMuted(!v);
    }

    private void OnVibrationSwitched(bool v)
    {
        VibrationManager.Instance.SetVibrationEnabled(v);
    }
}
