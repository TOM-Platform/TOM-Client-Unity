using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOM.Common.Config
{
    public class VisualLog : MonoBehaviour
    {
        public TextMesh DebugText;

        private static string logMessage = null;
        private static bool valuesUpdated = false;

        // Start is called before the first frame update
        void Start()
        {
            Log("");
        }

        // Update is called once per frame
        void Update()
        {
            if (valuesUpdated)
            {
                DebugText.text = logMessage;
                valuesUpdated = false;
            }
        }

        public static void Log(string message)
        {
            logMessage = message;
            valuesUpdated = true;
        }

        public static void DismissLog()
        {
            logMessage = "";
            valuesUpdated = true;
        }
    }
}
