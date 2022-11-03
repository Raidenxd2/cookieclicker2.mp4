using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{

    public bool FpsLimit;
    public bool LimitToUsersRefreshRate;
    public int RefreshRate;

    // Start is called before the first frame update
    void Start()
    {
        if (FpsLimit)
        {
            Debug.Log("[DEBUG] Current FPS Limit: " + Application.targetFrameRate);
            SetMaxFPS();
        }
    }

    void SetMaxFPS()
    {
        int refreshRate = Screen.currentResolution.refreshRate;
        if (LimitToUsersRefreshRate)
        {
            Application.targetFrameRate = refreshRate;
        }
        else
        {
            Application.targetFrameRate = RefreshRate;
        }
        
        Debug.Log("[DEBUG] New FPS Limit: " + Application.targetFrameRate);
        Debug.Log("[DEBUG] User Refresh Rate: " + refreshRate);
    }
}
