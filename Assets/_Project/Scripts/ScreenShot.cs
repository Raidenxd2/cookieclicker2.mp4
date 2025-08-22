#if !UNITY_ANDROID
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.InputSystem;

public class ScreenShot : MonoBehaviour 
{
    public string filePath;
    public Notification notification;
    public int ScreenshotQuality;
    public TMP_Text ScreenshotQualityText;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();

        if (!Directory.Exists(Application.persistentDataPath + filePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + filePath);
        }
    }

    public void DeleteScreenshots()
    {
        Directory.Delete(Application.persistentDataPath + filePath, true);
        Directory.CreateDirectory(Application.persistentDataPath + filePath);
    }

    void Update()
    {
        if (playerInput.actions["Screenshot"].WasPressedThisFrame())
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

    public void OnValueChanged(float newValue)
    {
        string StringConvert;
        int IntConvert;
        StringConvert = newValue.ToString("0");
        
        IntConvert = int.Parse(StringConvert);
        ScreenshotQuality = IntConvert;
        ScreenshotQualityText.text = IntConvert + "x";
    }

    public void OpenScreenshotsFolder()
    {
        Application.OpenURL(Application.persistentDataPath + "/screenshots");
    }
}
#endif