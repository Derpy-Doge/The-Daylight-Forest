    using UnityEngine;
    using TMPro; 
    using System; 

    public class CountdownTimer : MonoBehaviour
    {
        public float startTime = 60f; 
        public TextMeshProUGUI timerText; 

        private float currentTime;
        private bool timerRunning = false;

        void Start()
        {
            currentTime = startTime;
            timerRunning = true; 
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
                    
                }
                UpdateTimerDisplay();
            }
        }

        void UpdateTimerDisplay()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
            
            timerText.text = string.Format("{1:00}.{2:000}", 
                                           timeSpan.Minutes, 
                                           timeSpan.Seconds, 
                                           timeSpan.Milliseconds);
        }

        
        public void StartCountdown()
        {
            currentTime = startTime;
            timerRunning = true;
        }


        public void StopCountdown()
        {
            timerRunning = false;
        }
    }