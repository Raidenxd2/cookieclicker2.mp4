using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieMonster : MonoBehaviour
{

    // Colors
    public Color FogBoss;
    public Color FogNormal;

    // Lighting/Fog
    public float FogDensityBoss;
    public float FogDensityNormal;

    // GameObjects
    public GameObject BossBar;
    public GameObject BakeCookieBTN;
    public GameObject CookieMonsterModel;

    // UI
    public Slider BossHealth;
    float progressbarvalue;

    // Variables
    public int BossHealthV;

    void Start()
    {
        RenderSettings.fogColor = FogNormal;
        RenderSettings.fogDensity = FogDensityNormal;
        BossBar.SetActive(false);
        BakeCookieBTN.SetActive(true);
        
    }

    void Update()
    {
        var speed = 2; //Defines how fast it switches between the 2 values
 
        progressbarvalue = Mathf.Lerp(progressbarvalue, BossHealthV, Time.deltaTime * speed);
        BossHealth.value = progressbarvalue;
        // LoggerSystem.Logger.Log("" + progressbarvalue, LoggerSystem.LogTypes.Normal);
    }

    public void StartBoss()
    {
        RenderSettings.fogColor = FogBoss;
        RenderSettings.fogDensity = FogDensityBoss;
        BossBar.SetActive(true);
        BakeCookieBTN.SetActive(false);
        BossHealth.maxValue = 100;
        BossHealth.minValue = 0;
        BossHealth.value = 100;
        BossHealthV = 100;
    }
}
