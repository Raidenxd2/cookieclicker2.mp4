using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using TMPro;
using UnityEngine.SceneManagement;

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

    // scripts
    [Header("Scripts")]
    public OfflineManager offlineManager;
    public AdvancedQualitySettings ad;
    public SoundManager soundManager;

    // text
    [Header("Text")]
    public TMP_Text CookiesText;
    public TMP_Text Shop_Autoclicker;
    public TMP_Text Shop_Doublecookie;
    public TMP_Text Shop_Drill;

    // performance mode stuff
    

    // stats
    [Header("Stats")]
    public TMP_Text Stats_Cookies;
    public TMP_Text Stats_Doublecookies;
    public TMP_Text Stats_Autoclickers;
    public TMP_Text Stats_Drills;
    public TMP_Text Stats_CPS;
    public TMP_Text Stats_CPC;

    // animations
    [Header("Animations")]
    public Animator Fade;

    // audio
    [Header("Audio")]
    private GameObject MusicSource;
    private GameObject SoundSource;
    public int nothinghereyet;


    // Start is called before the first frame update
    void Start()
    {
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
            Debug.LogError("Could not assign audio. Did you load from the Init scene?");
        }
    }

    void SoundAssign()
    {
        soundManager = GameObject.FindGameObjectWithTag("audio").GetComponent<SoundManager>();
        MusicSource = GameObject.FindGameObjectWithTag("music");
        SoundSource = GameObject.FindGameObjectWithTag("sound");
    }

    void CheckPrices()
    {
        if (AutoclickerPrice <= 25)
        {
            Autoclickers = 0;
            AutoclickerPrice = 25;
        }
        if (DoublecookiePrice <= 50)
        {
            Doublecookies = 0;
            DoublecookiePrice = 50;
        }
        if (DrillPrice <= 100)
        {
            Drills = 0;
            DrillPrice = 100;
        }
    }

    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(60);
        SavePlayer();
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
        offlineManager.SaveTime();
        ad.SaveGraphics();
        SaveSystem.SavePlayer(this, offlineManager, ad);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        SaveSystem.LoadPlayer();

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
        // ad.PostProcessing = data.PostProcessing;
        // ad.Particals = data.Particals;
        // ad.Lighting = data.Lighting;
        // ad.Trees = data.Trees;
        // ad.VSync = data.VSync;
        // ad.TextureQuality = data.TextureQuality;
        LastSavedGameVersion = data.LastSavedGameVersion;
        Sounds = data.Sounds;
        Music = data.Music;
        // ad.Fog = data.Fog;
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
        SavePlayer();
        Debug.Log("Reset Data. Now reloading...");
        Reload();
    }

    public void BakeCookie()
    {
        Cookies += CPC;
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
        Resources.UnloadUnusedAssets();
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
        SceneManager.LoadScene("Reload");
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

    public void OpenHelp()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.OpenURL(Application.dataPath + "/StreamingAssets/help/graphicsapi/index.html");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CookiesText.text = "Cookies: " + Cookies;
        Shop_Autoclicker.text = "Autoclicker (" + AutoclickerPrice + " Cookies";
        Shop_Doublecookie.text = "Doublecookie (" + DoublecookiePrice + " Cookies";
        Shop_Drill.text = "Drill (" + DrillPrice + " Cookies";

        // performance mode
        // if (PerformanceMode)
        // {
        //     Trees.SetActive(false);
        //     GameCamera_AdditionalData.SetRenderer(0);
        //     QualitySettings.SetQualityLevel(0);
        //     Cookie_Normal.SetActive(false);
        //     Cookie_Performance.SetActive(true);
        //     AmbientParticals.SetActive(false);
        // }
        // else
        // {
        //     Trees.SetActive(true);
        //     GameCamera_AdditionalData.SetRenderer(1);
        //     QualitySettings.SetQualityLevel(1);
        //     Cookie_Normal.SetActive(true);
        //     Cookie_Performance.SetActive(false);
        //     AmbientParticals.SetActive(true);
        // }

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

        // music & sounds
        MusicSource.SetActive(Music);
        SoundSource.SetActive(Sounds);

        // reload
        if (Input.GetKey(KeyCode.Tab) && Input.GetKey(KeyCode.R))
        {
            SavePlayer();
            Reload();
        }
    }
}
