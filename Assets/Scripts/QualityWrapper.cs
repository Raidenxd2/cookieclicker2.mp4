using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
 
public class QualityWrapper : MonoBehaviour
{
    UniversalRenderPipelineAsset m_cachedRenderPipeline;
 
    UniversalRenderPipelineAsset CachedRenderPipeline
    {
        get
        {
            if (m_cachedRenderPipeline == null)
                m_cachedRenderPipeline = (UniversalRenderPipelineAsset) QualitySettings.renderPipeline;
 
            return m_cachedRenderPipeline;
        }
    }
 
    public float CurrentRenderScale
    {
        get
        {
            VerifyCachedRenderPipeline ();
            if (CachedRenderPipeline == null) return -1;
            return CachedRenderPipeline.renderScale;
        }
    }
 
    public int CurrentMSAASampleCount
    {
        get
        {
            VerifyCachedRenderPipeline ();
            if (CachedRenderPipeline == null) return -1;
            return CachedRenderPipeline.msaaSampleCount;
        }
    }
 
    public int CurrentVSYNCCount
    {
        get
        {
            VerifyCachedRenderPipeline ();
            if (CachedRenderPipeline == null) return -1;
            return QualitySettings.vSyncCount;
        }
    }
 
    private float m_tmpRenderScale;
 
    public void SetMSAA (MsaaQuality msaaQuality)
    {
        VerifyCachedRenderPipeline ();
        if (CachedRenderPipeline == null)
        {
            Debug.LogError ("[QualityWrapper](SetMSAA): Current Pipeline is null");
            return;
        }
 
        CachedRenderPipeline.msaaSampleCount = (int) msaaQuality;
    }
 
    // public void SetVSync (VSyncCount vSyncCount)
    // {
    //     VerifyCachedRenderPipeline ();
    //     if (CachedRenderPipeline == null)
    //     {
    //         Debug.LogError ("[QualityWrapper](SetVSync): Current Pipeline is null");
    //         return;
    //     }
 
    //     QualitySettings.vSyncCount = (int) vSyncCount;
    // }
 
    public void IncreaseRenderScale (float _amount)
    {
        m_tmpRenderScale = Mathf.Clamp (CurrentRenderScale + _amount, 0.2f, 1.0f);
        SetRenderScale (m_tmpRenderScale);
    }
 
    public void DecreaseRenderScale (float _amount)
    {
        m_tmpRenderScale = Mathf.Clamp (CurrentRenderScale - _amount, 0.2f, 1.0f);
        SetRenderScale (m_tmpRenderScale);
    }
 
    public void SetRenderScale (float value)
    {
        VerifyCachedRenderPipeline ();
        if (CachedRenderPipeline == null)
        {
            Debug.LogError ("[QualityWrapper](SetRenderScale): Current Pipeline is null");
            return;
        }
 
        CachedRenderPipeline.renderScale = Mathf.Clamp (value, 0.2f, 1);
    }
 
    private void VerifyCachedRenderPipeline ()
    {
        if ((UniversalRenderPipelineAsset) QualitySettings.renderPipeline == null)
            return;
 
        if (CachedRenderPipeline != (UniversalRenderPipelineAsset) QualitySettings.renderPipeline)
        {
            m_cachedRenderPipeline = (UniversalRenderPipelineAsset) QualitySettings.renderPipeline;
        }
    }
 
}