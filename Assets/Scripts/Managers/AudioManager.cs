using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public bool SoundsMuted { get; private set; }

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip buttonClickClip;

    protected override void Awake()
    {
        base.Awake();

        SoundsMuted = PlayerPrefs.GetInt("MuteSounds", 0) == 1;
        audioSource.mute = SoundsMuted;
    }

    public void SetSoundsMuted(bool v)
    {
        SoundsMuted = v;
        audioSource.mute = v;
        PlayerPrefs.SetInt("MuteSounds", v ? 1 : 0);
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayButtonClick()
    {
        PlayOneShot(buttonClickClip);
    }
}