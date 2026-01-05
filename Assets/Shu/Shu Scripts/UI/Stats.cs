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
    [SerializeField] private ItemSO seed2;

    private string mainScene;

    void Awake()
    {
        spawner = FindFirstObjectByType<EnemySpawner>();
        xp = FindFirstObjectByType<XPController>();
        player = GetComponent<Player_Movement>();
        tm = FindFirstObjectByType<TimeManager>();
        mainScene = SaveData.instance.mainScene;
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

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainScene)
        {
            spawner = FindFirstObjectByType<EnemySpawner>();
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
                        tm.tileManager.hbc.inventory.AddItem(seed2, 1);
                    }
                    else if (esp.ID == 3)
                    {
                        xp.EnemyXp3();
                        tm.tileManager.hbc.inventory.AddItem(seed2, 2);
                    }
                }
            }
            
            expAwarded = true;
            Destroy(gameObject);
        }
        if (Health_Current <= 0 && isPlayer)
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
