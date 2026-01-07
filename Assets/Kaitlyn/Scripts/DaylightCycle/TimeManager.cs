using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float dayDuration = 360;

    public bool isDay;
    public bool isNight;

    public PlayerAttackManager pam;
    private GameObject player;
    private Player_Movement playerMovement;
    

    private GameObject fog;
    private List<GameObject> fogs = new List<GameObject>();

    private EnemySpawner enemySpawner;

    private GameObject dialogueParent;
    private List<GameObject> dialogueBoxes = new List<GameObject>();

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
        mainScene = "updated map";
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<Player_Movement>();
        fog = GameObject.FindWithTag("Fog"); // make sure all fog is childed to one thing with the fog tag and the individual fogs dont have the tag
        foreach (Transform child in fog.transform)
        {
            fogs.Add(child.gameObject);
        }
        dialogueParent = GameObject.FindWithTag("DialogueBoxes");
        foreach (Transform child in dialogueParent.transform)
        {
            dialogueBoxes.Add(child.gameObject);
        }
        BGM = FindFirstObjectByType<AudioSource>();
        //saveData = GameObject.FindWithTag("SaveData");
        //sd = saveData.GetComponent<SaveData>();

        if (isDay && globalLight != null)
        {
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
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainScene)
        {
            globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light>();
            fog = GameObject.FindWithTag("Fog"); // make sure all fog is childed to one thing with the fog tag and the individual fogs dont have the tag
            foreach(Transform child in fog.transform)
            {
                fogs.Add(child.gameObject);
            }
            tileManager = GetComponent<TileManager>();
            dialogueParent = GameObject.FindWithTag("DialogueBoxes");
            foreach(Transform child in dialogueParent.transform)
            {
                dialogueBoxes.Add(child.gameObject);
            }

            if (saveData == null)
            {
                saveData = GameObject.FindWithTag("SaveData");
                if (saveData != null)
                    sd = saveData.GetComponent<SaveData>();
            }

            if (sd != null && sd.playerData != null && !SaveData.firstLoad)
            {
                hours = sd.playerData.hours;
                minutes = sd.playerData.minutes;
                days = sd.playerData.days;

                float dayFraction = (hours * 60f + minutes) / (24f * 60f);
                currentTime = dayFraction * dayDuration;

                SetLightingToTime();

                if (hours >= 7 && hours < 21)
                {
                    isDay = true;
                    isNight = false;
                    BGM.clip = dayBGM;
                    BGM.Play();
                }
                else
                {
                    isNight = true;
                    isDay = false;
                    BGM.clip = nightBGM;
                    BGM.Play();
                }
            }

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
        if(tempSeconds >= .125f) // the seconds in each minute, resets each minute
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

        float rotationAngle = ((currentTime / 360f) * 360f);

        if(globalLight != null)
        {
            globalLight.transform.rotation = Quaternion.Euler(50f, rotationAngle, 0f);
        }

        if (minutes == 15 && hours == 7 && days == 0)
        {
            dialogueBoxes[3].GetComponent<Dialogue>().StartDialogue();
        }


        if (minutes == 1 && hours == 6 || minutes == 1 && hours == 20)
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

        }
        else if (value == 7 && isNight) // when youre locked from doing stuff caus you gotta go back to the farm
        {
            isDay = true;
            StartCoroutine(WaitForPoofEnd());
            isNight = false;

            if (dayBGM != null)
            {
                BGM.clip = dayBGM;
                BGM.Play();
            }
        }
        else if (value == 8) //day
        {
            StartCoroutine(LerpLight(gradientSunriseToDay, 5f));
            StartCoroutine(FadeLightIntesity(1f, 5000, 5f));
        }
        else if(value == 12)
        {
            fogs.ForEach(fogPart =>
            {
                fogPart.GetComponent<Collider>().enabled = true;
            });
        }
        else if (value == 18) //sunset
        {
            StartCoroutine(LerpLight(gradientDayToSunset, 5f));
            StartCoroutine(FadeLightIntesity(.75f, 2981f, 5f));
        }
        else if (value == 20) //night
        {
            StartCoroutine(LerpLight(gradientSunsetToNight, 5f));
            StartCoroutine(FadeLightIntesity(.4f, 15000f, 5f));
            if (nightBGM != null)
            {
                BGM.clip = nightBGM;
                BGM.Play();
            }
        }
        else if (value == 21 && isDay) // when youre locked from doing stuff cause you gotta go to the forest (copilot replicated my spelling error im gonna cry :wilted_rose:)
        {
            StartCoroutine(WaitForTransformationEnd());
            isNight = true;
            isDay = false;

            if (days == 0)
            {
                dialogueBoxes[1].GetComponent<Dialogue>().StartDialogue();
            }

            fogs.ForEach(fogPart =>
            {
                fogPart.GetComponent<Collider>().enabled = false;
            });
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left fog trigger");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered fog trigger");
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

    
    public IEnumerator WaitForTransformationEnd(float timeout = 2f)
    {
        if (playerMovement?.myAnimator == null)
            yield break;

        var animator = playerMovement.myAnimator;

        animator.SetBool("isTransforming", true);
        playerMovement.enabled = false;
        animator.Update(0f);

        int transformHash = Animator.StringToHash("Transform");

        // Stage 1: wait for Transform to start (or transition to it) up to timeout
        float timer = 0f;
        bool started = false;
        while (timer < timeout)
        {
            var current = animator.GetCurrentAnimatorStateInfo(0);
            var next = animator.GetNextAnimatorStateInfo(0);

            if (current.shortNameHash == transformHash)
            {
                started = true;
                break;
            }
            // If we're in transition and the next state is Transform, treat as started
            if (animator.IsInTransition(0) && next.shortNameHash == transformHash)
            {
                started = true;
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!started)
        {
            // Transform never started within timeout -> cleanup and exit
            animator.SetBool("isTransforming", false);
            playerMovement.enabled = true;
            yield break;
        }

        // Stage 2: wait for Transform to finish
        timer = 0f;
        while (timer < timeout)
        {
            var current = animator.GetCurrentAnimatorStateInfo(0);

            if (current.shortNameHash != transformHash && !animator.IsInTransition(0))
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isTransforming", false);
        playerMovement.enabled = true;
        animator.SetBool("Iswolf", true);
    }

    public IEnumerator WaitForPoofEnd(float timeout = 2f)
    {
        if (playerMovement?.myAnimator == null)
            yield break;

        var animator = playerMovement.myAnimator;

        animator.SetBool("isUntransforming", true);
        playerMovement.enabled = false;
        animator.Update(0f);

        int transformHash = Animator.StringToHash("Poof");

        float timer = 0f;
        bool started = false;
        while (timer < timeout)
        {
            var current = animator.GetCurrentAnimatorStateInfo(0);
            var next = animator.GetNextAnimatorStateInfo(0);

            if (current.shortNameHash == transformHash)
            {
                started = true;
                break;
            }
            
            if (animator.IsInTransition(0) && next.shortNameHash == transformHash)
            {
                started = true;
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!started)
        {
            animator.SetBool("isUntransforming", false);
            playerMovement.enabled = true;
            yield break;
        }

        timer = 0f;
        while (timer < timeout)
        {
            var current = animator.GetCurrentAnimatorStateInfo(0);

            if (current.shortNameHash != transformHash && !animator.IsInTransition(0))
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isUntransforming", false);
        playerMovement.enabled = true;
        animator.SetBool("Iswolf", false);
    }

    public IEnumerator WaitForAnimationEnd(string animationName, string animationBool, float timeout = 1f) // for tools
    {
        if (playerMovement?.myAnimator == null)
            yield break;

        var animator = playerMovement.myAnimator;

        animator.SetBool(animationBool, true);
        playerMovement.enabled = false;
        animator.Update(0f);

        int transformHash = Animator.StringToHash(animationName);

        float timer = 0f;
        bool started = false;
        while (timer < timeout)
        {
            var current = animator.GetCurrentAnimatorStateInfo(0);
            var next = animator.GetNextAnimatorStateInfo(0);

            if (current.shortNameHash == transformHash)
            {
                started = true;
                break;
            }
            
            if (animator.IsInTransition(0) && next.shortNameHash == transformHash)
            {
                started = true;
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!started)
        {
            animator.SetBool(animationBool, false);
            playerMovement.enabled = true;
            yield break;
        }

        timer = 0f;
        while (timer < timeout)
        {
            var current = animator.GetCurrentAnimatorStateInfo(0);

            if (current.shortNameHash != transformHash && !animator.IsInTransition(0))
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetBool(animationBool, false);
        playerMovement.enabled = true;
    }

    private void SetLightingToTime()
    {
        if (globalLight == null) return;

        float rotationAngle = ((currentTime / dayDuration) * 360f);
        globalLight.transform.rotation = Quaternion.Euler(50f, rotationAngle, 0f);

        // Adjust light color and intensity based on time of day
        if (hours >= 6 && hours < 8) // Sunrise
        {
            globalLight.intensity = .75f;
            globalLight.colorTemperature = 2981f;
            globalLight.color = new Color(1f, 0.8039216f, 0.627451f);
        }
        else if (hours >= 8 && hours < 18) // Day
        {
            globalLight.intensity = 1f;
            globalLight.colorTemperature = 5000f;
            globalLight.color = new Color(1f, 0.9568627f, 0.8392157f);
        }
        else if (hours >= 18 && hours < 20) // Sunset
        {
            globalLight.intensity = .75f;
            globalLight.colorTemperature = 2981f;
            globalLight.color = new Color(1f, 0.8039216f, 0.627451f);
        }
        else // Night
        {
            globalLight.intensity = .4f;
            globalLight.colorTemperature = 15000f;
            globalLight.color = new Color(0.6862745f, 0.8117647f, 0.9058824f);
        }
    }
}
