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
    public Enemy_AI_Dashing ai_dashing;

    private EnemySpawner spawner;
    private XPController xp;
    private TimeManager tm; //trust me on this :skull:

    void Awake()
    {
        spawner = FindFirstObjectByType<EnemySpawner>();
        xp = FindFirstObjectByType<XPController>();
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
        if (ai_dashing != null)
        {
            Speed = ai_dashing.Emovespeed;
        }
    }

    void Update()
    {
        if (Health_Current <= 0 && ai != null || ai_dashing != null || tm.isDay && ai != null || ai_dashing != null)
        { 
            foreach(EnemySpawnPoint esp in spawner.spawners)
            {
                if (esp.ID == 1)
                {
                    xp.EnemyXp1();
                }
                else if (esp.ID == 2)
                {
                    xp.EnemyXp2();
                }
                else if (esp.ID == 3)
                {
                    xp.EnemyXp3();
                }
            }
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
