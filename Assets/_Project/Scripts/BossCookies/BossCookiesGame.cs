using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class BossCookiesGame : MonoBehaviour
{
    [SerializeField] private BossCookieSO[] BossCookies;
    [SerializeField] private GameObject Cookie;
    [SerializeField] private Transform CookieParent;

    private List<BossCookieObject> BossCookieObjects;
    private int CurrentBossCookieObjectsIndex;

    public Transform Camera;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;

    [SerializeField] private Ease ease;
    [SerializeField] private float time;

    [SerializeField] private LocalizedString CookiesGainedNotificationTitle;
    [SerializeField] private LocalizedString CookiesGainedNotificationMessage;

    public float HammerDamage;

    public static BossCookiesGame instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BossCookieObjects = new();

        float zPos = 8;
        foreach (var cookie in BossCookies)
        {
            zPos -= 12;
            BossCookieObject bossCookie = Instantiate(Cookie, CookieParent).GetComponent<BossCookieObject>();
            bossCookie.transform.localPosition = new(14, 0, zPos);
            bossCookie.CookieNameText.text = cookie.CookieName;
            bossCookie.CookiesText.text = "Cookies: " + cookie.CookiesAmount;
            bossCookie.CookieRenderer.materials[0].color = cookie.CookieColor;
            bossCookie.CookiesAmount = cookie.CookiesAmount;
            bossCookie.StartingHealth = cookie.StartingHealth;
            bossCookie.Health = cookie.StartingHealth;
            BossCookieObjects.Add(bossCookie);
        }
    }

    public void Left()
    {
        RightArrow.SetActive(true);

        CurrentBossCookieObjectsIndex--;
        if (CurrentBossCookieObjectsIndex <= 0)
        {
            LeftArrow.SetActive(false);
        }

        LMotion.Create(Camera.position, new Vector3(100, 3, BossCookieObjects[CurrentBossCookieObjectsIndex].transform.position.z), time)
            .WithEase(ease)
            .BindToPosition(Camera);
    }

    public void Right()
    {
        LeftArrow.SetActive(true);

        CurrentBossCookieObjectsIndex++;
        if (CurrentBossCookieObjectsIndex >= BossCookieObjects.Count - 1)
        {
            RightArrow.SetActive(false);
        }

        LMotion.Create(Camera.position, new Vector3(100, 3, BossCookieObjects[CurrentBossCookieObjectsIndex].transform.position.z), time)
            .WithEase(ease)
            .BindToPosition(Camera);
    }

    public void HitCookie()
    {
        BossCookieObjects[CurrentBossCookieObjectsIndex].Health -= HammerDamage;

        if (BossCookieObjects[CurrentBossCookieObjectsIndex].Health <= 0)
        {
            Game.instance.Cookies += BossCookieObjects[CurrentBossCookieObjectsIndex].CookiesAmount;
            ShowCookiesGainedNotificationAsync().Forget();

            BossCookieObjects[CurrentBossCookieObjectsIndex].Health = BossCookieObjects[CurrentBossCookieObjectsIndex].StartingHealth;
        }
    }

    private async UniTaskVoid ShowCookiesGainedNotificationAsync()
    {
        Notification.instance.ShowNotification(string.Format(await CookiesGainedNotificationMessage.GetLocalizedStringAsync(), BossCookieObjects[CurrentBossCookieObjectsIndex].CookiesAmount), await CookiesGainedNotificationTitle.GetLocalizedStringAsync());
    }

    private void OnDestroy()
    {
        instance = null;
    }
}