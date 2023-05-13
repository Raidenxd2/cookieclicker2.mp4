using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSLimiter : MonoBehaviour
{

    public bool FpsLimit;
    public bool LimitToUsersRefreshRate;
    public int RefreshRate;

    public TMP_InputField fpsInput;

    // Start is called before the first frame update
    void Start()
    {
        if (FpsLimit)
        {
            Debug.Log("[DEBUG] Current FPS Limit: " + Application.targetFrameRate);
            SetMaxFPS();
        }
    }

    public void SetMaxFPS()
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

    public void SetFPS()
    {
        Application.targetFrameRate = int.Parse(fpsInput.text);
    }
}
