using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;
    [SerializeField] private bool isTesting;

    private string gameid;

    public void OnInitializationComplete()
    {
        Debug.Log("ads go bruu");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("No money booo");
    }
    private void Awake()
    {
#if UNITY_IOS
        gameid = iosGameId; // Fix: Use assignment operator instead of subtraction operator
#elif UNITY_ANDROID
        gameid = androidGameId;
#elif UNITY_EDITOR
        gameid = androidGameId;
#endif

        if (Advertisement.isSupported)
        {
            if (!Advertisement.isInitialized)
            {
                Advertisement.Initialize(gameid, isTesting, this);
                Debug.Log("Initializing Unity Ads...");
            }
            else
            {
                Debug.Log("Unity Ads is already initialized.");
            }
        }
        else
        {
            Debug.Log("Unity Ads is not supported on this platform.");
        }
    }


}
