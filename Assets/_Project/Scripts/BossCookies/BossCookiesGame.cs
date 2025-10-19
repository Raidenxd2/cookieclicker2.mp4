using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private GameObject NECDialog;

    [SerializeField] private Ease ease;
    [SerializeField] private float time;

    [SerializeField] private LocalizedString CookiesGainedNotificationTitle;
    [SerializeField] private LocalizedString CookiesGainedNotificationMessage;

    [SerializeField] private TMP_Text CookiesText;
    [SerializeField] private TMP_Text HammerStrengthText;
    [SerializeField] private TMP_Text HammerStrengthPriceText;

    public Vector3 OldVRPosition;
    public Quaternion OldVRRotation;
    public Transform OldVRParent;

    public static BossCookiesGame instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (Game.instance.BossCookies_HammerStrengthUpgradePrice <= 1000)
        {
            Game.instance.BossCookies_HammerStrength = 1f;
            Game.instance.BossCookies_HammerStrengthUpgradePrice = 1000;
        }

        if (VRManager.instance.VREnabled)
        {
            OldVRPosition = Game.instance.XROrigin.transform.position;
            OldVRRotation = Game.instance.XROrigin.transform.rotation;
            OldVRParent = Game.instance.XROrigin.transform.parent;

            // Camera.SetActive(false);

            // Game.instance.XROrigin.transform.SetPositionAndRotation(VRCameraPosition.position, VRCameraPosition.rotation);
            // Game.instance.XROrigin.transform.parent = Player;

            // UI.transform.parent = Player;
        }

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
        BossCookieObjects[CurrentBossCookieObjectsIndex].Health -= (float)Game.instance.BossCookies_HammerStrength;

        if (BossCookieObjects[CurrentBossCookieObjectsIndex].Health <= 0)
        {
            Game.instance.Cookies += BossCookieObjects[CurrentBossCookieObjectsIndex].CookiesAmount;
            ShowCookiesGainedNotificationAsync().Forget();

            BossCookieObjects[CurrentBossCookieObjectsIndex].Health = BossCookieObjects[CurrentBossCookieObjectsIndex].StartingHealth;
        }
    }

    public void Exit()
    {
        BossCookiesLoader.instance.UnloadMinigameMine();
    }

    private async UniTaskVoid ShowCookiesGainedNotificationAsync()
    {
        Notification.instance.ShowNotification(string.Format(await CookiesGainedNotificationMessage.GetLocalizedStringAsync(), BossCookieObjects[CurrentBossCookieObjectsIndex].CookiesAmount), await CookiesGainedNotificationTitle.GetLocalizedStringAsync());
    }

    public void BuyHammerStrengthUpgrade()
    {
        if (Game.instance.Cookies >= Game.instance.BossCookies_HammerStrengthUpgradePrice)
        {
            Game.instance.Cookies -= Game.instance.BossCookies_HammerStrengthUpgradePrice;
            Game.instance.BossCookies_HammerStrengthUpgradePrice *= 1.5f;
            Game.instance.BossCookies_HammerStrength += 1f;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        CookiesText.text = "Cookies: " + Game.instance.Cookies;
        HammerStrengthText.text = "Hammer Strength: " + Game.instance.BossCookies_HammerStrength;

        HammerStrengthPriceText.text = "Hammer Strength Upgrade (" + Game.instance.BossCookies_HammerStrengthUpgradePrice + " Cookies)";
    }

    private void OnDestroy()
    {
        instance = null;
    }
}