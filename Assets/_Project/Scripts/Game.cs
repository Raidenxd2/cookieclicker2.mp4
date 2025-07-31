using System.Collections;
using UnityEngine;
using BreakInfinity;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoggerSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System;
using System.IO;

#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
using System.Collections.Generic;
using UnityEngine.XR.Management;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR.Features.Meta;
#endif

public class Game : MonoBehaviour
{
    public static Game instance;

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
    public BigDouble CookieFactorys;
    public BigDouble CookieFactoryPrice;
    public bool HasPlayed;
    public bool ResearchFactory;
    public bool Music;
    public bool Sounds;

    // game objects
    [Header("Game Objects")]
    public GameObject Drill_Partical;
    public GameObject NECDialog;
    public GameObject SDIE;
    public GameObject NoNetworkScreen;
    [SerializeField] private GameObject GlobalDark;

    // scripts
    [Header("Scripts")]
    [SerializeField] private OfflineManager offlineManager;
    [SerializeField] private AdvancedQualitySettings ad;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Notification notification;
    [SerializeField] private AddressableLightmaps al;
    [SerializeField] private BetaContent bc;
#if !CC2_REMOVE_VR_SUPPORT
    [SerializeField] private VRFadeCanvas vrFade;
#endif

    // text
    [Header("Text")]
    public TMP_Text CookiesText;
    public TMP_Text Shop_Autoclicker;
    public TMP_Text Shop_Doublecookie;
    public TMP_Text Shop_Drill;
    public TMP_Text ErrorText;
    public TMP_Text SmallErrorText;
    public TMP_Text Shop_Grandma;
    public TMP_Text VersionText;
    public TMP_Text Shop_CookieFactory;

    // stats
    [Header("Stats")]
    public TMP_Text Stats_Cookies;
    public TMP_Text Stats_Doublecookies;
    public TMP_Text Stats_Autoclickers;
    public TMP_Text Stats_Drills;
    public TMP_Text Stats_CPS;
    public TMP_Text Stats_CPC;
    public TMP_Text Stats_Grandmas;
    public TMP_Text Stats_CookieFactorys;

    // animations
    [Header("Animations")]
    public Animator Fade;
    public CanvasGroup FadeCanvasGroup;

    // audio
    [Header("Audio")]
    private GameObject MusicSource;
    private GameObject SoundSource;

    private AudioSource MusicAudioSource;
    private AudioSource SoundAudioSource;

    public Camera gameCamera;
    public Volume CVDFilter;
    public VolumeProfile CBNormal;
    public VolumeProfile CBProtanopia;
    public VolumeProfile CBProtanomaly;
    public VolumeProfile CBDeuteranopia;
    public VolumeProfile CBDeuteranomaly;
    public VolumeProfile CBTritanopia;
    public VolumeProfile CBTritanomaly;
    public VolumeProfile CBAchromatopsia;
    public VolumeProfile CBAchromatomaly;
    public Volume PP;
    public VolumeProfile VRProfile;

    [Header("BetaContent")]
    public GameObject BetaContentWarningScreen;
    public GameObject BetaContentScreen;
    public Toggle[] BetaContentToggles;
    public GameObject ScreenshotOptionsBTN;
    public GameObject QuitBTN;

    [Header("Particles")]
    public GameObject CookieVFX;
    public GameObject CookieGains;
    public Transform CookieVFXSpot;
    public Transform CookieGainsSpot;

    [Header("Research Factory")]
    public GameObject Research_Factory_Particals;
    public ResearchFactory researchFactory;

    [Header("Minigame Mine")]
    public float HammerStrength;
    public float HammerEnergy;
    public double HammerStrengthUpgradePrice;
    public double Coins;
    public double HammerEnergyUpgradePrice;
    public double CoinMultiplierUpgradePrice;
    public double CoinMultiplier;
    [Header("VR")]
    [SerializeField] private AssetReference VRPrefab;
    private GameObject VRPrefabGO;
    [SerializeField] private Transform VRPrefabParent;
    [SerializeField] private GameObject AndroidVROnlySettingsButton;
    [SerializeField] private TMP_Dropdown OculusQuestRefreshRateDropdown;
    public GameObject XROrigin;

#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
    private XRDisplaySubsystem displaySubsystem;
#endif

    private bool AllowUpdate;

    public WaitForSeconds oneSecond;
    public WaitForSeconds sixtySeconds;

    [SerializeField] private AssetReference DefaultThemeScene;

    private void Awake()
    {
        instance = this;

        if (!File.Exists(Application.persistentDataPath + "/Saves/Default.cookie"))
        {
            BetterPrefs.Load("/Saves/Default.cookie");
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        VersionText.text = "v" + Application.version + "-" + Application.platform + " (" + Application.unityVersion + ")";

        StartAsync().Forget();
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    private async UniTaskVoid StartAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
#if UNITY_ANDROID || UNITY_WEBGL
        ScreenshotOptionsBTN.SetActive(false);
#endif
#if UNITY_WEBGL
        QuitBTN.SetActive(false);
#endif

        if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
        {
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
            ad.Particals = true;
            ad.PostProcessing = true;
            ad.VSync = false;
            ResetData();
        }

        oneSecond = new(1);
        sixtySeconds = new(60);

        offlineManager.LoadOfflineTime();
        StartCoroutine(AutoSave());
        StartCoroutine(Tick());
        CheckPrices();

        soundManager = GameObject.FindGameObjectWithTag("audio").GetComponent<SoundManager>();
        MusicSource = GameObject.FindGameObjectWithTag("music");
        SoundSource = GameObject.FindGameObjectWithTag("sound");

        ad.LoadGraphics();

        BetaContentToggles[0].onValueChanged.AddListener(delegate { ChangeBetaContentFeatureValue("BETA_ResearchFactory", BetaContentToggles[0].isOn); });

        MusicAudioSource = MusicSource.GetComponent<AudioSource>();
        SoundAudioSource = SoundSource.GetComponent<AudioSource>();

        AllowUpdate = true;

        // ThemeManager.instance.SelectTheme(DefaultThemeScene).Forget();
    }

    public void PlayInitialFadeOut()
    {
        Fade.Play("FadeOut");
    }

#if !CC2_REMOVE_VR_SUPPORT
    public async UniTask InitVR()
    {
        if (VRManager.instance.VREnabled)
        {
            try
            {
                VRPrefabGO = Instantiate(await Addressables.LoadAssetAsync<GameObject>(VRPrefab), VRPrefabParent);
                VRPrefabObject vrpo = VRPrefabGO.GetComponent<VRPrefabObject>();

                XROrigin = vrpo.XROrigin;
                researchFactory.VRCamera = vrpo.XROrigin.transform;
                researchFactory.MainSceneVR = vrpo.MainSceneVR;

                vrFade.InitVR();
            }
            catch (Exception ex)
            {
                LogSystem.Log(ex.ToString(), LogTypes.Exception);
                LoadVRFallbackScene();
                return;
            }

            PP.profile = VRProfile;

            gameCamera.gameObject.SetActive(false);
        }
#if UNITY_ANDROID
        if (VRManager.instance.IsMobileVR)
        {
            try
            {
                displaySubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
                if (displaySubsystem.TryGetSupportedDisplayRefreshRates(Unity.Collections.Allocator.Temp, out var refreshRates))
                {
                    List<string> options = new();
                    OculusQuestRefreshRateDropdownData oqrrdd = OculusQuestRefreshRateDropdown.GetComponent<OculusQuestRefreshRateDropdownData>();
                    foreach (var rf in refreshRates)
                    {
                        options.Add(rf.ToString() + " FPS");
                        oqrrdd.refreshRates.Add(rf);
                    }
                    OculusQuestRefreshRateDropdown.AddOptions(options);
                }
                else
                {
                    LogSystem.Log("Failed to get supported refresh rates.", LogTypes.Warning);
                }
            }
            catch
            {
                LogSystem.Log("Unknown error while getting supported refresh rates.", LogTypes.Error);
            }

            XRSettings.useOcclusionMesh = false;

            AndroidVROnlySettingsButton.SetActive(true);
        }
#endif
    }
#endif

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
        if (CookieFactoryPrice < 320)
        {
            CookieFactorys = 0;
            CookieFactoryPrice = 320;
        }
    }

    IEnumerator AutoSave()
    {
        yield return sixtySeconds;
        SavePlayer();
        StartCoroutine(AutoSave());
    }

    IEnumerator Tick()
    {
        yield return oneSecond;
        Cookies += CPS;
        TimePlayed += 1;
        StartCoroutine(Tick());
    }

    public void SavePlayer()
    {
        PlayerPrefs.Save();
        offlineManager.SaveTime();

        BetterPrefs.SetString("Cookies", Cookies.ToString());
        BetterPrefs.SetString("CPC", CPC.ToString());
        BetterPrefs.SetString("CPS", CPS.ToString());
        BetterPrefs.SetString("TimePlayed", TimePlayed.ToString());
        BetterPrefs.SetBool("HasPlayed", HasPlayed);
        BetterPrefs.SetString("Autoclickers", Autoclickers.ToString());
        BetterPrefs.SetString("Doublecookies", Doublecookies.ToString());
        BetterPrefs.SetString("AutoclickerPrice", AutoclickerPrice.ToString());
        BetterPrefs.SetString("DoublecookiePrice", DoublecookiePrice.ToString());
        BetterPrefs.SetString("Drills", Drills.ToString());
        BetterPrefs.SetString("DrillPrice", DrillPrice.ToString());
        BetterPrefs.SetBool("ResearchFactory", ResearchFactory);
        BetterPrefs.SetBool("offlineProgressCheck", offlineManager.offlineProgressCheck);
        BetterPrefs.SetString("OfflineTime", offlineManager.OfflineTime);
        BetterPrefs.SetBool("Sounds", Sounds);
        BetterPrefs.SetBool("Music", Music);
        BetterPrefs.SetString("Grandmas", Grandmas.ToString());
        BetterPrefs.SetString("GrandmaPrice", GrandmaPrice.ToString());
        BetterPrefs.SetString("CookieFactorys", CookieFactorys.ToString());
        BetterPrefs.SetString("CookieFactoryPrice", CookieFactoryPrice.ToString());
        BetterPrefs.SetString("ResearchPoints", researchFactory.ResearchPoints.ToString());
        BetterPrefs.SetBool("BigCookieResearched", researchFactory.BigCookieResearched);
        BetterPrefs.SetFloat("HammerStrength", HammerStrength);
        BetterPrefs.SetFloat("HammerEnergy", HammerEnergy);
        BetterPrefs.SetString("HammerStrengthUpgradePrice", HammerEnergyUpgradePrice.ToString());
        BetterPrefs.SetString("Coins", Coins.ToString());
        BetterPrefs.SetString("HammerEnergyUpgradePrice", HammerEnergyUpgradePrice.ToString());
        BetterPrefs.SetString("CoinMultiplierUpgradePrice", CoinMultiplierUpgradePrice.ToString());
        BetterPrefs.SetString("CoinMultiplier", CoinMultiplier.ToString());

        BetterPrefs.Save();
    }

    public void LoadPlayer()
    {
        ad.LoadGraphics();

        try
        {
            BetterPrefs.Load(Application.persistentDataPath + "/Saves/Default.cookie");
        }
        catch(Exception ex)
        {
            SDIE.SetActive(true);
            SmallErrorText.text = "" + ex.Message;
            ErrorText.text = "" + ex;
        }

        Cookies = BigDouble.Parse(BetterPrefs.GetString("Cookies", "0"));
        CPC = BigDouble.Parse(BetterPrefs.GetString("CPC", "1"));
        CPS = BigDouble.Parse(BetterPrefs.GetString("CPS", "0"));
        TimePlayed = BigDouble.Parse(BetterPrefs.GetString("TimePlayed", "0"));
        HasPlayed = BetterPrefs.GetBool("HasPlayed", false);
        Autoclickers = BigDouble.Parse(BetterPrefs.GetString("Autoclickers", "0"));
        Doublecookies = BigDouble.Parse(BetterPrefs.GetString("Doublecookies", "0"));
        AutoclickerPrice = BigDouble.Parse(BetterPrefs.GetString("AutoclickerPrice", "0"));
        DoublecookiePrice = BigDouble.Parse(BetterPrefs.GetString("DoublecookiePrice", "0"));
        Drills = BigDouble.Parse(BetterPrefs.GetString("Drills", "0"));
        DrillPrice = BigDouble.Parse(BetterPrefs.GetString("DrillPrice", "0"));
        ResearchFactory = BetterPrefs.GetBool("ResearchFactory", false);
        offlineManager.offlineProgressCheck = BetterPrefs.GetBool("offlineProgressCheck", false);;
        offlineManager.OfflineTime = BetterPrefs.GetString("OfflineTime", "");
        Sounds = BetterPrefs.GetBool("Sounds", false);
        Music = BetterPrefs.GetBool("Music", false);
        Grandmas = BigDouble.Parse(BetterPrefs.GetString("Grandmas", "0"));
        GrandmaPrice = BigDouble.Parse(BetterPrefs.GetString("GrandmaPrice", "0"));
        CookieFactorys = BigDouble.Parse(BetterPrefs.GetString("CookieFactorys", "0"));
        CookieFactoryPrice = BigDouble.Parse(BetterPrefs.GetString("CookieFactoryPrice", "0"));

        researchFactory.ResearchPoints = BigDouble.Parse(BetterPrefs.GetString("ResearchPoints", "1"));
        researchFactory.BigCookieResearched = BetterPrefs.GetBool("BigCookieResearched", false);

        researchFactory.LoadResearchFactory();

        HammerStrength = BetterPrefs.GetFloat("HammerStrength", 0.2f);
        HammerEnergy = BetterPrefs.GetFloat("HammerEnergy", 100);
        HammerStrengthUpgradePrice = double.Parse(BetterPrefs.GetString("HammerStrengthUpgradePrice", "100"));
        Coins = double.Parse(BetterPrefs.GetString("Coins", "100"));
        HammerEnergyUpgradePrice = double.Parse(BetterPrefs.GetString("HammerEnergyUpgradePrice", "200"));
        CoinMultiplierUpgradePrice = double.Parse(BetterPrefs.GetString("CoinMultiplierUpgradePrice", "300"));
        CoinMultiplier = double.Parse(BetterPrefs.GetString("CoinMultiplier", "1"));

        CheckResearchFactory();
        CheckDrill();
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
        CookieFactorys = 0;
        CookieFactoryPrice = 0;

        researchFactory.ResearchPoints = 1;
        researchFactory.BigCookieUnlocked = false;
        researchFactory.BigCookieResearched = false;

        HammerStrength = 0.2f;
        HammerEnergy = 100;
        HammerStrengthUpgradePrice = 100;
        Coins = 100;
        HammerEnergyUpgradePrice = 200;
        CoinMultiplierUpgradePrice = 300;
        CoinMultiplier = 1;

        BetterPrefs.DeleteAll();
        BetterPrefs.Save();

        ad.SetDefaults();

        SavePlayer();

        Reload();
    }

    public void BakeCookie()
    {
        Cookies += CPC;
        Instantiate(CookieGains, CookieGainsSpot);
        Instantiate(CookieVFX, CookieVFXSpot);

        if (!BetterPrefs.GetBool("BakeCookie", false))
        {
            BetterPrefs.SetBool("BakeCookie", true);

            AchievementManager.instance.UpdateAchievements();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        SavePlayer();
    }

    public void Reload()
    {
        ReloadAsync().Forget();
    }

    private async UniTaskVoid ReloadAsync()
    {
        Fade.Play("FadeIn");
        FadeCanvasGroup.blocksRaycasts = true;
        await UniTask.WaitForSeconds(1);

        MusicManager.instance.UnloadSong();

        await ThemeManager.instance.UnloadTheme();

        await Addressables.UnloadSceneAsync(AddressableHandles.instance.gameSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

#if !CC2_REMOVE_VR_SUPPORT
        Destroy(VRPrefabGO);
        Addressables.Release(VRPrefab);
#endif

        AddressableHandles.instance.initSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.initSceneRef, LoadSceneMode.Single);
        await AddressableHandles.instance.initSceneHandle;
    }

    public void SoundToggle(bool Toggle)
    {
        Sounds = Toggle;
    }

    public void MusicToggle(bool Toggle)
    {
        Music = Toggle;
    }

#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
    public void ChangeRefreshRate(float val)
    {
        if (displaySubsystem == null)
        {
            LogSystem.Log("displaySubsystem was null!", LogTypes.Error);
        }
        if (!displaySubsystem.TryRequestDisplayRefreshRate(val))
        {
            LogSystem.Log("Failed to request refresh rate of " + val, LogTypes.Error);
        }
    }
#endif

#if !CC2_REMOVE_VR_SUPPORT
    public void LoadVRFallbackScene()
    {
        LoadVRFallbackSceneAsync().Forget();
    }

    private async UniTaskVoid LoadVRFallbackSceneAsync()
    {
        SavePlayer();
        
        await Addressables.UnloadSceneAsync(AddressableHandles.instance.gameSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        AddressableHandles.instance.vrFallbackSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.vrFallbackSceneRef, LoadSceneMode.Single);
        await AddressableHandles.instance.vrFallbackSceneHandle;
    }
#endif

    public void BuyAutoclicker()
    {
        if (Cookies >= AutoclickerPrice)
        {
            Cookies -= AutoclickerPrice;
            AutoclickerPrice += 25;
            Autoclickers += 1;
            CPS += 1;

            if (!BetterPrefs.GetBool("TenAC", false))
            {
                if (Autoclickers >= 10)
                {
                    BetterPrefs.SetBool("TenAC", true);
                    AchievementManager.instance.UpdateAchievements();
                }
            }
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

            if (!BetterPrefs.GetBool("TenDC", false))
            {
                if (Autoclickers >= 10)
                {
                    BetterPrefs.SetBool("TenDC", true);
                    AchievementManager.instance.UpdateAchievements();
                }
            }
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

            if (!BetterPrefs.GetBool("TenDrills", false))
            {
                if (Autoclickers >= 10)
                {
                    BetterPrefs.SetBool("TenDrills", true);
                    AchievementManager.instance.UpdateAchievements();
                }
            }

            CheckDrill();
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

            if (!BetterPrefs.GetBool("TenGrandmas", false))
            {
                if (Autoclickers >= 10)
                {
                    BetterPrefs.SetBool("TenGrandmas", true);
                    AchievementManager.instance.UpdateAchievements();
                }
            }
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void BuyCookieFactory()
    {
        if (Cookies >= CookieFactoryPrice)
        {
            Cookies -= CookieFactoryPrice;
            CookieFactoryPrice += 320;
            CookieFactorys++;
            CPS += 8;
            CPC += 4;

            if (!BetterPrefs.GetBool("TenCF", false))
            {
                if (Autoclickers >= 10)
                {
                    BetterPrefs.SetBool("TenCF", true);
                    AchievementManager.instance.UpdateAchievements();
                }
            }
        }
        else
        {
            NECDialog.SetActive(true);
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

    public void EnableBetaContent()
    {
        BetterPrefs.SetBool("BetaContent", true);
        BetaContentScreen.SetActive(true);
        BetaContentWarningScreen.GetComponent<WindowAnimations>().HideWindow();
    }

    public void DisableBetaContent()
    {
        BetterPrefs.SetBool("BETA_ResearchFactory", false);

        bc.UpdateBetaContent();
        BetaContentScreen.GetComponent<WindowAnimations>().HideWindow();

        BetterPrefs.SetBool("BetaContent", false);
    }

    public void ChangeBetaContentFeatureValue(string name, bool toggle)
    {
        BetterPrefs.SetBool(name, toggle);
        bc.UpdateBetaContent();
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

    public void CheckResearchFactory()
    {
        if (ResearchFactory)
        {
            ThemeManager.instance.CurrentTheme.ResearchFactory.SetActive(true);
            Research_Factory_Particals.SetActive(true);
        }
        else
        {
            ThemeManager.instance.CurrentTheme.ResearchFactory.SetActive(false);
            Research_Factory_Particals.SetActive(false);
        }
    }

    public void CheckDrill()
    {
        if (Drills >= 1)
        {
            ThemeManager.instance.CurrentTheme.Drill.SetActive(true);
            Drill_Partical.SetActive(true);
        }
        else
        {
            ThemeManager.instance.CurrentTheme.Drill.SetActive(false);
            Drill_Partical.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!AllowUpdate)
        {
            return;
        }

        CookiesText.text = "Cookies: " + Cookies;
        Shop_Autoclicker.text = "Autoclicker (" + AutoclickerPrice + " Cookies)";
        Shop_Doublecookie.text = "Doublecookie (" + DoublecookiePrice + " Cookies)";
        Shop_Drill.text = "Drill (" + DrillPrice + " Cookies)";
        Shop_Grandma.text = "Grandma (" + GrandmaPrice + " Cookies)";
        Shop_CookieFactory.text = "Cookie Factory (" + CookieFactoryPrice + " Cookies)";

        // stats
        Stats_Cookies.text = "Cookies: " + Cookies;
        Stats_Autoclickers.text = "Autoclickers: " + Autoclickers;
        Stats_Doublecookies.text = "Doublecookies: " + Doublecookies;
        Stats_Drills.text = "Drills: " + Drills;
        Stats_CPC.text = "Cookies Per Click: " + CPC;
        Stats_CPS.text = "Cookies Per Second: " + CPS;
        Stats_Grandmas.text = "Grandmas: " + Grandmas;
        Stats_CookieFactorys.text = "Cookie Factorys: " + CookieFactorys;

        // music & sounds
        if (Music == false)
        {
            MusicAudioSource.volume = 0;
        }
        else
        {
            MusicAudioSource.volume = 1;
        }
        if (Sounds == false)
        {
            SoundAudioSource.volume = 0;
        }
        else
        {
            SoundAudioSource.volume = 1;
        }
    }
}