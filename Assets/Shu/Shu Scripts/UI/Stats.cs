using UnityEngine;
using UnityEngine.SceneManagement;

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
    [HideInInspector] public float Passive_Attack; // for enemies only, damage you take when you run into them

    private Player_Movement player;
    public Enemy_AI ai;

    private TimeManager tm; //trust me on this :skull:

    void Awake()
    {
        player = GetComponent<Player_Movement>();
        tm = FindFirstObjectByType<TimeManager>();
    }

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
        if (Health_Current <= 0 && ai != null)
        {
            Destroy(gameObject);
        }
        if (Health_Current <= 0 && player != null)
        {
            SceneManager.LoadScene("Game-Over");
            if (tm != null)
            {
                tm.enabled = false;
            }
        }


        if (Health_Current > Health_Max)
        {
            Health_Current = Health_Max;
        }

        Passive_Attack = Attack_Power * 0.5f;
    }
}
