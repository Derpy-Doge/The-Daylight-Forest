using UnityEngine;

public class Hazard : MonoBehaviour
{
    public Stats enemyStats;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Stats playerStats))
        {
            float calculatedDamage = enemyStats.Attack_Power - playerStats.Defense;
            playerStats.Health_Current -= calculatedDamage;
            Debug.Log("Player hit a hazard! Damage taken: " + calculatedDamage);
        }
    }
}
