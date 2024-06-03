using PrimeTween;
using UnityEngine;

public class CookieAnimations : MonoBehaviour
{
    public Ease inEase;
    public Ease outEase;
    public float Time;

    public void GoDown()
    {
        transform.DOLocalMoveY(-0.20f, Time).SetEase(inEase).OnComplete(() => GoUp());
    }

    public void GoUp()
    {
        transform.DOLocalMoveY(0, Time).SetEase(outEase);
    }
}