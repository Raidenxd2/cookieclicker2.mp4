using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieAnimations : MonoBehaviour
{

    public LeanTweenType inType;
    public LeanTweenType outType;
    public float Time;

    public void GoDown()
    {
        LeanTween.moveLocalY(gameObject, -0.20f, Time).setOnComplete(GoUp);
    }

    public void GoUp()
    {
        LeanTween.moveLocalY(gameObject, 0, Time);
    }
}
