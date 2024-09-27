using UnityEngine;
using System;
using System.Threading.Tasks;
using Microsoft;
using Google.Protobuf;

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;


namespace TOM.Common.Utils
{
    public class GazeTracker
    {
        private MovingAverageFilter gazeCameraPosition;
        private Vector3 screenSpacePosition;
        private Vector3 avgCameraSpacePosition;
        private Vector3 gazePosition;
        private bool isGazing;

        public GazeTracker(int bufferSize) {
            this.gazeCameraPosition = new MovingAverageFilter(bufferSize);
        }

        public void updateGaze(IMixedRealityEyeGazeProvider eyeGazeProvider, float deltaTime, float cursorDistance)
        {
            try
            {
                handleGazeGesture(eyeGazeProvider, Camera.main, deltaTime, cursorDistance);
            }
            catch (Exception e)
            {
                Debug.LogError("handleGaze: " + e.Message);
            }       

        }

        private void handleGazeGesture(IMixedRealityEyeGazeProvider eyeGazeProvider, Camera camera, float deltaTime, float cursorDistance)
        {
            if (eyeGazeProvider == null || camera == null)
            {
                Debug.Log("handleGaze: gaze null");
                this.isGazing = false;
                gazeCameraPosition.Clear();
                return;
            }

            this.isGazing = true;
            this.gazePosition = eyeGazeProvider.GazeOrigin + eyeGazeProvider.GazeDirection.normalized * cursorDistance;
            Vector3 cameraSpacePosition = camera.transform.InverseTransformPoint(gazePosition);
            this.screenSpacePosition = SpaceConverter.CameraToScreen(cameraSpacePosition, camera);
            this.avgCameraSpacePosition = gazeCameraPosition.Process(this.screenSpacePosition);   
        }

        public Vector3 getCameraSpacePosition()
        {
            return this.avgCameraSpacePosition;
        }

        public Vector3 getScreenSpacePosition()
        {
            return this.screenSpacePosition;
        }

        public Vector3 getGazePosition()
        {
            return this.gazePosition;
        }

        public bool isGazeDetected() 
        {
            return this.isGazing;
        }

        public void resetGazeTracker()
        {
            gazeCameraPosition.Clear();
        }
    }
}
