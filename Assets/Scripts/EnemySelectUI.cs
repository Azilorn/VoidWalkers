using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySelectUI : MonoBehaviour
{
    public Image icon;
    public List<GameObject> partyCountIcons = new List<GameObject>();
    public TextMeshProUGUI text;

    public void SetPartyCountIcons(PlayerParty party)
    {
        int count = 0;
        int averageLevel = 0;
        for (int i = 0; i < party.party.Length; i++)
        {
            if (party.party[i].creatureSO != null) {
                count++;
                averageLevel += party.party[i].creatureStats.level;
            }
        }
        averageLevel = averageLevel / count;
        text.text = "Average LVL: " + averageLevel;
        CreatureSO creatureSO = party.party[0].creatureSO;
        icon.sprite = creatureSO.creatureEnemyIcon;
        icon.rectTransform.sizeDelta = new Vector2(creatureSO.width, creatureSO.height);

        for (int i = 0; i < count; i++) {
            partyCountIcons[i].SetActive(true);
        }
        for (int i = count; i < partyCountIcons.Count; i++) {
            partyCountIcons[i].SetActive(false);
        }
    }
}
