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
    public int TextureQuality;
    public float RenderQuality;
    public GameObject pp_normal;
    public GameObject pp_performance;
    public TMP_Text RenderQualityText;
    public TMP_InputField RenderQualityInput;
    public QualityWrapper qualityWrapper;

    [Header("Performance Mode")]
    public GameObject TreesReal;
    public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData GameCamera_AdditionalData;
    public GameObject Cookie_Performance;
    public GameObject Cookie_Normal;
    public GameObject Research_Factory_Normal;
    public GameObject ParticalsReal;

    // Start is called before the first frame update
    void Start()
    {
        RenderQuality = 1;
    }

    public void PostProcessToggle(bool Toggle)
    {
        PostProcessing = Toggle;
    }

    public void LightingToggle(bool Toggle)
    {
        Lighting = Toggle;
    }

    public void ParticalsToggle(bool Toggle)
    {
        Particals = Toggle;
    }

    public void TreesToggle(bool Toggle)
    {
        Trees = Toggle;
    }

    public void VSyncToggle(bool Toggle)
    {
        VSync = Toggle;
    }
    
    public void FogToggle(bool Toggle)
    {
        Fog = Toggle;
    }

    public void RenderQualityChange()
    {
        RenderQuality = float.Parse(RenderQualityInput.text);
    }

    public void GraphicsPresetChanged(int value)
    {
        Debug.Log(value);
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
        
    }
}
