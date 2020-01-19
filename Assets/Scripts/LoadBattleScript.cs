using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBattleScript : MonoBehaviour
{
    public void LoadBattle()
    {
        BattleUI.Instance.SetBattleUIAtStart();
        BattleUI.Instance.BattleCanvasTransform.gameObject.SetActive(true);
    }
    
}
