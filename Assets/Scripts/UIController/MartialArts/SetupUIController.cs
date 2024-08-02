using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using static TextContentUnit;

namespace MartialArts
{
    public class SetupUIController : BaseUIController
    {
        public GameObject setupUI;

        public GameObject configPanel;
        public GameObject instructionPanel;
        public ConfigSlider intervalSlider;
        public ConfigSlider durationSlider;
        public GameObject toggleSettingsOpenButton;

        public GameObject rightHandJabPad;
        public GameObject leftHandJabPad;
        public GameObject rightHandCrossPad;
        public GameObject leftHandCrossPad;
        public GameObject[] pads;
        public Dictionary<GameObject, PadDetails> padToDetailsMap;

        public GameObject rightHandJabButton;
        public GameObject leftHandJabButton;
        public GameObject rightHandCrossButton;
        public GameObject leftHandCrossButton;

        public override void ResetUI()
        {
            this.SetPadVisible(leftHandJabPad);
            configPanel.SetActive(false);
        }

        public SessionConfigData GetConfig()
        {
            SessionConfigData configData = new SessionConfigData
            {
                Duration = durationSlider.GetCurrentValue(),
                Interval = (int)intervalSlider.GetCurrentValue()
            };
            for (int i = 0; i < pads.Length; i++)
            {
                configData.PadConfigs.Add(GetPadConfig(pads[i]));
            }

            return configData;
        }

        private PadConfig GetPadConfig(GameObject pad)
        {
            PadConfig config = new PadConfig();
            PadDetails padDetails = padToDetailsMap[pad];
            config.Handedness = (int)padDetails.handedness;
            config.PunchType = (int)padDetails.punchType;

            MaVector position = new MaVector
            {
                X = pad.transform.position.x,
                Y = pad.transform.position.y,
                Z = pad.transform.position.z
            };
            config.Position = position;

            MaVector rotation = new MaVector
            {
                X = pad.transform.rotation.x,
                Y = pad.transform.rotation.y,
                Z = pad.transform.rotation.z
            };
            config.Rotation = rotation;

            return config;
        }

        public void ToggleConfigPanel()
        {
            configPanel.SetActive(!configPanel.activeSelf);
        }

        public void ShowConfigPanel()
        {
            if (configPanel.activeSelf)
            {
                return;
            }

            configPanel.SetActive(true);
        }

        public void SetConfig(SessionConfigData configData)
        {
            UpdatePadConfigs(configData.PadConfigs.ToArray());
            durationSlider.SetCurrentValue(configData.Duration);
            intervalSlider.SetCurrentValue(configData.Interval);
        }

        public void UpdatePadConfigs(PadConfig[] padConfigs)
        {
            foreach (PadConfig padConfig in padConfigs)
            {
                foreach (GameObject pad in pads)
                {
                    PadDetails padDetails = padToDetailsMap[pad];

                    if (IsSamePad(padConfig, padDetails))
                    {
                        pad.transform.position = new Vector3(
                            padConfig.Position.X,
                            padConfig.Position.Y,
                            padConfig.Position.Z
                        );
                        pad.transform.rotation = new Quaternion(
                            padConfig.Rotation.X,
                            padConfig.Rotation.Y,
                            padConfig.Rotation.Z,
                            pad.transform.rotation.w
                        );
                        break;
                    }
                }
            }
        }

        public void SetPadVisible(GameObject gameObject)
        {
            foreach (GameObject pad in pads)
            {
                if (pad != gameObject)
                {
                    pad.SetActive(false);
                }
                else
                {
                    pad.SetActive(true);
                    instructionPanel
                        .GetComponent<TextContentUnit>()
                        ?.UpdateText("Configuring " + gameObject.name, TextContentUnitType.Content);
                }
            }
        }

        protected override GameObject getRootObject()
        {
            return setupUI;
        }

        // Start is called before the first frame update
        void Start()
        {
            RunInitSequence(InitPads);
            ResetUI();
        }

        // Update is called once per frame
        void Update() { }

        // Runs init functions sequentially, take in variable number of arguments.
        private void RunInitSequence(params Action[] initFuncs)
        {
            foreach (var initFunc in initFuncs)
            {
                initFunc?.Invoke();
            }
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

            padToDetailsMap = new Dictionary<GameObject, PadDetails>();

            padToDetailsMap[leftHandJabPad] = new PadDetails(Handedness.Left, PunchType.LeftJab);
            padToDetailsMap[rightHandJabPad] = new PadDetails(Handedness.Right, PunchType.RightJab);
            padToDetailsMap[leftHandCrossPad] = new PadDetails(
                Handedness.Left,
                PunchType.LeftCross
            );
            padToDetailsMap[rightHandCrossPad] = new PadDetails(
                Handedness.Right,
                PunchType.RightCross
            );
        }

        private bool IsSamePad(PadConfig padConfig, PadDetails padDetails)
        {
            // check same hand or not
            if (
                padConfig.Handedness == (int)padDetails.handedness
                && padConfig.PunchType == (int)padDetails.punchType
            )
            {
                return true;
            }

            return false;
        }
    }

    public struct PadDetails
    {
        public Handedness handedness;
        public PunchType punchType;

        public PadDetails(Handedness handedness, PunchType punchType)
            : this()
        {
            this.handedness = handedness;
            this.punchType = punchType;
        }
    }
}
