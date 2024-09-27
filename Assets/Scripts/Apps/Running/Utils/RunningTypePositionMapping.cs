using System.Collections.Generic;
using UnityEngine;

namespace TOM.Apps.Running
{

    internal class RunningTypePositonMapping
    {
        private static IDictionary<UIDataType, UIPosition> runningTypePositionMap = null;

        internal static void setDefaultRunningTypePositionMapping()
        {
            if (runningTypePositionMap == null)
            {
                runningTypePositionMap = new Dictionary<UIDataType, UIPosition>()
                {
                    { UIDataType.RunningSpeed, UIPosition.TopLeft },
                    { UIDataType.RunningCalories, UIPosition.TopCenter },
                    { UIDataType.RunningHeartRate, UIPosition.TopRight },
                    { UIDataType.RunningTarget, UIPosition.BottomLeftTop },
                    { UIDataType.RunningTime, UIPosition.BottomLeftBottom },
                    { UIDataType.SummaryDetail, UIPosition.Top },
                    { UIDataType.SummaryDistance, UIPosition.TopLeft },
                    { UIDataType.SummarySpeed, UIPosition.TopCenter },
                    { UIDataType.SummaryDuration, UIPosition.TopRight },
                    { UIDataType.SummaryTime, UIPosition.BottomLeftBottom },
                    { UIDataType.SelectionSpeed, UIPosition.SelectionLeft },
                    { UIDataType.SelectionDistance, UIPosition.SelectionRight },
                };
            }
        }

        internal static UIPosition getRunningTypePosition(UIDataType type)
        {
            if (!runningTypePositionMap.ContainsKey(type))
            {
                Debug.LogError("Unknown UIPosition mapping for UIDataType: " + type);
                return UIPosition.Unsupported;
            }

            return runningTypePositionMap[type];
        }

        internal static void UpdateRunningUIPositionMapping(RunningTypePositionMappingData runningTypePositionMapping)
        {
            Debug.Log("TypePositionMappingData:\n" +
                      "RunningDistancePosition: " + runningTypePositionMapping.RunningDistancePosition + "\n" +
                      "RunningHeartRatePosition: " + runningTypePositionMapping.RunningHeartRatePosition + "\n" +
                      "RunningCaloriesPosition: " + runningTypePositionMapping.RunningCaloriesPosition + "\n" +
                      "RunningSpeedPosition: " + runningTypePositionMapping.RunningSpeedPosition + "\n" +
                      "RunningDurationPosition: " + runningTypePositionMapping.RunningDurationPosition + "\n" +
                      "RunningTimePosition: " + runningTypePositionMapping.RunningTimePosition + "\n" +

                      "SummaryDetailPosition: " + runningTypePositionMapping.SummaryDetailPosition + "\n" +
                      "SummaryDistancePosition: " + runningTypePositionMapping.SummaryDistancePosition + "\n" +
                      "SummarySpeedPosition: " + runningTypePositionMapping.SummarySpeedPosition + "\n" +
                      "SummaryDurationPosition: " + runningTypePositionMapping.SummaryDurationPosition + "\n" +

                      "SelectionSpeedPosition: " + runningTypePositionMapping.SelectionSpeedPosition + "\n" +
                      "SelectionDistancePosition: " + runningTypePositionMapping.SummaryDistancePosition + "\n");

            updateRunningTypePosition(UIDataType.RunningDistance, runningTypePositionMapping.RunningDistancePosition);
            updateRunningTypePosition(UIDataType.RunningHeartRate, runningTypePositionMapping.RunningHeartRatePosition);
            updateRunningTypePosition(UIDataType.RunningCalories, runningTypePositionMapping.RunningCaloriesPosition);
            updateRunningTypePosition(UIDataType.RunningSpeed, runningTypePositionMapping.RunningSpeedPosition);
            updateRunningTypePosition(UIDataType.RunningDuration, runningTypePositionMapping.RunningDurationPosition);
            updateRunningTypePosition(UIDataType.RunningTime, runningTypePositionMapping.RunningTimePosition);

            updateRunningTypePosition(UIDataType.SummaryDetail, runningTypePositionMapping.SummaryDetailPosition);
            updateRunningTypePosition(UIDataType.SummaryDistance, runningTypePositionMapping.SummaryDistancePosition);
            updateRunningTypePosition(UIDataType.SummarySpeed, runningTypePositionMapping.SummarySpeedPosition);
            updateRunningTypePosition(UIDataType.SummaryDuration, runningTypePositionMapping.SummaryDurationPosition);

            updateRunningTypePosition(UIDataType.SelectionSpeed, runningTypePositionMapping.SelectionSpeedPosition);
            updateRunningTypePosition(UIDataType.SelectionDistance,
                runningTypePositionMapping.SelectionDistancePosition);
        }

        internal static void updateRunningTypePosition(UIDataType uiDataType, int position)
        {
            if (!UIPosition.IsDefined(typeof(UIPosition), position))
            {
                Debug.LogError("Update failed:: unknown position: " + position);
                return;
            }

            UIPosition uiPosition = (UIPosition)position;
            if (!runningTypePositionMap.ContainsKey(uiDataType))
            {
                Debug.Log("Adding new UIDataType: " + uiDataType);
                runningTypePositionMap.Add(uiDataType, uiPosition);
            }
            else
            {
                runningTypePositionMap[uiDataType] = uiPosition;
            }
        }
    }

}
