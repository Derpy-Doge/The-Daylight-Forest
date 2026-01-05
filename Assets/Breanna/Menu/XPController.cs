using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XPController : MonoBehaviour
{

    private TextMeshProUGUI LevelText;
    private TextMeshProUGUI SkillText;
    public int Level;
    public int Skill;
    public float CurrentXp;
    [SerializeField] private float TargetXp;
    [SerializeField] private Image XpProgressBar;
    [SerializeField] private GameObject xpBar;

    public float Enemy1;
    public float Enemy2;    
    public float Enemy3;

    public float Plant1;
    public float Plant2;

    private SaveData sd;
    private string mainScene;

    public static XPController instance;

    void Start()
    {
        sd = FindFirstObjectByType<SaveData>();
        mainScene = sd.mainScene;
        xpBar = GameObject.FindGameObjectWithTag("EXPBar");
        XpProgressBar = xpBar.GetComponent<Image>();
        LevelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextMeshProUGUI>();
        SkillText = GameObject.FindGameObjectWithTag("SkillPointText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        ExperienceController();
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
            xpBar = GameObject.FindGameObjectWithTag("EXPBar");
            XpProgressBar = xpBar.GetComponent<Image>();
            LevelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextMeshProUGUI>();
           SkillText = GameObject.FindGameObjectWithTag("SkillPointText").GetComponent<TextMeshProUGUI>();
        }
        else if (scene.name != mainScene)
        {
            xpBar = null;
            XpProgressBar = null;
            LevelText = null;
            SkillText = null;
        }
    }

    public void ExperienceController()
    {
        if(LevelText == null || SkillText == null)
        {
            return;
        }

        LevelText.text = "Level : " + Level.ToString();
        SkillText.text = "Skill Points : " + Skill.ToString();
        XpProgressBar.fillAmount = (CurrentXp / TargetXp);

        if(CurrentXp >= TargetXp)
        {
            CurrentXp = CurrentXp - TargetXp;
            Level++;
            Skill++;
            TargetXp *= (float)1.25;
        }
    }

    public void SpendXp()
    {
        if(Skill <= 0)
        {
            return;
        }
        Skill -= 1;
    }

    public void EnemyXp1()
    {
        CurrentXp += Enemy1;
    }
    public void EnemyXp2()
    {
        CurrentXp += Enemy2;
    }
    public void EnemyXp3()
    {
        CurrentXp += Enemy3;
    }

    public void PlantXp1()
    {
        CurrentXp += Plant1;
    }
    public void PlantXp2()
    {
        CurrentXp += Plant2;
    }
}
