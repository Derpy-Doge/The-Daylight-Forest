    using UnityEngine;
    using TMPro; // Important for TextMeshPro
    using System; // For TimeSpan

    public class CountdownTimer : MonoBehaviour
    {
        public float startTime = 60f; // Set your desired countdown duration in seconds
        public TextMeshProUGUI timerText; // Assign your TextMeshPro Text component here

        private float currentTime;
        private bool timerRunning = false;

        void Start()
        {
            currentTime = startTime;
            timerRunning = true; // Start the timer automatically
            UpdateTimerDisplay();
        }

        void Update()
        {
            if (timerRunning)
            {
                currentTime -= Time.deltaTime;

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    timerRunning = false;
                    Debug.Log("Countdown Finished!");
                    // Add any actions to perform when the countdown ends
                }
                UpdateTimerDisplay();
            }
        }

        void UpdateTimerDisplay()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
            // Format for seconds and milliseconds (f3 for three decimal places of milliseconds)
            timerText.text = string.Format("{1:00}.{2:000}", 
                                           timeSpan.Minutes, 
                                           timeSpan.Seconds, 
                                           timeSpan.Milliseconds);
        }

        // Optional: Method to start/reset the timer
        public void StartCountdown()
        {
            currentTime = startTime;
            timerRunning = true;
        }

        // Optional: Method to stop the timer
        public void StopCountdown()
        {
            timerRunning = false;
        }
    }