using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class SpeechCommunication : MonoBehaviour
{
    public DictationHandler dictationHandler;
    
    private bool dictationActive = false;

    private bool isSpeechAvailable = false;

    private string speechResult = "";
    public string SpeechResult
    {
        get
        {
            isSpeechAvailable = false;
            string result = speechResult.Trim();
            speechResult = "";
            VisualLog.Log(speechResult);
            return result;
        }
        set
        {
            speechResult = value;
            VisualLog.Log(speechResult);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

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

}
