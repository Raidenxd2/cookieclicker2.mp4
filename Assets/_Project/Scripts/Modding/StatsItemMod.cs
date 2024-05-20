using UnityEngine;
using TMPro;

public class StatsItemMod : MonoBehaviour
{

    public int ShopItemAmount;
    public string ShopItemName;
    public TMP_Text StatsItemText;

    void Update()
    {
        ShopItemAmount = PlayerPrefs.GetInt("MOD_" + ShopItemName + "_Amount");
        StatsItemText.text = ShopItemName + "s: " + ShopItemAmount;
    }
}