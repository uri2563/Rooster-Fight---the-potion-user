
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public bool revived;
    public static AdsManager Instance;

    private string placement = "rewardedVideo";
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize("3848699");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
         
        revived = false;
    }

    public void ShowAdd()
    {
        Advertisement.Show(placement);
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(showResult == ShowResult.Finished && !revived)
        {
            revived = true;
            Debug.Log("revive");
            GameUIManager.Instance.Revive();
        }
    }
}
