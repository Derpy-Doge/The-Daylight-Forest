using UnityEngine;


public enum TimeState
{
    Paused = 1, //like when its night and bro has to go to forest
    Running = 2, // when he dont got nowhere to be
}

[System.Serializable]
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance { get; set; }
    public event System.Action<TimeState> OnTimeStateChanged;
    public TimeState currentTimeState { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetTimeState(TimeState newState)
    {
        if (currentTimeState != newState)
        {
            currentTimeState = newState;
            OnTimeStateChanged?.Invoke(newState);
        }
    }
}