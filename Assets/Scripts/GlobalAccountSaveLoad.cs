using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAccountSaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start  ()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SaveLoadManager.Instance.LoadGlobalSaveData();
        
    }

   
}
