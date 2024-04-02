using LoggerSystem;
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
            LogSystem.Log("[QualityWrapper](SetMSAA): Current Pipeline is null", LogTypes.Error);
            return;
        }
 
        CachedRenderPipeline.msaaSampleCount = (int) msaaQuality;
    }
 
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
            LogSystem.Log("[QualityWrapper](SetRenderScale): Current Pipeline is null", LogTypes.Error);
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