using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAnimations : MonoBehaviour
{

    public LeanTweenType inType;
    public LeanTweenType outType;
    public float Time;

    //shows the window
    void OnEnable()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), Time).setEase(inType);
        LeanTween.rotateX(gameObject, 30, Time).setEase(inType);
    }

    //hides the window
    public void HideWindow()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), Time).setEase(outType).setOnComplete(HideWindow2);
        LeanTween.rotateX(gameObject, -90, Time).setEase(outType);
    }

    //LEAN TWEEN WONT LET ME SETACTIVE IN THE THING
    public void HideWindow2()
    {
        gameObject.SetActive(false);
    }
}
