using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialAdManager : MonoBehaviour
{

    [SerializeField] private InterstitialAdsButton interstitialAds;

    void Start()
    {
        Invoke("LoadInterstitialAd", 2f);
    }

    public void LoadInterstitialAd()
    {
        
    }

}
