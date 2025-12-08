using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Stats stats;

    public Image healthBar;

    void Start()
    {
        
    }

    void Update()
    {
        healthBar.fillAmount = stats.Health_Current / stats.Health_Max;
    }
}
