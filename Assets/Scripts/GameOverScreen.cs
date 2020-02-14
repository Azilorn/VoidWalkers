using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void ContinueButton() {

        StartCoroutine(ContinueButtonCoroutine());
        StartCoroutine(AdsManager.Instance.StartAd());
    }
    public IEnumerator ContinueButtonCoroutine() {

        while (!AdsManager.Instance.AddIsFinished)
            yield return null;
       
            SaveLoadManager.SaveRetryData();
            SceneController.Instance.LoadScene(2);
            CoreGameInformation.isRetry = true;
    }
    public void ExitButton() {
        StartCoroutine(ExitButtonCoroutine());
    }
    public IEnumerator ExitButtonCoroutine() {
        SaveLoadManager.Instance.SaveGlobalSaveData();
        SaveLoadManager.DeleteSave();
        StartCoroutine(SceneController.Instance.LoadSceneAsync(0, 1));
        yield return new WaitForEndOfFrame();
    }
   
}
