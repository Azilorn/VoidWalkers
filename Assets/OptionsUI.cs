using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider BGMSlider;

    private void OnEnable()
    {
        SFXSlider.value = ReturnVolume(AudioManager.Instance.SFXSource.volume);
        BGMSlider.value = ReturnVolume(AudioManager.Instance.BattleMusic.volume);
    }

    private void Start()
    {
        SFXSlider.onValueChanged.AddListener(delegate { OnSFXSliderChange(); });
        BGMSlider.onValueChanged.AddListener(delegate { OnBGMSliderChange(); });
    }
    public void OnSFXSliderChange() {

        AudioManager.Instance.SFXSource.volume = SetVolume((int)SFXSlider.value);
        AudioManager.Instance.UiSFXSource.volume = SetVolume((int)SFXSlider.value);
    }
    public void OnBGMSliderChange()
    {
        AudioManager.Instance.BattleMusic.volume = SetVolume((int)BGMSlider.value);
        AudioManager.Instance.MusicSource.volume = SetVolume((int)BGMSlider.value);
        AudioManager.Instance.MusicSource2.volume = SetVolume((int)BGMSlider.value);
    }
    public float SetVolume(int value) {

        switch (value) {

            case 0:
                return 0;
            case 1:
                return 0.25f;
            case 2:
                return 0.5f;
            case 3:
                return 0.75f;
            case 4:
                return 1;
        }
        return 1;
    }
    public int ReturnVolume(float volume) {

        switch (volume)
        {
            case 0:
                return 0;
            case 0.25f:
                return 1;
            case 0.5f:
                return 2;
            case 0.75f:
                return 3;
            case 1f:
                return 4;
        }
        return 1;
    }
    public void SaveSettings() {

        if (SaveLoadManager.Instance.GlobalDataExists()) {

            SaveLoadManager.Instance.SaveGlobalSaveData();
        }
    }
}
