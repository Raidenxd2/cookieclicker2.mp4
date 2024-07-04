using TMPro;
using UnityEngine;

public class OpenURLManager : MonoBehaviour
{
    public static OpenURLManager instance;

    private string currentURL;

    [SerializeField] private GameObject OpenWebpageScreen;

    [SerializeField] private TMP_Text LinkText;

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

    public void OpenURLInBrowser()
    {
        OpenWebpageScreen.GetComponent<WindowAnimations>().HideWindow();

        Application.OpenURL(currentURL);
    }
}