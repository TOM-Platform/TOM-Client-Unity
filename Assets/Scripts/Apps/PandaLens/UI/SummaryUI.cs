using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

namespace TOM.Apps.PandaLens
{
    public class SummaryUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject checklistPanel;
        [SerializeField]
        private GameObject microphoneButton;
        [SerializeField]
        private GameObject escapeButton;
        [SerializeField]
        private GameObject rowPrefab;
        [SerializeField]
        private ScrollingObjectCollection scrollingObjectCollection;
        [SerializeField]
        private GridObjectCollection gridObjectCollection;

        // Start is called before the first frame update
        void Start()
        {
            microphoneButton.SetActive(true);
            checklistPanel.SetActive(true);
            escapeButton.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (!checklistPanel.activeInHierarchy)
            {
                foreach(Transform child in gridObjectCollection.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void RenderMomentsList(string[] moments)
        {
            foreach (string moment in moments)
            {
                GameObject newRow = Instantiate(rowPrefab, gridObjectCollection.transform);
                // Configure the row (e.g., set the text)
                Transform summary = newRow.transform.Find("SummaryNote/Note");
                summary.GetComponent<TextMeshPro>().SetText(moment);
            }
            gridObjectCollection.UpdateCollection();
            scrollingObjectCollection.UpdateContent();
        }
    }
}
