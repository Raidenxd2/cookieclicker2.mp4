using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshotOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string datetime = System.DateTime.Now.ToString("MM-dd-yyyy hh;mm;ss");
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + datetime + ".png");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
