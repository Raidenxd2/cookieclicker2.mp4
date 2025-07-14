using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using LoggerSystem;

public class AdvancedQualitySettings : MonoBehaviour
{
    public bool PostProcessing;
    public bool Particals;
    public bool Trees;
    public bool VSync;
    public bool Fog;
    public int TextureQuality;
    public float RenderQuality;
    public GameObject pp_normal;
    public TMP_Text RenderQualityText;
    public TMP_InputField RenderQualityInput;
    public QualityWrapper qualityWrapper;

    [Header("Performance Mode")]
    public GameObject TreesReal;
    public UniversalAdditionalCameraData GameCamera_AdditionalData;
    public GameObject ParticalsReal;

    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else   
            return false;
    }

    public void PostProcessToggle(bool Toggle)
    {
        PostProcessing = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_PostProcessing", boolToInt(Toggle));

        UpdateSettings();
    }

    public void ParticalsToggle(bool Toggle)
    {
        Particals = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Particles", boolToInt(Toggle));

        UpdateSettings();
    }

    public void TreesToggle(bool Toggle)
    {
        Trees = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Trees", boolToInt(Toggle));

        UpdateSettings();
    }
    
    public void FogToggle(bool Toggle)
    {
        Fog = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Fog", boolToInt(Toggle));

        UpdateSettings();
    }

    public void RenderQualityChange()
    {
        RenderQuality = float.Parse(RenderQualityInput.text);
        PlayerPrefs.SetFloat("GRAPHICS_RenderQuality", RenderQuality);

        UpdateSettings();
    }

    public void GraphicsPresetChanged(int value)
    {
        LogSystem.Log(value.ToString());
        PlayerPrefs.SetInt("GRAPHICS_TextureQuality", value);
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

    public void SaveGraphics()
    {
        PlayerPrefs.Save();
    }

    public void LoadGraphics()
    {
        var ppTemp = PlayerPrefs.GetInt("GRAPHICS_PostProcessing");
        var ParticlesTemp = PlayerPrefs.GetInt("GRAPHICS_Particles");
        var TreesTemp = PlayerPrefs.GetInt("GRAPHICS_Trees");
        var VSyncTemp = PlayerPrefs.GetInt("GRAPHICS_VSync");
        var FogTemp = PlayerPrefs.GetInt("GRAPHICS_Fog");
        var TextureQualityTemp = PlayerPrefs.GetInt("GRAPHICS_TextureQuality");
        PostProcessing = intToBool(ppTemp);
        Particals = intToBool(ParticlesTemp);
        Trees = intToBool(TreesTemp);
        VSync = intToBool(VSyncTemp);
        Fog = intToBool(FogTemp);
        TextureQuality = TextureQualityTemp;
        RenderQuality = PlayerPrefs.GetFloat("GRAPHICS_RenderQuality", 1);

        UpdateSettings();
    }

    public void SetDefaults()
    {
#if UNITY_ANDROID
        Particals = true;
        Trees = true;
        VSync = false;
        Fog = true;
        TextureQuality = 0;
        RenderQuality = 0.75f;
        if (VRManager.instance.IsMobileVR)
        {
            PostProcessing = true;
            RenderQuality = 1f;
        }
        
#endif

#if UNITY_WEBGL
        Particals = true;
        Trees = true;
        VSync = false;
        Fog = true;
        TextureQuality = 0;
        RenderQuality = 1f;
#endif

#if UNITY_STANDALONE
        PostProcessing = true;
        Particals = true;
        Trees = true;
        VSync = true;
        Fog = true;
        TextureQuality = 0;
        RenderQuality = 1f;
#endif

        PlayerPrefs.SetInt("GRAPHICS_PostProcessing", boolToInt(PostProcessing));
        PlayerPrefs.SetInt("GRAPHICS_Particles", boolToInt(Particals));
        PlayerPrefs.SetInt("GRAPHICS_Trees", boolToInt(Trees));
        PlayerPrefs.SetInt("GRAPHICS_VSync", boolToInt(VSync));
        PlayerPrefs.SetInt("GRAPHICS_Fog", boolToInt(Fog));
        PlayerPrefs.SetInt("GRAPHICS_TextureQuality", TextureQuality);
        PlayerPrefs.SetFloat("GRAPHICS_RenderQuality", RenderQuality);

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
            ParticalsReal.SetActive(true);
        }
        else
        {
            ParticalsReal.SetActive(false);
        }
        if (Trees)
        {
            TreesReal.SetActive(true);
        }
        else
        {
            TreesReal.SetActive(false);
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

#if UNITY_ANDROID
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