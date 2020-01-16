using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGameButton : MonoBehaviour
{
    public void ContinueGame() {

        CoreGameInformation.SetGameLoadState(true);

        StartCoroutine(SceneController.Instance.LoadSceneAsync(2,0));
    }
}
