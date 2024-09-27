using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using System.Collections.Generic;

namespace TOM.Apps.PandaLens
{
    public class IndexManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject gridObjectCollection;

        void Start()
        {
            UpdateIndexes();
        }

        void Update()
        {
            UpdateIndexes();
        }

        private void UpdateIndexes()
        {
            int idx = 1;
            foreach (Transform child in gridObjectCollection.transform)
            {
                GameObject idxNumberObject = child.Find("IndexNumber/Number").gameObject;
                TextMeshPro idxText = idxNumberObject.GetComponent<TextMeshPro>();
                idxText.SetText(idx.ToString() + ".");
                idx++;
            }
        }
    }
}
