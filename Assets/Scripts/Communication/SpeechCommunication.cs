using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using TOM.Common.Config;

namespace TOM.Common.Communication
{
    
    public class SpeechCommunication : MonoBehaviour
    {
        public DictationHandler dictationHandler;

        private bool dictationActive = false;

        private bool isSpeechAvailable = false;

        private string speechResult = "";

        //FIXME: REMOVE THIS WHEN ALL TOM SERVICES UTILISE THE GET SET FUNCTION BELOW
        public string SpeechResult
        {
            get
            {
                Debug.Log(speechResult);
                VisualLog.Log(speechResult);
                return this.speechResult;
            }
            set
            {
                this.speechResult = value;
                VisualLog.Log(speechResult);
                Debug.Log(speechResult);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (dictationHandler == null)
            {
                Debug.LogError("DictationHandler not assigned!");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (IsListening() != dictationActive)
            {
                dictationActive = IsListening();
                if (!dictationActive)
                {
                    Debug.Log("Final Speech: " + speechResult);
                    isSpeechAvailable = (speechResult != null && speechResult != "");
                }
            }
        }

        public bool IsListening()
        {
            return dictationHandler.IsListening;
        }

        public void StartListening()
        {
            dictationHandler.StartRecording();
        }

        public void StopListening()
        {
            dictationHandler.StopRecording();
        }

        public bool IsSpeechAvailable()
        {
            return isSpeechAvailable;
        }

        public void ResetSpeechResult()
        {
            this.speechResult = "";
			VisualLog.Log(speechResult);
            this.isSpeechAvailable = false;
        }
    }
    
}
