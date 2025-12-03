using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    public DirectionHandler dh;
    public SpawnHitBox shb;
    public HealthBar hb;

    public GameObject healthBar;

    public void OnNnightDisable()
    {
        dh.enabled = false;
        shb.enabled = false;
        hb.enabled = false;
        healthBar.SetActive(false);
    }

    public void OnNightEnable()
    {
        dh.enabled = true;
        shb.enabled = true;
        hb.enabled = true;
        healthBar.SetActive(true);
    }
}
