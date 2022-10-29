using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;

public class ResearchFactory : MonoBehaviour
{

    public Game game;
    public GameObject NECDialog;
    public Animator CameraAnimation;

    [Header("UI")]
    public GameObject LockedText;
    public GameObject BuyBTN;
    public GameObject UnlockedUI;
    public GameObject GameCanvas;
    public GameObject ResearchCanvas;
    public GameObject ResearchScreen;
    public Transform GameCamera;
    public Transform MainScene;
    public Transform UnityIsSoFuckingStupidItHurts;
    public Animator Fade;
    public BigDouble ResearchPoints;
    public int BigCookieDuration;
    public TMP_Text BigCookieText;
    public bool BigCookieUnlocked;
    public TMP_Text ResearchPointsText;

    void Start()
    {
        ResearchPoints = 1;
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1);
        if (BigCookieDuration >= 0)
        {
            BigCookieDuration -= 1;
        }
        StartCoroutine(Tick());
    }

    public void UnlockResearchFactory()
    {
        if (game.Cookies >= 20000)
        {
            game.ResearchFactory = true;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void EnterScienceFactory()
    {
        StartCoroutine(FadeSHIT1());
    }

    public void ExitScienceFactory()
    {
        StartCoroutine(FadeSHIT2());
    }

    IEnumerator FadeSHIT1()
    {
        Fade.Play("FadeIn");
        yield return new WaitForSeconds(1);
        GameCamera.position = UnityIsSoFuckingStupidItHurts.position;
        GameCamera.rotation = UnityIsSoFuckingStupidItHurts.rotation;
        FinishEnter();
        Fade.Play("FadeOut");
    }

    IEnumerator FadeSHIT2()
    {
        Fade.Play("FadeIn");
        yield return new WaitForSeconds(1);
        GameCamera.position = MainScene.position;
        GameCamera.rotation = MainScene.rotation;
        FinishExit();
        Fade.Play("FadeOut");
    }

    public void FinishEnter()
    {
        GameCanvas.SetActive(false);
        ResearchCanvas.SetActive(true);
        ResearchScreen.SetActive(true);
    }

    public void FinishExit()
    {
        GameCanvas.SetActive(true);
        ResearchCanvas.SetActive(false);
    }

    public void ResearchBigCookie()
    {
        if (ResearchPoints >= 1)
        {
            BigCookieDuration = 30;
        }
    }

    void Update()
    {
        if (game.ResearchFactory)
        {
            LockedText.SetActive(false);
            BuyBTN.SetActive(false);
            UnlockedUI.SetActive(true);
        }
        else
        {
            LockedText.SetActive(true);
            BuyBTN.SetActive(true);
            UnlockedUI.SetActive(false);
        }
        if (BigCookieDuration <= 0)
        {
            BigCookieUnlocked = true;
        }
        BigCookieText.text = BigCookieDuration + "s remaining";
        ResearchPointsText.text = "" + ResearchPoints;
    }
}
