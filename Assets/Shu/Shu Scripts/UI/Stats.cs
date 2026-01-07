using Inventory.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    public float Health_Max;
    public float Health_Current;

    public float Attack_Speed;
    public float Attack_Power;
    public float Defense;
    public float Crop_Exp; // make sure this starts at 0
    public float Crop_Growth;
    public float Speed;
    [HideInInspector] public float Passive_Attack; // for enemies only, damage you take when you run into them

    private Player_Movement player;
    public Enemy_AI ai;
    public Enemy_AI_Dashing ai_dashing;

    private EnemySpawner spawner;
    private XPController xp;
    private TimeManager tm; //trust me on this :skull:

    [SerializeField] private ItemSO seed1;

    private string mainScene;

    public static bool isDead = false;

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

        //mainScene = SaveData.instance.mainScene;

    }

    public void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainScene)
        {
            spawner = FindFirstObjectByType<EnemySpawner>();
            xp = FindFirstObjectByType<XPController>();
            tm = FindFirstObjectByType<TimeManager>();
        }
        else
        {
            spawner = null;
        }
    }


    void Update()
    {
        bool isEnemy = ai != null || ai_dashing != null;
        bool isPlayer = player != null;
        bool expAwarded = false;

        if (Health_Current <= 0 && isEnemy && !expAwarded)
        {
            foreach (LevelSpawnpoints level in spawner.spawners)
            {
                foreach (EnemySpawnPoint esp in level.spawners)
                {
                    if (esp.ID == 1)
                    {
                        xp.EnemyXp1();
                        tm.tileManager.hbc.inventory.AddItem(seed1, 1);
                    }
                    else if (esp.ID == 2)
                    {
                        xp.EnemyXp2();
                        tm.tileManager.hbc.inventory.AddItem(seed1, 3);
                    }
                }
            }
            
            expAwarded = true;
            Destroy(gameObject);
        }
        if (tm.isDay && isEnemy)
        {
            Destroy(gameObject);
        }
        if (Health_Current <= 0 && isPlayer && !isDead)
        {
            isDead = true;
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
