using System.Collections;
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
    public Transform WhatWasThisNamed;
    public Animator Fade;
    public int BigCookieDuration;
    public TMP_Text BigCookieText;
    public bool BigCookieUnlocked;
    public bool BigCookieResearching;
    public TMP_Text ResearchPointsText;
    [SerializeField] private Transform CookieTransform;
    [SerializeField] private Notification notification;

    [Header("Variables")]
    public BigDouble ResearchPoints;
    public bool BigCookieResearched;

    void Start()
    {
        // ResearchPoints = 1;
        BigCookieDuration = 0;
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1);
        
        if (BigCookieDuration > -1 && BigCookieResearching == true)
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
            game.Cookies -= 20000;

            game.CheckResearchFactory();
            CheckIfUserOwnsResearchFactory();
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void EnterScienceFactory()
    {
        StartCoroutine(Fade1());
    }

    public void ExitScienceFactory()
    {
        StartCoroutine(Fade2());
    }

    IEnumerator Fade1()
    {
        Fade.Play("FadeIn");
        yield return new WaitForSeconds(1);
        GameCamera.SetPositionAndRotation(WhatWasThisNamed.position, WhatWasThisNamed.rotation);
        FinishEnter();
        Fade.Play("FadeOut");
    }

    IEnumerator Fade2()
    {
        Fade.Play("FadeIn");
        yield return new WaitForSeconds(1);
        GameCamera.SetPositionAndRotation(MainScene.position, MainScene.rotation);
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
            BigCookieResearching = true;
            ResearchPoints -= 1;
        }
    }

    public void LoadResearchFactory()
    {
        if (BigCookieResearched)
        {
            CookieTransform.localScale = new Vector3(600, 600, 600);
        }

        CheckIfUserOwnsResearchFactory();
    }

    public void CheckIfUserOwnsResearchFactory()
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
    }

    void Update()
    {
        if (BigCookieDuration < 0 && BigCookieResearching == true)
        {
            BigCookieUnlocked = true;
            BigCookieResearched = true;
            BigCookieResearching = false;
            BigCookieDuration = 0;

            game.CPC += 10;
            game.CPS += 10;

            CookieTransform.localScale = new Vector3(600, 600, 600);

            notification.ShowNotification("Big Cookie researched!", "Research");
        }

        BigCookieText.text = BigCookieDuration + "s remaining";
        ResearchPointsText.text = "Points: " + ResearchPoints;
    }
}