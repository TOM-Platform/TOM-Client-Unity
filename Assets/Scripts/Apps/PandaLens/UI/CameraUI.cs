using TOM.Common.UI;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;

namespace TOM.Apps.PandaLens
{
    public class CameraUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject cameraButton;
        [SerializeField]
        private GameObject summaryButton;
        [SerializeField]
        private GameObject progressIndicator;

        // Start is called before the first frame update
        void Start()
        {
            cameraButton.SetActive(true);
            summaryButton.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartLoadingDisplay()
        {
            ProgressLoader progressLoader = progressIndicator.GetComponent<ProgressLoader>();
            progressLoader.StartProgressBehavior();
        }

        public void StopLoadingDisplay()
        {
            ProgressLoader progressLoader = progressIndicator.GetComponent<ProgressLoader>();
            progressLoader.StopProgressBehavior();
        }
    }
}
