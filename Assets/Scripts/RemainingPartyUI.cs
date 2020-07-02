using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RemainingPartyUI : MonoBehaviour
{
    public Image[] partyUI;

    public void SetPartyUI(PlayerParty playerParty)
    {
        for (int i = 0; i < playerParty.party.Length; i++) {

            if (playerParty.party[i].creatureSO == null)
            {
                partyUI[i].gameObject.SetActive(false);
                continue;
            }
            else if (playerParty.party[i].creatureStats.HP <= 0)
            {
                partyUI[i].color = new Color32(255, 0, 0, 255);
            }
            else {
                if (i == playerParty.selectedCreature)
                {
                    partyUI[i].color = new Color32(48,84,166, 255);
                }
                else {
                    partyUI[i].color = Color.grey;
                }
            } 
            partyUI[i].gameObject.SetActive(true);
        }
    }
}
