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
}
