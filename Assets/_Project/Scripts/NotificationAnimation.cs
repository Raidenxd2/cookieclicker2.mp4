using System.Collections;
using UnityEngine;

public class NotificationAnimation : MonoBehaviour
{
    public Animator NotificationAnimations;
    public GameObject NotificationObject;
    private Vector2 position;
    private bool Playing;
    public RectTransform OringinalPos;

    private WaitForSeconds fiveSeconds;

    private void Awake()
    {
        fiveSeconds = new(5);
    }

    void OnEnable()
    {
        Playing = true;
        NotificationAnimations.Play("NotificationOpen");
        position = OringinalPos.anchoredPosition;
        StartCoroutine(NotificationWaitThenClose());
    }

    IEnumerator NotificationWaitThenClose()
    {
        yield return fiveSeconds;
        NotificationAnimations.Play("NotificationClose");
        yield return Game.instance.oneSecond;
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