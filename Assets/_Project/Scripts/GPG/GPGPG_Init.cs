using LoggerSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GPGPG_Init : MonoBehaviour
{
    private static bool IsInitialized;
    [SerializeField] private UniversalRenderPipelineAsset URPAsset;

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
            URPAsset.msaaSampleCount = 4;
        
            Application.targetFrameRate = 60;
        }
    }
}