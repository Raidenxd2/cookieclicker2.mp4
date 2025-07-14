using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{

    private string NotificationText;
    private string NotificationTitle;
    public TMP_Text NotificationTextObject;
    public TMP_Text NotificationTitleObject;
    public GameObject NotificationObject;

    public void ShowNotification(string text, string title)
    {
        NotificationText = text;
        NotificationTitle = title;
        NotificationTextObject.text = NotificationText;
        NotificationTitleObject.text = NotificationTitle;
        NotificationObject.SetActive(true);
    }
}
