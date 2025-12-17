using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;
    private static bool firstLoad = true;

    public string mainMenu = "Start Menu";
    public string mainScene;

    private GameObject player;
    private XPController XPController;
    private Stats playerStats;
    private SpawnHitBox attackStats;
    private Player_Movement moveSpeed; // not sure if ill actually need this one yet or if i can just use the stat from stat script
    private Vector3 playerPosition;

    private TimeManager tm;

    [SerializeField] public PlayerData playerData;
    private string savePath;

    private Button saveButton; // trust me on this
    private GameObject saveButtonObject;
    private GameObject pauseMenu;

    void Awake()
    {

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        savePath = Path.Combine(Application.persistentDataPath, "Player_data.json");
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
        if(scene.name == mainScene)
        {
            if (firstLoad)
            {
                pauseMenu = GameObject.FindWithTag("PauseMenu");
                saveButtonObject = GameObject.FindWithTag("SaveButton");
                saveButton = saveButtonObject.GetComponent<Button>();
                SaveButton();
                pauseMenu.SetActive(false);

                player = GameObject.FindWithTag("Player");
                XPController = FindFirstObjectByType<XPController>();
                playerStats = player.GetComponent<Stats>();
                attackStats = player.GetComponent<SpawnHitBox>();
                moveSpeed = player.GetComponent<Player_Movement>();

                tm = FindFirstObjectByType<TimeManager>();

                SavePlayerData(); // making it so that you cant use svae data from an old run by immidiately quitting to menu and reloading :man_juggling:
                firstLoad = false;
            }
            else if (!firstLoad)
            {
                pauseMenu = GameObject.FindWithTag("PauseMenu");
                saveButtonObject = GameObject.FindWithTag("SaveButton");
                saveButton = saveButtonObject.GetComponent<Button>();
                SaveButton();
                pauseMenu.SetActive(false);

                player = GameObject.FindWithTag("Player");
                playerStats = player.GetComponent<Stats>();
                attackStats = player.GetComponent<SpawnHitBox>();
                moveSpeed = player.GetComponent<Player_Movement>();

                tm = FindFirstObjectByType<TimeManager>();

                LoadPlayerData();
            }



        }
        else if (scene.name == mainMenu)
        {
            pauseMenu = null;
            saveButtonObject = null;
            saveButton = null;

            player = null;
            playerStats = null;
            attackStats = null;
            moveSpeed = null;

            tm = null;
        }
        else if (scene.name == "Game-Over")
        {
            pauseMenu = null;
            saveButtonObject = null;
            saveButton = null;
            player = null;
            playerStats = null;
            attackStats = null;
            moveSpeed = null;
            tm = null;
        }
        else if (scene.name == "Skill_Tree")
        {
            pauseMenu = null;
            saveButtonObject = null;
            saveButton = null;

            player = GameObject.FindWithTag("Player");
            playerStats = player.GetComponent<Stats>();
            attackStats = player.GetComponent<SpawnHitBox>();
            moveSpeed = player.GetComponent<Player_Movement>();
        }
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(savePath, json);

        #region stats
        playerData.health = playerStats.Health_Current;
        playerData.exp = XPController.CurrentXp;
        playerData.attackSpeed = playerStats.Attack_Speed;
        playerData.attackPower = playerStats.Attack_Power;
        playerData.defense = playerStats.Defense;
        playerData.cropExp = playerStats.Crop_Exp;
        playerData.cropGrowth = playerStats.Crop_Growth;
        playerData.speed = playerStats.Speed;

        playerData.slashCooldown = attackStats.slashCooldown;
        playerData.slashRange = attackStats.slashRange;

        playerData.stunRadius = attackStats.stunRadius;
        playerData.stunDuration = attackStats.stunDuration;
        playerData.stunCooldown = attackStats.stunCooldown;

        playerData.knockbackRadius = attackStats.knockbackRadius;
        playerData.knockbackForce = attackStats.knockbackForce;
        playerData.knckbackCooldown = attackStats.knckbackCooldown;
        if(SceneManager.GetActiveScene().name != "Skill_Tree")
        {
            playerData.playerPosition = player.transform.position;
        }

        if(tm!= null)
        {
            playerData.minutes = tm.Minutes;
            playerData.hours = tm.Hours;
            playerData.days = tm.Days;

        }
        #endregion

        Debug.Log("Player Data Saved to " + savePath);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, playerData);

            #region stats
            playerStats.Health_Current = playerData.health;
            XPController.CurrentXp = playerData.exp;
            playerStats.Attack_Speed = playerData.attackSpeed;
            playerStats.Attack_Power = playerData.attackPower;
            playerStats.Defense = playerData.defense;
            playerStats.Crop_Exp = playerData.cropExp;
            playerStats.Crop_Growth = playerData.cropGrowth;
            playerStats.Speed = playerData.speed;

            attackStats.slashCooldown = playerData.slashCooldown;
            attackStats.slashRange = playerData.slashRange;

            attackStats.stunRadius = playerData.stunRadius;
            attackStats.stunDuration = playerData.stunDuration;
            attackStats.stunCooldown = playerData.stunCooldown;

            attackStats.knockbackRadius = playerData.knockbackRadius;
            attackStats.knockbackForce = playerData.knockbackForce;
            attackStats.knckbackCooldown = playerData.knckbackCooldown;
            player.transform.position = playerData.playerPosition;

            tm.Minutes = playerData.minutes;
            tm.Hours = playerData.hours;
            tm.Days = playerData.days;
            #endregion

            Debug.Log("Player Data Loaded from " + savePath);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + savePath);
        }
    }

    public void SaveButton()
    {
        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(SavePlayerData);
    }
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData") ]
public class PlayerData : ScriptableObject
{


    public float health;
    public float exp;
    public float attackSpeed;
    public float attackPower;
    public float defense;
    public float cropExp;
    public float cropGrowth;
    public float speed;

    public float slashRange;
    public float slashCooldown;

    public float stunRadius;
    public float stunDuration;
    public float stunCooldown;

    public float knockbackRadius;
    public float knockbackForce;
    public float knckbackCooldown;

    public Vector3 playerPosition;

    public int minutes;
    public int hours;
    public int days;
}
