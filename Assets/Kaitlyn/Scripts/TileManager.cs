using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public string mainScene;

    public Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile; 

    [SerializeField] private Tile PlowedTile;
    [SerializeField] private Tile WateredTile;

    [SerializeField] private TileDataManager tdm;
    public HotbarController hbc;
    private FarmingManager fm;

    public GameObject farmSprite;
    private Transform spriteParent;

    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private List<InventoryItem> Plants; // 0 for seed1, 1 for seed2
    public static bool plant2Unlocked = false;

    private TimeManager tm;

    public Dictionary<Vector3Int, GameObject> farmTileSprites = new Dictionary<Vector3Int, GameObject>();

    private void Awake()
    {
        GameObject hotbar = GameObject.FindGameObjectWithTag("Hotbar");
        hbc = hotbar.GetComponent<HotbarController>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        fm = player.GetComponent<FarmingManager>();

        var parentGo = GameObject.FindGameObjectWithTag("FarmSpriteParent");
        spriteParent = parentGo != null ? parentGo.transform : null;

        // Prefer inspector-assigned prefab. fallback to any scene object with tag "FarmSprite"
        if (farmSprite == null)
        {
            var prefabGo = GameObject.FindGameObjectWithTag("FarmSprite");
            if (prefabGo != null)
                farmSprite = prefabGo;
        }

        tm = FindFirstObjectByType<TimeManager>();
        tdm = FindFirstObjectByType<TileDataManager>();
        if (tdm != null)
        {
            // avoid duplicate subscription
            tdm.OnTileGrowthUpdated -= HandleTileGrowthUpdated;
            tdm.OnTileGrowthUpdated += HandleTileGrowthUpdated;
        }
    }

    void Start()
    {
        mainScene = SaveData.instance.mainScene;
        interactableMap = GameObject.FindGameObjectWithTag("InteractableMap").GetComponent<Tilemap>();

        // Initialize TileData entries for visible interactable tiles if missing,
        // but don't overwrite existing persistent data in tdm.
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible")
            {
                interactableMap.SetTile(position, hiddenInteractableTile);

                var existing = tdm?.GetData(position);
                if (existing == null)
                {
                    var td = new TileData();
                    td.tileType = hiddenInteractableTile.name;
                    tdm?.SetData(position, td);
                }
                else
                {
                    if (string.IsNullOrEmpty(existing.tileType))
                    {
                        existing.tileType = hiddenInteractableTile.name;
                        tdm?.SetData(position, existing);
                    }
                }
            }
        }

        // Drain any growth updates that were enqueued while resources were not ready.
        DrainPendingGrowthUpdates();
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
        if (scene.name == mainScene && !SaveData.firstLoad)
        {
            interactableMap = GameObject.FindGameObjectWithTag("InteractableMap").GetComponent<Tilemap>();

            GameObject hotbar = GameObject.FindGameObjectWithTag("Hotbar");
            hbc = hotbar.GetComponent<HotbarController>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            fm = player.GetComponent<FarmingManager>();

            var parentGo = GameObject.FindGameObjectWithTag("FarmSpriteParent");
            spriteParent = parentGo != null ? parentGo.transform : null;
            var farmPrefabObj = GameObject.FindGameObjectWithTag("FarmSprite");
            if (farmSprite == null && farmPrefabObj != null)
                farmSprite = farmPrefabObj;

            tm = FindFirstObjectByType<TimeManager>();
            tdm = FindFirstObjectByType<TileDataManager>();

            if (tdm != null)
            {
                tdm.OnTileGrowthUpdated -= HandleTileGrowthUpdated;
                tdm.OnTileGrowthUpdated += HandleTileGrowthUpdated;
            }

            // Restore tilemap tile assets using saved tileType and recreate farm sprites for planted tiles
            if (tdm != null && interactableMap != null)
            {
                foreach (var kv in tdm.tileData)
                {
                    var pos = kv.Key;
                    var data = kv.Value;

                    // Restore tile asset from saved tileType
                    TileBase tileToSet = null;
                    if (!string.IsNullOrEmpty(data.tileType))
                    {
                        if (data.tileType == PlowedTile?.name) tileToSet = PlowedTile;
                        else if (data.tileType == WateredTile?.name) tileToSet = WateredTile;
                        else if (data.tileType == hiddenInteractableTile?.name) tileToSet = hiddenInteractableTile;
                        // If you have more tiles, add checks here or expose a lookup in TileDataManager
                    }

                    if (tileToSet != null)
                    {
                        interactableMap.SetTile(pos, tileToSet);
                        Debug.Log($"Restored tile at {pos} -> {tileToSet.name}");
                    }

                    // Recreate farm sprite for planted tiles (and set the right growth stage via handler)
                    if (data != null && data.IsPlanted)
                    {
                        if (!farmTileSprites.ContainsKey(pos))
                        {
                            Vector3 worldPos = interactableMap.GetCellCenterWorld(pos);
                            if (farmSprite == null)
                            {
                                Debug.LogWarning("TileManager: farmSprite prefab is null. Assign it in inspector.");
                            }
                            else
                            {
                                GameObject instance = Instantiate(farmSprite, worldPos, Quaternion.identity);
                                if (spriteParent != null) instance.transform.SetParent(spriteParent, true);
                                farmTileSprites[pos] = instance;
                            }
                        }

                        // update sprite to correct stage
                        HandleTileGrowthUpdated(pos, data);
                    }
                }
            }
        }
        else if (scene.name == mainScene && SaveData.firstLoad)
        {
            return;
        }
        else
        {
            return;
        }
    }

    public bool IsInteractable(Vector3 position)
    {
        Vector3Int cellPos = interactableMap.WorldToCell(position);
        TileBase tile = interactableMap.GetTile(cellPos);
        var data = tdm?.GetData(cellPos);

        if (tile != null)
        {
            if(tile == hiddenInteractableTile && hbc.usingPlow)
            {
                return true;
            }
            else if(tile == PlowedTile && (hbc.usingSeed1 || (hbc.usingSeed2 && plant2Unlocked)))
            {
                return true;
            }
            else if((data != null && data.SeedsPlanted[0] && hbc.usingWateringCan) || (data != null && data.SeedsPlanted[1] && hbc.usingWateringCan))
            {
                return true;
            }
            else if(data != null && data.SeedsPlanted[2] && hbc.handsEmpty) 
            {
                Debug.Log("plus ultra");
                return true;
            }
        }

        Debug.Log("you either need a tool or youre using the wrong tool... idiot");
        return false;
    }

    public void SetInteracted(Vector3 position)
    {
        Vector3Int cellPos = interactableMap.WorldToCell(position);
        TileBase tile = interactableMap.GetTile(cellPos);

        if (hbc == null)
            return;

        // PLOW
        if (hbc.usingPlow)
        {
            tm.StartCoroutine(tm.WaitForAnimationEnd("Plow", "isPlowing"));
            interactableMap.SetTile(cellPos, PlowedTile);

            var dataPlow = tdm?.GetData(cellPos) ?? new TileData();
            dataPlow.tileType = PlowedTile.name;
            tdm?.SetData(cellPos, dataPlow);
            return;
        }

        // WATERING CAN
        if (hbc.usingWateringCan)
        {
            tm.StartCoroutine(tm.WaitForAnimationEnd("Water", "isWatering"));
            interactableMap.SetTile(cellPos, WateredTile);

            var dataWater = tdm?.GetData(cellPos) ?? new TileData();

            // don't use the "grown" flag for water. use explicit IsWatered
            if (dataWater.IsWatered)
                return;

            // mark watered (growth will start progressing in TileDataManager.Update)
            dataWater.IsWatered = true;
            dataWater.tileType = WateredTile.name;
            tdm?.SetData(cellPos, dataWater);

            // ensure a visual is present / updated immediately
            HandleTileGrowthUpdated(cellPos, dataWater);

            return;
        }

        // PLANT SEED1
        if (hbc.usingSeed1 &&
            hbc.inventory.GetItemAt(hbc.SelectedIndex).quantity > 0 &&
            !(tdm?.GetData(cellPos)?.SeedsPlanted[0] ?? false))
        {
            tm.StartCoroutine(tm.WaitForAnimationEnd("Plant Seed", "isPlanting"));
            tdm?.PlantSeed(cellPos, 0, fm.seed1GrowthTime, fm.seed1GrowthStages.Count);
            PutSpriteOnTile(position);
            hbc.UseItem();
            return;
        }

        // PLANT SEED2
        if (hbc.usingSeed2 &&
            hbc.inventory.GetItemAt(hbc.SelectedIndex).quantity > 0 &&
            !(tdm?.GetData(cellPos)?.SeedsPlanted[1] ?? false))
        {
            tm.StartCoroutine(tm.WaitForAnimationEnd("Plant Seed", "isPlanting"));
            tdm?.PlantSeed(cellPos, 1, fm.seed2GrowthTime, fm.seed2GrowthStages.Count);
            PutSpriteOnTile(position);
            hbc.UseItem();
            return;
        }

        // HARVEST
        if (hbc.handsEmpty)
        {
            farmTileSprites.TryGetValue(cellPos, out GameObject farmSpriteObj);

            if (farmSpriteObj != null)
            {
                Destroy(farmSpriteObj);
                farmTileSprites.Remove(cellPos);
            }

            interactableMap.SetTile(cellPos, hiddenInteractableTile);

            var dataHarvest = tdm?.GetData(cellPos);
            if (dataHarvest != null)
            {
                if (dataHarvest.SeedsPlanted[0])
                {
                    inventoryData.AddItem(Plants[0]);
                    dataHarvest.SeedsPlanted[0] = false;
                }
                else if (dataHarvest.SeedsPlanted[1])
                {
                    inventoryData.AddItem(Plants[1]);
                    dataHarvest.SeedsPlanted[1] = false;
                }

                dataHarvest.SeedsPlanted[2] = false;
                // reset watered/grown state when harvesting
                dataHarvest.IsWatered = false;
                tdm?.SetData(cellPos, dataHarvest);
            }
            return;
        }

        // No valid action
        return;
    }

    public void PutSpriteOnTile(Vector3 pos)
    {
        if (interactableMap == null || farmSprite == null)
            return;

        Vector3Int cellPos = interactableMap.WorldToCell(pos);
        TileBase tile = interactableMap.GetTile(cellPos);
        if (tile == null)
            return;

        Vector3 spawnPos = interactableMap.GetCellCenterWorld(cellPos);

        if (spriteParent != null)
        {
            foreach (Transform child in spriteParent)
            {
                if (Vector3.Distance(child.position, spawnPos) < 0.01f)
                {
                    return; //if theres already a sprite at this tile
                }
            }
        }

        if (farmTileSprites.ContainsKey(cellPos))
            return;

        GameObject instance = Instantiate(farmSprite, spawnPos, Quaternion.identity);
        farmTileSprites.Add(cellPos, instance);

        if (spriteParent != null)
        {
            instance.transform.SetParent(spriteParent, true);
        }
    }

    private List<(Vector3Int pos, TileData data)> pendingGrowthUpdates = new List<(Vector3Int pos, TileData data)>();

    private void DrainPendingGrowthUpdates()
    {
        if (pendingGrowthUpdates == null || pendingGrowthUpdates.Count == 0)
            return;

        // Process snapshot to avoid re-entrancy issues modifying the list while iterating.
        var snapshot = pendingGrowthUpdates.ToArray();
        pendingGrowthUpdates.Clear();

        foreach (var entry in snapshot)
        {
            // Call the handler; now that we drained and cleared the queue, the handler will run immediately.
            HandleTileGrowthUpdated(entry.pos, entry.data);
        }
    }

    private void HandleTileGrowthUpdated(Vector3Int pos, TileData data)
    {
        // If necessary resources aren't ready yet (commonly happens during scene re-entry),
        // enqueue the update for later once OnSceneLoaded finishes setting up maps/prefabs.
        if (interactableMap == null || fm == null || farmSprite == null || spriteParent == null)
        {
            int existingIndex = pendingGrowthUpdates.FindIndex(t => t.pos == pos);
            if (existingIndex >= 0)
            {
                // replace stale data for the same position with fresh data
                pendingGrowthUpdates[existingIndex] = (pos, data);
            }
            else
            {
                pendingGrowthUpdates.Add((pos, data));
            }
            return;
        }

        // set sprite according to seed type and growthstage
        if (!farmTileSprites.TryGetValue(pos, out var obj))
        {
            Vector3 worldPos = interactableMap.GetCellCenterWorld(pos);
            obj = Instantiate(farmSprite, worldPos, Quaternion.identity, spriteParent);
            farmTileSprites[pos] = obj;
        }

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        if (data.SeedType == 0)
        {
            sr.sprite = fm.seed1GrowthStages[Mathf.Clamp(data.GrowthStage, 0, fm.seed1GrowthStages.Count - 1)];
        }
        else if (data.SeedType == 1)
        {
            sr.sprite = fm.seed2GrowthStages[Mathf.Clamp(data.GrowthStage, 0, fm.seed2GrowthStages.Count - 1)];
        }
    }

}
