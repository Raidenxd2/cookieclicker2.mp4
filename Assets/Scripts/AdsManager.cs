using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AdsManager : MonoBehaviour
{

    private RewardedAd rewardedAd;

    public Game game;

    private BannerView bannerView;

    public RectTransform CookiesCounterRect;
    private InterstitialAd interstitial;

    void OnApplicationPause(bool isPaused) 
    {                 

    }

    void Start()
    {
        CookiesCounterRect.anchoredPosition = new Vector3(0, -94, 0);
        MobileAds.Initialize(initStatus => { });
        this.CreateAndLoadRewardedAd();
        this.RequestInterstitial();

        // this.RequestBanner();
    }

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-9890267398226052/9266314069";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);

        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosedClick;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void ShowAd()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }

    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-9890267398226052/3892265123";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        AdSize adaptiveSize = AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Top);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;

        

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    // Start is called before the first frame update
    public void CreateAndLoadRewardedAd()
    {

        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-9890267398226052/1648125603";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        rewardedAd = new RewardedAd(adUnitId);


        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        // UserChoseToWatchAd();
    }

    private void UserChoseToWatchAd()
    {
        
        if (rewardedAd.IsLoaded()) 
        {
            
        }
    }

    public void MoneySimulator()
    {
        rewardedAd.Show();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.LoadAdError);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        LoggerSystem.Logger.Log(type, LoggerSystem.LogTypes.Normal);
        LoggerSystem.Logger.Log(amount.ToString(), LoggerSystem.LogTypes.Normal);
        if (type == "Cookies")
        {
            if (game.CPS >= 0 && game.CPS <= 10)
            {
                game.Cookies += 10000;
            }
            else if (game.CPS >= 10 && game.CPS <= 20)
            {
                game.Cookies += 20000;
            }
            else if (game.CPS >= 20 && game.CPS <= 30)
            {
                game.Cookies += 30000;
            }
            else if (game.CPS >= 30 && game.CPS <= 40)
            {
                game.Cookies += 40000;
            }
            else if (game.CPS >= 40 && game.CPS <= 50)
            {
                game.Cookies += 50000;
            }
            else if (game.CPS >= 50 && game.CPS <= 60)
            {
                game.Cookies += 60000;
            }
            else if (game.CPS >= 60 && game.CPS <= 70)
            {
                game.Cookies += 70000;
            }
            else if (game.CPS >= 70 && game.CPS <= 80)
            {
                game.Cookies += 80000;
            }
            else if (game.CPS >= 80 && game.CPS <= 90)
            {
                game.Cookies += 90000;
            }
            else if (game.CPS >= 90)
            {
                game.Cookies += 100000;
            }
            
        }
        CreateAndLoadRewardedAd();
    }

    public void showRewardedVideoCookies(int Cookies)
    {
        if (1 >= 1)
        {
            LoggerSystem.Logger.Log("Showing a Rewarded Video", LoggerSystem.LogTypes.Normal);
        }
        else
        {
            LoggerSystem.Logger.Log("Rewarded Video not available.", LoggerSystem.LogTypes.Error);
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        CookiesCounterRect.anchoredPosition = new Vector3(0, -238, 0);
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdClosedClick(object sender, EventArgs args)
    {
        RequestInterstitial();
        MonoBehaviour.print("HandleAdClosed event received");
    }

}
