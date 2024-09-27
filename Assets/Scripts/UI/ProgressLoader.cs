using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.Audio;
using Microsoft.MixedReality.Toolkit.UI;

namespace TOM.Common.UI
{
    
    public class ProgressLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject progressIndicatorObject;
        [SerializeField]
        private GameObject RotatingOrbs;
        private bool startedProgressBehavior = false;
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (startedProgressBehavior)
            {
                RotatingOrbs.SetActive(true);
            }
            else
            {
                RotatingOrbs.SetActive(false);
            }
        }

        public async void StartProgressBehavior()
        {
            if (startedProgressBehavior)
            {
                Debug.Log("Can't start until behavior is completed.");
                return;
            }
            startedProgressBehavior = true;

            IProgressIndicator indicator = progressIndicatorObject.GetComponent<IProgressIndicator>();
            await indicator.OpenAsync();
        }

        public async void StopProgressBehavior()
        {
            if (!startedProgressBehavior)
            {
                Debug.Log("Can't stop behavior unless it is started.");
                return;
            }
            startedProgressBehavior = false;

            IProgressIndicator indicator = progressIndicatorObject.GetComponent<IProgressIndicator>();
            await indicator.CloseAsync();
        }
    }

}
