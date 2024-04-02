#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using System.Collections;
using TMPro;
using BreakInfinity;
using IngameDebugConsole;
using Tayx.Graphy;
using LoggerSystem;

public class DEBUG : MonoBehaviour
{
    public TMP_Text FPSText;
    public TMP_Text RendererText;
    public TMP_InputField CookiesInput;
    public Game game;
    public GameObject DEBUGScreen;
    public GameObject TerrainObject;
    public int FPS;
    public TMP_InputField FPSInput;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FPSDisplay());
        LogSystem.Log("Current Renderer: " + SystemInfo.graphicsDeviceType);
        GraphyManager.Instance.Disable();
        RendererText.text = "Current Renderer: " + SystemInfo.graphicsDeviceType;
    }

    IEnumerator FPSDisplay()
    {
        yield return new WaitForSeconds(1);
        FPS = (int)(1f / Time.unscaledDeltaTime);
        FPSText.text = FPS.ToString("FPS: " + "0");
        StartCoroutine(FPSDisplay());
    }

    public void ShowDebugConsole()
    {
        DebugLogManager.Instance.ShowLogWindow();
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

    public void ShowGraphy()
    {
        var fps = GraphyManager.Instance.CurrentFPS;
        GraphyManager.Instance.Enable();
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