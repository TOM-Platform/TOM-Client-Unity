using UnityEngine;

namespace TOM.Common.UI
{

    public class ProgressIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject progressIndicatorObject;

        public void ShowProgressIndicator(string message)
        {
            progressIndicatorObject.GetComponent<ProgressIndicatorObject>().StartProgressIndicator(message);
        }

        public void UpdateProgressIndicator(string message)
        {
            progressIndicatorObject.GetComponent<ProgressIndicatorObject>().UpdateProgressIndicator(message);
        }

        public void StopProgressIndicator(string message)
        {
            progressIndicatorObject.GetComponent<ProgressIndicatorObject>().StopProgressIndicator(message);
        }
    }

}
