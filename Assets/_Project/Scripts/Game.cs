using System.Collections;
using UnityEngine;
using BreakInfinity;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoggerSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

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
    public BigDouble CookieFactorys;
    public BigDouble CookieFactoryPrice;
    public bool HasPlayed;
    public bool ResearchFactory;
    public bool Music;
    public bool Sounds;

    // game objects
    [Header("Game Objects")]
    public GameObject Drill_Model;
    public GameObject Drill_Partical;
    public GameObject NECDialog;
    public GameObject SDIE;
    public GameObject NoNetworkScreen;
    public GameObject SaveDataWarningScreen;

    // scripts
    [Header("Scripts")]
    public OfflineManager offlineManager;
    public AdvancedQualitySettings ad;
    public SoundManager soundManager;
    public Notification notification;
    public AddressableLightmaps al;
    public BetaContent bc;

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
    public TMP_Text SaveDataWarningInfo;

    [Header("Buttons")]
    public Button SaveDataWarningYesButton;

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

    // audio
    [Header("Audio")]
    private GameObject MusicSource;
    private GameObject SoundSource;

    private AudioSource MusicAudioSource;
    private AudioSource SoundAudioSource;

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

    [Header("BetaContent")]
    public GameObject BetaContentWarningScreen;
    public GameObject BetaContentScreen;
    public Toggle[] BetaContentToggles;
    public GameObject ScreenshotOptionsBTN;

    [Header("IAP")]
    public GameObject StarterBundleBTN;
    public bool StarterBundleBought;

    [Header("Particles")]
    public GameObject CookieVFX;
    public GameObject CookieGains;
    public Transform CookieVFXSpot;
    public Transform CookieGainsSpot;

    [Header("Research Factory")]
    public GameObject Research_Factory_Normal;
    public GameObject Research_Factory_Particals;
    public ResearchFactory researchFactory;

    [Header("Localization")]
    public LocalizedString SaveDataWarningTexturesText;
    public LocalizedString SaveDataWarningLightingText;

    // Start is called before the first frame update
    void Start()
    {
        VersionText.text = "v" + Application.version + "-" + Application.platform + " (" + Application.unityVersion + ")";
        
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
        CheckPrices();
        try
        {
            SoundAssign();
        }
        catch
        {
            LogSystem.Log("Could not assign audio. Did you load from the Init scene?", LogTypes.Error);
        }
        ad.LoadGraphics();
        
        BetaContentToggles[0].onValueChanged.AddListener(delegate{ChangeBetaContentFeatureValue("BETA_EnableSideBar", BetaContentToggles[0].isOn);});
        BetaContentToggles[1].onValueChanged.AddListener(delegate{ChangeBetaContentFeatureValue("BETA_ResearchFactory", BetaContentToggles[1].isOn);});
        if (StarterBundleBought)
        {
            StarterBundleBTN.SetActive(false);
        }

        MusicAudioSource = MusicSource.GetComponent<AudioSource>();
        SoundAudioSource = SoundSource.GetComponent<AudioSource>();

        al.InitAddressableLightmaps();
    }

    public void HideStarterBundleButton()
    {
        StarterBundleBTN.SetActive(false);
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
        if (CookieFactoryPrice < 320)
        {
            CookieFactorys = 0;
            CookieFactoryPrice = 320;
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
        SaveSystem.SavePlayer(this, offlineManager, ad, researchFactory);
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
        Sounds = data.Sounds;
        Music = data.Music;
        Grandmas = data.Grandmas;
        GrandmaPrice = data.GrandmaPrice;
        StarterBundleBought = data.StarterBundleBought;
        CookieFactorys = data.CookieFactorys;
        CookieFactoryPrice = data.CookieFactoryPrice;

        researchFactory.ResearchPoints = data.ResearchPoints;
        researchFactory.BigCookieResearched = data.BigCookieResearched;

        researchFactory.LoadResearchFactory();

        CheckResearchFactory();
        CheckDrill();

        if (PlayerPrefs.GetInt("GRAPHICS_Textures") == 0)
        {
            SaveDataWarningScreen.SetActive(true);
            SaveDataWarningInfo.text = SaveDataWarningTexturesText.GetLocalizedString();
            SaveDataWarningYesButton.onClick.AddListener(() => PlayerPrefs.SetInt("GRAPHICS_Textures", 1));
        }

        
        if (PlayerPrefs.GetInt("GRAPHICS_Lighting") == 0)
        {
            SaveDataWarningScreen.SetActive(true);
            SaveDataWarningInfo.text = SaveDataWarningLightingText.GetLocalizedString();
            SaveDataWarningYesButton.onClick.AddListener(() => PlayerPrefs.SetInt("GRAPHICS_Lighting", 1));
        }
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

        SavePlayer();
        LogSystem.Log("Reset Data. Now reloading...");
        Reload();
    }

    public void BakeCookie()
    {
        Cookies += CPC;
        Instantiate(CookieGains, CookieGainsSpot);
        Instantiate(CookieVFX, CookieVFXSpot);
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
        LogSystem.Log("Reloading..");

        al.UnloadLightmaps();

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

        LogSystem.Log("Loading Init scene and unloading the Game scene.", LogTypes.Normal);

        Addressables.UnloadSceneAsync(AddressableHandles.instance.gameSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        AddressableHandles.instance.initSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.initSceneRef, LoadSceneMode.Single);

        while (!AddressableHandles.instance.initSceneHandle.IsDone)
        {
            yield return null;
        }
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
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void ChangeCameraBGColor(string color)
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

    public void EnableBetaContent()
    {
        PlayerPrefs.SetInt("BetaContent", 1);
        BetaContentScreen.SetActive(true);
        BetaContentWarningScreen.GetComponent<WindowAnimations>().HideWindow();
    }

    public void DisableBetaContent()
    {
        PlayerPrefs.SetInt("BETA_EnableSideBar", 0);
        PlayerPrefs.SetInt("BETA_Mods", 0);
        PlayerPrefs.SetInt("BETA_FPSLIMIT", 0);
        PlayerPrefs.SetInt("BETA_CookieMonster", 0);
        PlayerPrefs.SetInt("BETA_ResearchFactory", 0);

        bc.UpdateBetaContent();
        BetaContentScreen.GetComponent<WindowAnimations>().HideWindow();

        PlayerPrefs.SetInt("BetaContent", 0);
    }

    public void ChangeBetaContentFeatureValue(string name, bool toggle)
    {
        switch (toggle)
        {
            case false:
                PlayerPrefs.SetInt(name, 0);
                bc.UpdateBetaContent();
                break;
            case true:
                PlayerPrefs.SetInt(name, 1);
                bc.UpdateBetaContent();
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

    public void EntermodioScreen()
    {
        gameCamera.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void ExitmodioScreen()
    {
        gameCamera.transform.rotation = new Quaternion(30, 0, 0, 0);
    }

    public void CheckResearchFactory()
    {
        if (ResearchFactory)
        {
            Research_Factory_Normal.SetActive(true);
            Research_Factory_Particals.SetActive(true);
        }
        else
        {
            Research_Factory_Normal.SetActive(false);
            Research_Factory_Particals.SetActive(false);
        }
    }

    private void CheckDrill()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
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