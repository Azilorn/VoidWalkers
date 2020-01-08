using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance;

    [SerializeField] private List<AudioClipExtendedList> worldfloorBGM = new List<AudioClipExtendedList>();
    [SerializeField] private List<AudioClipExtendedList> mobsEssentialsAudio = new List<AudioClipExtendedList>();
    [SerializeField] private List<AudioClipExtendedList> kobsTavernAudio = new List<AudioClipExtendedList>();
    [SerializeField] private AudioClip confirmAudio;
    [SerializeField] private AudioClip deniedAudio;
    [SerializeField] private AudioClip partyMenuOpenAudio;
    [SerializeField] private AudioClip partyMenuCloseAudio;
    [SerializeField] private AudioClip itemMenuOpenAudio;
    [SerializeField] private AudioClip itemMenuCloseAudio;

    public List<AudioClipExtendedList> WorldfloorBGM { get => worldfloorBGM; set => worldfloorBGM = value; }
    public List<AudioClipExtendedList> MobsEssentialsAudio { get => mobsEssentialsAudio; set => mobsEssentialsAudio = value; }
    public List<AudioClipExtendedList> KobsTavernAudio { get => kobsTavernAudio; set => kobsTavernAudio = value; }
    public AudioClip ConfirmAudio { get => confirmAudio; set => confirmAudio = value; }
    public AudioClip DeniedAudio { get => deniedAudio; set => deniedAudio = value; }
    public AudioClip PartyMenuOpenAudio { get => partyMenuOpenAudio; set => partyMenuOpenAudio = value; }
    public AudioClip ItemMenuOpenAudio { get => itemMenuOpenAudio; set => itemMenuOpenAudio = value; }
    public AudioClip PartyMenuCloseAudio { get => partyMenuCloseAudio; set => partyMenuCloseAudio = value; }
    public AudioClip ItemMenuCloseAudio { get => itemMenuCloseAudio; set => itemMenuCloseAudio = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayConfirmAudio() {
        AudioManager.Instance.PlayUISFX(ConfirmAudio);
    }
    public void PlayDenyAudio()
    {
        AudioManager.Instance.PlayUISFX(DeniedAudio);
    }
    public void PlayPartyMenuOpenAudio(bool opened) {

        if (opened) {
            AudioManager.Instance.PlayUISFX(PartyMenuOpenAudio);
        }
        else AudioManager.Instance.PlayUISFX(PartyMenuCloseAudio);
    }
    public void PlayItemMenuOpenAudio(bool opened)
    {
        if (opened)
        {
            AudioManager.Instance.PlayUISFX(ItemMenuOpenAudio);
        }
        else AudioManager.Instance.PlayUISFX(ItemMenuCloseAudio);
    }
}
[Serializable]
public class AudioClipExtended {

    public AudioClip audio;
    public bool looping;
    public bool stopAfterPlay;
}
[Serializable]
public class AudioClipExtendedList {
    public List<AudioClipExtended> AudioList = new List<AudioClipExtended>();
}
