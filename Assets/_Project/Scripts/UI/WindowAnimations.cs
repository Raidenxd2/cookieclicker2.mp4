using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class WindowAnimations : MonoBehaviour
{
    public Ease inEase;
    public Ease outEase;
    public float Time;

    //shows the window
    void OnEnable()
    {
        LMotion.Create(Vector3.zero, Vector3.one, Time)
            .WithEase(inEase)
            .BindToLocalScale(transform);
        LMotion.Create(new Vector3(-90, 0, 0), Vector3.zero, Time)
            .WithEase(inEase)
            .BindToLocalEulerAngles(transform);
    }

    //hides the window
    public void HideWindow()
    {
        LMotion.Create(Vector3.one, Vector3.zero, Time)
            .WithEase(outEase)
            .WithOnComplete(() => gameObject.SetActive(false))
            .BindToLocalScale(transform);
        LMotion.Create(Vector3.zero, new Vector3(-90, 0, 0), Time)
            .WithEase(outEase)
            .BindToLocalEulerAngles(transform);
    }
}