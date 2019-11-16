using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneController.Instance.LoadCoreGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
