using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public static AdsManager Instance;
    readonly string placement = "rewardedVideo";
    public bool AddIsFinished = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        AddIsFinished = false;
        Advertisement.AddListener(this);
        if(Application.platform == RuntimePlatform.Android || Application.isEditor)
        {
            Advertisement.Initialize("3453801", true);
            Debug.Log("Android");
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Advertisement.Initialize("3453800", true);
            Debug.Log("iOS");
        }
    }
    // Start is called before the first frame update
    public IEnumerator StartAd()
    {
        AddIsFinished = false;
        while (!Advertisement.IsReady(placement))
            yield return null;
        Advertisement.Show(placement);
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished) {
            AddIsFinished = true;
            Debug.Log("ad finished");
        }
        if (showResult == ShowResult.Skipped) {
            AddIsFinished = false;
        }
        if (showResult == ShowResult.Failed) {
            AddIsFinished = false;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        AudioManager.Instance.StopMusic();
    }

    public void OnUnityAdsReady(string placementId)
    {
       
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }
}
