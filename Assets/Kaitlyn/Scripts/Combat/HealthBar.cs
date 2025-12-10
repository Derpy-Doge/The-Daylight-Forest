using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Stats stats;
    private GameObject player;

    public Image healthBar;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<Stats>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        healthBar.fillAmount = stats.Health_Current / stats.Health_Max;
    }
}
