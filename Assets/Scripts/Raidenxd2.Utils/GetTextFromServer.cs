using System.IO;
using UnityEngine;
using TMPro;

public class GetTextFromServer : MonoBehaviour
{
    public string URL;
    public int Item;
    public TMP_Text Text;
    public string ErrorText = "Uh oh, something went wrong while downloading the file. Please try again later. Error details: ";

    // Start is called before the first frame update
    void Start()
    {
        if (CacheFile.DownloadedFile)
        {
            var data = File.ReadAllText(Application.temporaryCachePath + "/Cache/CookieStore.txt");
            var dataSplit = data.Split(",");
            Text.text = dataSplit[Item];
        }
    }
}