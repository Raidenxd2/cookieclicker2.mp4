using UnityEngine;
using System.IO;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ScreenShot : MonoBehaviour 
{
    public string filePath;
    public Notification notification;
    public int ScreenshotQuality;
    public TMP_Text ScreenshotQualityText;

#if !UNITY_WEBGL
    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + filePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + filePath);
        }
    }
#endif

    public void DeleteScreenshots()
    {
#if !UNITY_WEBGL
        Directory.Delete(Application.persistentDataPath + filePath, true);
        Directory.CreateDirectory(Application.persistentDataPath + filePath);
#endif
    }

#if !UNITY_WEBGL
    void Update()
    {
        if (Keyboard.current.f12Key.wasPressedThisFrame)
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        string datetime = System.DateTime.Now.ToString("MM-dd-yyyy hh;mm;ss");

        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/screenshots/" + datetime + ".png", ScreenshotQuality);
        notification.ShowNotification("Screenshot saved at " + Application.persistentDataPath + filePath + "/" + datetime + ".png", "Screenshot Taken");
    }
#endif

#if !UNITY_WEBGL
    public void OnValueChanged(float newValue)
    {
        string StringConvert;
        int IntConvert;
        StringConvert = newValue.ToString("0");
        
        IntConvert = int.Parse(StringConvert);
        ScreenshotQuality = IntConvert;
        ScreenshotQualityText.text = IntConvert + "x";
    }
#endif

    public void OpenScreenshotsFolder()
    {
#if !UNITY_WEBGL
        Application.OpenURL(Application.persistentDataPath + "/screenshots");
#endif
    }
}