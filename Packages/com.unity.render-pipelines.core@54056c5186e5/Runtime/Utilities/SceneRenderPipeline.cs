#if UNITY_EDITOR
using UnityEditor;
#endif

using System;

namespace UnityEngine.Rendering
{
    /// <summary>
    /// Setup a specific render pipeline on scene loading.
    /// This need to be used with caution as it will change project configuration.
    /// </summary>
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    public class SceneRenderPipeline : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] bool firstTimeCreated = true;

        /// <summary>
        /// Scriptable Render Pipeline Asset to setup on scene load.
        /// </summary>
        public RenderPipelineAsset renderPipelineAsset;

        void Awake()
        {
            if (firstTimeCreated)
            {
                renderPipelineAsset = GraphicsSettings.defaultRenderPipeline;
                firstTimeCreated = false;
            }
        }

        void OnEnable()
        {
            GraphicsSettings.defaultRenderPipeline = renderPipelineAsset;
        }

#endif
    }
}
