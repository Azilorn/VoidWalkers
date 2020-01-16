using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MenuTransitionsController.Instance.StartTransition(0, false);
        StartCoroutine(SceneController.Instance.LoadSceneAsync(2, 0));
    }
}
