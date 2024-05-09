using DG.Tweening;
using UnityEngine;

public class WindowAnimations : MonoBehaviour
{
    public Ease inEase;
    public Ease outEase;
    public float Time;

    //shows the window
    void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(-90, 0, 0);

        transform.DOScale(new Vector3(1, 1, 1), Time).SetEase(inEase);
        transform.DORotate(new Vector3(30, 0, 0), Time).SetEase(inEase);
    }

    //hides the window
    public void HideWindow()
    {
        transform.DOScale(new Vector3(0, 0, 0), Time).SetEase(outEase).onComplete = () => gameObject.SetActive(false);
        transform.DORotate(new Vector3(-90, 0, 0), Time).SetEase(outEase);
    }
}