using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentProgressCreatureUI : MonoBehaviour
{
    public CreatureSO creatureSO;
    public Image icon;
    public Image background;
    public TextMeshProUGUI creatureLevel;

    public void SetUI(PlayerCreatureStats playerCreatureStats) {
        creatureSO = playerCreatureStats.creatureSO;
        icon.sprite = creatureSO.creaturePlayerIcon;
        creatureLevel.text = playerCreatureStats.creatureSaveData.creatureStat.level.ToString();
    }
    public void ClearUI() {

        creatureSO = null;
        icon.sprite = null;
        creatureLevel.text = "";
    }
}
