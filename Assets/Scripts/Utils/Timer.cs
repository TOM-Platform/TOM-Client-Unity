using System;
using UnityEngine;

namespace TOM.Common.Utils
{
    public class Timer : MonoBehaviour
    {
        public event Action<float> OnTimerUpdate; // Event triggered every frame with the current time
        public event Action OnTimerStart; // Event triggered when the timer starts
        public event Action OnTimerStop; // Event triggered when the timer stops
        public event Action OnTimerFinish; // Event triggered when the timer finishes

        private float currentTime = 0f;
        private float? duration = null; // Nullable duration

        private bool isCounting = false;
        private bool isPaused = false;

        private void Update()
        {
            if (isCounting && !isPaused)
            {
                currentTime += Time.deltaTime;
                OnTimerUpdate?.Invoke(currentTime);
                if (duration.HasValue && currentTime >= duration.Value)
                {
                    StopTimer();
                    OnTimerFinish?.Invoke();
                }
            }
        }

        public void StartTimer(float? newDuration = null)
        {
            if (!isCounting)
            {
                isCounting = true;
                isPaused = false;
                if (newDuration != null)
                    duration = newDuration;
                OnTimerStart?.Invoke();
            }
        }

        public void StopTimer()
        {
            if (isCounting)
            {
                isCounting = false;
                OnTimerStop?.Invoke();
            }
        }

        public void PauseTimer()
        {
            isPaused = true;
        }

        public void ResumeTimer()
        {
            isPaused = false;
        }

        public void ResetTime()
        {
            currentTime = 0f;
        }

        public void ResetTimerStates()
        {
            currentTime = 0f;
            OnTimerFinish = null;
            OnTimerStart = null;
            OnTimerStop = null;
            OnTimerUpdate = null;
            isCounting = false;
            isPaused = false;
        }

        public void SetDuration(float newDuration)
        {
            duration = newDuration;
        }

        public float GetCurrentTime()
        {
            return currentTime;
        }

        public bool GetIsCounting()
        {
            return isCounting;
        }

        public bool IsOnTimerFinishSubscribed()
        {
            return OnTimerFinish != null;
        }
    }
}
