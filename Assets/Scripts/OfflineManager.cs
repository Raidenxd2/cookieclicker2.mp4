using System;
using UnityEngine;
using TMPro;
using BreakInfinity;

public class OfflineManager : MonoBehaviour
{

    public TMP_Text TimeAway;
    public TMP_Text CookiesGained;

    public GameObject OfflineProgressScreen;

    public DateTime currentTime;
    public DateTime oldTime;

    public Game game;

    public bool offlineProgressCheck;
    public string OfflineTime;

    public void LoadOfflineTime()
    {
        Debug.Log("offlineProgressCheck: " + offlineProgressCheck);
        if (offlineProgressCheck)
        {
            var tempOfflineTime = Convert.ToInt64(OfflineTime);
            var oldTime = DateTime.FromBinary(tempOfflineTime);
            currentTime = DateTime.Now;
            
            var difference = currentTime.Subtract(oldTime);
            var rawTime = (float) difference.TotalSeconds;
            var offlineTime = rawTime / 2;
            OfflineProgressScreen.SetActive(true);
            TimeSpan timer = TimeSpan.FromSeconds(rawTime);
            TimeAway.text = $"{timer:dd\\:hh\\:mm\\:ss}";

            BigDouble CookiesGain = ((int)offlineTime) * game.CPS;
            game.Cookies += CookiesGain;
            CookiesGained.text = CookiesGain + " Cookies";

            //debug
            Debug.Log("cookies gained from offline: " + CookiesGain);
            Debug.Log("Offline Time: " + offlineTime);
            Debug.Log("CPS: " + game.CPS);
        }
    }

    public void SaveTime()
    {
        OfflineTime = System.DateTime.Now.ToBinary().ToString();
        offlineProgressCheck = true;
    }
}
