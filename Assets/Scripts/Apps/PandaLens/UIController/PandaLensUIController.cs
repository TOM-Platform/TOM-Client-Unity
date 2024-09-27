using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.Audio;

// TODO: this entire class can be simplified down to half the amount of lines required if using UniRX
namespace TOM.Apps.PandaLens
{
    public class PandaLensUIController : MonoBehaviour
    {
        [SerializeField]
        private TextToSpeech textToSpeech;
        [SerializeField]
        public GameObject cameraUI;
        [SerializeField]
        private GameObject dialogueUI;
        [SerializeField]
        private GameObject summaryUI;
        
        // Start is called before the first frame update
        void Start()
        {
            ResetUI();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void StartSpeaking(string text)
        {
            textToSpeech.StartSpeaking(text);
        }

        public void StopSpeaking()
        {
            textToSpeech.StopSpeaking();
        }

        public void SetDialogueUIActive(bool active)
        {
            dialogueUI.SetActive(active);
        }

        public void SetSummaryUIActive(bool active)
        {
            summaryUI.SetActive(active);
        }

        public void SetCameraUIActive(bool active)
        {
            cameraUI.SetActive(active);
        }

        public bool IsDialogueUIActive()
        {
            return dialogueUI.activeInHierarchy;
        }

        public bool IsSummaryUIActive()
        {
            return summaryUI.activeInHierarchy;
        }

        public bool IsCameraUIActive()
        {
            return cameraUI.activeInHierarchy;
        }

        public string GetSpeechContent()
        {
            DialogueUI dialogue = dialogueUI.GetComponent<DialogueUI>();
            return dialogue.GetListenTextUI();
        }

        public void ResetUI()
        {
            summaryUI.SetActive(false);
            dialogueUI.SetActive(false);
            cameraUI.SetActive(true);
        }

        public void SetConversationUIActive(bool active)
        {
            SetDialogueUIActive(active);
            SetSummaryUIActive(!active);
            SetCameraUIActive(!active);
        }

        public void SetBlogUIActive(bool active)
        {
            SetDialogueUIActive(!active);
            SetSummaryUIActive(active);
            SetCameraUIActive(!active);
        }

        public void ToggleDialogueUI()
        {
            DialogueUI uiScript = GetComponent<DialogueUI>();
            uiScript.ToggleDialogueUI();
        }

        public void RenderQuestion(string image, string content)
        {
            DialogueUI uiScript = GetComponent<DialogueUI>();
            uiScript.RenderQuestion(image, content);
        }

        public void RenderResponse(string content)
        {
            DialogueUI uiScript = GetComponent<DialogueUI>();
            uiScript.RenderResponse(content);
        }

        public void RenderListeningSpeech()
        {
            DialogueUI uiScript = GetComponent<DialogueUI>();
            uiScript.RenderListeningSpeech();
        }

        public void RenderMomentsList(String[] moments)
        {
            SummaryUI uiScript = GetComponent<SummaryUI>();
            uiScript.RenderMomentsList(moments);
        }

        public void StartLoadingDisplay()
        {
            CameraUI uiScript = GetComponent<CameraUI>();
            uiScript.StartLoadingDisplay();
        }

        public void StopLoadingDisplay()
        {
            CameraUI uiScript = GetComponent<CameraUI>();
            uiScript.StopLoadingDisplay();
        }
    }
}
