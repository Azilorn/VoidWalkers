using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAudio : MonoBehaviour
{
    public float delay;
    public AudioClip abilityAudio;

    private void OnEnable()
    {
        AudioManager.Instance.PlaySFX(abilityAudio, 1, delay);
    }
}
