using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class XPController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI LevelText;
<<<<<<< HEAD
    [SerializeField] private TextMeshProUGUI SkillText;
    [SerializeField] private TextMeshProUGUI ExperienceText;
=======
>>>>>>> 5d19b04da71b4e98666fc82cd1987850cf8d3f90
    [SerializeField] private int Level;
    [SerializeField] private int Skill;
    public float CurrentXp;
    [SerializeField] private float TargetXp;
    [SerializeField] private Image XpProgressBar;

    public float Enemy1;
    public float Enemy2;    
    public float Enemy3;
    public float SpendXP;
   
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentXp += 100;
        }


        ExperienceController();
    }

    public void ExperienceController()
    {
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
}
