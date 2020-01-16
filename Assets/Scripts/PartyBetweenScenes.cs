using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBetweenScenes : MonoBehaviour
{
    public static PartyBetweenScenes Instance;
    public static PlayerParty party;

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
        DontDestroyOnLoad(gameObject);
    }

    public void SetPartyBetweenScenes(PlayerParty p) {

        party = p;
    }
}
