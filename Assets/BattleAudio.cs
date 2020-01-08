using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudio : MonoBehaviour
{
    public static BattleAudio Instance;

    [SerializeField] private List<AudioClipExtendedList> battleMusic = new List<AudioClipExtendedList>();

    public List<AudioClipExtendedList> BattleMusic { get => battleMusic; set => battleMusic = value; }

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
