#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using TMPro;
using BreakInfinity;
using LoggerSystem;

public class DebugMenu : MonoBehaviour
{
    public TMP_InputField CookiesInput;
    public Game game;
    public TMP_InputField FPSInput;
    public GameObject DEBUGButton;

    // Start is called before the first frame update
    private void Start()
    {
        DEBUGButton.SetActive(true);
    }

    public void UpdateCookies()
    {
        try
        {
            game.Cookies = BigDouble.Parse(CookiesInput.text);
        }
        catch
        {
            LogSystem.Log("Failed to set Cookies.", LogTypes.Error);
        }
    }

    public void LoadCookies()
    {
        CookiesInput.text = "" + game.Cookies;
    }

    public void SetFPS()
    {
        Application.targetFrameRate = int.Parse(FPSInput.text);
    }
}
#endif