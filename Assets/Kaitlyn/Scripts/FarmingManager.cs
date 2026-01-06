using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class FarmingManager : MonoBehaviour
{
    private GameObject player;
    private Stats playerStats;
    private TileDataManager tdm;
    private XPController xpc;

    public TileManager tileManager;

    public List<Sprite> seed1GrowthStages;
    public float seed1GrowthTime = 360f;

    public List<Sprite> seed2GrowthStages;
    public float seed2GrowthTime = 720f;

    public string mainScene;

    void Start()
    {
        mainScene = SaveData.instance.mainScene;

        player = this.gameObject;
        playerStats = player.GetComponent<Stats>();
        tdm = FindFirstObjectByType<TileDataManager>();
        tileManager = FindFirstObjectByType<TileManager>();
        xpc = FindFirstObjectByType<XPController>();

        seed1GrowthTime *= playerStats.Crop_Growth;
        seed2GrowthTime *= playerStats.Crop_Growth;

        // subscribe to growth updates
        if (tdm != null)
            tdm.OnTileGrowthUpdated += HandleTileGrowthUpdated;
    }

    private void OnDestroy()
    {
        if (tdm != null)
            tdm.OnTileGrowthUpdated -= HandleTileGrowthUpdated;
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
            tdm = FindFirstObjectByType<TileDataManager>();
            tileManager = FindFirstObjectByType<TileManager>();
            xpc = FindFirstObjectByType<XPController>();
        }
        else
        {
            tdm = null;
            tileManager = null;
            xpc = null;
        }
    }

    public void CanInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0) return;

        if (tileManager.hbc.usingPlant1 && tileManager.hbc.inventory.GetItemAt(tileManager.hbc.SelectedIndex).quantity > 0) 
        {
            tileManager.hbc.UseItem();
            playerStats.Health_Current += 15;
            xpc.PlantXp1();
        }
        else if (tileManager.hbc.usingPlant2 && tileManager.hbc.inventory.GetItemAt(tileManager.hbc.SelectedIndex).quantity > 0) 
        {
            tileManager.hbc.UseItem();
            playerStats.Health_Current += 25;
            xpc.PlantXp2();
        }
        else
        {
            if (TimeManager.instance.tileManager.IsInteractable(player.transform.position))
            {
                TimeManager.instance.tileManager.SetInteracted(player.transform.position);
            }
            else
            {
                Debug.Log("nah twn");
            }
        }       
    }


    // called by TileManager when planting instead of starting a coroutine
    public void PlantSeedAtCell(Vector3Int cellPos, int seedType)
    {
        if (seedType == 0)
            tdm.PlantSeed(cellPos, 0, seed1GrowthTime, seed1GrowthStages.Count);
        else if (seedType == 1)
            tdm.PlantSeed(cellPos, 1, seed2GrowthTime, seed2GrowthStages.Count);
    }

    // event handler to update visual sprites when TileDataManager advances growth
    private void HandleTileGrowthUpdated(Vector3Int pos, TileData data)
    {
        // find the spawned farm sprite for this cell (TileManager keeps farmTileSprites)
        if (tileManager == null) tileManager = FindFirstObjectByType<TileManager>();

        if (!tileManager.farmTileSprites.TryGetValue(pos, out GameObject farmSpriteObj) || farmSpriteObj == null)
            return;

        var sr = farmSpriteObj.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        if (data.SeedType == 0)
        {
            int stage = Mathf.Clamp(data.GrowthStage, 0, seed1GrowthStages.Count - 1);
            sr.sprite = seed1GrowthStages[stage];
        }
        else if (data.SeedType == 1)
        {
            int stage = Mathf.Clamp(data.GrowthStage, 0, seed2GrowthStages.Count - 1);
            sr.sprite = seed2GrowthStages[stage];
        }
    }
}
