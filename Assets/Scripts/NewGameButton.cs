using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
   public void StartNewGame() {

        CoreGameInformation.SetGameLoadState(false);
        SceneController.Instance.LoadCoreGame();
    }
}
