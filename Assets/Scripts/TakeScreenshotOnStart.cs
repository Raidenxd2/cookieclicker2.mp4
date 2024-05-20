using UnityEngine;

public class TakeScreenshotOnStart : MonoBehaviour
{
    void Start()
    {
        string datetime = System.DateTime.Now.ToString("MM-dd-yyyy hh;mm;ss");
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + datetime + ".png");
    }
}