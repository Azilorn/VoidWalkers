using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBattleScript : MonoBehaviour
{
    public void LoadBattle()
    {
        CoreUI.Instance.SetBattleUIAtStart();
        CoreUI.Instance.BattleCanvasTransform.gameObject.SetActive(true);
    }
    
}
