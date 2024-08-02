using UnityEngine;

namespace MartialArts
{
    public class SummaryUIController : BaseUIController
    {
        SummaryController summaryController;
        public GameObject summaryUI;

        public TextContentUnit totalPunches;
        public TextContentUnit correctPunches;
        public TextContentUnit offTargetPunches;
        public TextContentUnit badAnglePunches;

        public TextContentUnit duration;
        public TextContentUnit avgReactionTime;

        public override void ResetUI()
        {
            totalPunches.UpdateText("0", TextContentUnit.TextContentUnitType.Content);
            correctPunches.UpdateText("0", TextContentUnit.TextContentUnitType.Content);
            offTargetPunches.UpdateText("0", TextContentUnit.TextContentUnitType.Content);
            badAnglePunches.UpdateText("0", TextContentUnit.TextContentUnitType.Content);

            duration.UpdateText("0", TextContentUnit.TextContentUnitType.Content);
            avgReactionTime.UpdateText("0", TextContentUnit.TextContentUnitType.Content);
        }

        public void UpdateSummary(MaPostSessionMetrics postSessionMetrics)
        {
            totalPunches.UpdateText(
                postSessionMetrics.TotalPunches.ToString(),
                TextContentUnit.TextContentUnitType.Content
            );
            correctPunches.UpdateText(
                postSessionMetrics.CorrectPunches.ToString(),
                TextContentUnit.TextContentUnitType.Content
            );
            offTargetPunches.UpdateText(
                postSessionMetrics.OffTargetPunches.ToString(),
                TextContentUnit.TextContentUnitType.Content
            );
            badAnglePunches.UpdateText(
                postSessionMetrics.BadAnglePunches.ToString(),
                TextContentUnit.TextContentUnitType.Content
            );

            duration.UpdateText(
                postSessionMetrics.SessionDuration,
                TextContentUnit.TextContentUnitType.Content
            );

            // round avg reaction time to 2 decimal places
            postSessionMetrics.AvgReactionTime =
                Mathf.Round(postSessionMetrics.AvgReactionTime * 100f) / 100f;
            avgReactionTime.UpdateText(
                postSessionMetrics.AvgReactionTime.ToString() + "s",
                TextContentUnit.TextContentUnitType.Content
            );
        }

        protected override GameObject getRootObject()
        {
            return summaryUI;
        }
    }
}
