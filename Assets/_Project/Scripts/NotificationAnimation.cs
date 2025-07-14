using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationAnimation : MonoBehaviour
{

    public Animation NotificationAnimations;
    public GameObject NotificationObject;
    private RectTransform rectTransform;
    private Vector2 position;
    private bool Playing;
    public RectTransform OringinalPos;

    void OnEnable()
    {
        Playing = true;
        NotificationAnimations.Play("NotificationOpen");
        position = OringinalPos.anchoredPosition;
        StartCoroutine(NotificationWaitThenClose());
    }

    IEnumerator NotificationWaitThenClose()
    {
        yield return new WaitForSeconds(5);
        NotificationAnimations.Play("NotificationClose");
        yield return new WaitForSeconds(1);
        Playing = false;
    }

    void Update()
    {
        if (Playing == false)
        {
            OringinalPos.anchoredPosition = position;
            NotificationObject.SetActive(false);
        }
    }
}
