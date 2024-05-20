using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using LoggerSystem;

public class AdvancedQualitySettings : MonoBehaviour
{
    public bool PostProcessing;
    public bool Particals;
    public bool Lighting;
    public bool Trees;
    public bool VSync;
    public bool Fog;
    public bool Textures;
    public bool AO;
    public bool HDR;
    public int TextureQuality;
    public float RenderQuality;
    public GameObject pp_normal;
    public GameObject pp_performance;
    public TMP_Text RenderQualityText;
    public TMP_InputField RenderQualityInput;
    public QualityWrapper qualityWrapper;
    public SwitchRendererFeature switchRendererFeature;
    [SerializeField] private UniversalRenderPipelineAsset asset;

    [Header("Performance Mode")]
    public GameObject TreesReal;
    public UniversalAdditionalCameraData GameCamera_AdditionalData;
    public GameObject Cookie_Performance;
    public GameObject Cookie_Normal;
    public GameObject Research_Factory_Normal;
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

    public void LightingToggle(bool Toggle)
    {
        Lighting = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Lighting", boolToInt(Toggle));

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

    public void VSyncToggle(bool Toggle)
    {
        VSync = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_VSync", boolToInt(Toggle));

        UpdateSettings();
    }
    
    public void FogToggle(bool Toggle)
    {
        Fog = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Fog", boolToInt(Toggle));

        UpdateSettings();
    }

    public void TexturesToggle(bool Toggle)
    {
        Textures = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Textures", boolToInt(Toggle));

        UpdateSettings();
    }

    public void RenderQualityChange()
    {
        RenderQuality = float.Parse(RenderQualityInput.text);
        PlayerPrefs.SetFloat("GRAPHICS_RenderQuality", RenderQuality);

        UpdateSettings();
    }

    public void AOToggle(bool Toggle)
    {
        AO = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_AO", boolToInt(Toggle));

        UpdateSettings();
    }

    public void HDRToggle(bool Toggle)
    {
        HDR = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_HDR", boolToInt(Toggle));

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
        LogSystem.Log("Saved Graphics Options");
    }

    public void LoadGraphics()
    {
        var ppTemp = PlayerPrefs.GetInt("GRAPHICS_PostProcessing");
        var LightingTemp = PlayerPrefs.GetInt("GRAPHICS_Lighting");
        var ParticlesTemp = PlayerPrefs.GetInt("GRAPHICS_Particles");
        var TreesTemp = PlayerPrefs.GetInt("GRAPHICS_Trees");
        var VSyncTemp = PlayerPrefs.GetInt("GRAPHICS_VSync");
        var FogTemp = PlayerPrefs.GetInt("GRAPHICS_Fog");
        var TextureQualityTemp = PlayerPrefs.GetInt("GRAPHICS_TextureQuality");
        var TexturesTemp = PlayerPrefs.GetInt("GRAPHICS_Textures");
        var AOTemp = PlayerPrefs.GetInt("GRAPHICS_AO");
        var HDRTemp = PlayerPrefs.GetInt("GRAPHICS_HDR");
        PostProcessing = intToBool(ppTemp);
        Lighting = intToBool(LightingTemp);
        Particals = intToBool(ParticlesTemp);
        Trees = intToBool(TreesTemp);
        VSync = intToBool(VSyncTemp);
        Fog = intToBool(FogTemp);
        TextureQuality = TextureQualityTemp;
        Textures = intToBool(TexturesTemp);
        AO = intToBool(AOTemp);
        HDR = intToBool(HDRTemp);
        RenderQuality = PlayerPrefs.GetFloat("GRAPHICS_RenderQuality", 1);

        UpdateSettings();

        LogSystem.Log("Loaded Graphics Options");
    }

    public void SetDefaults()
    {
        #if UNITY_ANDROID
        if (IsGPGPC.instance.isPC)
        {
            PostProcessing = true;
            Lighting = true;
            Particals = true;
            Trees = true;
            VSync = true;
            Fog = true;
            TextureQuality = 0;
            Textures = true;
            AO = false;
            HDR = true;
            RenderQuality = 1f;
        }
        
        if (Application.isMobilePlatform && !IsGPGPC.instance.isPC)
        {
            PostProcessing = false;
            Lighting = true;
            Particals = true;
            Trees = true;
            VSync = false;
            Fog = true;
            TextureQuality = 0;
            Textures = true;
            AO = false;
            HDR = false;
            RenderQuality = 0.75f;
        }
        #endif

        #if UNITY_STANDALONE
        PostProcessing = true;
        Lighting = true;
        Particals = true;
        Trees = true;
        VSync = true;
        Fog = true;
        TextureQuality = 0;
        Textures = true;
        AO = false;
        HDR = true;
        RenderQuality = 1f;
        #endif

        PlayerPrefs.SetInt("GRAPHICS_PostProcessing", boolToInt(PostProcessing));
        PlayerPrefs.SetInt("GRAPHICS_Lighting", boolToInt(Lighting));
        PlayerPrefs.SetInt("GRAPHICS_Particles", boolToInt(Particals));
        PlayerPrefs.SetInt("GRAPHICS_Trees", boolToInt(Trees));
        PlayerPrefs.SetInt("GRAPHICS_VSync", boolToInt(VSync));
        PlayerPrefs.SetInt("GRAPHICS_Fog", boolToInt(Fog));
        PlayerPrefs.SetInt("GRAPHICS_TextureQuality", TextureQuality);
        PlayerPrefs.SetInt("GRAPHICS_Textures", boolToInt(Textures));
        PlayerPrefs.SetInt("GRAPHICS_AO", boolToInt(AO));
        PlayerPrefs.SetInt("GRAPHICS_HDR", boolToInt(HDR));
        PlayerPrefs.SetFloat("GRAPHICS_RenderQuality", RenderQuality);

        UpdateSettings();
    }

    private void UpdateSettings()
    {
        if (PostProcessing)
        {
            pp_normal.SetActive(true);
            #if UNITY_ANDROID
            GameCamera_AdditionalData.renderPostProcessing = true;
            #endif
        }
        else
        {
            pp_normal.SetActive(false);
            #if UNITY_ANDROID
            GameCamera_AdditionalData.renderPostProcessing = false;
            #endif
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

        QualitySettings.globalTextureMipmapLimit = TextureQuality;

        RenderQualityText.text = RenderQuality + "x";

        qualityWrapper.SetRenderScale(RenderQuality);

        switchRendererFeature.set(AO);

        if (IsGPGPC.instance.isPC)
        {
            Application.targetFrameRate = 60;
        }
    }
}