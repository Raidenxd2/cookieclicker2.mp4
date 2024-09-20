using LitMotion;
using UnityEngine;

public class CookieAnimations : MonoBehaviour
{
    public Ease inEase;
    public Ease outEase;
    public float Time;

    public void GoDown()
    {
        LMotion.Create(new Vector3(0, -11.62416f, 13.0128f), new Vector3(0, -11.82416f, 13.0128f), Time)
            .WithEase(inEase)
            .WithOnComplete(() => GoUp());
    }

    public void GoUp()
    {
        LMotion.Create(new Vector3(0, -11.82416f, 13.0128f), new Vector3(0, -11.62416f, 13.0128f), Time)
            .WithEase(outEase);
    }
}