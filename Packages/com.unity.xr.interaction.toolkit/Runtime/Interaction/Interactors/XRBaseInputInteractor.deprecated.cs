using System;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace UnityEngine.XR.Interaction.Toolkit.Interactors
{
    public abstract partial class XRBaseInputInteractor
    {
        /// <summary>
        /// Controls whether input can be obtained through the deprecated legacy method for backwards compatibility.
        /// This is only used for backwards compatibility and will be eventually removed in a future version.
        /// </summary>
        /// <seealso cref="inputCompatibilityMode"/>
        [Obsolete("InputCompatibilityMode introduced in version 3.0.0 is marked for removal. This is only used for backwards compatibility and will be eventually removed in a future version.")]
        public enum InputCompatibilityMode
        {
            /// <summary>
            /// Automatically determine whether to use the deprecated legacy input based on whether the interactor has an XR Controller component.
            /// If the <see cref="xrController"/> is set, then the deprecated legacy input will be used.
            /// </summary>
            Automatic,

            /// <summary>
            /// Force the interactor to read inputs from the deprecated legacy XR Controller component.
            /// This is how the interactor used to read inputs prior to version 3.0.0.
            /// </summary>
            ForceDeprecatedInput,

            /// <summary>
            /// Force the interactor to read inputs from the input reader properties on this interactor.
            /// This is how the interactor is recommended to read inputs starting in version 3.0.0.
            /// </summary>
            ForceInputReaders,
        }

        public partial class LogicalInputState
        {
            /// <summary>
            /// (Deprecated) Read whether the button stopped performing this frame, which typically means whether the button stopped being pressed during this frame.
            /// This is typically only true for one single frame.
            /// </summary>
            [Obsolete("wasUnperformedThisFrame has been deprecated in version 3.0.0-pre.2. It has been renamed to wasCompletedThisFrame. (UnityUpgradable) -> wasCompletedThisFrame")]
            public bool wasUnperformedThisFrame => wasCompletedThisFrame;
        }

        [SerializeField]
        bool m_HideControllerOnSelect;

        /// <summary>
        /// Controls whether this Interactor should hide the controller model on selection.
        /// </summary>
        /// <seealso cref="XRBaseController.hideControllerModel"/>
        [Obsolete("hideControllerOnSelect has been deprecated in version 3.0.0.")]
        public bool hideControllerOnSelect
        {
            get => m_HideControllerOnSelect;
            set
            {
                m_HideControllerOnSelect = value;
                if (!m_HideControllerOnSelect && m_Controller != null)
                    m_Controller.hideControllerModel = false;
            }
        }

        [SerializeField]
        [Obsolete("m_InputCompatibilityMode introduced in version 3.0.0 is marked for removal. This is only used for backwards compatibility and will be eventually removed in a future version.")]
        InputCompatibilityMode m_InputCompatibilityMode = InputCompatibilityMode.Automatic;


        /// <summary>
        /// Controls whether input is obtained through the deprecated legacy method where the XR Controller component is used.
        /// This is only used for backwards compatibility and will be eventually removed in a future version.
        /// </summary>
        /// <seealso cref="InputCompatibilityMode"/>
        /// <seealso cref="forceDeprecatedInput"/>
        [Obsolete("inputCompatibilityMode introduced in version 3.0.0 is marked for removal. This is only used for backwards compatibility and will be eventually removed in a future version.")]
        public InputCompatibilityMode inputCompatibilityMode
        {
            get => m_InputCompatibilityMode;
            set => m_InputCompatibilityMode = value;
        }

        /// <summary>
        /// Controls whether this interactor is being forced to use the deprecated input path where the input values are obtained through the <see cref="xrController"/>.
        /// This is only used for backwards compatibility and will be eventually removed in a future version.
        /// </summary>
        [Obsolete("forceDeprecatedInput introduced in version 3.0.0 is marked for removal. This is only used for backwards compatibility and will be eventually removed in a future version.")]
        public bool forceDeprecatedInput
        {
            get => (m_HasXRController && m_InputCompatibilityMode == InputCompatibilityMode.Automatic) || m_InputCompatibilityMode == InputCompatibilityMode.ForceDeprecatedInput;
            set => m_InputCompatibilityMode = value ? InputCompatibilityMode.ForceDeprecatedInput : InputCompatibilityMode.ForceInputReaders;
        }

        [Obsolete("m_Controller has been deprecated in version 3.0.0.")]
        XRBaseController m_Controller;

        /// <summary>
        /// (Deprecated) The controller instance that is queried for input.
        /// </summary>
        [Obsolete("xrController has been deprecated in version 3.0.0.")]
        public XRBaseController xrController
        {
            get => m_Controller;
            set
            {
                if (m_Controller != value)
                {
                    m_Controller = value;
                    OnXRControllerChanged();
                }
            }
        }

        bool m_HasXRController;

        /// <summary>
        /// (Deprecated) (Read Only) Whether or not Unity considers the UI Press controller input pressed.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if active. Otherwise, returns <see langword="false"/>.</returns>
        [Obsolete("isUISelectActive has been deprecated in version 3.0.0. Use a serialized XRInputButtonReader to read button input instead. Some derived interactors have a uiPressInput property that can be used instead.")]
        protected virtual bool isUISelectActive => m_Controller != null && m_Controller.uiPressInteractionState.active;

        /// <summary>
        /// (Deprecated) (Read Only) The current scroll value Unity would apply to the UI.
        /// </summary>
        /// <returns>Returns a Vector2 with scroll strength for each axis. </returns>
        [Obsolete("uiScrollValue has been deprecated in version 3.0.0. Use a serialized XRInputValueReader<Vector2> to read scroll input instead. Some derived interactors have a uiScrollInput property that can be used instead.")]
        protected Vector2 uiScrollValue => m_Controller != null ? m_Controller.uiScrollValue : Vector2.zero;

        /// <summary>
        /// (Deprecated) Override this method to handle internal changes when the <see cref="xrController"/> property value
        /// changes.
        /// </summary>
        [Obsolete("OnXRControllerChanged has been deprecated in version 3.0.0.")]
        private protected virtual void OnXRControllerChanged()
        {
            m_HasXRController = m_Controller != null;
        }

        void WarnMixedInputConfiguration()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            if (forceDeprecatedInput)
#pragma warning restore CS0618
            {
                const string warning = "The interactor has input properties configured to be used but the interactor is set to read input through the deprecated XR Controller component instead." +
                    " If you want to force the input readers to be used even when an XR Controller component is present, set Input Compatibility Mode to Force Input Readers.";
                foreach (var reader in buttonReaders)
                {
                    if ((reader.inputSourceMode == XRInputButtonReader.InputSourceMode.InputActionReference && (reader.inputActionReferencePerformed != null || reader.inputActionReferenceValue != null)) ||
                        (reader.inputSourceMode != XRInputButtonReader.InputSourceMode.InputActionReference && reader.inputSourceMode != XRInputButtonReader.InputSourceMode.Unused))
                    {
                        Debug.LogWarning(warning, this);
                        return;
                    }
                }

                foreach (var reader in valueReaders)
                {
                    if ((reader.inputSourceMode == XRInputValueReader.InputSourceMode.InputActionReference && reader.inputActionReference != null) ||
                        (reader.inputSourceMode != XRInputValueReader.InputSourceMode.InputActionReference && reader.inputSourceMode != XRInputValueReader.InputSourceMode.Unused))
                    {
                        Debug.LogWarning(warning, this);
                        return;
                    }
                }
            }
        }

        #region API Updater Configuration Validation false positive failure suppression

        [Obsolete("CreateEffectsAudioSource has been deprecated in version 3.0.0.")]
        void CreateEffectsAudioSource()
        {
        }

        [Obsolete("HandleSelecting has been deprecated in version 3.0.0.")]
        void HandleSelecting()
        {
        }

        [Obsolete("HandleDeselecting has been deprecated in version 3.0.0.")]
        void HandleDeselecting()
        {
        }

        // ReSharper disable once RedundantOverriddenMember -- Method needed for API Updater Configuration Validation false positive
        /// <inheritdoc />
        protected override void OnHoverEntering(HoverEnterEventArgs args)
        {
            base.OnHoverEntering(args);
        }

        // ReSharper disable once RedundantOverriddenMember -- Method needed for API Updater Configuration Validation false positive
        /// <inheritdoc />
        protected override void OnHoverExiting(HoverExitEventArgs args)
        {
            base.OnHoverExiting(args);
        }

        // ReSharper disable UnusedMember.Local -- Method needed for API Updater Configuration Validation false positive
        static ActivateEventArgs CreateActivateEventArgs() => new ActivateEventArgs();
        static DeactivateEventArgs CreateDeactivateEventArgs() => new DeactivateEventArgs();
        // ReSharper restore UnusedMember.Local

        #endregion
    }
}
