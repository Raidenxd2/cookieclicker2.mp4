using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using RecRoomRipoff.Independent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class BossCookiesGame : MonoBehaviour
{
    [SerializeField] private BossCookieSO[] BossCookies;
    [SerializeField] private GameObject Cookie;
    [SerializeField] private Transform CookieParent;

    private List<BossCookieObject> BossCookieObjects;
    private int CurrentBossCookieObjectsIndex;

    public Transform Camera;
    [SerializeField] private Camera VRModeCamera;
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

    [SerializeField] private Canvas UI;
    [SerializeField] private Canvas NormalModeUI;
    [SerializeField] private GameObject NormalModeRoot;
    [SerializeField] private GameObject VRModeRoot;
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform VRCameraPosition;
    [SerializeField] private Transform VRRoot;

    [SerializeField] private GameObject ChooseModeScreen;
    [SerializeField] private GameObject GlobalDark;

    [SerializeField] private Vector3 NotificationPos;
    [SerializeField] private Vector3 NotificationRot;

#if !CC2_REMOVE_VR_SUPPORT
    public Vector3 OldVRPosition;
    public Quaternion OldVRRotation;
    public Transform OldVRParent;
#endif

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

#if CC2_DISABLEBOSSCOOKIESVRMODE
        ChooseNormalMode();
#else
        GlobalDark.SetActive(true);
        ChooseModeScreen.SetActive(true);
#endif

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            OldVRPosition = Game.instance.XROrigin.transform.position;
            OldVRRotation = Game.instance.XROrigin.transform.rotation;
            OldVRParent = Game.instance.XROrigin.transform.parent;

            Camera.gameObject.SetActive(false);

            Game.instance.XROrigin.transform.SetPositionAndRotation(VRCameraPosition.position, VRCameraPosition.rotation);
            Game.instance.XROrigin.transform.parent = VRRoot;

            UI.transform.parent = VRRoot;
            NormalModeUI.transform.parent = VRRoot;

            Notification.instance.NotificationCanvas.transform.SetPositionAndRotation(NotificationPos, Quaternion.Euler(NotificationRot));
            Notification.instance.NotificationCanvas.transform.parent = VRRoot;
        }
#endif

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

    public void ChooseNormalMode()
    {
        NormalModeRoot.SetActive(true);
        VRModeRoot.SetActive(false);
    }

    public void ChooseVRMode()
    {
        ChooseVRModeAsync().Forget();
    }

    private async UniTaskVoid ChooseVRModeAsync()
    {
#if !CC2_DISABLEBOSSCOOKIESVRMODE
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            NormalModeUI.gameObject.SetActive(false);
            Game.instance.XROrigin.SetActive(false);

            await UniTask.WaitForEndOfFrame();
        }
#endif

        NormalModeRoot.SetActive(false);
        VRModeRoot.SetActive(true);
        Camera.gameObject.SetActive(false);

        Notification.instance.NotificationCanvas.renderMode = RenderMode.WorldSpace;
        Notification.instance.NotificationCanvas.transform.parent = Player.transform;
        Notification.instance.NotificationCanvas.GetComponent<RectTransform>().localPosition = new(1, 0.5f, 0);

        UI.renderMode = RenderMode.WorldSpace;
        UI.transform.parent = Player.transform;
        if (VRManager.instance.VREnabled)
        {
            UI.transform.localPosition = new(1, 1.5f, 0);
        }
        else
        {
            UI.transform.localPosition = new(1, 0.5f, 0);
        }
        UI.transform.localScale = new(0.001f, 0.001f, 0.001f);
        UI.worldCamera = VRModeCamera;
        UI.GetComponent<GraphicRaycaster>().enabled = false;
        UI.gameObject.AddComponent<RaycasterWorld>();
#endif
    }

    public void Left()
    {
        RightArrow.SetActive(true);

        CurrentBossCookieObjectsIndex--;
        if (CurrentBossCookieObjectsIndex <= 0)
        {
            LeftArrow.SetActive(false);
        }

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            LMotion.Create(VRRoot.position, new Vector3(100, 3, BossCookieObjects[CurrentBossCookieObjectsIndex].transform.position.z), time)
            .WithEase(ease)
            .BindToPosition(VRRoot);
        }
        else
        {
#endif
            LMotion.Create(Camera.position, new Vector3(100, 3, BossCookieObjects[CurrentBossCookieObjectsIndex].transform.position.z), time)
            .WithEase(ease)
            .BindToPosition(Camera);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif
        
    }

    public void Right()
    {
        LeftArrow.SetActive(true);

        CurrentBossCookieObjectsIndex++;
        if (CurrentBossCookieObjectsIndex >= BossCookieObjects.Count - 1)
        {
            RightArrow.SetActive(false);
        }

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            LMotion.Create(VRRoot.position, new Vector3(100, 3, BossCookieObjects[CurrentBossCookieObjectsIndex].transform.position.z), time)
            .WithEase(ease)
            .BindToPosition(VRRoot);
        }
        else
        {
#endif
            LMotion.Create(Camera.position, new Vector3(100, 3, BossCookieObjects[CurrentBossCookieObjectsIndex].transform.position.z), time)
            .WithEase(ease)
            .BindToPosition(Camera);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif
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

    public async UniTaskVoid ShowCookiesGainedNotificationAsync(float Cookies = 0)
    {
        if (Cookies == 0)
        {
            Notification.instance.ShowNotification(string.Format(await CookiesGainedNotificationMessage.GetLocalizedStringAsync(), BossCookieObjects[CurrentBossCookieObjectsIndex].CookiesAmount), await CookiesGainedNotificationTitle.GetLocalizedStringAsync());
        }
        else
        {
            Notification.instance.ShowNotification(string.Format(await CookiesGainedNotificationMessage.GetLocalizedStringAsync(), Cookies), await CookiesGainedNotificationTitle.GetLocalizedStringAsync());
        }
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