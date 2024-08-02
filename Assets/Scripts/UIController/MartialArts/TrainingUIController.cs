using System;
using System.Collections.Generic;
using UnityEngine;
using static TextContentUnit;

namespace MartialArts
{
    public class TrainingUIController : BaseUIController
    {
        public TrainingController trainingController;
        public GameObject trainingUI;

        // UI panels
        public GameObject sequencePanel;
        public GameObject feedbackPanel;
        public GameObject timerPanel;

        // pads
        public GameObject rightHandJabPad;
        public GameObject leftHandJabPad;
        public GameObject rightHandCrossPad;
        public GameObject leftHandCrossPad;
        public GameObject[] pads;
        public Dictionary<PunchType, GameObject> punchTypesToObjMap;

        private Dictionary<int, GameObject> dataTypeToPanelMap;

        // timer
        private TextContentUnit timerText;

        public void onPadCollisionStart(MaCollisionData collisionData)
        {
            if (!trainingUI.activeSelf)
            {
                return;
            }

            trainingController.onPadCollisionStart(collisionData);
        }

        public void onPadCollisionEnd() { }

        public void StartSession()
        {
            // set pads to training mode
            SetAllPadsTrainingMode(true);
            trainingUI.SetActive(true);

            timerText = timerPanel.GetComponent<TextContentUnit>();
        }

        public override void ResetUI()
        {
            foreach (GameObject panel in dataTypeToPanelMap.Values)
            {
                panel.GetComponent<TextContentUnit>()?.UpdateText("", TextContentUnitType.Content);
            }
        }

        protected override GameObject getRootObject()
        {
            return trainingUI;
        }

        internal void UpdatePanelContent(int dataType, string newContent)
        {
            GameObject panel = dataTypeToPanelMap.GetValueOrDefault(dataType, null);

            if (panel == null)
            {
                Debug.Log($"[UpdatePanelContent] No such panel for data type: {dataType}");
                return;
            }

            panel
                .GetComponent<TextContentUnit>()
                ?.UpdateText(newContent, TextContentUnitType.Content);
        }

        // Start is called before the first frame update
        void Start()
        {
            RunInitSequence(InitPanelMap, InitPads);

            ResetUI();
        }

        // Update is called once per frame
        void Update()
        {
            if (!trainingUI.activeSelf)
            {
                return;
            }
        }

        public void SetFeedbackPanel(string liveFeedback)
        {
            feedbackPanel
                .GetComponent<TextContentUnit>()
                .UpdateText(liveFeedback, TextContentUnitType.Content);
        }

        public void UpdateSequencePanelText(string newText)
        {
            // set sequence panel for new sequence
            sequencePanel
                .GetComponent<TextContentUnit>()
                .UpdateText(newText, TextContentUnitType.Content);
        }

        public void SetPadActive(PunchType punchType)
        {
            GameObject nextPad = punchTypesToObjMap.GetValueOrDefault(punchType, null);
            foreach (GameObject pad in pads)
            {
                if (pad != nextPad)
                {
                    pad.SetActive(false);
                }
                else
                {
                    pad.SetActive(true);
                }
            }
        }

        public void ShowDefaultPad()
        {
            if (pads.Length > 0)
            {
                pads[0].SetActive(true);
            }
        }

        public void HideAllPads()
        {
            foreach (GameObject pad in pads)
            {
                pad.SetActive(false);
            }
        }

        public void UpdatePadsDraggable(bool draggable)
        {
            foreach (GameObject pad in pads)
            {
                pad.GetComponent<Pad>().SetIsTrainingPad(!draggable);
            }
        }

        private void SetAllPadsTrainingMode(bool isTrainingMode)
        {
            foreach (GameObject pad in pads)
            {
                Pad padComponent = pad.GetComponent<Pad>();
                if (padComponent != null)
                {
                    padComponent.isTrainingPad = isTrainingMode;
                }
            }
        }

        // Runs init functions sequentially, take in variable number of arguments.
        private void RunInitSequence(params Action[] initFuncs)
        {
            foreach (var initFunc in initFuncs)
            {
                initFunc?.Invoke();
            }
        }

        private void InitPanelMap()
        {
            dataTypeToPanelMap = new Dictionary<int, GameObject>
            {
                { DataTypes.MA_SEQUENCE_DATA, sequencePanel },
                { DataTypes.MA_FEEDBACK_LIVE_DATA, feedbackPanel }
            };
        }

        private void InitPads()
        {
            pads = new GameObject[]
            {
                leftHandJabPad,
                rightHandJabPad,
                leftHandCrossPad,
                rightHandCrossPad,
            };

            // init Dict for fast lookup of pads
            punchTypesToObjMap = new Dictionary<PunchType, GameObject>();
            punchTypesToObjMap[PunchType.LeftCross] = leftHandCrossPad;
            punchTypesToObjMap[PunchType.RightCross] = rightHandCrossPad;
            punchTypesToObjMap[PunchType.LeftJab] = leftHandJabPad;
            punchTypesToObjMap[PunchType.RightJab] = rightHandJabPad;

            foreach (GameObject obj in pads)
            {
                Pad pad = obj.GetComponent<Pad>();
                if (pad == null)
                {
                    continue;
                }

                // add collision delegates for all pads
                pad.onCollisionStart = onPadCollisionStart;
                pad.onCollisionEnd = onPadCollisionEnd;
            }
        }

        public void SetTimerString(TimeSpan timeSpan)
        {
            if (!timerText)
            {
                return;
            }

            timerText.UpdateText(
                string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds),
                TextContentUnitType.Content
            );
        }
    }
}
