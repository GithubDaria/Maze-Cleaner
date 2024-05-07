using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;

    [SerializeField] private bool IsTesting;

     public ScreenManag screennang;
    private string adUnitId;

    private void Awake()
    {
#if UNITY_IOS
        Debug.Log("Setting adUnitId for iOS: " + iosGameId);
        adUnitId = iosGameId;
#elif UNITY_ANDROID
        Debug.Log("Setting adUnitId for Android: " + androidGameId);
        adUnitId = androidGameId;
#elif UNITY_EDITOR
        Debug.Log("Setting adUnitId for Editor: " + androidGameId);
        adUnitId = androidGameId;
#endif

    }
    public void LoadRewardedAd()
    {
        Advertisement.Load(adUnitId, this);
    }
    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAd();
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Failed");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete called with placementId: " + placementId + ", showCompletionState: " + showCompletionState);
        if (IsTesting)
        {
            if (screennang != null)
            {
                Debug.Log("Ad was watched");
                screennang.ApplyAddReward();
                Debug.Log("ApplyAddReward called");
            }
            else
            {
                screennang = GameObject.FindWithTag("Screen").GetComponent<ScreenManag>();
                Debug.Log("Ad was watched");
                screennang.ApplyAddReward();
                Debug.Log("ApplyAddReward called");
            }
        }
        else
        {
            if (placementId == adUnitId && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
            {
                Debug.Log("Ad was watched");
                if (screennang != null)
                {
                    screennang.ApplyAddReward();
                    Debug.Log("ApplyAddReward called");
                }
                else
                {
                    Debug.LogWarning("ScreenManag.Instance is null. Cannot apply reward.");
                }
            }
            else
            {
                Debug.Log("Ad was not watched or placementId does not match adUnitId.");
            }
        }
        
    }
}
