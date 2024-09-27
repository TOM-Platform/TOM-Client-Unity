using TOM.Common.Communication;
using TOM.Common.Utils;

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


namespace TOM.Apps.Template
{

    public class TemplateController : HomeController
    {
        public SocketCommunication socketCommunication;

        public TemplateUIController templateUIController;

        private const float TEMPLATE_REQUEST_GAP_SECONDS = 5f;

        // Start is called before the first frame update
        void Start()
        {
            templateUIController.ResetUI();
            InvokeRepeating(nameof(sendTemplateRequestToServer), 2.0f, TEMPLATE_REQUEST_GAP_SECONDS);
        }



        // Update is called once per frame
        void Update()
        {
            handleCommunication();
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

                if (dataType == DataTypes.TEMPLATE_DATA)
                {
                    try
                    {
                        TemplateData templateData = TemplateData.Parser.ParseFrom(data);
                        UpdateTemplateUI(dataType, templateData);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Bytes received cannot be decoded as TemplateData: " + e.Message);
                        return false;
                    }

                }
                else
                {
                    Debug.LogError("Datatype is not supported: " + dataType);
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Bytes received are not SocketData: " + e.Message + ", " +
                               System.Text.Encoding.UTF8.GetString(byteData));
                return false;
            }
        }

        private void UpdateTemplateUI(int dataType, TemplateData templateData)
        {
            Debug.Log("TemplateData:\n" +
                      "dataType: " + dataType + "\n" +
                      "text_message: " + templateData.TextMessage + "\n" +
                      "image: " + templateData.Image.ToByteArray() + "\n" +
                      "audio_path: " + templateData.AudioPath + "\n");

            templateUIController.UpdateText(templateData.TextMessage);
            templateUIController.SetImage(templateData.Image.ToByteArray());
            templateUIController.PlayAudio(templateData.AudioPath);
        }

        private void sendTemplateRequestToServer()
        {
            sendRequestToServer(DataTypes.REQUEST_TEMPLATE_DATA);
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

    }

}
