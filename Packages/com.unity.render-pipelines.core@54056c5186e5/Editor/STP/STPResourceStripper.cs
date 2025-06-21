using UnityEngine.Rendering;

namespace UnityEditor.Rendering
{
    class STPResourceStripper : IRenderPipelineGraphicsSettingsStripper<STP.RuntimeResources>
    {
        public bool active => true;

        public bool CanRemoveSettings(STP.RuntimeResources resources)
        {
            return true;
        }
    }
}
