using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class XPController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private int Level;
    public float CurrentXp;
    [SerializeField] private float TargetXp;
    [SerializeField] private Image XpProgressBar;




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentXp += 25;
        }


        ExperienceController();
    }

    public void ExperienceController()
    {
        LevelText.text = "Level : " + Level.ToString();
        XpProgressBar.fillAmount = (CurrentXp / TargetXp);

        if(CurrentXp >= TargetXp)
        {
            CurrentXp = CurrentXp - TargetXp;
            Level++;
            TargetXp *= (float)1.25;
        }
    }

    public void EnemyXp1()
    {
        CurrentXp += 10;
    }
    public void EnemyXp2()
    {
        CurrentXp += 25;
    }
    public void EnemyXp3()
    {
        CurrentXp += 50;
    }
}
