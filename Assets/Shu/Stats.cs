using UnityEngine;

public class Stats : MonoBehaviour
{
    public float Health_Max;
    [HideInInspector] public float Health_Current;

    public float Attack_Speed;
    public float Attack_Power;
    public float Defense;
    public float Crop_Exp;
    public float Crop_Growth;
    public float Speed;

    public Player_Movement player;
    public Enemy_AI ai;

    void Start()
    {
        Health_Current = Health_Max;

        if (player != null)
        {
            Speed = player.movespeed;
        }
        if (ai != null)
        {
            Speed = ai.movespeed;
        }
    }

    void Update()
    {
        if (Health_Current <= 0)
        {
            Destroy(gameObject);
        }

        if (Health_Current > Health_Max)
        {
            Health_Current = Health_Max;
        }
    }
}
