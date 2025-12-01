using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 60f;
    public TextMeshProUGUI timerText;

    private float currentTime;
    private bool timerActive = false;

    void Start()
    {
        currentTime = startTime;
        timerActive = true; 
    }

    void Update()
    {
        if (timerActive)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                currentTime = 0;   
                timerActive = false;
                UpdateTimerDisplay();
                Debug.Log("Countdown Finished!");
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        currentTime = startTime;
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        timerActive = false;
        UpdateTimerDisplay();
    }
}
