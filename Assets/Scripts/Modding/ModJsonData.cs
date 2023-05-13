using System;

[Serializable]
public class ModJsonData
{
    public string mod_name { get; set; }
    public string mod_version { get; set; }
    public string shop_item_name { get; set; }
    public int shop_item_cps { get; set; }
    public int shop_item_cpc { get; set; }
    public int shop_item_price { get; set; }
    public string mod_type { get; set; }
    public string theme_name { get; set; }
    public string theme_customsky_enabled { get; set; }
    public string theme_customsky_name { get; set; }
    public string android_support { get; set; }
    public string windows_support { get; set; }
    public string mac_support { get; set; }
    public string linux_support { get; set;  }
}
