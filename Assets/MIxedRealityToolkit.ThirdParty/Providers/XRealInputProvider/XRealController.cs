using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
#if BUILD_XREAL
using NRKernal;
#endif
using Unity.Profiling;
using UnityEngine;
using TrackingState = Microsoft.MixedReality.Toolkit.TrackingState;

namespace MixedReality.Toolkit.XReal.Input
{
	[MixedRealityController(SupportedControllerType.ArticulatedHand, new[] { Handedness.Left, Handedness.Right })]
	public class XRealController : BaseController, IMixedRealityHapticFeedback
	{
		/// <summary>
		/// Constructor for a Nreal Articulated Hand
		/// </summary>
		/// <param name="trackingState">Tracking state for the controller</param>
		/// <param name="controllerHandedness">Handedness of this controller (Left or Right)</param>
		/// <param name="inputSource">The origin of user input for this controller</param>
		/// <param name="interactions">The controller interaction map between physical inputs and the logical representation in MRTK</param>
		public XRealController(
			Microsoft.MixedReality.Toolkit.TrackingState trackingState,
			Handedness controllerHandedness,
			IMixedRealityInputSource inputSource = null,
			MixedRealityInteractionMapping[] interactions = null)
			: base(trackingState, controllerHandedness, inputSource, interactions, new ArticulatedHandDefinition(inputSource, controllerHandedness))
		{ }

		private static readonly ProfilerMarker UpdateStatePerfMarker = new ProfilerMarker("[MRTK] NrealMotionDevkitCtrl.UpdateState");

        /// <summary>
        /// Tracks between touching / not touching to trigger. 
        /// </summary>
        private bool IsTouching = false;


		/// <summary>
		/// Updates the joint poses and interactions for the articulated hand.
		/// </summary>
		public void UpdateState()
		{
			
			using (UpdateStatePerfMarker.Auto())
			{
				UpdateInteractions();
			}
		}

		/// <summary>
		/// Updates the visibility of the hand ray and raises input system events based on joint pose data.
		/// </summary>
		protected void UpdateInteractions()
		{
#if BUILD_XREAL
			// MixedRealityPose pointerPose = jointPoses[TrackedHandJoint.Palm];
			// MixedRealityPose gripPose = jointPoses[TrackedHandJoint.Palm];
			// MixedRealityPose indexPose = jointPoses[TrackedHandJoint.IndexTip];
			// // Only update the hand ray if the hand is in pointing pose
			// if (IsInPointingPose)
			// {
			// 	HandRay.Update(pointerPose.Position, GetPalmNormal(), CameraCache.Main.transform, ControllerHandedness);
			// 	Ray ray = HandRay.Ray;

			// 	pointerPose.Position = jointPoses[TrackedHandJoint.IndexKnuckle].Position;//ray.origin;
			// 	pointerPose.Rotation = Quaternion.LookRotation(ray.direction);
			// }

			var controllerAnchor = NRInput.DomainHand == ControllerHandEnum.Left ? ControllerAnchorEnum.LeftModelAnchor : ControllerAnchorEnum.RightModelAnchor;
			var pointerAnchor = NRInput.RaycastMode == RaycastModeEnum.Gaze ? ControllerAnchorEnum.GazePoseTrackerAnchor : controllerAnchor;
			var controller = NRInput.AnchorsHelper.GetAnchor(pointerAnchor);

			var lastState = TrackingState;
			TrackingState = NRInput.CheckControllerAvailable(NRInput.DomainHand) ? TrackingState.Tracked : TrackingState.NotTracked;
			if (lastState != TrackingState)
			{
				CoreServices.InputSystem?.RaiseSourceTrackingStateChanged(InputSource, this, TrackingState);
			}
			if (TrackingState == TrackingState.Tracked)
			{
				CoreServices.InputSystem?.RaiseSourcePoseChanged(InputSource, this, new MixedRealityPose(controller.position, controller.rotation));

				// NOTE: NRInput.GetTouch() --> Returns a value between -1 to 1, (0, 0) being the center point. 
				// CK NOTE: It is easier to translate normal spatial pointer events to the Interfaces. 
				
				var IsNowTouching = NRInput.IsTouching();
                if (!IsTouching && IsNowTouching) {
					IsTouching = true;
                    CoreServices.InputSystem?.RaiseOnTouchStarted(InputSource, this, ControllerHandedness, NRInput.GetTouch());
                }
                else if (IsTouching && !IsNowTouching) {
					IsTouching = false;
                    CoreServices.InputSystem?.RaiseOnTouchCompleted(InputSource, this, ControllerHandedness, NRInput.GetTouch());
                }
				else if (IsTouching) {
					// Update on touch position. 
					CoreServices.InputSystem?.RaiseOnTouchUpdated(InputSource, this, ControllerHandedness, NRInput.GetTouch());
				}
			}

			for (int i = 0; i < Interactions?.Length; i++)
			{
				switch (Interactions[i].InputType)
				{
					case DeviceInputType.SpatialPointer:
						var pointerPose = new MixedRealityPose(controller.position, controller.rotation);
						Interactions[i].PoseData = pointerPose;
						CoreServices.InputSystem?.RaisePointerDragged(InputSource.Pointers[0], Interactions[i].MixedRealityInputAction, ControllerHandedness);
						if (Interactions[i].Changed)
						{
							CoreServices.InputSystem?.RaisePoseInputChanged(InputSource, ControllerHandedness, Interactions[i].MixedRealityInputAction, pointerPose);
						}
						break;
					// case DeviceInputType.SpatialGrip:
					// 	Interactions[i].PoseData = gripPose;
					// 	if (Interactions[i].Changed)
					// 	{
					// 		CoreServices.InputSystem?.RaisePoseInputChanged(InputSource, ControllerHandedness, Interactions[i].MixedRealityInputAction, gripPose);
					// 	}
					// 	break;
					case DeviceInputType.Select:
					case DeviceInputType.TriggerPress:
					case DeviceInputType.GripPress:
						Interactions[i].BoolData = NRInput.GetButton(ControllerButton.TRIGGER);
						if (Interactions[i].Changed)
						{
							if (Interactions[i].BoolData)
							{
								CoreServices.InputSystem?.RaiseOnInputDown(InputSource, ControllerHandedness, Interactions[i].MixedRealityInputAction);
								CoreServices.InputSystem?.RaisePointerDown(InputSource.Pointers[0], Interactions[i].MixedRealityInputAction);
							}
							else
							{
								CoreServices.InputSystem?.RaisePointerClicked(InputSource.Pointers[0], Interactions[i].MixedRealityInputAction, 0, ControllerHandedness);
								CoreServices.InputSystem?.RaiseOnInputUp(InputSource, ControllerHandedness, Interactions[i].MixedRealityInputAction);
								CoreServices.InputSystem?.RaisePointerUp(InputSource.Pointers[0], Interactions[i].MixedRealityInputAction);
							}
						}
						break;
					// case DeviceInputType.IndexFinger:
					// 	HandDefinition?.UpdateCurrentIndexPose(Interactions[i]);
					// 	break;
					case DeviceInputType.ThumbStick:
						// Debug.Log("THUMBSTICK");
						// 	HandDefinition?.UpdateCurrentTeleportPose(Interactions[i]);
						break;
				}
			}
#endif
		}

		public bool StartHapticImpulse(float intensity, float durationInSeconds = float.MaxValue)
        {
#if BUILD_XREAL
            bool supportHaptic = NRInput.GetControllerAvailableFeature(ControllerAvailableFeature.CONTROLLER_AVAILABLE_FEATURE_HAPTIC_VIBRATE);
            if (supportHaptic)
            {
                NRInput.TriggerHapticVibration(durationInSeconds, intensity);
                return true;
            }
            return false;
#else
			return false;
#endif
        }

        public void StopHapticFeedback() { }
	}
}