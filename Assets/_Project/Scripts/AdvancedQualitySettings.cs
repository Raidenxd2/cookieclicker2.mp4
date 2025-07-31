using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class AdvancedQualitySettings : MonoBehaviour
{
    public bool PostProcessing;
    public bool Particals;
    public bool VSync;
    public bool Fog;
    public int TextureQuality;
    public float RenderQuality;
    public GameObject pp_normal;
    public TMP_Text RenderQualityText;
    public TMP_InputField RenderQualityInput;
    public QualityWrapper qualityWrapper;

    [Header("Performance Mode")]
    public UniversalAdditionalCameraData GameCamera_AdditionalData;
    public GameObject ParticalsReal2;

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
        BetterPrefs.SetInt("GRAPHICS_TextureQuality", value);
        if (value == 0)
        {
            TextureQuality = 0;
        }
        if (value == 1)
        {
            TextureQuality = 1;
        }
        if (value == 2)
        {
            TextureQuality = 2;
        }
        if (value == 3)
        {
            TextureQuality = 3;
        }

        UpdateSettings();
    }

    public void LoadGraphics()
    {
        PostProcessing = BetterPrefs.GetBool("GRAPHICS_PostProcessing", false);
        Particals = BetterPrefs.GetBool("GRAPHICS_Particles", false);
        VSync = BetterPrefs.GetBool("GRAPHICS_VSync", false);
        Fog = BetterPrefs.GetBool("GRAPHICS_Fog", false);
        TextureQuality = BetterPrefs.GetInt("GRAPHICS_TextureQuality", 0);
        RenderQuality = BetterPrefs.GetFloat("GRAPHICS_RenderQuality", 1);

        UpdateSettings();
    }

    public void SetDefaults()
    {
#if UNITY_ANDROID
        Particals = true;
        VSync = false;
        Fog = true;
        TextureQuality = 0;
        RenderQuality = 0.75f;
#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.IsMobileVR)
        {
            PostProcessing = true;
            RenderQuality = 1f;
        }
#endif
#endif

#if UNITY_WEBGL
        Particals = true;
        VSync = false;
        Fog = true;
        TextureQuality = 0;
        RenderQuality = 1f;
#endif

#if UNITY_STANDALONE
        PostProcessing = true;
        Particals = true;
        VSync = true;
        Fog = true;
        TextureQuality = 0;
        RenderQuality = 1f;
#endif

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

#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.IsMobileVR)
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            qualityWrapper.SetMSAA(MsaaQuality._4x);
        }
#endif

        QualitySettings.globalTextureMipmapLimit = TextureQuality;

        RenderQualityText.text = RenderQuality + "x";

        qualityWrapper.SetRenderScale(RenderQuality);
    }
}