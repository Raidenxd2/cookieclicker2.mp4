using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class AdvancedQualitySettings : MonoBehaviour
{
    public bool FPSDisplay;
    public bool PostProcessing;
    public bool Particals;
    public bool VSync;
    public bool Fog;
    public int TextureQuality;
    public float RenderQuality;
    public GameObject pp_normal;
    public TMP_Text RenderQualityText;
    public TMP_InputField RenderQualityInput;

    [SerializeField] private Toggle FPSDisplayUIToggle;
    [SerializeField] private Toggle PostProcessingUIToggle;
    [SerializeField] private Toggle ParticlesUIToggle;
    [SerializeField] private Toggle VSyncUIToggle;
    [SerializeField] private Toggle FogUIToggle;
    [SerializeField] private TMP_Dropdown TextureQualityDropdown;

    private UniversalRenderPipelineAsset URPAsset;
    
    public UniversalAdditionalCameraData GameCamera_AdditionalData;
    public GameObject ParticalsReal2;

    private void Awake()
    {
        URPAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
    }

    public void FPSDisplayToggle(bool Toggle)
    {
        FPSDisplay = Toggle;
        BetterPrefs.SetBool("GRAPHICS_FPSDisplay", Toggle);

        UpdateSettings();
    }

    public void PostProcessToggle(bool Toggle)
    {
        PostProcessing = Toggle;
        BetterPrefs.SetBool("GRAPHICS_PostProcessing", Toggle);

        UpdateSettings();
    }

    public void ParticalsToggle(bool Toggle)
    {
        Particals = Toggle;
        BetterPrefs.SetBool("GRAPHICS_Particles", Toggle);

        UpdateSettings();
    }
    
    public void FogToggle(bool Toggle)
    {
        Fog = Toggle;
        BetterPrefs.SetBool("GRAPHICS_Fog", Toggle);

        UpdateSettings();
    }

    public void RenderQualityChange()
    {
        RenderQuality = float.Parse(RenderQualityInput.text);
        BetterPrefs.SetFloat("GRAPHICS_RenderQuality", RenderQuality);

        UpdateSettings();
    }

    public void GraphicsPresetChanged(int value)
    {
        TextureQuality = value;
        BetterPrefs.SetInt("GRAPHICS_TextureQuality", value);

        UpdateSettings();
    }

    public void LoadGraphics()
    {
        FPSDisplay = BetterPrefs.GetBool("GRAPHICS_FPSDisplay", false);
        PostProcessing = BetterPrefs.GetBool("GRAPHICS_PostProcessing", false);
        Particals = BetterPrefs.GetBool("GRAPHICS_Particles", false);
        VSync = BetterPrefs.GetBool("GRAPHICS_VSync", false);
        Fog = BetterPrefs.GetBool("GRAPHICS_Fog", false);
        TextureQuality = BetterPrefs.GetInt("GRAPHICS_TextureQuality", 0);
        RenderQuality = BetterPrefs.GetFloat("GRAPHICS_RenderQuality", 1);

        FPSDisplayUIToggle.isOn = FPSDisplay;
        PostProcessingUIToggle.isOn = PostProcessing;
        ParticlesUIToggle.isOn = Particals;
        VSyncUIToggle.isOn = VSync;
        FogUIToggle.isOn = Fog;
        TextureQualityDropdown.value = TextureQuality;

        UpdateSettings();
    }

    public void SetDefaults()
    {
        FPSDisplay = false;
        PostProcessing = true;
        Particals = true;
        VSync = true;
        Fog = true;
        TextureQuality = 0;
        RenderQuality = 1f;

        BetterPrefs.SetBool("GRAPHICS_FPSDisplay", FPSDisplay);
        BetterPrefs.SetBool("GRAPHICS_PostProcessing", PostProcessing);
        BetterPrefs.SetBool("GRAPHICS_Particles", Particals);
        BetterPrefs.SetBool("GRAPHICS_VSync", VSync);
        BetterPrefs.SetBool("GRAPHICS_Fog", Fog);
        BetterPrefs.SetInt("GRAPHICS_TextureQuality", TextureQuality);
        BetterPrefs.SetFloat("GRAPHICS_RenderQuality", RenderQuality);

        UpdateSettings();
    }

    private void UpdateSettings()
    {
        if (PostProcessing)
        {
            pp_normal.SetActive(true);
            GameCamera_AdditionalData.renderPostProcessing = true;
        }
        else
        {
            pp_normal.SetActive(false);
            GameCamera_AdditionalData.renderPostProcessing = false;
        }
        if (Particals)
        {
            ThemeManager.instance.CurrentTheme.ThemeParticles.SetActive(true);
            ParticalsReal2.SetActive(true);
        }
        else
        {
            ThemeManager.instance.CurrentTheme.ThemeParticles.SetActive(false);
            ParticalsReal2.SetActive(false);
        }
        if (VSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        if (Fog)
        {
            RenderSettings.fog = true;
        }
        else
        {
            RenderSettings.fog = false;
        }
        if (FPSDisplay)
        {
            SingletonVariables.instance.FPSDisplayRoot.SetActive(true);
        }
        else
        {
            SingletonVariables.instance.FPSDisplayRoot.SetActive(false);
        }

        QualitySettings.globalTextureMipmapLimit = TextureQuality;

        RenderQualityText.text = RenderQuality + "x";

        URPAsset.renderScale = RenderQuality;
    }
}