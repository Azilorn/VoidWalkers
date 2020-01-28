using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class ContinueGameButton : MonoBehaviour
{

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/saveData.sav")){
            gameObject.SetActive(true);
        } else gameObject.SetActive(false);
    }
 
    public void ContinueGame() {

        CoreGameInformation.SetGameLoadState(true);

        StartCoroutine(SceneController.Instance.LoadSceneAsync(2,0));
    }
}
