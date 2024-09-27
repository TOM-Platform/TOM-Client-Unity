using TOM.Common.Communication;

using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace TOM.Apps.MartialArts
{
    public class MartialArtsController : HomeController
    {
        public SocketCommunication socketCommunication;
        public SpeechCommunication speechCommunication;

        public TrainingController trainingController;
        public SetupController setupController;
        public SummaryController summaryController;

        private const string SET_DURATION_COMMAND = "SET_DURATION";
        private const string SET_INTERVAL_COMMAND = "SET_INTERVAL";
        private float DURATION_LOWER_BOUND = 0.5f;
        private int DURATION_UPPER_BOUND = 10;
        private int INTERVAL_LOWER_BOUND = 0;
        private int INTERVAL_UPPER_BOUND = 5;

        public void OnPressStart()
        {
            summaryController.HideSummary();
            SessionConfigData config = setupController.GetCurrentConfig();
            setupController.SendUpdatedConfigToServer();
            setupController.HideSetup();
            trainingController.BeginSession(config.Duration, config.Interval);
        }

        public void OnPressStop()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;
            long unixTimestamp = dateTimeOffset.ToUnixTimeSeconds();
            MaEndSessionData endSessionData = new MaEndSessionData
            {
                Datetime = unixTimestamp,
                SessionDuration = trainingController.GetElapsedTime(),
                IntervalDuration = setupController.GetCurrentConfig().Interval
            };
            SendDataToServer<MaEndSessionData>(DataTypes.MA_END_SESSION_COMMAND, endSessionData);
            setupController.HideSetup();
            trainingController.HidePads();
            trainingController.HideTraining();
            SendRequestToServer(DataTypes.MA_POST_SESSION_FEEDBACK_DATA);

            summaryController.ShowSummary();
        }

        public void SetupSession()
        {
            summaryController.HideSummary();
            trainingController.ShowPads();
            trainingController.HideTraining();
            setupController.ShowSetup();
        }

        // Start is called before the first frame update
        void Start()
        {
            // initial UI shown is setup UI
            SetupSession();
        }

        // Update is called once per frame
        void Update()
        {
            if (socketCommunication.DataReceived())
            {
                List<byte[]> messages = socketCommunication.GetMessages();
                foreach (byte[] message in messages)
                {
                    ProcessSocketData(message);
                }
            }

            handleSpeech();
        }

        private void ProcessSocketData(byte[] socketData)
        {
            bool decodeSucess = ProcessDataBytes(socketData);

            if (!decodeSucess)
            {
                string stringData = System.Text.Encoding.UTF8.GetString(socketData);
                Debug.Log("ProcessDataString: " + stringData);
            }
        }

        // Handle different data types received from server
        private bool ProcessDataBytes(byte[] byteData)
        {
            try
            {
                SocketData socketData = SocketData.Parser.ParseFrom(byteData);

                int dataType = socketData.DataType;
                ByteString data = socketData.Data;
                
                Debug.Log("ProcessDataBytes: " + dataType);

                // TODO: Add more datatypes received from server
                switch (dataType)
                {
                    case DataTypes.MA_CONFIG_DATA:
                        SessionConfigData configData = SessionConfigData.Parser.ParseFrom(data);
                        setupController.LoadConfig(configData);
                        return true;
                    case DataTypes.MA_FEEDBACK_LIVE_DATA:
                        RequestData requestData = RequestData.Parser.ParseFrom(data);
                        trainingController.SetLiveFeedback(requestData.Detail);
                        return true;
                    case DataTypes.MA_SEQUENCE_DATA:
                        SequenceData sequenceData = SequenceData.Parser.ParseFrom(data);
                        trainingController.SetNextSequence(sequenceData);
                        return true;
                    case DataTypes.MA_POST_SESSION_FEEDBACK_DATA:
                        MaPostSessionMetrics postSessionFeedbackData =
                            MaPostSessionMetrics.Parser.ParseFrom(data);
                        summaryController.SetSummary(postSessionFeedbackData);
                        return true;
                    case DataTypes.SPEECH_INPUT_DATA:
                        SpeechData speechData = SpeechData.Parser.ParseFrom(data);
                        handleVoiceCommand(speechData.Voice);
                        return true;
                    default:
                        return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(
                    "Bytes received is not SocketData: "
                        + e.Message
                        + ", "
                        + System.Text.Encoding.UTF8.GetString(byteData)
                );
                return false;
            }
        }

        public void SendDataToServer<T>(int requestType, T data)
            where T : IMessage
        {
            SocketData socketData = new SocketData
            {
                DataType = requestType,
                Data = ByteString.CopyFrom(data.ToByteArray())
            };

            socketCommunication.SendMessages(socketData.ToByteArray());
        }

        public void SendRequestToServer(int requestType, string data = "")
        {
            RequestData requestData = new RequestData { Detail = data };

            SocketData socketData = new SocketData
            {
                DataType = requestType,
                Data = ByteString.CopyFrom(requestData.ToByteArray())
            };

            socketCommunication.SendMessages(socketData.ToByteArray());
        }

        public void StartListening()
        {
            if (speechCommunication == null)
            {
                return;
            }

            speechCommunication.StartListening();
            setupController.ShowConfigPanel();
        }

        private void handleSpeech()
        {
            if (speechCommunication.IsSpeechAvailable())
            {
                sendSpeechData(speechCommunication.SpeechResult);
                speechCommunication.ResetSpeechResult();
            }
        }

        private void sendSpeechData(string voice)
        {
            SpeechData speechData = new SpeechData();
            speechData.Voice = voice;

            SocketData socketData = new SocketData();
            socketData.DataType = DataTypes.SPEECH_INPUT_DATA;
            socketData.Data = ByteString.CopyFrom(speechData.ToByteArray());

            socketCommunication.SendMessages(socketData.ToByteArray());
        }

        private void handleVoiceCommand(string command)
        {
            string[] commandSplit = command.Split(";");
            if (commandSplit.Length < 2)
            {
                return;
            }

            SessionConfigData currentConfig = setupController.GetCurrentConfig();

            switch (commandSplit[0])
            {
                case SET_DURATION_COMMAND:
                    float duration = float.Parse(commandSplit[1]);
                    if (duration < DURATION_LOWER_BOUND || duration > DURATION_UPPER_BOUND)
                    {
                        break;
                    }
                    currentConfig.Duration = duration;
                    break;
                case SET_INTERVAL_COMMAND:
                    int interval = int.Parse(commandSplit[1]);
                    if (interval < INTERVAL_LOWER_BOUND || interval > INTERVAL_UPPER_BOUND)
                    {
                        break;
                    }
                    currentConfig.Interval = interval;
                    break;
            }

            setupController.LoadConfig(currentConfig);
        }
    }
}
