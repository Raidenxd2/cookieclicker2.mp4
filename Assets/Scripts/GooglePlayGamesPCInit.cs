using LoggerSystem;
using UnityEngine;

public class GooglePlayGamesPCInit : MonoBehaviour
{
    [SerializeField] private bool Editor_PCMode;

    private void Start()
    {
        if (PlatformCheck.IsGooglePlayGames || Editor_PCMode)
        {
            LogSystem.Log("PC Init");
            
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.numerator;
            QualitySettings.SetQualityLevel(1);
        }
    }
}