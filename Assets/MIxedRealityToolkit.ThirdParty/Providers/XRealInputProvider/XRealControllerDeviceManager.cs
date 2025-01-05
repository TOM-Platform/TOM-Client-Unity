using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
#if BUILD_XREAL
using NRKernal;
#endif

namespace MixedReality.Toolkit.XReal.Input
{
    [MixedRealityDataProvider(
    typeof(IMixedRealityInputSystem),
    SupportedPlatforms.Android |
    SupportedPlatforms.WindowsEditor |
    SupportedPlatforms.MacEditor |
    SupportedPlatforms.LinuxEditor,
    "Nreal Devkit Ctrl Device Manager")]
    public class XRealControllerDeviceManager : BaseInputDeviceManager,
        IMixedRealityCapabilityCheck
    {

        private XRealController controller = null;

        public XRealControllerDeviceManager(
            IMixedRealityInputSystem inputSystem,
            string name,
            uint priority,
            BaseMixedRealityProfile profile) : base(inputSystem, name, priority, profile)
        {

        }

        #region IMixedRealityCapabilityCheck Implementation

        /// <inheritdoc />
        public bool CheckCapability(MixedRealityCapability capability)
        {
            // Puck is a motion controller.
            return (capability == MixedRealityCapability.MotionController);
        }


        #endregion IMixedRealityCapabilityCheck Implementation

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();
        }


        /// <inheritdoc />
        public override void Update()
        {
            base.Update();
#if BUILD_XREAL
            if (controller == null)
            {
                NRInput.LaserVisualActive = false;
                NRInput.ReticleVisualActive = false;

                var inputSystem = Service as IMixedRealityInputSystem;
                var handedness = NRInput.DomainHand == ControllerHandEnum.Left ? Handedness.Left : Handedness.Right;
                var pointers = RequestPointers(SupportedControllerType.ArticulatedHand, handedness);
                var inputSource = inputSystem?.RequestNewGenericInputSource($"Nreal Light Controller", pointers, InputSourceType.Hand);
                controller = new XRealController(Microsoft.MixedReality.Toolkit.TrackingState.NotTracked, handedness, inputSource);
                // controller.SetupConfiguration(typeof(NRealDevkitController));
                for (int i = 0; i < controller.InputSource?.Pointers?.Length; i++)
                {
                    controller.InputSource.Pointers[i].Controller = controller;
                }
                inputSystem.RaiseSourceDetected(controller.InputSource, controller);
            }
            controller.UpdateState();
#endif
        }

    }
}