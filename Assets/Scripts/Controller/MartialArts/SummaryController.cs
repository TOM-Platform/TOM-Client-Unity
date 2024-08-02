using UnityEngine;

namespace MartialArts
{
    public class SummaryController : MonoBehaviour
    {
        public SummaryUIController summaryUIController;
        public MartialArtsController maController;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void HideSummary()
        {
            summaryUIController.SetUIVisible(false);
        }

        public void ShowSummary()
        {
            summaryUIController.SetUIVisible(true);
        }

        public void ResetSummary()
        {
            summaryUIController.ResetUI();
        }

        public void SetSummary(MaPostSessionMetrics data)
        {
            summaryUIController.UpdateSummary(data);
        }
    }
}
