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
    public bool HasPlayed;
    public bool PostProcessing;
    public bool PerformanceMode;
    public bool ResearchFactory;

    // Offline
    public bool offlineProgressCheck;
    public string OfflineTime;

    public PlayerData (Game ga, OfflineManager om)
    {
        Cookies = ga.Cookies;
        CPS = ga.CPS;
        CPC = ga.CPC;
        TimePlayed = ga.TimePlayed;
        HasPlayed = ga.HasPlayed;
        PostProcessing = ga.PostProcessing;
        PerformanceMode = ga.PerformanceMode;
        Autoclickers = ga.Autoclickers;
        Doublecookies = ga.Doublecookies;
        AutoclickerPrice = ga.AutoclickerPrice;
        DoublecookiePrice = ga.DoublecookiePrice;
        Drills = ga.Drills;
        DrillPrice = ga.DrillPrice;
        ResearchFactory = ga.ResearchFactory;
        offlineProgressCheck = om.offlineProgressCheck;
        OfflineTime = om.OfflineTime;
    }
}
