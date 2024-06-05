#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using System.Collections;
using TMPro;
using BreakInfinity;
using LoggerSystem;

public class DEBUG : MonoBehaviour
{
    public TMP_Text FPSText;
    public TMP_Text RendererText;
    public TMP_InputField CookiesInput;
    public Game game;
    public GameObject DEBUGScreen;
    public GameObject TerrainObject;
    public int FramesPerSec;
    public TMP_InputField FPSInput;
    public GameObject DEBUGButton;

    // Start is called before the first frame update
    void Start()
    {
        DEBUGButton.SetActive(true);

        StartCoroutine(FPSDisplay());
        LogSystem.Log("Current Renderer: " + SystemInfo.graphicsDeviceType);
        RendererText.text = "Current Renderer: " + SystemInfo.graphicsDeviceType;
    }

    IEnumerator FPSDisplay()
    {
        int lastFrameCount = Time.frameCount;
        float lastTime = Time.realtimeSinceStartup;
        yield return new WaitForSeconds(1f);
 
        float timeSpan = Time.realtimeSinceStartup - lastTime;
        int frameCount = Time.frameCount - lastFrameCount;
 
        FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);

        FPSText.text = "FPS: " + FramesPerSec.ToString();

        StartCoroutine(FPSDisplay());
    }

    public void UpdateCookies()
    {
        try
        {
            game.Cookies = BigDouble.Parse(CookiesInput.text);
        }
        catch 
        {
            LogSystem.Log("could not convert cookiesinput to a bigdouble due to invalid string", LogTypes.Error);
        }
    }

    public void LoadCookies()
    {
        CookiesInput.text = "" + game.Cookies;
    }

    public void HideTerrain()
    {
        TerrainObject.SetActive(false);
    }

    public void SetFPS()
    {
        Application.targetFrameRate = int.Parse(FPSInput.text);
    }
}
#endif