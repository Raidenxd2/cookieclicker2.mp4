using System.Collections;
using UnityEngine;
using TMPro;
using BreakInfinity;

public class ResearchFactory : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private AddressableLightmaps al;
    [SerializeField] private GameObject NECDialog;

    [Header("UI")]
    public GameObject LockedText;
    public GameObject BuyBTN;
    public GameObject UnlockedUI;
    public GameObject GameCanvas;
    public GameObject ResearchCanvas;
    public GameObject ResearchScreen;
    public Transform GameCamera;
    public Transform VRCamera;
    public Transform MainScene;
#if !CC2_REMOVE_VR_SUPPORT
    public Transform MainSceneVR;
#endif
    public Transform WhatWasThisNamed;
#if !CC2_REMOVE_VR_SUPPORT
    public Transform WhatWasThisNamedVR;
#endif
    public int BigCookieDuration;
    public TMP_Text BigCookieText;
    public bool BigCookieUnlocked;
    public bool BigCookieResearching;
    public TMP_Text ResearchPointsText;
    [SerializeField] private Notification notification;

    [Header("Variables")]
    public BigDouble ResearchPoints;
    public bool BigCookieResearched;

    void Start()
    {
        BigCookieDuration = 0;
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        yield return game.oneSecond;
        
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

            BetterPrefs.SetBool("UnlockResearchFactory", true);

            game.CheckResearchFactory();
            CheckIfUserOwnsResearchFactory();

            al.InitAddressableLightmaps();
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
        game.Fade.Play("FadeIn");
        game.FadeCanvasGroup.blocksRaycasts = true;
        yield return game.oneSecond;

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            VRCamera.position = WhatWasThisNamedVR.position;
        }
        else
        {
#endif
            GameCamera.SetPositionAndRotation(WhatWasThisNamed.position, WhatWasThisNamed.rotation);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

        FinishEnter();
        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }

    IEnumerator Fade2()
    {
        game.Fade.Play("FadeIn");
        game.FadeCanvasGroup.blocksRaycasts = true;
        yield return game.oneSecond;

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            VRCamera.position = MainSceneVR.position;
        }
        else
        {
#endif
            GameCamera.SetPositionAndRotation(MainScene.position, MainScene.rotation);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

        FinishExit();
        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
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
            ThemeManager.instance.CurrentTheme.Cookie.transform.localScale = new Vector3(600, 600, 600);
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

    void FixedUpdate()
    {
        if (BigCookieDuration < 0 && BigCookieResearching == true)
        {
            BigCookieUnlocked = true;
            BigCookieResearched = true;
            BigCookieResearching = false;
            BigCookieDuration = 0;

            game.CPC += 10;
            game.CPS += 10;

            ThemeManager.instance.CurrentTheme.Cookie.transform.localScale = new Vector3(600, 600, 600);

            notification.ShowNotification("Big Cookie researched!", "Research");
        }

        BigCookieText.text = BigCookieDuration + "s remaining";
        ResearchPointsText.text = "Points: " + ResearchPoints;
    }
}