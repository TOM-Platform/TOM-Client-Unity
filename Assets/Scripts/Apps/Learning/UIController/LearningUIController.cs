using TOM.Common.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.Audio;

namespace TOM.Apps.Learning
{

    public class LearningUIController : MonoBehaviour
    {
        public GameObject learningUI;

        public TextMesh panelInstruction;
        public ResizableTextContent panelDetail;
        public TextToSpeech textToSpeech;
        public GameObject selectionPoint;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ResetUI()
        {
            UpdateInstruction("");
            UpdateDetail("");
            HideSelectionPoint();
        }

        public void UpdateInstruction(string instruction)
        {
            panelInstruction.text = instruction;
        }

        public void UpdateDetail(string detail)
        {
            panelDetail.SetText(detail);
        }

        public void StartSpeaking(string text)
        {
            textToSpeech.StopSpeaking();
            textToSpeech.StartSpeaking(text);
        }

        public void StopSpeaking()
        {
            textToSpeech.StopSpeaking();
        }

        public void SetUIVisible(bool isVisible)
        {
            learningUI.SetActive(isVisible);
        }

        public void HideSelectionPoint()
        {
            selectionPoint.SetActive(false);
        }

        public void ShowSelectionPoint(Vector3 position, float displayDuration)
        {
            selectionPoint.SetActive(true);
            selectionPoint.transform.position = position;

            // Unparent the object if it was a child of the camera (to make sure they are positioned in world space, instead of camera)
            selectionPoint.transform.parent = null;

            if (displayDuration > 0)
            {
                StartCoroutine(hideSelectionPointAfter(displayDuration));
            }
        }

        IEnumerator hideSelectionPointAfter(float duration)
        {
            yield return new WaitForSeconds(duration);
            HideSelectionPoint();
        }
    }

}
