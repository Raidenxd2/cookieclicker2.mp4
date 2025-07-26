using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject WebTextScreen;
    [SerializeField] private GameObject DownloadedTextScroll;
    [SerializeField] private TMP_Text DownloadedText;
    [SerializeField] private GameObject NoNetworkScreen;
    [SerializeField] private string ErrorText = "Uh oh, something went wrong while downloading the file. Please try again later.";

    public void DownloadCredits(string url)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NoNetworkScreen.SetActive(true);
            return;
        }

        GetText(url).Forget();
    }

    private async UniTaskVoid GetText(string url)
    {
        try
        {
            DownloadedText.text = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        }
        catch
        {
            DownloadedText.text = ErrorText;
        }

        DownloadedTextScroll.SetActive(true);
    }
}