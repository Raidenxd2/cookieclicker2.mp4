#if UNITY_2020_2_OR_NEWER

using UnityEditor.Localization.Bridge;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Localization.Settings;
//using UnityEditor.Localization.UI.PropertyVariants;

namespace UnityEditor.Localization.UI
{
    [InitializeOnLoad]
    class DrivenPropertyDrawer
    {
        static DrivenPropertyDrawer()
        {
            EditorGUIUtilityBridge.beginProperty += BeginProperty;
        }

        static void BeginProperty(Rect rect, SerializedProperty property)
        {
            if (!LocalizationSettings.HasSettings ||
                EditorApplication.isPlayingOrWillChangePlaymode ||
                //GameObjectLocalizerEditor.CurrentTarget != null ||
                !LocalizationSettings.SelectedLocaleAsync.IsValid() ||
                LocalizationSettings.SelectedLocaleAsync.Result == null)
                return;

            if (DrivenPropertyManagerInternalBridge.IsDriving(LocalizationPropertyDriver.instance, property.serializedObject.targetObject, property.propertyPath))
            {
                // Properties driven by a UnityEvent are disabled as changes to them would be ignored.
                GUI.enabled = false;
                GUI.backgroundColor = PrefColorBridge.DrivenProperty;
            }
        }
    }
}
#endif
