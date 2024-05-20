using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class Credits : MonoBehaviour
{
    public GameObject WebTextScreen;
    public GameObject DownloadedTextScroll;
    public TMP_Text DownloadedText;
    public GameObject NoNetworkScreen;
    public string ErrorText = "Uh oh, something went wrong while downloading the file. Please try again later. Error details: ";

    public void DownloadCredits(string url)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NoNetworkScreen.SetActive(true);
            return;
        }
        StartCoroutine(GetText(url));
    }

    IEnumerator GetText(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            AsyncOperation operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                DownloadedTextScroll.SetActive(true);
                DownloadedText.SetText(ErrorText + www.error);
            }
            else
            {
                DownloadedTextScroll.SetActive(true);
                DownloadedText.text = www.downloadHandler.text;
            }
        }
    }
}