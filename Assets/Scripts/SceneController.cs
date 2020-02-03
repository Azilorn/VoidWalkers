using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) {
            LoadScene(1);
        }
    }
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void ReloadScene() {

        MenuTransitionsController.Instance.StartTransition(0, false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    //Used to Load An Scene
    public IEnumerator LoadSceneAsync(int indexToLoad, int transition) {

        int indexToUnload = SceneManager.GetActiveScene().buildIndex;
        yield return StartCoroutine(MenuTransitionsController.Instance.StartAsyncTransition(0));
        AsyncOperation async = SceneManager.LoadSceneAsync(indexToLoad, LoadSceneMode.Additive);
        while (!async.isDone) {

            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(MenuTransitionsController.Instance.EndAsyncTransition(0));
        StartCoroutine(UnloadSceneAsync(indexToUnload));

    }
    //Used to Unload An Scene
    public IEnumerator UnloadSceneAsync(int indexToUnload)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(indexToUnload, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        while (!asyncUnload.isDone)
            yield return new WaitForEndOfFrame();
    }
    public void CloseApplication()
    {
        Application.Quit();
    }
}
