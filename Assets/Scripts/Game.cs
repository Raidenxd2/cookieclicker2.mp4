using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using BreakInfinity;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Logger = LoggerSystem.Logger;
using LoggerSystem;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
using UnityEngine.Purchasing;

public class Game : MonoBehaviour
{

    // variables
    [Header("Game Variables")]
    public BigDouble Cookies;
    public BigDouble CPS;
    public BigDouble CPC;
    public BigDouble TimePlayed;
    public BigDouble Doublecookies;
    public BigDouble Autoclickers;
    public BigDouble DoublecookiePrice;
    public BigDouble AutoclickerPrice;
    public BigDouble Drills;
    public BigDouble DrillPrice;
    public BigDouble Grandmas;
    public BigDouble GrandmaPrice;
    public bool HasPlayed;
    public bool ResearchFactory;
    public bool Music;
    public bool Sounds;
    public float LastSavedGameVersion;
    public float GameVersion;

    // game objects
    [Header("Game Objects")]
    public GameObject RelaunchRequiredScreen;
    public GameObject Drill_Model;
    public GameObject Drill_Partical;
    public GameObject NECDialog;
    public GameObject SDIE;
    public GameObject SDUE;
    public GameObject NoNetworkScreen;

    // scripts
    [Header("Scripts")]
    public OfflineManager offlineManager;
    public AdvancedQualitySettings ad;
    public SoundManager soundManager;
    public Notification notification;
    public FPSLimiter fpsLimiter;
#if UNITY_ANDROID
    public GooglePlayGamesManager GPGM;
#endif
    public AdsManager adsManager;

    // text
    [Header("Text")]
    public TMP_Text CookiesText;
    public TMP_Text BSMCookiesText;
    public TMP_Text Shop_Autoclicker;
    public TMP_Text Shop_Doublecookie;
    public TMP_Text Shop_Drill;
    public TMP_Text ErrorText;
    public TMP_Text SmallErrorText;
    public TMP_Text Shop_Grandma;
    public TMP_Text VersionText;
    public TMP_Text Shop_AdText;

    // performance mode stuff
    

    // stats
    [Header("Stats")]
    public TMP_Text Stats_Cookies;
    public TMP_Text Stats_Doublecookies;
    public TMP_Text Stats_Autoclickers;
    public TMP_Text Stats_Drills;
    public TMP_Text Stats_CPS;
    public TMP_Text Stats_CPC;
    public TMP_Text Stats_Grandmas;

    // animations
    [Header("Animations")]
    public Animator Fade;

    // audio
    [Header("Audio")]
    private GameObject MusicSource;
    private GameObject SoundSource;
    public AudioClip[] Musics;

    public Camera gameCamera;
    public UnityEngine.Rendering.Volume CVDFilter;
    public UnityEngine.Rendering.VolumeProfile CBNormal;
    public UnityEngine.Rendering.VolumeProfile CBProtanopia;
    public UnityEngine.Rendering.VolumeProfile CBProtanomaly;
    public UnityEngine.Rendering.VolumeProfile CBDeuteranopia;
    public UnityEngine.Rendering.VolumeProfile CBDeuteranomaly;
    public UnityEngine.Rendering.VolumeProfile CBTritanopia;
    public UnityEngine.Rendering.VolumeProfile CBTritanomaly;
    public UnityEngine.Rendering.VolumeProfile CBAchromatopsia;
    public UnityEngine.Rendering.VolumeProfile CBAchromatomaly;

    [Header("addresables")]
    [SerializeField] private List<AssetReference> _audioReferences;

    [Header("BetaContent")]
    public GameObject BetaContentWarningScreen;
    public GameObject BetaContentScreen;
    public Toggle[] BetaContentToggles;
    public GameObject ScreenshotOptionsBTN;
    public GameObject RegisteredBTN;

    [Header("IAP")]
    public GameObject StarterBundleBTN;
    public bool StarterBundleBought;

    [Header("Ads")]
    public int Clicks;
    public TMP_Text DebugClicks;

    [Header("Particles")]
    public GameObject CookieVFX;
    public GameObject CookieGains;
    public Transform CookieVFXSpot;
    public Transform CookieGainsSpot;

    // Start is called before the first frame update
    void Start()
    {

        // PlayCloudDataManager.Instance.SaveToCloud(Application.persistentDataPath + "/cookie2");
#if UNITY_ANDROID
        PlayCloudDataManager.Instance.LoadFromCloud(delegate (string s) {
            File.WriteAllText(Application.persistentDataPath + "/cookie2_fromgoogle", s);
        });

        IAPManager.Instance.game = this;
#endif
        VersionText.text = "v" + Application.version + "-" + Application.platform + " (" + Application.unityVersion + ")";
        // IAPManager.Instance.Initialize();
        if (Application.platform == RuntimePlatform.Android)
        {
            ScreenshotOptionsBTN.SetActive(false);
        }
        else
        {
            ScreenshotOptionsBTN.SetActive(true);
        }
        if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
        {
            ad.SetDefaults();
            SavePlayer();
            PlayerPrefs.SetInt("HasPlayed", 1);
            PlayerPrefs.Save();
            Reload();
        }
        LoadPlayer();
        if (HasPlayed == false)
        {
            HasPlayed = true;
            Music = true;
            Sounds = true;
            ad.TextureQuality = 0;
            ad.Trees = true;
            ad.Particals = true;
            ad.Lighting = true;
            ad.PostProcessing = true;
            ad.VSync = false;
            ResetData();
        }
        offlineManager.LoadOfflineTime();
        StartCoroutine(AutoSave());
        StartCoroutine(Tick());
        RelaunchRequiredScreen.SetActive(false);
        CheckPrices();
        LastSavedGameVersion = GameVersion;
        try
        {
            SoundAssign();
        }
        catch
        {
            Logger.Log("Could not assign audio. Did you load from the Init scene?", LogTypes.Error);
        }
        ad.LoadGraphics();
        // Logger.Log(IAPManager.Instance.IsInitialized.ToString(), LogTypes.Normal);
        // if (IAPManager.Instance.IsInitialized)
        // {
        //     CheckIfUserRegistered();
        // }
        BetaContentToggles[0].onValueChanged.AddListener(delegate{ChangeBetaContentFeatureValue("BETA_EnableSideBar", BetaContentToggles[0].isOn);});
        BetaContentToggles[1].onValueChanged.AddListener(delegate{ChangeBetaContentFeatureValue("BETA_Mods", BetaContentToggles[1].isOn);});
        BetaContentToggles[2].onValueChanged.AddListener(delegate{ChangeBetaContentFeatureValue("BETA_FPSLIMIT", BetaContentToggles[2].isOn);});
        BetaContentToggles[3].onValueChanged.AddListener(delegate{ChangeBetaContentFeatureValue("BETA_CookieMonster", BetaContentToggles[3].isOn);});
        if (StarterBundleBought)
        {
            StarterBundleBTN.SetActive(false);
        }
    }

    public void HideStarterBundleButton()
    {
        StarterBundleBTN.SetActive(false);
    }

    public void OnPurchaseComplete(Product product)
    {
        Time.timeScale = 1f;
        Logger.Log(product.definition.id + " Should have added Cookies now.", LogTypes.Normal);
        if(product.definition.id == "com.raiden.cookieclicker2.mp4.cookies_50000")
	    {
	    	Cookies += 50000;
	    }
        if(product.definition.id == "com.raiden.cookieclicker2.mp4.cookies_100000")
        {
            Cookies += 100000;
        }
    }

    public void OnPurchaseClicked(string productId) 
    {
#if UNITY_ANDROID
        IAPManager.Instance.OnPurchaseClicked(productId);
#endif
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        Time.timeScale = 1f;
        Logger.Log(product.transactionID + " failed " + purchaseFailureReason, LogTypes.Error);
    }

    public void PurchaseItem(string ID)
    {
        // IAPManager.Instance.Purchase(ID, () => Time.timeScale = 0f);
    }

    void CheckIfUserRegistered()
    {
        // if(IAPManager.Instance.IsNonConsumablePurchased( "early_registration_reward" ) )
	    // {
		//     RegisteredBTN.SetActive(true);
	    // }
    }


    void SoundAssign()
    {
        soundManager = GameObject.FindGameObjectWithTag("audio").GetComponent<SoundManager>();
        MusicSource = GameObject.FindGameObjectWithTag("music");
        SoundSource = GameObject.FindGameObjectWithTag("sound");
    }

    void CheckPrices()
    {
        if (AutoclickerPrice < 25)
        {
            Autoclickers = 0;
            AutoclickerPrice = 25;
        }
        if (DoublecookiePrice < 50)
        {
            Doublecookies = 0;
            DoublecookiePrice = 50;
        }
        if (DrillPrice < 100)
        {
            Drills = 0;
            DrillPrice = 100;
        }
        if (GrandmaPrice < 150)
        {
            Grandmas = 0;
            GrandmaPrice = 150;
        }
    }

    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(60);
        SavePlayer();
        SoundAssign();
        StartCoroutine(AutoSave());
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1);
        Cookies += CPS;
        TimePlayed += 1;
        StartCoroutine(Tick());
    }

    public void SavePlayer()
    {
        PlayerPrefs.Save();
        offlineManager.SaveTime();
        ad.SaveGraphics();
        SaveSystem.SavePlayer(this, offlineManager, ad);
#if UNITY_ANDROID
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            try
            {
                GPGM.SetLeaderboardValue(Cookies, "CgkIleno69UVEAIQAg");
                GPGM.SetLeaderboardValue(CPS, "CgkIleno69UVEAIQBQ");
                GPGM.SetLeaderboardValue(CPC, "CgkIleno69UVEAIQBg");
                GPGM.IncreseAchievement("CgkIleno69UVEAIQAw", (Int32) Autoclickers);
                GPGM.IncreseAchievement("CgkIleno69UVEAIQBA", (Int32) Doublecookies);
                GPGM.IncreseAchievement("CgkIleno69UVEAIQBw", (Int32) Drills);
                GPGM.IncreseAchievement("CgkIleno69UVEAIQCA", (Int32) Grandmas);
            }
            catch
            {
                // do nothing
            }
            
        }
        PlayCloudDataManager.Instance.SaveToCloud(Application.persistentDataPath + "/cookie2");
#endif
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer(this);

        SaveSystem.LoadPlayer(this);

        ad.LoadGraphics();

        Cookies = data.Cookies;
        CPC = data.CPC;
        CPS = data.CPS;
        TimePlayed = data.TimePlayed;
        HasPlayed = data.HasPlayed;
        Autoclickers = data.Autoclickers;
        Doublecookies = data.Doublecookies;
        AutoclickerPrice = data.AutoclickerPrice;
        DoublecookiePrice = data.DoublecookiePrice;
        Drills = data.Drills;
        DrillPrice = data.DrillPrice;
        ResearchFactory = data.ResearchFactory;
        offlineManager.offlineProgressCheck = data.offlineProgressCheck;
        offlineManager.OfflineTime = data.OfflineTime;
        LastSavedGameVersion = data.LastSavedGameVersion;
        Sounds = data.Sounds;
        Music = data.Music;
        Grandmas = data.Grandmas;
        GrandmaPrice = data.GrandmaPrice;
        StarterBundleBought = data.StarterBundleBought;
        Clicks = data.Clicks;
    }

    public void ResetData()
    {
        Cookies = 0;
        CPS = 0;
        CPC = 1;
        TimePlayed = 0;
        Autoclickers = 0;
        Doublecookies = 0;
        ResearchFactory = false;
        Drills = 0;
        AutoclickerPrice = 0;
        DoublecookiePrice = 0;
        DrillPrice = 0;
        Grandmas = 0;
        GrandmaPrice = 0;
        StarterBundleBought = false;
        Clicks = 0;
        SavePlayer();
        Debug.Log("Reset Data. Now reloading...");
        Reload();
    }

    public void BakeCookie()
    {
        Cookies += CPC;
        Clicks += 1;
        Instantiate(CookieGains, CookieGainsSpot);
        Instantiate(CookieVFX, CookieVFXSpot);
        if (Clicks >= 250)
        {
            Clicks = 0;
            adsManager.ShowAd();
        }
#if UNITY_ANDROID
        try
        {
            GPGM.GiveAchievement("CgkIleno69UVEAIQAQ");
        }
        catch
        {
            Logger.Log("Could not give Google Play achievement.", LogTypes.Error);
        }
#endif
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        ad.SaveGraphics();
        SavePlayer();
    }

    public void Reload()
    {
        Debug.Log("Reloading..");
        StartCoroutine(ReloadWait());
    }

    public void SoundToggle(bool Toggle)
    {
        Sounds = Toggle;
    }

    public void MusicToggle(bool Toggle)
    {
        Music = Toggle;
    }

    IEnumerator ReloadWait()
    {
        Fade.Play("FadeIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Init");
    }

    public void BuyAutoclicker()
    {
        if (Cookies >= AutoclickerPrice)
        {
            Cookies -= AutoclickerPrice;
            AutoclickerPrice += 25;
            Autoclickers += 1;
            CPS += 1;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void BuyDoublecookie()
    {
        if (Cookies >= DoublecookiePrice)
        {
            Cookies -= DoublecookiePrice;
            DoublecookiePrice += 50;
            Doublecookies += 1;
            CPC += 1;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void BuyDrill()
    {
        if (Cookies >= DrillPrice)
        {
            Cookies -= DrillPrice;
            DrillPrice += 100;
            Drills += 1;
            CPC += 2;
            CPS += 2;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void BuyGrandma()
    {
        if (Cookies >= GrandmaPrice)
        {
            Cookies -= GrandmaPrice;
            GrandmaPrice += 150;
            Grandmas += 1;
            CPS += 4;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void OpenCreditsLink(int num)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NoNetworkScreen.SetActive(true);
            return;
        }
        switch (num)
        {
            
            case 1:
                Application.OpenURL("https://scripts.sil.org/cms/scripts/page.php?item_id=OFL_web");
                break;
            case 2:
                Application.OpenURL("https://raidenxd2.github.io/cookieclicker2.mp4/Music.txt");
                break;
            case 3:
                Application.OpenURL("https://github.com/tomc128/urp-kawase-blur");
                break;
            
        }
    }

    public void ChangeCameraBGColor(String color)
    {
        switch (color)
        {
            case "normal":
                gameCamera.backgroundColor = new Color32(53, 67, 89, 255);
                break;
            case "space":
                gameCamera.backgroundColor = new Color(0, 0, 0, 255);
                break;

        }


    }

    public void ChangeColorBlindNessMode(int index)
    {
        switch (index)
        {
            case 0:
                CVDFilter.profile = CBNormal;
                break;
            case 1:
                CVDFilter.profile = CBProtanopia;
                break;
            case 2:
                CVDFilter.profile = CBProtanomaly;
                break;
            case 3:
                CVDFilter.profile = CBDeuteranopia;
                break;
            case 4:
                CVDFilter.profile = CBDeuteranomaly;
                break;
            case 5:
                CVDFilter.profile = CBTritanopia;
                break;
            case 6:
                CVDFilter.profile = CBTritanomaly;
                break;
            case 7:
                CVDFilter.profile = CBAchromatopsia;
                break;
            case 8:
                CVDFilter.profile = CBAchromatomaly;
                break;
        }
    }

    public void ChangeMusic(int index)
    {
        switch (index)
        {
            case 0:
                AsyncOperationHandle<AudioClip> asyncOperationHandle = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[0]);
                asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
                break;
            case 1:
                AsyncOperationHandle<AudioClip> asyncOperationHandle2 = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[1]);
                asyncOperationHandle2.Completed += AsyncOperationHandle_Completed;
                break;
            case 2:
                AsyncOperationHandle<AudioClip> asyncOperationHandle3 = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[2]);
                asyncOperationHandle3.Completed += AsyncOperationHandle_Completed;
                break;
            case 3:
                AsyncOperationHandle<AudioClip> asyncOperationHandle4 = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[3]);
                asyncOperationHandle4.Completed += AsyncOperationHandle_Completed;
                break;
            case 4:
                AsyncOperationHandle<AudioClip> asyncOperationHandle5 = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[4]);
                asyncOperationHandle5.Completed += AsyncOperationHandle_Completed;
                break;
            case 5:
                AsyncOperationHandle<AudioClip> asyncOperationHandle6 = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[5]);
                asyncOperationHandle6.Completed += AsyncOperationHandle_Completed;
                break;
            case 6:
                AsyncOperationHandle<AudioClip> asyncOperationHandle7 = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[6]);
                asyncOperationHandle7.Completed += AsyncOperationHandle_Completed;
                break;
            case 7:
                AsyncOperationHandle<AudioClip> asyncOperationHandle8 = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[7]);
                asyncOperationHandle8.Completed += AsyncOperationHandle_Completed;
                break;
        }
    }

    public void EnableBetaContent()
    {
        PlayerPrefs.SetInt("BetaContent", 1);
        BetaContentScreen.SetActive(true);
        BetaContentWarningScreen.SetActive(false);
    }

    public void DisableBetaContent()
    {
        PlayerPrefs.SetInt("BetaContent", 0);
        PlayerPrefs.SetInt("BETA_EnableSideBar", 0);
        PlayerPrefs.SetInt("BETA_Mods", 0);
        PlayerPrefs.SetInt("BETA_FPSLIMIT", 0);
        SavePlayer();
        Reload();
    }

    public void ChangeBetaContentFeatureValue(string name, bool toggle)
    {
        switch (toggle)
        {
            case false:
                PlayerPrefs.SetInt(name, 0);
                break;
            case true:
                PlayerPrefs.SetInt(name, 1);
                break;
        }
    }

    public void ShowBetaContentWindow()
    {
        if (PlayerPrefs.GetInt("BetaContent", 0) == 1)
        {
            BetaContentScreen.SetActive(true);
        }
        else
        {
            BetaContentWarningScreen.SetActive(true);
        }
    }

    public void EnterBSM()
    {
        Application.targetFrameRate = 10;
    }

    public void ExitBSM()
    {
        if (fpsLimiter.FpsLimit)
        {
            fpsLimiter.SetMaxFPS();
        }
        else
        {
            Application.targetFrameRate = 999999999;
        }
    }

    public void EntermodioScreen()
    {
        gameCamera.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void ExitmodioScreen()
    {
        gameCamera.transform.rotation = new Quaternion(30, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            var musicisplaying = MusicSource.GetComponent<AudioSource>();
            if (!musicisplaying.isPlaying)
            {
                int randomIndex = UnityEngine.Random.Range(0, 7);
                Debug.Log(_audioReferences[randomIndex]);
                AsyncOperationHandle<AudioClip> asyncOperationHandle = Addressables.LoadAssetAsync<AudioClip>(_audioReferences[randomIndex]);

                asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
                
            }
        }
        catch(Exception ex)
        {
            Logger.Log("Could not assign music or play music" + ex, LogTypes.Error);
            SoundAssign();
        }
        CookiesText.text = "Cookies: " + Cookies;
        BSMCookiesText.text = "Cookies: " + Cookies;
        Shop_Autoclicker.text = "Autoclicker (" + AutoclickerPrice + " Cookies)";
        Shop_Doublecookie.text = "Doublecookie (" + DoublecookiePrice + " Cookies)";
        Shop_Drill.text = "Drill (" + DrillPrice + " Cookies)";
        Shop_Grandma.text = "Grandma (" + GrandmaPrice + " Cookies)";

        // if (ResearchFactory)
        // {
        //     Research_Factory_Normal.SetActive(true);
        //     if (PerformanceMode)
        //     {
        //         Research_Factory_Particals.SetActive(false);
        //     }
        //     else
        //     {
        //         Research_Factory_Particals.SetActive(true);
        //     }
        // }
        // else
        // {
        //     Research_Factory_Normal.SetActive(false);
        //     Research_Factory_Particals.SetActive(false);
        // }

        // drill
        if (Drills >= 1)
        {
            Drill_Model.SetActive(true);
            Drill_Partical.SetActive(true);
        }
        else
        {
            Drill_Model.SetActive(false);
            Drill_Partical.SetActive(false);
        }

        // stats
        Stats_Cookies.text = "Cookies: " + Cookies;
        Stats_Autoclickers.text = "Autoclickers: " + Autoclickers;
        Stats_Doublecookies.text = "Doublecookies: " + Doublecookies;
        Stats_Drills.text = "Drills: " + Drills;
        Stats_CPC.text = "Cookies Per Click: " + CPC;
        Stats_CPS.text = "Cookies Per Second: " + CPS;
        Stats_Grandmas.text = "Grandmas: " + Grandmas;

        // music & sounds
        var musicSource = MusicSource.GetComponent<AudioSource>();
        var soundsSource = SoundSource.GetComponent<AudioSource>();
        if (Music == false)
        {
            musicSource.volume = 0;
        }
        else
        {
            musicSource.volume = 1;
        }
        if (Sounds == false)
        {
            soundsSource.volume = 0;
        }
        else
        {
            soundsSource.volume = 1;
        }
        // MusicSource.SetActive(Music);
        // SoundSource.SetActive(Sounds);
        DebugClicks.text = "Clicks (500 for ad): " + Clicks;
        if (CPS >= 0 && CPS <= 10)
        {
            Shop_AdText.text = "Watch AD for 10000 Cookies";
        }
        else if (CPS >= 10 && CPS <= 20)
        {
            Shop_AdText.text = "Watch AD for 20000 Cookies";
        }
        else if (CPS >= 20 && CPS <= 30)
        {
            Shop_AdText.text = "Watch AD for 30000 Cookies";
        }
        else if (CPS >= 30 && CPS <= 40)
        {
            Shop_AdText.text = "Watch AD for 40000 Cookies";
        }
        else if (CPS >= 40 && CPS <= 50)
        {
            Shop_AdText.text = "Watch AD for 50000 Cookies";
        }
        else if (CPS >= 50 && CPS <= 60)
        {
            Shop_AdText.text = "Watch AD for 60000 Cookies";
        }
        else if (CPS >= 60 && CPS <= 70)
        {
            Shop_AdText.text = "Watch AD for 70000 Cookies";
        }
        else if (CPS >= 70 && CPS <= 80)
        {
            Shop_AdText.text = "Watch AD for 80000 Cookies";
        }
        else if (CPS >= 80 && CPS <= 90)
        {
            Shop_AdText.text = "Watch AD for 90000 Cookies";
        }
        else if (CPS >= 90)
        {
            Shop_AdText.text = "Watch AD for 100000 Cookies";
        }
    }

    // string error;

    // void OnEnable()
    // {
    //     Application.logMessageReceived += HandleLog;
    // }

    // void OnDisable()
    // {
    //     Application.logMessageReceived -= HandleLog;
    // }

    // void HandleLog(string logString, string stackTrace, LogType type)
    // {
        
    //     if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
    //     {
    //         Logger.Log("(" + type + ") " + logString + " " + stackTrace + "" + type, LogTypes.Error);
    //         notification.ShowNotification("(" + type + ") " + logString + " " + stackTrace, "" + type);
    //     }   
    // }

    private void AsyncOperationHandle_Completed(AsyncOperationHandle<AudioClip> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            SoundManager.Instance.RandomMusic(asyncOperationHandle.Result);
        }
        else
        {
            Logger.Log("Failed to load audio clip", LogTypes.Error);
        }
    }

}
