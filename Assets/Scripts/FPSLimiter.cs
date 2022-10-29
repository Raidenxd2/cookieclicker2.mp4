using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetMaxFPS();
    }

    void SetMaxFPS()
    {
        int refreshRate = Screen.currentResolution.refreshRate;
        Application.targetFrameRate = refreshRate;
        Debug.Log("[DEBUG] User Refresh Rate: " + refreshRate);
    }
}
