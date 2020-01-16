using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
   public void StartNewGame() {

        StartCoroutine(SceneController.Instance.LoadSceneAsync(1, 0));
    }
}
