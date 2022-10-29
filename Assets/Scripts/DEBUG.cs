using UnityEngine;
using TMPro;
using BreakInfinity;
using IngameDebugConsole;

public class DEBUG : MonoBehaviour
{

    public TMP_Text FPSText;
    public TMP_InputField CookiesInput;
    public Game game;
    public GameObject DEBUGScreen;

    // Start is called before the first frame update
    void Start()
    {
        
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

    // Update is called once per frame
    void Update()
    {
        if (DEBUGScreen.activeSelf)
        {
            FPSText.text = "FPS: " + 1.0f / Time.deltaTime;
        }
    }
}
