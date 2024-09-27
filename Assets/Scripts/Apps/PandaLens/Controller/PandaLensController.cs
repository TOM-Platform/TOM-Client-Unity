using TOM.Common.Communication;
using TOM.Common.Utils;

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Windows.WebCam;

using Google.Protobuf;
using Google.Protobuf.Collections;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Audio;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;

namespace TOM.Apps.PandaLens
{
    public class PandaLensController : HomeController
    {
        public SocketCommunication socketCommunication;
        public PandaLensUIController pandaLensUIController;
        public SpeechCommunication speechCommunication;
        private GazeTracker gazeTracker = null;
        private HandTracker handTracker = null;
        private float gazeTimer = 0.0f;
        private float handTimer = 0.0f;
        private float idleTimer = 0.0f;

        public bool enableHand = true;
        public bool enableGaze = true;


        // Start is called before the first frame update
        void Start()
        {
            // Turn on gaze pointer
            PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOn);

            handTracker = new HandTracker(PandaLensConfig.HAND_GESTURE_AVG_BUFFER_SIZE);
            gazeTracker = new GazeTracker(PandaLensConfig.GAZE_AVG_BUFFER_SIZE);
        }

        // Update is called once per frame
        void Update()
        {
            idleTimer += Time.deltaTime; 
            if (idleTimer > PandaLensConfig.PANDALENS_IDLE_THRESHOLD_SECONDS)
            {
                handleIdle();
                return;
            }
            if (enableGaze)
            {
                handleGaze();
            }

            if (enableHand) 
            {
                handleHand();
            }        

            handleSpeech();
            handleCommunication();
            handleKeyInputs();
        }

        private void handleKeyInputs()
        {
            // check keys codes
            // foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            // {
            //     Debug.Log("KeyCode down");
            //     if (Input.GetKey(kcode))
            //        Debug.Log("KeyCode down: " + kcode);
            // }

            if (Input.GetKeyDown(RingMouseKey.DOWN_ACTION_KEYCODE))
            {
                if (pandaLensUIController.IsDialogueUIActive() || pandaLensUIController.IsSummaryUIActive())
                {
                    SpeechDictationAction();
                }
            }
            else if (Input.GetKeyDown(RingMouseKey.UP_ACTION_KEYCODE))
            {
                if(pandaLensUIController.IsDialogueUIActive() || pandaLensUIController.IsSummaryUIActive())
                {
                    ResetToInitState();
                    sendEventData(PandaLensConfig.PANDALENS_IDLE_ACTION);
                }
            }
            else if (Input.GetKeyDown(RingMouseKey.CENTER_ACTION_KEYCODE))
            {
                if (pandaLensUIController.IsCameraUIActive())
                {
                    onCameraButtonClick();
                }
            }
            else if (Input.GetKeyDown(RingMouseKey.RIGHT_ACTION_KEYCODE))
            {
                if (pandaLensUIController.IsDialogueUIActive())
                {
                    StartCoroutine(DelayToNextFrame(() => pandaLensUIController.ToggleDialogueUI()));
                }
                if (pandaLensUIController.IsCameraUIActive())
                {
                    onSummaryButtonClick();
                }
            }
        }
        private void handleIdle()
        {
            ResetToInitState();
            sendEventData(PandaLensConfig.PANDALENS_IDLE_ACTION);
        }

        private void ResetToInitState()
        {
            resetAllTimers();
            speechCommunication.ResetSpeechResult();
            speechCommunication.StopListening();
            pandaLensUIController.ResetUI();
        }

        private void handleGaze()
        {
            if (!pandaLensUIController.IsCameraUIActive())
            {
                gazeTimer = 0.0f;
                return;
            }
            gazeTracker.updateGaze(CoreServices.InputSystem?.EyeGazeProvider, Time.deltaTime, PandaLensConfig.GAZE_CURSOR_DISTANCE);

            Vector3 avgCameraSpacePosition = gazeTracker.getCameraSpacePosition();
            Vector3 screenSpacePosition = gazeTracker.getScreenSpacePosition();
            Vector3 gazePosition = gazeTracker.getGazePosition();

            gazeTimer += Time.deltaTime;

            if (Vector3.Angle(avgCameraSpacePosition, screenSpacePosition) > PandaLensConfig.GAZE_ANGLE_DISPLACEMENT_THRESHOLD || 
                Vector3.Distance(avgCameraSpacePosition, screenSpacePosition) > PandaLensConfig.GAZE_POSITION_DISPLACEMENT_THRESHOLD)
                {
                    gazeTracker.resetGazeTracker();
                    gazeTimer = 0.0f;
                }
            
            gazeTimer += Time.deltaTime;

            if (gazeTimer > PandaLensConfig.GAZE_DURATION_THRESHOLD && gazeTracker.isGazeDetected())
            {
                sendGazePointData(avgCameraSpacePosition, gazePosition);
                gazeTracker.resetGazeTracker();
                gazeTimer = 0.0f;
            }
        }

        private void handleHand()
        {
            if (!pandaLensUIController.IsCameraUIActive())
            {
                handTimer = 0.0f;
                return;
            }

            handTracker.updateHand();
            Vector3 avgCameraSpacePosition = handTracker.getCameraSpacePosition();
            MixedRealityPose pose = handTracker.getHandPose();

            handTimer += Time.deltaTime;

            //Update the server with your hand pose movements at every frame if it is pinching
            if (handTimer > PandaLensConfig.GESTURE_SEND_GAP_SECONDS && handTracker.IsPinching())
            {
                sendHandPointData(avgCameraSpacePosition, pose.Position);
                handTimer = 0.0f;
            }
        }

        private void handleSpeech()
        {
            if (speechCommunication.IsSpeechAvailable())
            {   
                idleTimer = 0.0f;
                string speech = speechCommunication.SpeechResult;
                speechCommunication.ResetSpeechResult();
                if (speech != "")
                {
                    sendSpeechData(speech);
                }
            }
        }

        private void handleCommunication()
        {
            if (socketCommunication.DataReceived())
            {
                idleTimer = 0.0f;
                List<byte[]> messages = socketCommunication.GetMessages();
                foreach (byte[] message in messages)
                {
                    ProcessDataBytes(message);
                }
            }
        }
        // Parse ingoing data from server to client and update the UI
        private bool ProcessDataBytes(byte[] byteData)
        {
            try
            {
                Debug.Log("ProcessDataBytes");
                SocketData socketData = SocketData.Parser.ParseFrom(byteData);

                int dataType = socketData.DataType;
                ByteString data = socketData.Data;

                if (dataType == DataTypes.PANDALENS_QUESTION)
                {
                    try
                    {
                        PandaLensQuestion pandaLensQuestion = PandaLensQuestion.Parser.ParseFrom(data);
                        UpdatePandaLensQuestionUI(pandaLensQuestion);
                        pandaLensUIController.StopLoadingDisplay();

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as PandaLensQuestion: " + e.Message);
                        return false;
                    }

                }
                else if (dataType == DataTypes.PANDALENS_RESPONSE)
                {
                    try
                    {
                        PandaLensResponse pandaLensResponse = PandaLensResponse.Parser.ParseFrom(data);
                        UpdatePandaLensResponseUI(pandaLensResponse);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as PandaLensResponse: " + e.Message);
                        return false;
                    }

                }
                else if (dataType == DataTypes.PANDALENS_MOMENTS)
                {
                    try
                    {
                        PandaLensMoments pandaLensMoments = PandaLensMoments.Parser.ParseFrom(data);
                        UpdatePandaLensMomentsUI(pandaLensMoments);
                        pandaLensUIController.StopLoadingDisplay();

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as PandaLensMoments: " + e.Message);
                        return false;
                    }

                }
                else if (dataType == DataTypes.PANDALENS_ERROR)
                {
                    try
                    {
                        PandaLensError pandaLensError = PandaLensError.Parser.ParseFrom(data);
                        pandaLensUIController.StartSpeaking(pandaLensError.Error);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as PandaLensMoments: " + e.Message);
                        return false;
                    }

                }
                else if (dataType == DataTypes.PANDALENS_RESET)
                {
                    try
                    {
                        PandaLensReset pandaLensReset = PandaLensReset.Parser.ParseFrom(data);
                        ResetToInitState();
                        pandaLensUIController.StartSpeaking(pandaLensReset.Message);
                        pandaLensUIController.StopLoadingDisplay();

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as PandaLensMoments: " + e.Message);
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("Bytes received is unsupported: " + dataType + ", " + data.ToStringUtf8());
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Bytes received is not SocketData: " + e.Message + ", " + System.Text.Encoding.UTF8.GetString(byteData));
                return false;
            }
        }

        private void UpdatePandaLensQuestionUI(PandaLensQuestion pandaLensQuestion)
        {
            string question = pandaLensQuestion.Content;
            pandaLensUIController.SetConversationUIActive(true);
            pandaLensUIController.RenderQuestion(pandaLensQuestion.Image, pandaLensQuestion.Content);
            pandaLensUIController.StartSpeaking(pandaLensQuestion.Speech);
        }

        private void UpdatePandaLensResponseUI(PandaLensResponse pandaLensResponse)
        {
            string question = pandaLensResponse.Content;
            pandaLensUIController.SetConversationUIActive(true);
            pandaLensUIController.RenderResponse(pandaLensResponse.Content);
            pandaLensUIController.StartSpeaking(pandaLensResponse.Speech);
        }

        private void UpdatePandaLensMomentsUI(PandaLensMoments pandaLensMoments)
        {
            RepeatedField<string> moments = pandaLensMoments.Moments;
            string[] momentsArr = moments.ToArray();
            pandaLensUIController.SetBlogUIActive(true);
            pandaLensUIController.RenderMomentsList(momentsArr);
        }

        private void sendEventData(string eventType)
        {
            resetAllTimers();
            PandaLensInput pandaLensInput = new PandaLensInput();
            pandaLensInput.KeyInput = eventType;

            SocketData socketData = new SocketData();
            socketData.DataType = DataTypes.PANDALENS_EVENT_DATA;
            socketData.Data = ByteString.CopyFrom(pandaLensInput.ToByteArray());

            socketCommunication.SendMessages(socketData.ToByteArray());
        }

        private void resetAllTimers()
        {
            idleTimer = 0;
            gazeTimer = 0;
            handTimer = 0;
        }

        private void sendSpeechData(string voice)
        {
            resetAllTimers();
            SpeechData speechData = new SpeechData();
            speechData.Voice = voice;

            SocketData socketData = new SocketData();
            socketData.DataType = DataTypes.SPEECH_INPUT_DATA;
            socketData.Data = ByteString.CopyFrom(speechData.ToByteArray());

            socketCommunication.SendMessages(socketData.ToByteArray());
        }

        private void sendHandPointData(Vector3 gestureCameraPosition, Vector3 gestureWorldPosition)
        {
            Debug.Log("sendGesturePointData: " + gestureCameraPosition.ToString("F2"));

            FingerPoseData poseData = new FingerPoseData();
            poseData.CameraX = gestureCameraPosition.x;
            poseData.CameraY = gestureCameraPosition.y;
            poseData.CameraZ = gestureCameraPosition.z;

            poseData.WorldX = gestureWorldPosition.x;
            poseData.WorldY = gestureWorldPosition.y;
            poseData.WorldZ = gestureWorldPosition.z;

            SocketData socketData = new SocketData();
            socketData.DataType = DataTypes.FINGER_POINTING_DATA;
            socketData.Data = ByteString.CopyFrom(poseData.ToByteArray());

            socketCommunication.SendMessages(socketData.ToByteArray());
        }
        
        private void sendGazePointData(Vector3 gazeCameraPosition, Vector3 gazeWorldPosition)
        {
            Debug.Log("sendGazePointData: " + gazeCameraPosition.ToString("F2"));

            GazePointData gazeData = new GazePointData();
            gazeData.CameraX = gazeCameraPosition.x;
            gazeData.CameraY = gazeCameraPosition.y;
            gazeData.CameraZ = gazeCameraPosition.z;

            gazeData.WorldX = gazeWorldPosition.x;
            gazeData.WorldY = gazeWorldPosition.y;
            gazeData.WorldZ = gazeWorldPosition.z;

            SocketData socketData = new SocketData();
            socketData.DataType = DataTypes.GAZE_POINTING_DATA;
            socketData.Data = ByteString.CopyFrom(gazeData.ToByteArray());

            socketCommunication.SendMessages(socketData.ToByteArray());
        }

        private void SpeechDictationAction()
        {
            resetAllTimers();
            pandaLensUIController.RenderListeningSpeech();
            speechCommunication.ResetSpeechResult();
            speechCommunication.StartListening();
        }

        public void onMicrophoneButtonClick()
        {
            SpeechDictationAction();
        }

        public void onNotificationButtonClick()
        {
            StartCoroutine(DelayToNextFrame(() => pandaLensUIController.ToggleDialogueUI()));
        }

        public void onEscapeButtonClick()
        {
            StartCoroutine(DelayToNextFrame(() => {
                ResetToInitState();
                sendEventData(PandaLensConfig.PANDALENS_IDLE_ACTION);
                }));
        }

        public void onCameraButtonClick()
        {
            pandaLensUIController.StartLoadingDisplay();
            sendEventData(PandaLensConfig.PANDALENS_CAMERA_ACTION);
        }

        public void onSummaryButtonClick()
        {
            pandaLensUIController.StartLoadingDisplay();
            sendEventData(PandaLensConfig.PANDALENS_SUMMARY_ACTION);
        }

        private IEnumerator DelayToNextFrame(Action lambda)
        {
            // Wait for a frame to ensure all other updates are complete
            yield return null;
            // Do whatever
            lambda();
        }
    }
}
