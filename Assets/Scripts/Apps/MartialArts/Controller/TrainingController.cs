using TOM.Common.UI;
using TOM.Common.Utils;

using System;
using System.Text;
using UnityEngine;

namespace TOM.Apps.MartialArts
{
    public class TrainingController : MonoBehaviour
    {
        public MartialArtsController maController;
        public TrainingUIController trainingUIController;

        public AudioSource audioSource;

        private float duration; // in minutes
        private float interval; // in seconds

        // Variables related to timer
        public Timer sessionTimer;
        public Timer intervalTimer;
        public Timer reactionTimer;

        private TimeSpan timeSpan = TimeSpan.Zero;

        // Variables related to current sequence
        private SequenceData currentSequence;
        private int currentPunchIdx = 0;
        private StringBuilder sb = new StringBuilder();

        // Variables related to metrics
        private MetricsData metricsData = new MetricsData();

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void BeginSession(float duration, float interval)
        {
            trainingUIController.ResetUI();

            trainingUIController.UpdatePadsDraggable(false);

            maController.SendRequestToServer(DataTypes.MA_BEGIN_SESSION_COMMAND);
            maController.SendRequestToServer(DataTypes.MA_REQUEST_SEQUENCE_DATA);

            trainingUIController.StartSession();

            sessionTimer.ResetTimerStates();
            sessionTimer.OnTimerUpdate += UpdateTimer;
            sessionTimer.OnTimerFinish += EndSession;
            sessionTimer.StartTimer(duration * 60);

            intervalTimer.ResetTimerStates();

            this.duration = duration;
            this.interval = interval;
        }

        private void EndSession()
        {
            maController.OnPressStop();
            sessionTimer.ResetTimerStates();
            trainingUIController.UpdatePadsDraggable(true);
        }

        public void onPadCollisionStart(MaCollisionData collisionData)
        {
            float reactionTime = reactionTimer.GetCurrentTime();
            PunchData currentPunchData = currentSequence.Punches[currentPunchIdx];
            // if the wrong punch was done, do nothing
            if (currentPunchData.Hand != collisionData.Hand)
            {
                return;
            }

            audioSource.Play();

            // send collision data to server
            metricsData.PunchData = currentPunchData;
            metricsData.SequenceData = currentSequence;
            metricsData.CollisionData = collisionData;
            metricsData.ReactionTime = reactionTime;
            maController.SendDataToServer(DataTypes.MA_METRICS_DATA, metricsData);

            if (currentPunchIdx < currentSequence.Punches.Count - 1)
            {
                currentPunchIdx++;
                trainingUIController.SetPadActive(
                    currentSequence.Punches[currentPunchIdx].PunchType
                );
                reactionTimer.ResetTime();
                reactionTimer.StartTimer();
            }
            else
            {
                // reset interval timer and start timer for interval duration
                intervalTimer.ResetTimerStates();
                intervalTimer.StartTimer(interval);

                trainingUIController.HideAllPads();
                maController.SendRequestToServer(DataTypes.MA_REQUEST_SEQUENCE_DATA);
            }
        }

        public void SetLiveFeedback(string liveFeedback)
        {
            trainingUIController.SetFeedbackPanel(liveFeedback);
        }

        public void SetNextSequence(SequenceData sequence)
        {
            // if interval timer is still going when new sequence is received, recursively call this function when interval timer finishes
            if (intervalTimer.GetIsCounting())
            {
                trainingUIController.UpdateSequencePanelText("");
                // prevent multiple subscriptions to the same event
                if (intervalTimer.IsOnTimerFinishSubscribed())
                {
                    return;
                }

                intervalTimer.OnTimerFinish += () => SetNextSequence(sequence);
                return;
            }

            currentSequence = sequence;
            // show first punch in new sequence
            if (currentSequence.Punches.Count == 0)
            {
                return;
            }

            // reset punch index
            currentPunchIdx = 0;

            trainingUIController.UpdateSequencePanelText(GetSequenceString(currentSequence));
            trainingUIController.SetPadActive(currentSequence.Punches[currentPunchIdx].PunchType);
            reactionTimer.ResetTime();
            reactionTimer.StartTimer();
        }

        public void HideTraining()
        {
            trainingUIController.SetUIVisible(false);
            trainingUIController.UpdatePadsDraggable(true);
        }

        public void HidePads()
        {
            trainingUIController.HideAllPads();
        }

        public void ShowPads()
        {
            trainingUIController.ShowDefaultPad();
        }

        public string GetElapsedTime()
        {
            float secondsLeft = sessionTimer.GetCurrentTime();
            TimeSpan timeSpan = TimeSpan.FromSeconds(secondsLeft);
            return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }

        private string GetSequenceString(SequenceData seq)
        {
            sb.Clear();
            foreach (PunchData punch in seq.Punches)
            {
                sb.Append(punch.Name);
                sb.Append(" ");
            }

            return sb.ToString();
        }

        private void UpdateTimer(float elapsedTime)
        {
            if (elapsedTime < 0)
            {
                timeSpan = TimeSpan.FromSeconds(elapsedTime);
            }
            else
            {
                var timeLeftSeconds = duration * 60 - elapsedTime;
                timeSpan = TimeSpan.FromSeconds(timeLeftSeconds);
            }

            trainingUIController.SetTimerString(timeSpan);
        }
    }
}
