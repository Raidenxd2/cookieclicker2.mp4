using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public int TextureQuality;
    public float RenderQuality;
    public GameObject pp_normal;
    public GameObject pp_performance;
    public TMP_Text RenderQualityText;
    public TMP_InputField RenderQualityInput;
    public QualityWrapper qualityWrapper;
    public SwitchRendererFeature switchRendererFeature;

    [Header("Performance Mode")]
    public GameObject TreesReal;
    public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData GameCamera_AdditionalData;
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

    // Start is called before the first frame update
    void Start()
    {
        RenderQuality = 1;
    }

    public void PostProcessToggle(bool Toggle)
    {
        PostProcessing = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_PostProcessing", boolToInt(Toggle));
    }

    public void LightingToggle(bool Toggle)
    {
        Lighting = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Lighting", boolToInt(Toggle));
    }

    public void ParticalsToggle(bool Toggle)
    {
        Particals = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Particles", boolToInt(Toggle));
    }

    public void TreesToggle(bool Toggle)
    {
        Trees = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Trees", boolToInt(Toggle));
    }

    public void VSyncToggle(bool Toggle)
    {
        VSync = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_VSync", boolToInt(Toggle));
    }
    
    public void FogToggle(bool Toggle)
    {
        Fog = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Fog", boolToInt(Toggle));
    }

    public void TexturesToggle(bool Toggle)
    {
        Textures = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_Textures", boolToInt(Toggle));
    }

    public void RenderQualityChange()
    {
        RenderQuality = float.Parse(RenderQualityInput.text);
    }

    public void AOToggle(bool Toggle)
    {
        AO = Toggle;
        PlayerPrefs.SetInt("GRAPHICS_AO", boolToInt(Toggle));
    }

    public void GraphicsPresetChanged(int value)
    {
        Debug.Log(value);
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
    }

    public void SaveGraphics()
    {
        PlayerPrefs.Save();
        Debug.Log("Saved Graphics Options");
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
        PostProcessing = intToBool(ppTemp);
        Lighting = intToBool(LightingTemp);
        Particals = intToBool(ParticlesTemp);
        Trees = intToBool(TreesTemp);
        VSync = intToBool(VSyncTemp);
        Fog = intToBool(FogTemp);
        TextureQuality = TextureQualityTemp;
        Textures = intToBool(TexturesTemp);
        AO = intToBool(AOTemp);
        Debug.Log("Loaded Graphics Options");
    }

    // Update is called once per frame
    void Update()
    {
        if (PostProcessing)
        {
            pp_normal.SetActive(true);
            pp_performance.SetActive(false);
        }
        else
        {
            pp_normal.SetActive(false);
            pp_performance.SetActive(true);
        }
        if (Lighting)
        {
            GameCamera_AdditionalData.SetRenderer(1);
        }
        else
        {
            GameCamera_AdditionalData.SetRenderer(0);
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

        QualitySettings.masterTextureLimit = TextureQuality;

        RenderQualityText.text = RenderQuality + "x";

        qualityWrapper.SetRenderScale(RenderQuality);

        switchRendererFeature.set(AO);
        
        
    }
}
