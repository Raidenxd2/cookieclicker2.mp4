#if UNITY_STANDALONE
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PCGraphics : MonoBehaviour
{
    [SerializeField] private UniversalAdditionalCameraData cameraData;

    [SerializeField] private GameObject PCOnlySettingsButton;

    private void Start()
    {
        cameraData.dithering = true;
        cameraData.renderPostProcessing = true;

        PCOnlySettingsButton.SetActive(true);
    }
}
#endif