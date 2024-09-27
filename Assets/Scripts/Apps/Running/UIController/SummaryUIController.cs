using static TOM.Common.UI.TextContentUnit;
using static TOM.Common.UI.TextContentUnit.TextContentUnitType;
using static TOM.Apps.Running.RunningTypePositonMapping;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOM.Apps.Running
{

    public class SummaryUIController : BaseUIController
    {
        public GameObject summaryUI;

        public GameObject summaryTop;
        public GameObject summaryTopLeft;
        public GameObject summaryTopCenter;
        public GameObject summaryTopRight;
        public GameObject summaryMapImage;
        public GameObject summaryBottomLeftTop;
        public GameObject summaryBottomLeftBottom;
        public GameObject summaryBottomRight;

        private Image summaryMap;

        // Start is called before the first frame update
        void Start()
        {
            summaryMap = summaryMapImage.GetComponentInChildren<Image>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void ResetUI()
        {
            UpdateText(UIPosition.Top, "", Footer);

            UpdateText(UIPosition.TopLeft, "", Content);
            UpdateText(UIPosition.TopCenter, "", Content);
            UpdateText(UIPosition.TopRight, "", Content);
            UpdateText(UIPosition.BottomLeftTop, "", Content);
            UpdateText(UIPosition.BottomLeftBottom, "", Content);

            UpdateText(UIPosition.TopLeft, "", Unit);
            UpdateText(UIPosition.TopCenter, "", Unit);
            UpdateText(UIPosition.TopRight, "", Unit);
            UpdateText(UIPosition.BottomLeftTop, "", Unit);
            UpdateText(UIPosition.BottomLeftBottom, "", Unit);
        }

        protected override GameObject getRootObject()
        {
            return summaryUI;
        }

        protected override GameObject getPanel(UIPosition position)
        {
            switch (position)
            {
                case UIPosition.Top:
                    return summaryTop;
                case UIPosition.TopLeft:
                    return summaryTopLeft;
                case UIPosition.TopCenter:
                    return summaryTopCenter;
                case UIPosition.TopRight:
                    return summaryTopRight;
                case UIPosition.BottomLeftTop:
                    return summaryBottomLeftTop;
                case UIPosition.BottomLeftBottom:
                    return summaryBottomLeftBottom;
                case UIPosition.BottomRight:
                    return summaryBottomRight;
                default:
                    return null;
            }
        }

        public override void UpdateText(UIPosition position, string value, TextContentUnitType contentUnitType)
        {
            base.UpdateText(position, value, contentUnitType);

            if (summaryTopLeft.activeSelf || summaryTopCenter.activeSelf || summaryTopRight.activeSelf)
            {
                summaryTop.SetActive(true);
            }
            else
            {
                summaryTop.SetActive(false);
            }
        }

        internal void UpdateSummaryUI(RunningSummaryData runningSummaryData, string targetContent, string targetFooter,
            bool isUnit)
        {
            Debug.Log("RunningSummaryData:\n" +
                      "isUnit: " + isUnit + "\n" +
                      "Detail: " + runningSummaryData.Detail + "\n" +
                      "Distance: " + runningSummaryData.Distance + "\n" +
                      "Speed: " + runningSummaryData.Speed + "\n" +
                      "Duration: " + runningSummaryData.Duration + "\n" +
                      "Time: " + runningSummaryData.Time);
            if (!isUnit)
            {
                UpdateText(getRunningTypePosition(UIDataType.SummaryDetail), runningSummaryData.Detail, Footer);
                Debug.Log("Map: " + runningSummaryData.Image.ToByteArray().Length + " bytes");
                UpdateMapImage(runningSummaryData.Image.ToByteArray());
            }

            TextContentUnitType contentUnitType = isUnit ? Unit : Content;
            UpdateText(getRunningTypePosition(UIDataType.SummaryDistance), runningSummaryData.Distance,
                contentUnitType);
            UpdateText(getRunningTypePosition(UIDataType.SummarySpeed), runningSummaryData.Speed, contentUnitType);
            UpdateText(getRunningTypePosition(UIDataType.SummaryDuration), runningSummaryData.Duration,
                contentUnitType);
            UpdateText(getRunningTypePosition(UIDataType.SummaryTime), runningSummaryData.Time, contentUnitType);
            if (!isUnit)
            {
                UpdateText(getRunningTypePosition(UIDataType.RunningTarget), $"{targetContent}KM", contentUnitType);
                UpdateText(getRunningTypePosition(UIDataType.RunningTarget), targetFooter, Footer);
            }

        }

        internal void UpdateMapImage(byte[] mapImageBytes)
        {
            try
            {
                // Load the image from the byte data
                Texture2D texture = new Texture2D(1, 1);
                bool isLoaded = texture.LoadImage(mapImageBytes);

                Debug.Log("UpdateMapImage: " + isLoaded);

                if (isLoaded)
                {
                    summaryMap.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                        Vector2.one * 0.5f);
                    Color spriteColor = Color.white;
                    spriteColor.a = 0.6f;
                    summaryMap.color = spriteColor;
                }

            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading map image: " + ex.Message);
                // Handle the exception accordingly (e.g., show an error message, fallback to a default image, etc.)
            }
        }

    }

}
