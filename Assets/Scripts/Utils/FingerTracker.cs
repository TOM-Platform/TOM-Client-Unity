using UnityEngine;
using System;
using Microsoft;
using Google.Protobuf;

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;


namespace TOM.Common.Utils
{
    public class HandTracker
    {
        private MovingAverageFilter gestureCameraPosition;
        private Vector3 screenSpacePosition;
        private Vector3 avgCameraSpacePosition;
        private MixedRealityPose pose;
        private bool isGesturing;

        //Adapted from https://localjoost.github.io/Basic-hand-gesture-recognition-with-MRTK-on-HoloLens-2/
        private const float PINCH_THRESHOLD = 0.7f; //Float ranging from 0 to 1. 0 if index finger is straight/not curled, 1 if index finger is curled
        private Handedness trackedHand;

        public HandTracker(int bufferSize, Handedness trackedHand=Handedness.Right)
        {
            this.trackedHand = trackedHand;
            this.gestureCameraPosition = new MovingAverageFilter(bufferSize);
        }

        public void updateHand()
        {
            //Set transform of the pose to all zeros by default
            MixedRealityPose handPose = MixedRealityPose.ZeroIdentity;
            MixedRealityPose gesturePose = MixedRealityPose.ZeroIdentity;

            try
            {
                HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, trackedHand, out handPose);
                this.pose = handPose;
                handlePointingGesture(handPose, Camera.main);
            }
            catch(Exception e)
            {
                Debug.LogError("handleGestures: " + e.Message);
            }        
        }

        private void handlePointingGesture(MixedRealityPose pose, Camera camera)
        {
            if (pose == MixedRealityPose.ZeroIdentity || camera == null)
            {
                this.isGesturing = false;
                gestureCameraPosition.Clear();
                return;
            }

            this.isGesturing = true;
            //get position tranform of handpose relative to camera
            Vector3 cameraSpacePosition = camera.transform.InverseTransformPoint(pose.Position);
            //get position of handpose relative to the screen displayed by camera
            this.screenSpacePosition = SpaceConverter.CameraToScreen(cameraSpacePosition, camera);
            //average out the position transform so that the positions seem stable
            this.avgCameraSpacePosition = gestureCameraPosition.Process(screenSpacePosition);

            //VisualLog.Log(avgCameraSpacePosition.ToString("F4"));
        }

        public Vector3 getCameraSpacePosition()
        {
            return this.avgCameraSpacePosition;
        }

        public Vector3 getScreenSpacePosition()
        {
            return this.screenSpacePosition;
        }

        public MixedRealityPose getHandPose()
        {
            return this.pose;
        }

        public bool isHandDetected()
        {
            return this.isGesturing;
        }

        public void resetHandTracker()
        {
            gestureCameraPosition.Clear();
        }

        public bool IsPinching()
        {
            return HandPoseUtils.CalculateIndexPinch(this.trackedHand) > PINCH_THRESHOLD;
        }
    }
}
