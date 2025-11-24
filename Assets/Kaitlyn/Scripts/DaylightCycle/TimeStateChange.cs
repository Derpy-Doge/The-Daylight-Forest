using UnityEngine;

// WHY DID I EVEN HAVE TO MAKE THIS ITS OWN SCRIPT

[System.Serializable]
[RequireComponent(typeof(TimeManager))]
public class TimeStateChange : MonoBehaviour 
{
    public TimeManager tm;
    public GameStateManager gsm;

    public void OnEnable()
    {
        gsm.OnTimeStateChanged += HandleTimeStateChange;
    }

    public void OnDisable() // lowk dont even think ill need this one
    {
        gsm.OnTimeStateChanged -= HandleTimeStateChange;
    }

    private void HandleTimeStateChange(TimeState newState)
    {
        if (newState == TimeState.Paused)
        {
            Debug.Log("u lowk gotta do othr stf rn twn");
            tm.enabled = false;
        }
        else if (newState == TimeState.Running)
        {
            Debug.Log("im so sry fr th othr 1 gng... :skull:"); // holy abomination of speech pattern :sob: thankfull i dont talk like that :man_juggling:
            tm.enabled = true;
        }
        //WHY IS COPILOT PREDICTING MY NONEXISTENT EMOJIS
    }
}
