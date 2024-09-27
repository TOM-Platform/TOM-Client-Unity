using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.Audio;

namespace TOM.Apps.PandaLens
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField]
        private RawImage rawImage;
        [SerializeField]
        private TextMeshProUGUI imageQuestionUI;
        [SerializeField]
        private TextMeshProUGUI responseUI;
        [SerializeField]
        private TextMeshProUGUI listenUI;
        [SerializeField]
        private GameObject microphoneButton;
        [SerializeField]
        private GameObject escapeButton;
        [SerializeField]
        private GameObject dialoguePage;
        [SerializeField]
        private GameObject dialogueType;
        [SerializeField]
        private GameObject notificationButton;


        // Start is called before the first frame update
        void Start()
        {
            notificationButton.SetActive(true);
            dialoguePage.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SetNotificationButtonActive(bool active)
        {
            notificationButton.SetActive(active);
        }

        private void SetDialogueActive(bool active)
        {
            dialoguePage.SetActive(active);
        }

        public void RenderQuestion(string image, string content)
        {
            //put image into raw image
            Texture2D texture = new Texture2D(1, 1);
            byte[] imageBytes = Convert.FromBase64String(image);
            texture.LoadImage(imageBytes);
            rawImage.texture = texture;
            //Put Text into textbox
            imageQuestionUI.SetText(content);
            SetDialogueActive(false);
            SetNotificationButtonActive(true);
            SelectDialogueUI(PandaLensConfig.PANDALENS_QUESTION_UI_INDEX);
        }

        public void RenderResponse(string content)
        {
            responseUI.SetText(content);
            SetDialogueActive(true);
            SetNotificationButtonActive(false);
            SelectDialogueUI(PandaLensConfig.PANDALENS_RESPONSE_UI_INDEX);
        }

        public void RenderListeningSpeech(string speech="")
        {
            listenUI.SetText(speech);
            SetDialogueActive(true);
            SetNotificationButtonActive(false);
            SelectDialogueUI(PandaLensConfig.PANDALENS_LISTEN_UI_INDEX);
        }

        private void SelectDialogueUI(int index)
        {
            UnselectAllDialogue();
            dialogueType.transform.GetChild(index).gameObject.SetActive(true);
        }

        private void UnselectAllDialogue()
        {
            foreach(Transform child in dialogueType.transform) {
                child.gameObject.SetActive(false);
            }
        }

        public void ToggleDialogueUI()
        {
            if (dialogueType.activeInHierarchy) 
            {
                SetDialogueActive(false);
                SetNotificationButtonActive(true);
            }
            else
            {
                SetDialogueActive(true);
                SetNotificationButtonActive(false);
            }
        }

        public string GetListenTextUI()
        {
            return listenUI.text;
        }
    }
}
