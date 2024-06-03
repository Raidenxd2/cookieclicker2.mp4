#if UNITY_STANDALONE
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PCGraphics : MonoBehaviour
{
    [SerializeField] private UniversalAdditionalCameraData cameraData;

    [SerializeField] private GameObject CookieStoreButton;
    [SerializeField] private GameObject PCOnlySettingsButton;
    [SerializeField] private GameObject CloudSaveBTN;

    private void Start()
    {
        cameraData.dithering = true;
        cameraData.renderPostProcessing = true;

        CookieStoreButton.SetActive(false);
        CloudSaveBTN.SetActive(false);
        PCOnlySettingsButton.SetActive(true);
    }
}
#endif