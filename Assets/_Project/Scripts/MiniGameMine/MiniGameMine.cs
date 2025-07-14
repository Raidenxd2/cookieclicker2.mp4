using System.Collections;
using UnityEngine;
using TMPro;

public class MiniGameMine : MonoBehaviour
{
    public float tempHammerStrength;
    public float tempHammerEnergy;
    public GameObject NECDialog;
    public Transform Player;
    public Vector3 StartPos;
    public bool Moving;
    public TMP_Text HammerStrengthText;
    public TMP_Text HammerEnergyText;
    public TMP_Text CoinsText;
    public TMP_Text HammerStrengthPriceText;
    public TMP_Text HammerEnergyPriceText;
    public TMP_Text Depth;
    public TMP_Text CoinMultiplierPriceText;
    public GameObject LoseScreen;
    public TMP_Text LoseScreen_Depth;
    public bool reachedStone;
    public bool reachedHardStone;
    public bool reachedHeat;
    public bool reachedRed;
    public bool reachedWhite;
    public bool reachedEnd;
    public GameObject WinScreen;

#if !CC2_REMOVE_VR_SUPPORT
    public Vector3 OldVRPosition;
    public Quaternion OldVRRotation;
    public Transform OldVRParent;
#endif

    [SerializeField] private GameObject Camera;
#if !CC2_REMOVE_VR_SUPPORT
    [SerializeField] private Transform VRCameraPosition;
#endif
    [SerializeField] private GameObject UI;

    public static MiniGameMine instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        if (Game.instance.HammerStrengthUpgradePrice <= 100)
        {
            Game.instance.HammerStrength = 0.2f;
            Game.instance.HammerStrengthUpgradePrice = 100;
        }
        if (Game.instance.HammerEnergyUpgradePrice <= 200)
        {
            Game.instance.HammerEnergy = 100;
            Game.instance.HammerEnergyUpgradePrice = 200;
        }
        if (Game.instance.CoinMultiplierUpgradePrice <= 300)
        {
            Game.instance.CoinMultiplier = 1;
            Game.instance.CoinMultiplierUpgradePrice = 300;
        }

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            OldVRPosition = Game.instance.XROrigin.transform.position;
            OldVRRotation = Game.instance.XROrigin.transform.rotation;
            OldVRParent = Game.instance.XROrigin.transform.parent;

            Camera.SetActive(false);

            Game.instance.XROrigin.transform.position = VRCameraPosition.position;
            Game.instance.XROrigin.transform.rotation = VRCameraPosition.rotation;
            Game.instance.XROrigin.transform.parent = Player;

            UI.transform.parent = Player;
        }
#endif

        StartCoroutine(Init());
    }

    void Update()
    {
        if (Moving)
        {
            moveDown();
        }
        HammerStrengthText.text = "Drill Strength: " + tempHammerStrength;
        HammerEnergyText.text = "Drill Energy: " + tempHammerEnergy;
        HammerStrengthPriceText.text = "Drill Strength Upgrade (" + Game.instance.HammerStrengthUpgradePrice + " Coins";
        HammerEnergyPriceText.text = "Drill Energy Upgrade (" + Game.instance.HammerEnergyUpgradePrice + " Coins";
        CoinMultiplierPriceText.text = "Coin Multiplier Upgrade(" + Game.instance.CoinMultiplierUpgradePrice + " Coins";
        Depth.text = "Depth: " + Player.localPosition.y;
        CoinsText.text = Game.instance.Coins.ToString("Coins: " + "0");
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);
        tempHammerStrength = Game.instance.HammerStrength;
        tempHammerEnergy = Game.instance.HammerEnergy;
        Player.localPosition = StartPos;
        reachedHardStone = false;
        reachedStone = false;
        reachedHardStone = false;
        reachedRed = false;
        reachedWhite = false;
    }

    public void GameWin()
    {
        WinScreen.SetActive(true);
    }

    public void GameLose()
    {
        StartCoroutine(Init());
    }

    public void GameExit()
    {
        MiniGameMineLoader.instance.UnloadMinigameMine();
    }

    void moveDown()
    {
        if (tempHammerEnergy <= 0)
        {
            LoseScreen_Depth.text = "Depth: " + Player.localPosition.y;
            LoseScreen.SetActive(true);
            return;
        }
        Vector3 currentPos;
        Vector3 newPos;
        currentPos = Player.localPosition;
        newPos = new Vector3(currentPos.x, currentPos.y - tempHammerStrength, currentPos.z);
        Player.localPosition = newPos;
        Game.instance.Coins += Game.instance.HammerStrength * Game.instance.CoinMultiplier;
        double DebugCoins;
        DebugCoins = Game.instance.HammerStrength * Game.instance.CoinMultiplier;
        tempHammerEnergy -= 0.2f;
    }

    public void DecreseHammerStrength(float decrese)
    {
        tempHammerStrength -= decrese;
    }

    public void BuyHammerStrengthUpgrade()
    {
        if (Game.instance.Coins >= Game.instance.HammerStrengthUpgradePrice)
        {
            Game.instance.Coins -= Game.instance.HammerStrengthUpgradePrice;
            Game.instance.HammerStrengthUpgradePrice += 100;
            Game.instance.HammerStrength += 0.3f;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void BuyHammerEnergyUpgrade()
    {
        if (Game.instance.Coins >= Game.instance.HammerEnergyUpgradePrice)
        {
            Game.instance.Coins -= Game.instance.HammerEnergyUpgradePrice;
            Game.instance.HammerEnergyUpgradePrice += 200;
            Game.instance.HammerEnergy += 10;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }

    public void BuyCoinMultiplyerUpgrade()
    {
        if (Game.instance.Coins >= Game.instance.CoinMultiplierUpgradePrice)
        {
            Game.instance.Coins -= Game.instance.CoinMultiplierUpgradePrice;
            Game.instance.CoinMultiplierUpgradePrice += 300;
            Game.instance.CoinMultiplier += 0.3;
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }
}