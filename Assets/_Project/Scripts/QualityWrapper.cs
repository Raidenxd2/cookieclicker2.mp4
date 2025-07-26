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