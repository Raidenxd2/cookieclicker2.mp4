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
    public bool PostProcessing;
    public bool PerformanceMode;
    public bool ResearchFactory;

    // game objects
    [Header("Game Objects")]
    public GameObject pp_normal;
    public GameObject pp_performance;
    public GameObject RelaunchRequiredScreen;
    public GameObject Drill_Model;
    public GameObject Drill_Partical;
    public GameObject NECDialog;

    // scripts
    [Header("Scripts")]
    public OfflineManager offlineManager;

    // text
    [Header("Text")]
    public TMP_Text CookiesText;
    public TMP_Text Shop_Autoclicker;
    public TMP_Text Shop_Doublecookie;
    public TMP_Text Shop_Drill;

    // performance mode stuff
    [Header("Performance Mode")]
    public GameObject Trees;
    public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData GameCamera_AdditionalData;
    public GameObject Cookie_Performance;
    public GameObject Cookie_Normal;
    public GameObject Research_Factory_Normal;
    public GameObject AmbientParticals;
    public GameObject Research_Factory_Particals;

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


    // Start is called before the first frame update
    void Start()
    {
        LoadPlayer();
        if (HasPlayed == false)
        {
            PostProcessing = true;
            HasPlayed = true;
            ResetData();
        }
        offlineManager.LoadOfflineTime();
        StartCoroutine(AutoSave());
        StartCoroutine(Tick());
        RelaunchRequiredScreen.SetActive(false);
        CheckPrices();
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
        SaveSystem.SavePlayer(this, offlineManager);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        SaveSystem.LoadPlayer();

        Cookies = data.Cookies;
        CPC = data.CPC;
        CPS = data.CPS;
        TimePlayed = data.TimePlayed;
        HasPlayed = data.HasPlayed;
        PostProcessing = data.PostProcessing;
        PerformanceMode = data.PerformanceMode;
        Autoclickers = data.Autoclickers;
        Doublecookies = data.Doublecookies;
        AutoclickerPrice = data.AutoclickerPrice;
        DoublecookiePrice = data.DoublecookiePrice;
        Drills = data.Drills;
        DrillPrice = data.DrillPrice;
        ResearchFactory = data.ResearchFactory;
        offlineManager.offlineProgressCheck = data.offlineProgressCheck;
        offlineManager.OfflineTime = data.OfflineTime;
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
        SavePlayer();
    }

    public void PostProcessToggle(bool Toggle)
    {
        PostProcessing = Toggle;
    }

    public void PerformanceModeToggle(bool Toggle)
    {
        PerformanceMode = Toggle;
    }

    public void Reload()
    {
        StartCoroutine(ReloadWait());
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

    // Update is called once per frame
    void Update()
    {
        CookiesText.text = "Cookies: " + Cookies;
        Shop_Autoclicker.text = "Autoclicker (" + AutoclickerPrice + " Cookies";
        Shop_Doublecookie.text = "Doublecookie (" + DoublecookiePrice + " Cookies";
        Shop_Drill.text = "Drill (" + DrillPrice + " Cookies";
        if (PostProcessing)
        {
            pp_normal.SetActive(true);
            pp_performance.SetActive(false);
        }
        else
        {
            pp_normal.SetActive(false);
            pp_performance.SetActive(true);
        }

        // performance mode
        if (PerformanceMode)
        {
            Trees.SetActive(false);
            GameCamera_AdditionalData.SetRenderer(0);
            QualitySettings.SetQualityLevel(0);
            Cookie_Normal.SetActive(false);
            Cookie_Performance.SetActive(true);
            AmbientParticals.SetActive(false);
        }
        else
        {
            Trees.SetActive(true);
            GameCamera_AdditionalData.SetRenderer(1);
            QualitySettings.SetQualityLevel(1);
            Cookie_Normal.SetActive(true);
            Cookie_Performance.SetActive(false);
            AmbientParticals.SetActive(true);
        }

        if (ResearchFactory)
        {
            Research_Factory_Normal.SetActive(true);
            if (PerformanceMode)
            {
                Research_Factory_Particals.SetActive(false);
            }
            else
            {
                Research_Factory_Particals.SetActive(true);
            }
        }
        else
        {
            Research_Factory_Normal.SetActive(false);
            Research_Factory_Particals.SetActive(false);
        }

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

        // reload
        if (Input.GetKey(KeyCode.Tab) && Input.GetKey(KeyCode.R))
        {
            SavePlayer();
            Reload();
        }
    }
}
