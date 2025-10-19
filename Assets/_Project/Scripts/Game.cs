using System.Collections;
using UnityEngine;
using BreakInfinity;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoggerSystem;
using UnityEngine.Rendering;
using Cysharp.Threading.Tasks;
using System;
using SimpleFileBrowser;
using UnityEngine.Localization;
using System.IO;

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
    public BigDouble CookieFarms;
    public BigDouble CookieFarmPrice;
    public bool HasPlayed;
    public bool ResearchFactory;
    public bool Music;
    public bool Sounds;
    public bool VRMirrorCamera;

    // game objects
    [Header("Game Objects")]
    public GameObject Drill_Partical;
    public GameObject NECDialog;
    public GameObject SDIE;
    public GameObject NoNetworkScreen;
    [SerializeField] private GameObject GlobalDark;
    [SerializeField] private GameObject VREnableCustomMirrorCameraToggle;

    // scripts
    [Header("Scripts")]
    [SerializeField] private OfflineManager offlineManager;
    public AdvancedQualitySettings ad;
    [SerializeField] private Notification notification;
    [SerializeField] private AddressableLightmaps al;
    [SerializeField] private BetaContent bc;
    [SerializeField] private VRFadeCanvas vrFade;

    // text
    [Header("Text")]
    public TMP_Text CookiesText;
    public TMP_Text Shop_Autoclicker;
    public TMP_Text Shop_Doublecookie;
    public TMP_Text Shop_Drill;
    public TMP_Text Shop_Grandma;
    public TMP_Text Shop_CookieFactory;
    public TMP_Text Shop_CookieFarm;
    public TMP_Text ErrorText;
    public TMP_Text SmallErrorText;
    public TMP_Text VersionText;

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
    public TMP_Text Stats_CookieFarms;

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

    [Header("Boss Cookies")]
    public BigDouble BossCookies_HammerStrength;
    public BigDouble BossCookies_HammerStrengthUpgradePrice;

    [Header("VR")]
    private GameObject VRPrefabGO;
    private VRPrefabObject vrpo;
    [SerializeField] private Transform VRPrefabParent;
    public GameObject XROrigin;
    [SerializeField] private UISkin FileBrowserUISkin;
    [SerializeField] private LocalizedString ExportSaveFileSuccess;
    [SerializeField] private LocalizedString SaveManagement;
    [SerializeField] private GameObject ExportImportSaveFileDark;
    [SerializeField] private GameObject ImportSaveFileWarningScreen;

    private bool AllowUpdate;

    public WaitForSeconds oneSecond;
    public WaitForSeconds sixtySeconds;

    private void Awake()
    {
        instance = this;

        BetterPrefs.Load("/Saves/Default.cookie");
    }

    private void OnDestroy()
    {
        instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        VersionText.text = "v" + Application.version + "-" + Application.platform + " (" + Application.unityVersion + ", " + SystemInfo.graphicsDeviceType + ")";

        if (!VRManager.instance.VREnabled)
        {
            VREnableCustomMirrorCameraToggle.SetActive(false);
        }

        LoadPlayer();

        if (HasPlayed == false)
        {
            HasPlayed = true;
            Music = true;
            Sounds = true;
            VRMirrorCamera = false;
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

        MusicSource = GameObject.FindGameObjectWithTag("music");
        SoundSource = GameObject.FindGameObjectWithTag("sound");

        ad.LoadGraphics();

        BetaContentToggles[0].onValueChanged.AddListener(delegate { ChangeBetaContentFeatureValue("BETA_ResearchFactory", BetaContentToggles[0].isOn); });

        MusicAudioSource = MusicSource.GetComponent<AudioSource>();
        SoundAudioSource = SoundSource.GetComponent<AudioSource>();

        AllowUpdate = true;

        FileBrowser.Skin = FileBrowserUISkin;

        UpdateAudio();
    }

    public void PlayInitialFadeOut()
    {
        Fade.Play("FadeOut");
    }

    public async UniTask InitVR()
    {
        if (VRManager.instance.VREnabled)
        {
            try
            {
                VRPrefabGO = Instantiate(await Resources.LoadAsync("GameScene_VRPrefab") as GameObject);
                vrpo = VRPrefabGO.GetComponent<VRPrefabObject>();

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

        UpdateMirrorCamera();
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
        if (CookieFactoryPrice < 320)
        {
            CookieFactorys = 0;
            CookieFactoryPrice = 320;
        }
        if (CookieFarmPrice < 1100)
        {
            CookieFarms = 0;
            CookieFarmPrice = 1100;
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
        BetterPrefs.SetString("CookieFarms", CookieFarms.ToString());
        BetterPrefs.SetString("CookieFarmPrice", CookieFarmPrice.ToString());
        BetterPrefs.SetString("ResearchPoints", researchFactory.ResearchPoints.ToString());
        BetterPrefs.SetBool("BigCookieResearched", researchFactory.BigCookieResearched);
        BetterPrefs.SetFloat("HammerStrength", HammerStrength);
        BetterPrefs.SetFloat("HammerEnergy", HammerEnergy);
        BetterPrefs.SetString("HammerStrengthUpgradePrice", HammerEnergyUpgradePrice.ToString());
        BetterPrefs.SetString("Coins", Coins.ToString());
        BetterPrefs.SetString("HammerEnergyUpgradePrice", HammerEnergyUpgradePrice.ToString());
        BetterPrefs.SetString("CoinMultiplierUpgradePrice", CoinMultiplierUpgradePrice.ToString());
        BetterPrefs.SetString("CoinMultiplier", CoinMultiplier.ToString());
        BetterPrefs.SetBool("VR_MirrorCamera", VRMirrorCamera);
        BetterPrefs.SetString("BossCookies_HammerStrength", BossCookies_HammerStrength.ToString());
        BetterPrefs.SetString("BossCookies_HammerStrengthUpgradePrice", BossCookies_HammerStrengthUpgradePrice.ToString());

        BetterPrefs.Save();
    }

    public void LoadPlayer()
    {
        ad.LoadGraphics();

        try
        {
            BetterPrefs.Load(Application.persistentDataPath + "/Saves/Default.cookie");
        }
        catch (Exception ex)
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
        offlineManager.offlineProgressCheck = BetterPrefs.GetBool("offlineProgressCheck", false); ;
        offlineManager.OfflineTime = BetterPrefs.GetString("OfflineTime", "");
        Sounds = BetterPrefs.GetBool("Sounds", false);
        Music = BetterPrefs.GetBool("Music", false);
        Grandmas = BigDouble.Parse(BetterPrefs.GetString("Grandmas", "0"));
        GrandmaPrice = BigDouble.Parse(BetterPrefs.GetString("GrandmaPrice", "0"));
        CookieFactorys = BigDouble.Parse(BetterPrefs.GetString("CookieFactorys", "0"));
        CookieFactoryPrice = BigDouble.Parse(BetterPrefs.GetString("CookieFactoryPrice", "0"));
        CookieFarms = BigDouble.Parse(BetterPrefs.GetString("CookieFarms", "0"));
        CookieFarmPrice = BigDouble.Parse(BetterPrefs.GetString("CookieFarmPrice", "0"));

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

        BossCookies_HammerStrength = BigDouble.Parse(BetterPrefs.GetString("BossCookies_HammerStrength", "1"));
        BossCookies_HammerStrengthUpgradePrice = BigDouble.Parse(BetterPrefs.GetString("BossCookies_HammerStrengthUpgradePrice", "1000"));

        VRMirrorCamera = BetterPrefs.GetBool("VR_MirrorCamera", false);

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
        CookieFarms = 0;
        CookieFarmPrice = 0;

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

        BossCookies_HammerStrength = 1;
        BossCookies_HammerStrengthUpgradePrice = 1000;

        BetterPrefs.DeleteAll();
        BetterPrefs.Save();

        ad.SetDefaults();

        SavePlayer();

        Reload();
    }

    public void ExportSaveFile()
    {
        ExportImportSaveFileDark.SetActive(true);
        FileBrowser.SetFilters(false, ".cookie");
        FileBrowser.ShowSaveDialog(ExportOnSuccess, ExportOnCancel, FileBrowser.PickMode.Files, false, null, "Default.cookie", "Export Default.cookie", "Export");
    }

    private void ExportOnSuccess(string[] paths)
    {
        FileBrowserHelpers.WriteTextToFile(paths[0], File.ReadAllText(Application.persistentDataPath + "/Saves/Default.cookie"));
        ExportImportSaveFileDark.SetActive(false);
        notification.ShowNotification(ExportSaveFileSuccess.GetLocalizedString(), SaveManagement.GetLocalizedString());
    }

    private void ExportOnCancel()
    {
        ExportImportSaveFileDark.SetActive(false);
    }

    public void ImportSaveFile()
    {
        ExportImportSaveFileDark.SetActive(true);
        Time.timeScale = 0;

        FileBrowser.SetFilters(false, ".cookie");
        FileBrowser.ShowLoadDialog(ImportOnSuccess, ImportOnCancel, FileBrowser.PickMode.Files, false, null, null, "Import", "Import");
    }

    public static string importPath;

    private void ImportOnSuccess(string[] paths)
    {
        importPath = paths[0];
        ExportImportSaveFileDark.SetActive(false);
        ImportSaveFileWarningScreen.SetActive(true);
        Time.timeScale = 1;
    }

    public void ImportSaveFileFinish()
    {
        ReloadAsync().Forget();
    }

    public void ImportCancel()
    {
        importPath = null;
    }

    private void ImportOnCancel()
    {
        ExportImportSaveFileDark.SetActive(false);
        Time.timeScale = 1;
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

#if UNITY_STANDALONE_WIN
    public void RestartGame()
    {
        SavePlayer();

        Application.OpenURL(Application.dataPath + "\\..\\Cookieclicker2.mp4.exe");

        Application.Quit();
    }
#endif

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

        if (VRPrefabGO != null)
        {
            Destroy(VRPrefabGO);
        }

        await SceneManager.LoadSceneAsync(AddressableHandles.initSceneRef);
    }

    public void MirrorCameraToggle(bool Toggle)
    {
        VRMirrorCamera = Toggle;

        UpdateMirrorCamera();
    }

    private void UpdateMirrorCamera()
    {
        if (vrpo == null)
        {
            return;
        }

        if (VRMirrorCamera)
        {
            vrpo.MirrorCamera.SetActive(true);
        }
        else
        {
            vrpo.MirrorCamera.SetActive(false);
        }
    }

    public void SoundToggle(bool Toggle)
    {
        Sounds = Toggle;

        UpdateAudio();
    }

    public void MusicToggle(bool Toggle)
    {
        Music = Toggle;

        UpdateAudio();
    }

    private void UpdateAudio()
    {
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

    public void UpdateCheckerToggle(bool Toggle)
    {
        BetterPrefs.SetBool("DisableUpdateChecker", !Toggle);
    }

    public void LoadVRFallbackScene()
    {
        LoadVRFallbackSceneAsync().Forget();
    }

    private async UniTaskVoid LoadVRFallbackSceneAsync()
    {
        SavePlayer();

        await SceneManager.LoadSceneAsync(AddressableHandles.vrFallbackSceneRef);
    }

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
                if (Doublecookies >= 10)
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
                if (Drills >= 10)
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
                if (Grandmas >= 10)
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
                if (CookieFactorys >= 10)
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

    public void BuyCookieFarm()
    {
        if (Cookies >= CookieFarmPrice)
        {
            Cookies -= CookieFarmPrice;
            CookieFarmPrice += 1100;
            CookieFarms++;
            CPS += 18;
            CPC += 9;

            if (!BetterPrefs.GetBool("TenCFA", false))
            {
                if (CookieFarms >= 10)
                {
                    BetterPrefs.SetBool("TenCFA", true);
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
        if (BetterPrefs.GetBool("BetaContent", false))
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
    void FixedUpdate()
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
        Shop_CookieFarm.text = "Cookie Farm (" + CookieFarmPrice + " Cookies)";

        // stats
        Stats_Cookies.text = "Cookies: " + Cookies;
        Stats_Autoclickers.text = "Autoclickers: " + Autoclickers;
        Stats_Doublecookies.text = "Doublecookies: " + Doublecookies;
        Stats_Drills.text = "Drills: " + Drills;
        Stats_CPC.text = "Cookies Per Click: " + CPC;
        Stats_CPS.text = "Cookies Per Second: " + CPS;
        Stats_Grandmas.text = "Grandmas: " + Grandmas;
        Stats_CookieFactorys.text = "Cookie Factorys: " + CookieFactorys;
        Stats_CookieFarms.text = "Cookie Farms: " + CookieFarms;
    }
}