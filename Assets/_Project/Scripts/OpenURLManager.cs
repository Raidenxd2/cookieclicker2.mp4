using TMPro;
using UnityEngine;

public class OpenURLManager : MonoBehaviour
{
#if !UNITY_WEBGL
    public static OpenURLManager instance;

    private string currentURL;
#endif

    [SerializeField] private GameObject OpenWebpageScreen;

    [SerializeField] private TMP_Text LinkText;

#if !UNITY_WEBGL
    private void Awake()
    {
        instance = this;
    }

    public void OpenURL(string url)
    {
        currentURL = url;

        LinkText.text = url;

        OpenWebpageScreen.SetActive(true);
    }
#endif

    public void OpenURLInBrowser()
    {
#if !UNITY_WEBGL
        OpenWebpageScreen.GetComponent<WindowAnimations>().HideWindow();

        Application.OpenURL(currentURL);
#endif
    }
}