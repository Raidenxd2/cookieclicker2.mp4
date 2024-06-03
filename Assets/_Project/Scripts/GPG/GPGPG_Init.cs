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

            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            QualitySettings.antiAliasing = 4;
        
            Application.targetFrameRate = 60;
        }
    }
}