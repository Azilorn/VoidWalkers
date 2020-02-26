using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySelectUI : MonoBehaviour
{
    public Image icon;
    public Image border;
    public Image countBorder;
    public List<GameObject> partyCountIcons = new List<GameObject>();

    public void SetPartyCountIcons(PlayerParty party)
    {
        switch (party.partyType)
        {
            case PartyType.Battle:
                border.color = new Color32(56, 111, 130, 255);
                countBorder.color = new Color32(56, 111, 130, 255);
                break;
            case PartyType.Elite:
                border.color = new Color32(130, 56, 62, 255);
                countBorder.color = new Color32(130, 56, 62, 255);
                break;
            case PartyType.Boss:
                border.color = new Color32(130, 109, 56, 255);
                countBorder.color = new Color32(130, 109, 56, 255);
                break;
            case PartyType.Player:
                break;
        }
        int count = 0;
        for (int i = 0; i < party.party.Length; i++)
        {
            if (party.party[i].creatureSO != null) {
                count++;
            }
        }
        CreatureSO creatureSO = party.party[0].creatureSO;
        icon.sprite = creatureSO.creaturePlayerIcon;
        icon.rectTransform.sizeDelta = new Vector2(creatureSO.width, creatureSO.height);

        for (int i = 0; i < count; i++) {
            partyCountIcons[i].SetActive(true);
        }
        for (int i = count; i < partyCountIcons.Count; i++) {
            partyCountIcons[i].SetActive(false);
        }
    }
}
