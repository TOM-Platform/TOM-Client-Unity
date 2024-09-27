using TOM.Common.Communication;
using TOM.Common.UI;
using static TOM.Apps.Running.RunningTypePositonMapping;

using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Google.Protobuf;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Audio;

namespace TOM.Apps.Running
{

    public class RunningController : HomeController
    {
        public SocketCommunication socketCommunication;

        public RunningUIController runningUIController;
        public SummaryUIController summaryUIController;
        public TrainingModeSelectionUIController trainingModeSelectionUIController;
        public ProgressIndicator progressIndicator;

        public TextToSpeech textToSpeech;

        private const int SOCKET_COMMUNICATION_START_DELAY_SECONDS = 4;
        private const int RUNNING_DATA_REQUEST_DELAY_SECONDS = 1;
        private const int DIRECTION_DATA_REQUEST_DELAY_SECONDS = 5;
        private const int SPEECH_DELAY_SECONDS = 1;
        private const int SEND_CAMERA_DATA_DELAY_SECONDS = 1;

        private LinkedList<(int, string)> speechList = new LinkedList<(int, string)>();
        private string prevDirectionInstr = "";
        private string targetFooter = null;

        private RunningTrainingMode? trainingMode = null;

        // Start is called before the first frame update
        void Start()
        {
            setDefaultRunningTypePositionMapping();

            runningUIController.ResetUI();
            summaryUIController.ResetUI();
            speechList.Clear();
            Invoke(nameof(RequestInitSetup), SOCKET_COMMUNICATION_START_DELAY_SECONDS);
        }

        // Update is called once per frame
        void Update()
        {
            handleCommunication();
        }

        private void InvokeRequests()
        {
            InvokeRepeating(nameof(RequestRunningLiveData), SOCKET_COMMUNICATION_START_DELAY_SECONDS,
                RUNNING_DATA_REQUEST_DELAY_SECONDS);
            InvokeRepeating(nameof(RequestDirectionData), SOCKET_COMMUNICATION_START_DELAY_SECONDS,
                DIRECTION_DATA_REQUEST_DELAY_SECONDS);
            InvokeRepeating(nameof(SpeakInstructions), SOCKET_COMMUNICATION_START_DELAY_SECONDS, SPEECH_DELAY_SECONDS);
            if (runningUIController.getIsVisible())
            {
                SendRequestToServer(DataTypes.REQUEST_RANDOM_ROUTES_DATA);
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

        private void RequestInitSetup()
        {
            SendRequestToServer(DataTypes.REQUEST_RUNNING_TYPE_POSITION_MAPPING);
            SendRequestToServer(DataTypes.REQUEST_RUNNING_LIVE_UNIT);
            SendRequestToServer(DataTypes.REQUEST_RUNNING_SUMMARY_UNIT);
        }

        private void RequestRunningLiveData()
        {
            if (runningUIController.getIsVisible())
            {
                SendRequestToServer(DataTypes.REQUEST_RUNNING_LIVE_DATA);
            }
        }

        private void RequestDirectionData()
        {
            if (runningUIController.getIsVisible())
            {
                SendRequestToServer(DataTypes.REQUEST_DIRECTION_DATA);
            }
        }

        private void ProcessSocketData(byte[] socketData)
        {

            bool decodeSucess = ProcessDataBytes(socketData);

            if (!decodeSucess)
            {
                string stringData = System.Text.Encoding.UTF8.GetString(socketData);
                ProcessDataString(stringData);
            }
        }

        private void ProcessDataString(string stringData)
        {
            Debug.LogError("ProcessDataString: " + stringData);
        }

        private bool ProcessDataBytes(byte[] byteData)
        {
            try
            {
                long currentTimeMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Debug.Log("ProcessDataBytes");
                SocketData socketData = SocketData.Parser.ParseFrom(byteData);

                int dataType = socketData.DataType;
                ByteString data = socketData.Data;

                if (dataType == DataTypes.RUNNING_LIVE_DATA || dataType == DataTypes.RUNNING_LIVE_UNIT ||
                    dataType == DataTypes.RUNNING_LIVE_ALERT)
                {
                    try
                    {
                        RunningLiveData runningLiveData = RunningLiveData.Parser.ParseFrom(data);

                        UpdateRunningUI(dataType, runningLiveData);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as RunningLiveData: " + e.Message);
                        return false;
                    }

                }
                else if (dataType == DataTypes.RUNNING_SUMMARY_DATA || dataType == DataTypes.RUNNING_SUMMARY_UNIT)
                {
                    try
                    {
                        RunningSummaryData runningSummaryData = RunningSummaryData.Parser.ParseFrom(data);

                        UpdateSummaryUI(dataType, runningSummaryData);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as RunningSummaryData: " + e.Message);
                        return false;
                    }

                }
                else if (dataType == DataTypes.RUNNING_TYPE_POSITION_MAPPING_DATA)
                {
                    try
                    {
                        RunningTypePositionMappingData runningTypePositionMapping =
                            RunningTypePositionMappingData.Parser.ParseFrom(data);

                        UpdateRunningUIPositionMapping(runningTypePositionMapping);
                        trainingModeSelectionUIController.UpdateSelectionUI();
                        trainingModeSelectionUIController.SetUIVisible(true);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as RunningTypePositionMappingData: " +
                                       e.Message);
                        return false;
                    }

                }
                else if (dataType == DataTypes.DIRECTION_DATA)
                {
                    try
                    {
                        DirectionData directionData = DirectionData.Parser.ParseFrom(data);
                        UpdateDirectionUI(directionData);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as DirectionData: " + e.Message);
                        return false;
                    }
                }
                else if (dataType == DataTypes.RANDOM_ROUTES_DATA)
                {
                    try
                    {
                        Debug.Log("Received random routes data");
                        RandomRoutesData randomRoutesData = RandomRoutesData.Parser.ParseFrom(data);
                        UpdateImageCarousel(randomRoutesData);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as RandomRoutesData: " + e.Message);
                        return false;
                    }
                }
                else if (dataType == DataTypes.RUNNING_TARGET_DATA)
                {
                    RunningTargetData runningTargetData = RunningTargetData.Parser.ParseFrom(data);
                    UpdateTargetData(runningTargetData);
                    return true;
                }
                else if (dataType == DataTypes.RUNNING_PLACE_DATA)
                {
                    RunningPlaceData runningPlaceData = RunningPlaceData.Parser.ParseFrom(data);
                    UpdatePlaceData(runningPlaceData);
                    return true;
                }
                else
                {
                    Debug.LogError("Bytes received is unsupported: " + dataType + ", " + data.ToStringUtf8());
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Bytes received is not SocketData: " + e.Message + ", " +
                               System.Text.Encoding.UTF8.GetString(byteData));
                return false;
            }
        }

        private void UpdatePlaceData(RunningPlaceData runningPlaceData)
        {
            Debug.Log("Place data received: " + runningPlaceData);
            runningUIController.SetPlaceInfo(runningPlaceData);
        }

        private void UpdateImageCarousel(RandomRoutesData randomRoutesData)
        {
            Debug.Log("Routes received: " + randomRoutesData.Routes.Count);
            runningUIController.UpdateImageCarousel(randomRoutesData.Routes);
            progressIndicator.StopProgressIndicator("Displaying routes...");
        }

        private void UpdateTargetData(RunningTargetData runningTargetData)
        {
            Debug.Log("Target data received: " + runningTargetData);
            if (runningTargetData.HasDistance)
            {
                targetFooter = runningTargetData.Distance; // km
            }
            else if (runningTargetData.HasSpeed)
            {
                targetFooter = runningTargetData.Speed; // km/min
            }

            if (targetFooter != null) // Check if not null before updating
            {
                runningUIController.UpdateTargetFooter(targetFooter);
            }
        }


        private void UpdateRunningUI(int dataType, RunningLiveData runningLiveData)
        {
            bool isUnit = (dataType == DataTypes.RUNNING_LIVE_UNIT);
            bool isAlert = (dataType == DataTypes.RUNNING_LIVE_ALERT);
            string targetContent = "";

            if (isAlert && runningLiveData.AudioInstr)
            {
                AddInstrToSpeechList(DataTypes.RUNNING_LIVE_DATA, runningLiveData.Instruction);
            }

            if (!isUnit && !isAlert && runningLiveData.HasDistance)
            {
                targetContent = runningLiveData.Distance;
            }

            runningUIController.UpdateRunningUI(runningLiveData, targetContent, isUnit, isAlert);
        }

        private void UpdateSummaryUI(int dataType, RunningSummaryData runningSummaryData)
        {
            bool isUnit = (dataType == DataTypes.RUNNING_SUMMARY_UNIT);
            string targetContent = "";
            if (!isUnit)
            {
                if (runningSummaryData.HasDistance)
                {
                    targetContent = runningSummaryData.Distance;
                }

                progressIndicator.StopProgressIndicator("Displaying summary...");
            }

            summaryUIController.UpdateSummaryUI(runningSummaryData, targetContent, targetFooter, isUnit);
        }

        private void UpdateDirectionUI(DirectionData directionData)
        {
            if (directionData.AudioInstr)
            {
                AddInstrToSpeechList(DataTypes.DIRECTION_DATA, directionData.CurrInstr);
            }

            var isGeneratingRoutes = directionData.CurrInstr == "Generating random routes...";
            if (isGeneratingRoutes)
            {
                progressIndicator.ShowProgressIndicator(directionData.CurrInstr);
            }

            runningUIController.UpdateDirectionUI(directionData, isGeneratingRoutes);
        }


        private void SpeakInstructions()
        {
            // dont speak if queue has nothing or TTS is currently speaking
            if (!(speechList.Count == 0 || textToSpeech.IsSpeaking()))
            {
                // Check if direction instruction is available
                var directionInstr =
                    speechList.FirstOrDefault(instruction => instruction.Item1 == DataTypes.DIRECTION_DATA);
                if (directionInstr.Item1 != 0)
                {
                    textToSpeech.StartSpeaking(directionInstr.Item2);
                    speechList.Remove(directionInstr);
                }
                else
                {
                    // No direction instruction found, speak running data if available
                    var runningInstr =
                        speechList.FirstOrDefault(instruction => instruction.Item1 == DataTypes.RUNNING_LIVE_DATA);
                    if (runningInstr.Item1 != 0)
                    {
                        textToSpeech.StartSpeaking(runningInstr.Item2);
                        speechList.Remove(runningInstr);
                    }
                }
            }
        }

        private void AddInstrToSpeechList(int dataType, string instruction)
        {
            if (DataTypes.DIRECTION_DATA == dataType)
            {
                // to avoid saying the same direction instruction multiple times
                if (instruction == prevDirectionInstr)
                {
                    return;
                }

                prevDirectionInstr = instruction;
            }

            speechList.AddLast((dataType, instruction));
        }

        public void SetSelectionUIVisible(bool isVisible)
        {
            if (isVisible)
            {
                runningUIController.SetUIVisible(false);
                summaryUIController.SetUIVisible(false);
            }

            trainingModeSelectionUIController.SetUIVisible(isVisible);
        }

        public void SetRunningUIVisible(bool isVisible)
        {
            if (isVisible)
            {
                trainingModeSelectionUIController.SetUIVisible(false);
                summaryUIController.SetUIVisible(false);
            }

            runningUIController.SetUIVisible(isVisible);
        }

        public void SetSummaryUIVisible(bool isVisible)
        {
            if (isVisible)
            {
                trainingModeSelectionUIController.SetUIVisible(false);
                runningUIController.SetUIVisible(false);
                SendRequestToServer(DataTypes.REQUEST_RUNNING_SUMMARY_DATA);
                progressIndicator.ShowProgressIndicator("Fetching summary details...");
            }

            summaryUIController.SetUIVisible(isVisible);
        }

        public void SelectSpeedTraining()
        {
            trainingMode = RunningTrainingMode.SpeedTraining;
            SendRequestToServer(DataTypes.REQUEST_RUNNING_TRAINING_MODE_DATA,
                RunningTrainingMode.SpeedTraining.ToString());
            Debug.Log("Selected SpeedTraining");
            SetRunningUIVisible(true);
            InvokeRequests();
        }

        public void SelectDistanceTraining()
        {
            trainingMode = RunningTrainingMode.DistanceTraining;
            SendRequestToServer(DataTypes.REQUEST_RUNNING_TRAINING_MODE_DATA,
                RunningTrainingMode.DistanceTraining.ToString());
            Debug.Log("Selected DistanceTraining");
            SetRunningUIVisible(true);
            InvokeRequests();
        }

        internal void SendRequestToServer(int requestType, string data = "")
        {
            Debug.Log("SendRequestToServer: " + requestType + ", " + data);
            RequestData requestData = new RequestData();
            requestData.Detail = data;

            SocketData socketData = new SocketData();
            socketData.DataType = requestType;
            socketData.Data = ByteString.CopyFrom(requestData.ToByteArray());

            socketCommunication.SendMessages(socketData.ToByteArray());
        }

        internal void InvokeSendRunningCameraData()
        {
            InvokeRepeating(nameof(RunningController.SendRunningCameraDataToServer),
                SOCKET_COMMUNICATION_START_DELAY_SECONDS, SEND_CAMERA_DATA_DELAY_SECONDS);
        }

        private void SendRunningCameraDataToServer()
        {
            if (runningUIController.getIsVisible())
            {
                Vector3 position = CameraCache.Main.transform.position;
                Vector3 rotation = CameraCache.Main.transform.rotation.eulerAngles;
                Debug.Log("SendRunningCameraDataToServer:\nposition: " + position + "\nrotation: " + rotation);

                RunningCameraData runningCameraData = new RunningCameraData();
                runningCameraData.Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                runningCameraData.PositionX = position.x;
                runningCameraData.PositionY = position.y;
                runningCameraData.PositionZ = position.z;
                runningCameraData.RotationX = rotation.x;
                runningCameraData.RotationY = rotation.y;
                runningCameraData.RotationZ = rotation.z;

                SocketData socketData = new SocketData();
                socketData.DataType = DataTypes.RUNNING_CAMERA_DATA;
                socketData.Data = ByteString.CopyFrom(runningCameraData.ToByteArray());

                socketCommunication.SendMessages(socketData.ToByteArray());
            }
        }
    }

}
