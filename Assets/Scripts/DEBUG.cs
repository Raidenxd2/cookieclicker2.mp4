using UnityEngine;
using System.Collections;
using TMPro;
using BreakInfinity;
using IngameDebugConsole;
using Tayx.Graphy;

public class DEBUG : MonoBehaviour
{

    public TMP_Text FPSText;
    public TMP_Text RendererText;
    public TMP_InputField CookiesInput;
    public Game game;
    public GameObject DEBUGScreen;
    public int FPS;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FPSDisplay());
        Debug.Log("Current Renderer: " + SystemInfo.graphicsDeviceType);
        GraphyManager.Instance.Disable();
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
            Debug.LogError("could not convert cookiesinput to a bigdouble due to invalid string");
        }
    }

    public void LoadCookies()
    {
        CookiesInput.text = "" + game.Cookies;
    }

    public void AbortCrash()
    {
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);
    }

    public void AccessViolationCrash()
    {
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.AccessViolation);
    }

    public void FatalErrorCrash()
    {
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.FatalError);
    }

    public void MonoAbortCrash()
    {
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.MonoAbort);
    }

    public void PureVirtualFunctionCrash()
    {
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.PureVirtualFunction);
    }

    public void ShowGraphy()
    {
        var fps = GraphyManager.Instance.CurrentFPS;
        GraphyManager.Instance.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        RendererText.text = "Current Renderer: " + SystemInfo.graphicsDeviceType;
    }
}
