using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsLimitFocus : MonoBehaviour
{

    public FPSLimiter fpsLimiter;

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            fpsLimiter.SetMaxFPS();
        }
        else
        {
            Application.targetFrameRate = 10;
        }
    }
}
