using Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAttackManager : MonoBehaviour
{
    private SaveData sd;
    private string mainScene;
    private string mainMenu;

    private SpawnHitBox shb;
    private HealthBar hb;

    private FarmingManager fm;
    private InventoryController ic;

    private GameObject healthBar;
    private GameObject hotbar;

    private GameObject player;
    private PlayerInput playerInput;

    public void Awake()
    {
        sd = GameObject.FindGameObjectWithTag("SaveData").GetComponent<SaveData>();
        mainMenu = sd.mainMenu;
        mainScene = sd.mainScene;

        player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        shb = player.GetComponent<SpawnHitBox>();
        healthBar = FindFirstObjectByType<HealthBar>().gameObject;
        hb = healthBar.GetComponent<HealthBar>();
        hotbar = GameObject.FindWithTag("Hotbar");  
        fm = player.GetComponent<FarmingManager>();
        ic = player.GetComponent<InventoryController>();
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
            player = GameObject.FindWithTag("Player");
            playerInput = player.GetComponent<PlayerInput>();
            shb = player.GetComponent<SpawnHitBox>();
            healthBar = FindFirstObjectByType<HealthBar>().gameObject;
            hb = healthBar.GetComponent<HealthBar>();
            hotbar = GameObject.FindWithTag("Hotbar");
        }
        else if (scene.name == mainMenu)
        {
            player = null;
            playerInput = null;
            shb = null;
            healthBar = null;
            hb = null;
            hotbar = null;
        }
        else
        {
            fm.enabled = false;
            ic.enabled = false;
            hotbar.SetActive(false);
        }
    }

    public void Start()
    {

    }

    public void OnDayDisable()
    {
        if (SceneManager.GetActiveScene().name == mainScene)
        {
            fm.enabled = true;
            ic.enabled = true;
            hotbar.SetActive(true);


            shb.enabled = false;
            hb.enabled = false;
            healthBar.SetActive(false);

            playerInput.actions.FindAction("Slash").Disable();
            playerInput.actions.FindAction("Stun").Disable();
            playerInput.actions.FindAction("Knockback").Disable();
        }
        else
        {
            return;
        }
    }

    public void OnNightEnable()
    {
        if (SceneManager.GetActiveScene().name == mainScene)
        {
            fm.enabled = false;
            ic.enabled = false;
            hotbar.SetActive(false);


            shb.enabled = true;
            hb.enabled = true;
            healthBar.SetActive(true);

            playerInput.actions.FindAction("Slash").Enable();
            playerInput.actions.FindAction("Stun").Enable();
            playerInput.actions.FindAction("Knockback").Enable();
        }
        else
        {
            return;
        }      
    }
}
