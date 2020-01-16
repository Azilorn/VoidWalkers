using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentProgressCreatureUI : MonoBehaviour
{
    public CreatureSO creatureSO;
    public Image image;
    public TextMeshProUGUI creatureLevel;

    public void SetUI(PlayerCreatureStats playerCreatureStats) {
        creatureSO = playerCreatureStats.creatureSO;
        image.sprite = creatureSO.creaturePlayerIcon;
        creatureLevel.text = playerCreatureStats.creatureSaveData.creatureStat.level.ToString();
    }
    public void ClearUI() {

        creatureSO = null;
        image.sprite = null;
        creatureLevel.text = "";
    }
}
