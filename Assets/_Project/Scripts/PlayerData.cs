using BreakInfinity;

[System.Serializable]
public class PlayerData
{

    // Game
    public BigDouble Cookies;
    public BigDouble CPS;
    public BigDouble CPC;
    public BigDouble TimePlayed;
    public BigDouble Autoclickers;
    public BigDouble Doublecookies;
    public BigDouble AutoclickerPrice;
    public BigDouble DoublecookiePrice;
    public BigDouble Drills;
    public BigDouble DrillPrice;
    public BigDouble Grandmas;
    public BigDouble GrandmaPrice;
    public BigDouble CookieFactorys;
    public BigDouble CookieFactoryPrice;
    public bool HasPlayed;
    public bool ResearchFactory;
    public bool Music;
    public bool Sounds;
    public float LastSavedGameVersion;
    public bool StarterBundleBought;
    public int Clicks;

    // Offline
    public bool offlineProgressCheck;
    public string OfflineTime;

    // Advanced Quality Settings
    public bool PostProcessing;
    public bool Particals;
    public bool Lighting;
    public bool Trees;
    public bool VSync;
    public bool Fog;
    public int TextureQuality;

    // Research Factory
    public BigDouble ResearchPoints;
    public bool BigCookieResearched;

    public PlayerData (Game ga, OfflineManager om, AdvancedQualitySettings ad, ResearchFactory rf)
    {
        Cookies = ga.Cookies;
        CPS = ga.CPS;
        CPC = ga.CPC;
        TimePlayed = ga.TimePlayed;
        HasPlayed = ga.HasPlayed;
        Autoclickers = ga.Autoclickers;
        Doublecookies = ga.Doublecookies;
        AutoclickerPrice = ga.AutoclickerPrice;
        DoublecookiePrice = ga.DoublecookiePrice;
        Drills = ga.Drills;
        DrillPrice = ga.DrillPrice;
        ResearchFactory = ga.ResearchFactory;
        offlineProgressCheck = om.offlineProgressCheck;
        OfflineTime = om.OfflineTime;
        PostProcessing = ad.PostProcessing;
        Particals = ad.Particals;
        Lighting = ad.Lighting;
        Trees = ad.Trees;
        VSync = ad.VSync;
        TextureQuality = ad.TextureQuality;
        LastSavedGameVersion = ga.LastSavedGameVersion;
        Music = ga.Music;
        Sounds = ga.Sounds;
        Fog = ad.Fog;
        Grandmas = ga.Grandmas;
        GrandmaPrice = ga.GrandmaPrice;
        StarterBundleBought = ga.StarterBundleBought;
        Clicks = ga.Clicks;
        CookieFactorys = ga.CookieFactorys;
        CookieFactoryPrice = ga.CookieFactoryPrice;

        ResearchPoints = rf.ResearchPoints;
        BigCookieResearched = rf.BigCookieResearched;
    }
}
