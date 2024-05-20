using LoggerSystem;
using UnityEngine;

public class GPGPG_Init : MonoBehaviour
{
    private static bool IsInitialized;

    private void Start()
    {
        if (IsInitialized)
        {
            return;
        }

        if (IsGPGPC.instance.isPC)
        {
            LogSystem.Log("InitGPGPC");
            IsInitialized = true;

            QualitySettings.SetQualityLevel(1, true);
        
            Application.targetFrameRate = 60;
        }
    }
}