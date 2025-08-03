using UnityEngine.XR.Interaction.Toolkit.Attachment;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Gaze;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactables.Visuals;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// Constants for <see cref="HelpURLAttribute"/> for XR Interaction Toolkit.
    /// </summary>
    static partial class XRHelpURLConstants
    {
        const string k_CurrentDocsVersion = "3.1";
        const string k_BaseApi = "https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@" + k_CurrentDocsVersion + "/api/";
        const string k_HtmlFileSuffix = ".html";

        const string k_BaseNamespace = "UnityEngine.XR.Interaction.Toolkit.";
        const string k_AttachmentNamespace = "Attachment.";
        const string k_BodyUINamespace = "BodyUI.";
        const string k_CastersNamespace = "Casters.";
        const string k_FilteringNamespace = "Filtering.";
        const string k_GazeNamespace = "Gaze.";
        const string k_HapticsNamespace = "Haptics.";
        const string k_InputsNamespace = "Inputs.";
        const string k_InteractorsNamespace = "Interactors.";
        const string k_InteractablesNamespace = "Interactables.";
        const string k_ReadersNamespace = "Readers.";
        const string k_TransformersNamespace = "Transformers.";
        const string k_UINamespace = "UI.";
        const string k_UtilitiesNamespace = "Utilities.";
        const string k_VisualsNamespace = "Visuals.";

        /// <summary>
        /// Current documentation version for XR Interaction Toolkit API and Manual pages.
        /// </summary>
        internal static string currentDocsVersion => k_CurrentDocsVersion;
        /// <summary>
        /// Scripting API URL for <see cref="InteractionAttachController"/>
        /// </summary>
        public const string k_InteractionAttachController = k_BaseApi + k_BaseNamespace + k_AttachmentNamespace + nameof(InteractionAttachController) + k_HtmlFileSuffix;
        /// <summary>
        /// Scripting API URL for <see cref="PokeThresholdDatum"/>.
        /// </summary>
        public const string k_PokeThresholdDatum = k_BaseApi + k_BaseNamespace + k_FilteringNamespace + nameof(PokeThresholdDatum) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="TouchscreenHoverFilter"/>.
        /// </summary>
        public const string k_TouchscreenHoverFilter = k_BaseApi + k_BaseNamespace + k_FilteringNamespace + nameof(TouchscreenHoverFilter) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRPokeFilter"/>.
        /// </summary>
        public const string k_XRPokeFilter = k_BaseApi + k_BaseNamespace + k_FilteringNamespace + nameof(XRPokeFilter) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRTargetFilter"/>.
        /// </summary>
        public const string k_XRTargetFilter = k_BaseApi + k_BaseNamespace + k_FilteringNamespace + nameof(XRTargetFilter) + k_HtmlFileSuffix;
        /// <summary>
        /// Scripting API URL for <see cref="HapticImpulsePlayer"/>.
        /// </summary>
        public const string k_HapticImpulsePlayer = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_HapticsNamespace + nameof(HapticImpulsePlayer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceHapticImpulseProvider"/>.
        /// </summary>
        public const string k_XRInputDeviceHapticImpulseProvider = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_HapticsNamespace + nameof(XRInputDeviceHapticImpulseProvider) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="InputActionManager"/>.
        /// </summary>
        public const string k_InputActionManager = k_BaseApi + k_BaseNamespace + k_InputsNamespace + nameof(InputActionManager) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceBoolValueReader"/>.
        /// </summary>
        public const string k_XRInputDeviceBoolValueReader = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_ReadersNamespace + nameof(XRInputDeviceBoolValueReader) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceButtonReader"/>.
        /// </summary>
        public const string k_XRInputDeviceButtonReader = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_ReadersNamespace + nameof(XRInputDeviceButtonReader) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceFloatValueReader"/>.
        /// </summary>
        public const string k_XRInputDeviceFloatValueReader = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_ReadersNamespace + nameof(XRInputDeviceFloatValueReader) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceInputTrackingStateValueReader"/>.
        /// </summary>
        public const string k_XRInputDeviceInputTrackingStateValueReader = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_ReadersNamespace + nameof(XRInputDeviceInputTrackingStateValueReader) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceQuaternionValueReader"/>.
        /// </summary>
        public const string k_XRInputDeviceQuaternionValueReader = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_ReadersNamespace + nameof(XRInputDeviceQuaternionValueReader) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceVector2ValueReader"/>.
        /// </summary>
        public const string k_XRInputDeviceVector2ValueReader = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_ReadersNamespace + nameof(XRInputDeviceVector2ValueReader) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputDeviceVector3ValueReader"/>.
        /// </summary>
        public const string k_XRInputDeviceVector3ValueReader = k_BaseApi + k_BaseNamespace + k_InputsNamespace + k_ReadersNamespace + nameof(XRInputDeviceVector3ValueReader) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRHandSkeletonPokeDisplacer"/>.
        /// </summary>
        public const string k_XRHandSkeletonPokeDisplacer = k_BaseApi + k_BaseNamespace + k_InputsNamespace + nameof(XRHandSkeletonPokeDisplacer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInputModalityManager"/>.
        /// </summary>
        public const string k_XRInputModalityManager = k_BaseApi + k_BaseNamespace + k_InputsNamespace + nameof(XRInputModalityManager) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRTransformStabilizer"/>.
        /// </summary>
        public const string k_XRTransformStabilizer = k_BaseApi + k_BaseNamespace + k_InputsNamespace + nameof(XRTransformStabilizer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="CurveInteractionCaster"/>
        /// </summary>
        public const string k_CurveInteractionCaster = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + k_CastersNamespace + nameof(CurveInteractionCaster) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="SphereInteractionCaster"/>
        /// </summary>
        public const string k_SphereInteractionCaster = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + k_CastersNamespace + nameof(SphereInteractionCaster) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="NearFarInteractor"/>
        /// </summary>
        public const string k_NearFarInteractor = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + nameof(NearFarInteractor) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="CurveVisualController"/>
        /// </summary>
        public const string k_CurveVisualController = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + k_VisualsNamespace + nameof(CurveVisualController) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRDualGrabFreeTransformer"/>.
        /// </summary>
        public const string k_XRDualGrabFreeTransformer = k_BaseApi + k_BaseNamespace + k_TransformersNamespace + nameof(XRDualGrabFreeTransformer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRGeneralGrabTransformer"/>.
        /// </summary>
        public const string k_XRGeneralGrabTransformer = k_BaseApi + k_BaseNamespace + k_TransformersNamespace + nameof(XRGeneralGrabTransformer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRSingleGrabFreeTransformer"/>.
        /// </summary>
        public const string k_XRSingleGrabFreeTransformer = k_BaseApi + k_BaseNamespace + k_TransformersNamespace + nameof(XRSingleGrabFreeTransformer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRSingleGrabOffsetPreserveTransformer"/>.
        /// </summary>
        public const string k_XRSingleGrabOffsetPreserveTransformer = k_BaseApi + k_BaseNamespace + k_TransformersNamespace + nameof(k_XRSingleGrabOffsetPreserveTransformer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRSocketGrabTransformer"/>.
        /// </summary>
        public const string k_XRSocketGrabTransformer = k_BaseApi + k_BaseNamespace + k_TransformersNamespace + nameof(XRSocketGrabTransformer) + k_HtmlFileSuffix;
        /// <summary>
        /// Scripting API URL for <see cref="HandMenu"/>.
        /// </summary>
        public const string k_HandMenu = k_BaseApi + k_BaseNamespace + k_UINamespace + k_BodyUINamespace + nameof(HandMenu) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="FollowPresetDatum"/>.
        /// </summary>
        public const string k_FollowPresetDatum = k_BaseApi + k_BaseNamespace + k_UINamespace + k_BodyUINamespace + nameof(FollowPresetDatum) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="CanvasOptimizer"/>.
        /// </summary>
        public const string k_CanvasOptimizer = k_BaseApi + k_BaseNamespace + k_UINamespace + nameof(CanvasOptimizer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="CanvasTracker"/>.
        /// </summary>
        public const string k_CanvasTracker = k_BaseApi + k_BaseNamespace + k_UINamespace + nameof(CanvasTracker) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="LazyFollow"/>.
        /// </summary>
        public const string k_LazyFollow = k_BaseApi + k_BaseNamespace + k_UINamespace + nameof(LazyFollow) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="TrackedDeviceGraphicRaycaster"/>.
        /// </summary>
        public const string k_TrackedDeviceGraphicRaycaster = k_BaseApi + k_BaseNamespace + k_UINamespace + nameof(TrackedDeviceGraphicRaycaster) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="TrackedDevicePhysicsRaycaster"/>
        /// </summary>
        public const string k_TrackedDevicePhysicsRaycaster = k_BaseApi + k_BaseNamespace + k_UINamespace + nameof(TrackedDevicePhysicsRaycaster) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRUIInputModule"/>.
        /// </summary>
        public const string k_XRUIInputModule = k_BaseApi + k_BaseNamespace + k_UINamespace + nameof(XRUIInputModule) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="DisposableManagerSingleton"/>.
        /// </summary>
        public const string k_DisposableManagerSingleton = k_BaseApi + k_BaseNamespace + k_UtilitiesNamespace + nameof(DisposableManagerSingleton) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRDebugLineVisualizer"/>.
        /// </summary>
        public const string k_XRDebugLineVisualizer = k_BaseApi + k_BaseNamespace + k_UtilitiesNamespace + nameof(XRDebugLineVisualizer) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRControllerRecorder"/>.
        /// </summary>
        public const string k_XRControllerRecorder = k_BaseApi + k_BaseNamespace + nameof(XRControllerRecorder) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRControllerRecording"/>.
        /// </summary>
        public const string k_XRControllerRecording = k_BaseApi + k_BaseNamespace + nameof(XRControllerRecording) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRDirectInteractor"/>.
        /// </summary>
        public const string k_XRDirectInteractor = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + nameof(XRDirectInteractor) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRGazeAssistance"/>.
        /// </summary>
        public const string k_XRGazeAssistance = k_BaseApi + k_BaseNamespace + k_GazeNamespace + nameof(XRGazeAssistance) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRGazeInteractor"/>.
        /// </summary>
        public const string k_XRGazeInteractor = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + nameof(XRGazeInteractor) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRGrabInteractable"/>.
        /// </summary>
        public const string k_XRGrabInteractable = k_BaseApi + k_BaseNamespace + k_InteractablesNamespace + nameof(XRGrabInteractable) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInteractableSnapVolume"/>.
        /// </summary>
        public const string k_XRInteractableSnapVolume = k_BaseApi + k_BaseNamespace + k_InteractablesNamespace + nameof(XRInteractableSnapVolume) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInteractionGroup"/>.
        /// </summary>
        public const string k_XRInteractionGroup = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + nameof(XRInteractionGroup) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInteractionManager"/>.
        /// </summary>
        public const string k_XRInteractionManager = k_BaseApi + k_BaseNamespace + nameof(XRInteractionManager) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInteractorLineVisual"/>.
        /// </summary>
        public const string k_XRInteractorLineVisual = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + k_VisualsNamespace + nameof(XRInteractorLineVisual) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRInteractorReticleVisual"/>.
        /// </summary>
        public const string k_XRInteractorReticleVisual = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + k_VisualsNamespace + nameof(XRInteractorReticleVisual) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRPokeInteractor"/>.
        /// </summary>
        public const string k_XRPokeInteractor = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + nameof(XRPokeInteractor) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRRayInteractor"/>.
        /// </summary>
        public const string k_XRRayInteractor = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + nameof(XRRayInteractor) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRSimpleInteractable"/>.
        /// </summary>
        public const string k_XRSimpleInteractable = k_BaseApi + k_BaseNamespace + k_InteractablesNamespace + nameof(XRSimpleInteractable) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRSocketInteractor"/>.
        /// </summary>
        public const string k_XRSocketInteractor = k_BaseApi + k_BaseNamespace + k_InteractorsNamespace + nameof(XRSocketInteractor) + k_HtmlFileSuffix;

        /// <summary>
        /// Scripting API URL for <see cref="XRTintInteractableVisual"/>.
        /// </summary>
        public const string k_XRTintInteractableVisual = k_BaseApi + k_BaseNamespace + k_InteractablesNamespace + k_VisualsNamespace + nameof(XRTintInteractableVisual) + k_HtmlFileSuffix;
    }
}
