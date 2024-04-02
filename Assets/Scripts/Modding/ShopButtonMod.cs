using UnityEngine;
using TMPro;

public class ShopButtonMod : MonoBehaviour
{
    public TMP_Text ShopItemNameText;
    public string ShopItemName;
    public int ShopItemPrice;
    public int ShopItemOldPrice;
    public int ShopItemCPS;
    public int ShopItemCPC;
    public int ShopItemAmount;
    public Game game;
    public GameObject NECDialog;

    void OnEnable()
    {
        ShopItemAmount = PlayerPrefs.GetInt("MOD_" + ShopItemName + "_Amount", 0);
        ShopItemPrice = PlayerPrefs.GetInt("MOD_" + ShopItemName + "_Price", ShopItemOldPrice);
    }

    void Update()
    {
        ShopItemNameText.text = ShopItemName + "(" + ShopItemPrice + " Cookies)";
    }

    public void BuyShopItem()
    {
        if (game.Cookies >= ShopItemPrice)
        {
            game.Cookies -= ShopItemPrice;
            ShopItemPrice += ShopItemOldPrice;
            ShopItemAmount += 1;
            game.CPS += ShopItemCPS;
            game.CPC += ShopItemCPC;
            PlayerPrefs.SetInt("MOD_" + ShopItemName + "_Amount", ShopItemAmount);
            PlayerPrefs.SetInt("MOD_" + ShopItemName + "_Price", ShopItemPrice);
            PlayerPrefs.Save();
        }
        else
        {
            NECDialog.SetActive(true);
        }
    }
}