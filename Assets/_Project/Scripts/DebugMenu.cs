#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using TMPro;
using BreakInfinity;
using SerialPackage.Runtime;

public class DebugMenu : MonoBehaviour
{
    public TMP_InputField CookiesInput;
    public Game game;
    public TMP_InputField FPSInput;
    public GameObject DEBUGButton;

    public GameObject UtilsDebugRoot;

    // Start is called before the first frame update
    private void Start()
    {
        DEBUGButton.SetActive(true);

        if (!GameObject.Find("DebugRoot(Clone)"))
        {
            GameObject go = Instantiate(UtilsDebugRoot);
            DontDestroyOnLoad(go);
        }
    }

    public void UpdateCookies()
    {
        try
        {
            game.Cookies = BigDouble.Parse(CookiesInput.text);
        }
        catch
        {
            BeanLogger.LogError("Failed to set Cookies.", this);
        }
    }

    public void LoadCookies()
    {
        CookiesInput.text = "" + game.Cookies;
    }
}
#endif