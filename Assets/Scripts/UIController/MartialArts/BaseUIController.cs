using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartialArts
{
    public abstract class BaseUIController : MonoBehaviour
    {
        /*private const string whiteColor = "#FFFFFF";
        private const string defaultBackpanelColor = "#00000014";*/

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void SetUIVisible(bool isVisible)
        {
            GameObject mainUI = getRootObject();
            if (mainUI == null)
            {
                Debug.LogError("Unknown main UI");
                return;
            }

            mainUI.SetActive(isVisible);
        }

        public bool getIsVisible()
        {
            GameObject mainUI = getRootObject();
            if (mainUI == null)
            {
                Debug.LogError("Unknown main UI");
                return false;
            }

            return mainUI.activeSelf;
        }

        public abstract void ResetUI();
        protected abstract GameObject getRootObject();
    }
}
