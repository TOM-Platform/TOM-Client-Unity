using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Rendering;

namespace TOM.Common.UI
{

    public class ProgressIndicatorObject : MonoBehaviour, IProgressIndicatorObject
    {
        [SerializeField] private GameObject progressIndicatorObject = null;
        private IProgressIndicator progressIndicator;

        private void Awake()
        {
            progressIndicator = progressIndicatorObject.GetComponent<IProgressIndicator>();
        }

        public async void StartProgressIndicator(string message)
        {
            while (progressIndicator.State != ProgressIndicatorState.Closed)
            {
                await Task.Delay(200);
            }

            progressIndicator.Message = message;
            await progressIndicator.OpenAsync();
        }

        public void UpdateProgressIndicator(string message)
        {
            progressIndicator.Message = message;
        }

        public async void StopProgressIndicator(string message)
        {
            while (progressIndicator.State != ProgressIndicatorState.Open)
            {
                await Task.Delay(200);
            }

            progressIndicator.Message = message;
            await progressIndicator.CloseAsync();
        }
    }

}
