using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsItemMod : MonoBehaviour
{

    public int ShopItemAmount;
    public string ShopItemName;
    public TMP_Text StatsItemText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShopItemAmount = PlayerPrefs.GetInt("MOD_" + ShopItemName + "_Amount");
        StatsItemText.text = ShopItemName + "s: " + ShopItemAmount;
    }
}
