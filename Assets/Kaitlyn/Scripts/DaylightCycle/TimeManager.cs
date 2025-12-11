using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// if days go from 7am to 8pm(or 20:00) itll be abt 6 and a half minutes of farm play time

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    private int minutes;
    public int Minutes
    {
        get { return minutes; } 
        set { minutes = value; OnMinuteChange(value); }
    }

    private int hours;
    public int Hours
    {
        get { return hours; }
        set { hours = value; OnHourChange(value); }
    }

    private int days;
    public int Days
    {
        get { return days; }
        set { days = value; }
    }

    private float tempSeconds;

    public Gradient gradientNightToSunrise;
    public Gradient gradientSunriseToDay;
    public Gradient gradientDayToSunset;
    public Gradient gradientSunsetToNight;
    public Light globalLight;

    private float currentTime;
    public float dayDuration = 720; // minutes? idek atp...

    public bool isDay;
    public bool isNight;

    public GameStateManager gsm;
    public TimeStateChange tsc;

    public PlayerAttackManager pam;

    private GameObject fog;

    public GameObject[] dialogueBoxes;

    private AudioSource BGM;
    public AudioClip dayBGM;
    public AudioClip nightBGM;

    public GameObject saveData;
    public SaveData sd;
    private string mainScene;
    private string mainMenu;

    public TileManager tileManager; // this is pretty much a game manager atp :pensive:


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

        BGM = FindFirstObjectByType<AudioSource>();
        saveData = GameObject.FindWithTag("SaveData");
        sd = saveData.GetComponent<SaveData>();
        mainMenu = sd.mainMenu;
        mainScene = sd.mainScene;

        if (isDay && globalLight != null) // hopefully doing this stops me from having to do a seperate cycle for time on the farm and time in the forest :sob:
        {

            Time.timeScale = 1f; // __ times faster than rl seconds... didnt end up using it cause like i cant do math :skull:

            globalLight.intensity = .75f;
            hours = 7;
            globalLight.colorTemperature = 2981f;
            globalLight.color = new Color(1f, 0.8039216f, 0.627451f); // sunset color...
                                                                      //start an hour after sunrise
            BGM.clip = dayBGM;
            BGM.Play();
            isNight = false;
        }
        else if (isNight && globalLight != null)
        {

            Time.timeScale = 1f; ;

            globalLight.intensity = .4f;
            hours = 21; // i keep almost forgetting to do 24 hr time :sob:
            globalLight.colorTemperature = 15000f;
            globalLight.color = new Color(0.6862745f, 0.8117647f, 0.9058824f); // night color
            BGM.clip = nightBGM;
            BGM.Play();
            isDay = false;
        }
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainScene)
        {
            globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light>();
            fog = GameObject.FindWithTag("Fog"); // make sure all fog is childed to one thing with the fog tag and the individual fogs dont have the tag
            tileManager = GetComponent<TileManager>();
        }
        else if (scene.name == mainMenu)
        {
            Destroy(this.gameObject);
            globalLight = null;
        }
    }

    void Update()
    {


        tempSeconds = Time.deltaTime + tempSeconds;
        if(tempSeconds >= .15f) // the seconds in each minute, resets each minute
        {
            minutes ++;
            tempSeconds = 0;
            //Debug.Log ("holy gleebus it works"); // i didnt think I was allowed to put swears in here :skull:
        }

        if(globalLight != null)
        {
            OnMinuteChange(minutes);
            OnHourChange(hours);
        }

        currentTime = Time.deltaTime + currentTime;

        float rotationAngle = ((currentTime / 720f) * 360f);

        if(globalLight != null)
        {
            globalLight.transform.rotation = Quaternion.Euler(50f, rotationAngle, 0f);
        }

        if(minutes == 1 && hours == 6 || minutes == 1 && hours == 20)
        {
            dialogueBoxes[4].GetComponent<Dialogue>().StartDialogue(); // make "Turning Dialogue" 5th in the array
        }

        if (isNight)
        {
            if (pam != null)
                pam.OnNightEnable();
            if (fog != null)
                fog.GetComponentInChildren<Collider>().enabled = false;
        }
        else if (isDay)
        {
            if (pam != null)
                pam.OnDayDisable();
            if (fog != null)
                fog.GetComponentInChildren<Collider>().enabled = true;
        }

        if(hours == 21 && minutes == 0 && isDay)
        {
            dialogueBoxes[0].GetComponents<Dialogue>()[0].StartDialogue(); // its mad ineffecient but pls make sure "night dialogue" is first in the array 
        }
        else if(hours == 7 && minutes == 0 && isNight)
        {
            dialogueBoxes[2].GetComponent<Dialogue>().StartDialogue(); // make "morning dialogue" 3rd in the array
        }

        if(hours == 7 && minutes == 0 && days > 0)
        {
            sd.SavePlayerData();
            Debug.Log("Daily Auto Save!");
        }
    }

    private void OnMinuteChange(int value)
    {
        if(globalLight != null)
            globalLight.transform.Rotate(Vector3.up, (1f / dayDuration) * 1f, Space.World);
        if(value >= 60) //after 60 minutes add 1 hour and reset minutes after 24 hours add 1 day and reset hours, days never reset
        {
            hours++;
            minutes = 0;
        }
        if(hours >= 24)
        {
            days++;
            hours = 0;
            currentTime = 0;
        }
    }

    private void OnHourChange(int value)
    {
        if( value == 6) //sunrise
        {
            StartCoroutine(LerpLight(gradientNightToSunrise, 5f));
            StartCoroutine(FadeLightIntesity(.75f, 2981f, 5f));
            if(dayBGM != null)
            {
                BGM.clip = dayBGM;
                BGM.Play();
            }
        }
        else if (value == 7 && isNight) // when youre locked from doing stuff caus you gotta go back to the farm
        {
            isDay = true;
            isNight = false;
        }
        else if (value == 8) //day
        {
            StartCoroutine(LerpLight(gradientSunriseToDay, 5f));
            StartCoroutine(FadeLightIntesity(1f, 5000, 5f));
        }
        else if (value == 18) //sunset
        {
            StartCoroutine(LerpLight(gradientDayToSunset, 5f));
            StartCoroutine(FadeLightIntesity(.75f, 2981f, 5f));
            if(nightBGM != null)
            {
                BGM.clip = nightBGM;
                BGM.Play();
            }
        }
        else if (value == 20) //night
        {
            StartCoroutine(LerpLight(gradientSunsetToNight, 5f));
            StartCoroutine(FadeLightIntesity(.4f, 15000f, 5f));
        }
        else if (value == 21 && isDay) // when youre locked from doing stuff cause you gotta go to the forest (copilot replicated my spelling error im gonna cry :wilted_rose:)
        {
            isNight = true;
            isDay = false;
        }
    }

    private IEnumerator LerpLight(Gradient lightGradient , float time) //fade light
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            if (globalLight == null) yield break;
            globalLight.color = lightGradient.Evaluate(i / time);
            yield return null;
        }
    }

    private IEnumerator FadeLightIntesity(float endIntensity, float endTemp,  float duration) //name says it all :man_juggling:
    {
        float timer = 0f;
        float startIntensity =  globalLight.intensity;
        float startTemp = globalLight.colorTemperature;

        while(timer < duration)
        {
            if (globalLight == null) yield break;
            timer += Time.deltaTime;
            float t = timer / duration;
            globalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
            globalLight.colorTemperature = Mathf.Lerp(startTemp, endTemp, t);
            yield return null;
        }
        globalLight.intensity = endIntensity;
        globalLight.colorTemperature = endTemp;
    }
}
