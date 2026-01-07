using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileDataManager : MonoBehaviour
{
    private static TileDataManager Instance;
    [SerializeField] private Tilemap interactableTilemap;

    [SerializeField] private List<TileBase> tileTypes;

    public string mainScene;

    public Dictionary<Vector3Int, TileData> tileData = new Dictionary<Vector3Int, TileData>();

    // event raised when tile growth changes (stage or becomes mature)
    public event Action<Vector3Int, TileData> OnTileGrowthUpdated;

    // pending updates queued while interactableTilemap is not available (e.g. re-entering a scene)
    private List<(Vector3Int pos, TileData data)> pendingGrowthUpdates = new List<(Vector3Int, TileData)>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // only build initial dictionary if empty (avoid wiping persistent saved state)
        if (tileData == null) tileData = new Dictionary<Vector3Int, TileData>();
        if (tileData.Count == 0)
        {
            var mapObj = GameObject.FindWithTag("InteractableMap");
            interactableTilemap = mapObj?.GetComponent<Tilemap>();
            if (interactableTilemap != null)
            {
                foreach (var pos in interactableTilemap.cellBounds.allPositionsWithin)
                {
                    if (!interactableTilemap.HasTile(pos)) continue;
                    tileData[pos] = new TileData(); // default empty state
                }
            }
        }
    }

    public void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
       // SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainScene)
        {
            interactableTilemap = GameObject.FindWithTag("InteractableMap")?.GetComponent<Tilemap>();
            if (interactableTilemap != null)
            {
                // Now that we have the map reference, flush any pending notifications
                DrainPendingGrowthUpdates();
            }
        }
        else
        {
            interactableTilemap = null;
        }
    }

    private void Update()
    {
        if (tileData.Count == 0) return;

        float dt = Time.deltaTime;
        // tick growth for all planted tiles
        foreach (var kv in tileData)
        {
            var pos = kv.Key;
            var data = kv.Value;
            // only tick growth when tile is planted AND watered
            if (!data.IsPlanted || !data.IsWatered) continue;

            bool stageChanged = data.Tick(dt);
            if (stageChanged)
            {
                Debug.Log($"Tile growth stage changed at {pos} -> stage {data.GrowthStage} remaining {data.RemainingSeconds:F1}s");
                EnqueueOrInvoke(pos, data);
            }
        }
    }

    public void PlantSeed(Vector3Int pos, int seedTypeId, float totalGrowthSeconds, int maxStages)
    {
        if (!tileData.TryGetValue(pos, out var data))
        {
            data = new TileData();
            tileData[pos] = data;
        }

        data.SeedType = seedTypeId;
        data.TotalGrowthSeconds = totalGrowthSeconds;
        // keep RemainingSeconds set but growth won't advance until watered
        data.RemainingSeconds = totalGrowthSeconds;
        data.MaxGrowthStages = Mathf.Max(1, maxStages);
        data.GrowthStage = 0;
        data.SeedsPlanted[seedTypeId] = true;
        // planting does not automatically water/start growth

        Debug.Log($"PlantSeed: pos={pos} seed={seedTypeId} totalSec={totalGrowthSeconds} stages={maxStages}");
        EnqueueOrInvoke(pos, data);
    }

    // New helper to mark a tile watered (will allow Tick to progress)
    public void SetWatered(Vector3Int pos, bool watered)
    {
        if (!tileData.TryGetValue(pos, out var data))
        {
            data = new TileData();
            tileData[pos] = data;
        }

        if (data.IsWatered == watered) return;

        data.IsWatered = watered;
        // persist change
        tileData[pos] = data;

        Debug.Log($"SetWatered: pos={pos} watered={watered}");
        // notify so visuals can update immediately (e.g. spawn sprite / show watered tile)
        EnqueueOrInvoke(pos, data);
    }

    public TileData GetData(Vector3Int position)
    {
        tileData.TryGetValue(position, out var d);
        return d;
    }

    public void SetData(Vector3Int pos, TileData data)
    {
        tileData[pos] = data;
    }

    // If interactableTilemap is present invoke immediately, otherwise enqueue for later.
    private void EnqueueOrInvoke(Vector3Int pos, TileData data)
    {
        if (interactableTilemap != null)
        {
            OnTileGrowthUpdated?.Invoke(pos, data);
        }
        else
        {
            // replace any existing pending entry for the same pos to avoid duplicates
            for (int i = 0; i < pendingGrowthUpdates.Count; i++)
            {
                if (pendingGrowthUpdates[i].pos == pos)
                {
                    pendingGrowthUpdates[i] = (pos, data);
                    return;
                }
            }
            pendingGrowthUpdates.Add((pos, data));
        }
    }

    // Flush pending updates once the interactableTilemap is available
    private void DrainPendingGrowthUpdates()
    {
        if (pendingGrowthUpdates.Count == 0) return;

        foreach (var p in pendingGrowthUpdates)
        {
            OnTileGrowthUpdated?.Invoke(p.pos, p.data);
        }
        pendingGrowthUpdates.Clear();
    }
}

[System.Serializable]
public class TileData
{
    // which seed was planted (0 or 1), -1 = none
    public int SeedType = -1;

    public List<bool> SeedsPlanted = new List<bool> { false, false, false }; // seed1, seed2, grown

    // explicitly track whether tile has been watered (growth only advances when true)
    public bool IsWatered = false;

    public int GrowthStage = 0;             // 0..MaxGrowthStages-1
    public int MaxGrowthStages = 1;         // number of visual stages
    public float TotalGrowthSeconds = 0f;   // full growth time in seconds
    public float RemainingSeconds = 0f;     // seconds left until fully grown

    // optional tile type string you already had:
    public string tileType;

    public bool IsPlanted => SeedType >= 0;

    // Tick returns true if the growth stage changed
    public bool Tick(float deltaSeconds)
    {
        if (!IsPlanted || RemainingSeconds <= 0f) return false;

        float prevStage = CurrentStageFraction();
        RemainingSeconds = Mathf.Max(0f, RemainingSeconds - deltaSeconds);

        float newStage = CurrentStageFraction();
        bool changed = Mathf.FloorToInt(prevStage * MaxGrowthStages) != Mathf.FloorToInt(newStage * MaxGrowthStages);

        if (RemainingSeconds <= 0f) // if done mark fully grown bool
        {
            GrowthStage = MaxGrowthStages - 1;
            SeedsPlanted[2] = true;
            return true;
        }

        // update GrowthStage based on progress
        float progress = 1f - (RemainingSeconds / TotalGrowthSeconds);
        GrowthStage = Mathf.Clamp(Mathf.FloorToInt(progress * MaxGrowthStages), 0, MaxGrowthStages - 1);
        return changed;
    }

    private float CurrentStageFraction()
    {
        if (TotalGrowthSeconds <= 0f) return 1f;
        return 1f - (RemainingSeconds / TotalGrowthSeconds);
    }
}
