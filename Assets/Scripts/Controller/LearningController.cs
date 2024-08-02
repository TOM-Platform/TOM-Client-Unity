using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.Audio;
using Google.Protobuf;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;


public class LearningController : HomeController
{
    public SocketCommunication socketCommunication;

    public LearningUIController learningUIController;

    public SpeechCommunication speechCommunication;

    public GameObject pointingCursor;
    public GameObject selectionCursor;

    public bool enableHand = true;
    public bool enableGaze = true;

    
    private const int GESTURE_SEND_GAP_SECONDS = 1;
    private const int GESTURE_AVG_BUFFER_SIZE = 10;
    private double lastGestureSendTime = 0;

    private GameObject indexFingerMarker;
    private MovingAverageFilter gestureCameraPosition;

    private const float DEFAULT_GAZE_CURSOR_DISTANCE = 2f;

    private bool gazeSelectActive = false;

    private const int GAZE_SEND_GAP_SECONDS = 1;
    private const int GAZE_AVG_BUFFER_SIZE = 10;
    private double lastGazeSendTime = 0;

    private MovingAverageFilter gazeCameraPosition;

    private const float GAZE_AUTO_DESELECT_GAP_SECONDS = 0.5f;

    private const float HIGHLIGHT_POINT_DISPLAY_DURATION = 5f;

    private const float LEARN_REQUEST_GAP_SECONDS = 4f;


    // Start is called before the first frame update
    void Start()
    {
        learningUIController.ResetUI();

        indexFingerMarker = Instantiate(pointingCursor, this.transform);
        gestureCameraPosition = new MovingAverageFilter(GESTURE_AVG_BUFFER_SIZE);

        gazeCameraPosition = new MovingAverageFilter(GAZE_AVG_BUFFER_SIZE);

        // Turn on gaze pointer
        PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOn);

        // FIXME: hack to request learning data
        InvokeRepeating("sendLearningRequestToServer", 2.0f, LEARN_REQUEST_GAP_SECONDS);
    }

    // Update is called once per frame
    void Update()
    {
        if (enableGaze && gazeSelectActive)
        {
            handleGaze();
        }

        if (enableHand) {
            handleHandGestures();
        }        

        handleSpeech();
        handleCommunication();        
    }

    public void OnGazeSelect()
    {
        gazeSelectActive = true;
        // auto deactive after GAZE_AUTO_DESELECT_GAP_SECONDS 
        Invoke("OnGazeUnselect", GAZE_AUTO_DESELECT_GAP_SECONDS); 
    }

    public void OnGazeUnselect()
    {
        gazeSelectActive = false;
    }

    private void handleGaze()
    {
        try
        {
            handleGazeGesture(CoreServices.InputSystem?.EyeGazeProvider,  Camera.main);
        }
        catch (Exception e)
        {
            Debug.LogError("handleGaze: " + e.Message);
        }       

    }

    private void handleHandGestures()
    {
        indexFingerMarker.GetComponent<Renderer>().enabled = false;

        MixedRealityPose indexFingerPose = MixedRealityPose.ZeroIdentity;
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out indexFingerPose))
        {
            indexFingerMarker.GetComponent<Renderer>().enabled = true;
            indexFingerMarker.transform.position = indexFingerPose.Position;            
        }

        try
        {
            handlePointingGesture(indexFingerPose, Camera.main);
        }
        catch(Exception e)
        {
            Debug.LogError("handleGestures: " + e.Message);
        }        
    }

    private void handleSpeech()
    {
        if (speechCommunication.IsSpeechAvailable())
        {
            sendSpeechData(speechCommunication.SpeechResult);
        }
    }

    private void handleCommunication()
    {
        if (socketCommunication.DataReceived())
        {
            List<byte[]> messages = socketCommunication.GetMessages();
            foreach (byte[] message in messages)
            {
                ProcessDataBytes(message);
            }
        }
    }

    private bool ProcessDataBytes(byte[] byteData)
    {
        try
        {
            Debug.Log("ProcessDataBytes");
            SocketData socketData = SocketData.Parser.ParseFrom(byteData);

            int dataType = socketData.DataType;
            ByteString data = socketData.Data;

            if (dataType == DataTypes.LEARNING_DATA)
            {
                try
                {
                    LearningData learningData = LearningData.Parser.ParseFrom(data);

                    UpdateLearningUI(dataType, learningData);

                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError("Bytes received cannot be decoded as LearningData: " + e.Message);
                    return false;
                }

            }
            else if (dataType == DataTypes.HIGHLIGHT_POINT_DATA)
            {
                try
                {
                    HighlightPointData highlightPointData = HighlightPointData.Parser.ParseFrom(data);

                    HighlightPointSelection(dataType, highlightPointData);

                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError("Bytes received cannot be decoded as HighlightPointData: " + e.Message);
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

    private void UpdateLearningUI(int dataType, LearningData learningData)
    {
        Debug.Log("LearningData:\n" +
                                "dataType: " + dataType + "\n" +
                                "Instruction: " + learningData.Instruction + "\n" +
                                "Detail: " + learningData.Detail + "\n" +
                                "Speech: " + learningData.Speech + "\n" );


        learningUIController.UpdateInstruction(learningData.Instruction);
        learningUIController.UpdateDetail(learningData.Detail);
        learningUIController.StartSpeaking(learningData.Speech);
    }

    public void HighlightPointSelection(int dataType, HighlightPointData highlightPointData)
    {
        Debug.Log("HighlightPointData:\n" +
                         "dataType: " + dataType + "\n" +
                         "X: " + highlightPointData.WorldX + "\n" +
                         "Y: " + highlightPointData.WorldY + "\n" +
                         "Z: " + highlightPointData.WorldZ + "\n");

        Vector3 point = new Vector3((float)highlightPointData.WorldX, (float)highlightPointData.WorldY, (float)highlightPointData.WorldZ);
       
        learningUIController.ShowSelectionPoint(point, HIGHLIGHT_POINT_DISPLAY_DURATION);
    }

    public void SetLearningUIVisible(bool isVisible)
    {
        learningUIController.SetUIVisible(isVisible);
    }

    private void handlePointingGesture(MixedRealityPose pose, Camera camera)
    {
        if (pose == MixedRealityPose.ZeroIdentity || camera == null)
        {
            gestureCameraPosition.Clear();
            return;
        }

        Vector3 cameraSpacePosition = camera.transform.InverseTransformPoint(pose.Position);
        Vector3 screenSpacePosition = SpaceConverter.CameraToScreen(cameraSpacePosition, camera);
        Vector3 avgCameraSpacePosition = gestureCameraPosition.Process(screenSpacePosition);

        //VisualLog.Log(avgCameraSpacePosition.ToString("F4"));

        if (Time.time - lastGestureSendTime > GESTURE_SEND_GAP_SECONDS )
        {
            lastGestureSendTime = Time.time;
            sendGesturePointData(avgCameraSpacePosition, pose.Position);
        }
    }

    private void handleGazeGesture(IMixedRealityEyeGazeProvider eyeGazeProvider, Camera camera)
    {
        if (eyeGazeProvider == null || camera == null)
        {
            Debug.Log("handleGaze: gaze null");
            gazeCameraPosition.Clear();
            return;
        }

        Vector3 gazePosition = eyeGazeProvider.GazeOrigin + eyeGazeProvider.GazeDirection.normalized * DEFAULT_GAZE_CURSOR_DISTANCE;

        Vector3 cameraSpacePosition = camera.transform.InverseTransformPoint(gazePosition);
        Vector3 screenSpacePosition = SpaceConverter.CameraToScreen(cameraSpacePosition, camera);
        Vector3 avgCameraSpacePosition = gazeCameraPosition.Process(screenSpacePosition);

        //VisualLog.Log(avgCameraSpacePosition.ToString("F4"));

        if (Time.time - lastGazeSendTime > GAZE_SEND_GAP_SECONDS)
        {
            lastGazeSendTime = Time.time;
            sendGazePointData(avgCameraSpacePosition, gazePosition);
        }
    }

    private void sendGesturePointData(Vector3 gestureCameraPosition, Vector3 gestureWorldPosition)
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

    private void sendSpeechData(string voice)
    {
        Debug.Log("sendSpeechData: " + voice);

        SpeechData speechData = new SpeechData();
        speechData.Voice = voice;

        SocketData socketData = new SocketData();
        socketData.DataType = DataTypes.SPEECH_INPUT_DATA;
        socketData.Data = ByteString.CopyFrom(speechData.ToByteArray());

        socketCommunication.SendMessages(socketData.ToByteArray());
    }


    private void sendRequestToServer(int requestType, string data = "")
    {
        Debug.Log("sendRequestToServer: " + requestType + ", " + data);
        RequestData requestData = new RequestData();
        requestData.Detail = data;

        SocketData socketData = new SocketData();
        socketData.DataType = requestType;
        socketData.Data = ByteString.CopyFrom(requestData.ToByteArray());

        socketCommunication.SendMessages(socketData.ToByteArray());
    }

    private void sendLearningRequestToServer()
    {
        sendRequestToServer(DataTypes.REQUEST_LEARNING_DATA);
    }

}



class MovingAverageFilter
{
    private Queue<Vector3> buffer;
    private Vector3 sum = Vector3.zero;
    private int bufferSize;

    public MovingAverageFilter(int bufferSize)
    {
        this.bufferSize = bufferSize;
        this.buffer = new Queue<Vector3>(bufferSize);
    }

    public Vector3 Process(Vector3 newPoint)
    {
        if (buffer.Count >= bufferSize)
        {
            Vector3 oldPoint = buffer.Dequeue();
            sum -= oldPoint;
        }

        buffer.Enqueue(newPoint);
        sum += newPoint;
        return sum / buffer.Count;
    }
    public void Clear()
    {
        buffer.Clear();
        sum = Vector3.zero;
    }
}
