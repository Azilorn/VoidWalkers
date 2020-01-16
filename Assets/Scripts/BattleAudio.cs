using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudio : MonoBehaviour
{
    public static BattleAudio Instance;

    [SerializeField] private List<AudioClipExtendedList> battleMusic = new List<AudioClipExtendedList>();
    [SerializeField] private AudioClip poison;
    [SerializeField] private AudioClip frozen;
    [SerializeField] private AudioClip burnt;

    public List<AudioClipExtendedList> BattleMusic { get => battleMusic; set => battleMusic = value; }
    public AudioClip Poison { get => poison; set => poison = value; }
    public AudioClip Frozen { get => frozen; set => frozen = value; }
    public AudioClip Burnt { get => burnt; set => burnt = value; }

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
}
