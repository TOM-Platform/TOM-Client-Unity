using TOM.Common.Communication;

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

namespace TOM.Common.UI
{

    public class SpeechButtonControl : MonoBehaviour
    {
        public SpeechCommunication speechCommunication;
        public AudioClip recording_start, recording_stop;

        private bool listening = false;
        private bool buttonClick = false;
        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

            if (speechCommunication.IsListening() != listening)
            {
                listening = speechCommunication.IsListening();
                audioSource.PlayOneShot(listening ? recording_start : recording_stop, 0.6f);
            }

            if (buttonClick)
            {
                buttonClick = false;
                handleButtonClick();
            }
        }

        public void ClickButton()
        {
            buttonClick = true;
        }

        private void handleButtonClick()
        {
            Debug.Log("handleButtonClick");
            try
            {
                if (speechCommunication.IsListening())
                {
                    speechCommunication.StopListening();
                }
                else
                {
                    speechCommunication.StartListening();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Dictation failed: " + e.Message);
            }
        }
        
    }

}
