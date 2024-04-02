using UnityEngine;
using System.IO;
using TMPro;
using LoggerSystem;
using UnityEngine.InputSystem;

public class ScreenShot : MonoBehaviour 
{
    public string filePath;
    public Notification notification;
    public int ScreenshotQuality;
    public TMP_Text ScreenshotQualityText;
    public bool NotificationsInScreenshots;
    public GameObject Nnotification;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();

        try
        {
            if (!Directory.Exists(Application.persistentDataPath + filePath))
            {
                Directory.CreateDirectory(Application.persistentDataPath + filePath);
            }
        }
        catch (IOException ex)
        {
            LogSystem.Log(ex.Message, LogTypes.Exception);
            notification.ShowNotification(ex.Message, "Error");
        }
    }

    public void DeleteScreenshots()
    {
        try
        {
            Directory.Delete(Application.persistentDataPath + filePath, true);
            Directory.CreateDirectory(Application.persistentDataPath + filePath);
        }
        catch (IOException ex)
        {
            LogSystem.Log(ex.Message, LogTypes.Exception);
            notification.ShowNotification(ex.Message, "Error");
        }
    }

    public void NotifcationScreenshotToggle()
    {
        NotificationsInScreenshots = !NotificationsInScreenshots;
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

        if (NotificationsInScreenshots)
        {
            Nnotification.SetActive(true);
        }
        else
        {
            Nnotification.SetActive(false);
        }
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/screenshots/" + datetime + ".png", ScreenshotQuality);
        notification.ShowNotification("Screenshot saved at " + Application.persistentDataPath + filePath + "/" + datetime + ".png", "Screenshot Taken");
        Nnotification.SetActive(true);
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
}