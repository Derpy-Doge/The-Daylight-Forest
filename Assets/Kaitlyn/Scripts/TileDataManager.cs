using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileDataManager : MonoBehaviour
{
    private static TileDataManager Instance;
    private GameObject interactableMapObject;
    private Tilemap interactableTilemap;

    public Dictionary<Vector3Int, TileData> tileData = new Dictionary<Vector3Int, TileData>();

    private void Awake()
    {
        if (Instance == null)
        {
          Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        interactableMapObject = GameObject.FindWithTag("InteractableMap");
        interactableTilemap = interactableMapObject.GetComponent<Tilemap>();
    }
    void Start()
    {
        
    }

    public void SetData(Vector3Int pos, TileData data)
    {
        if(tileData.ContainsKey(pos))
        {
            tileData[pos] = data;
        }
        else
        {
            tileData.Add(pos, data);
        }
    }

    public TileData GetData(Vector3Int position)
    {
        if (tileData.TryGetValue(position, out TileData data))
        {
            return data;
        }

        return null;
    }


    public void TriggerBool(Vector3Int tilePosition, int boolIndex)
    {
        TileData data = GetData(tilePosition);
        if (data != null)
        {
            data.TriggerSpecificBool(boolIndex);
        }
        else
        {
            Debug.LogWarning("no data found at position {tilePosition}");
        }
    }

    public void DisplayDictionaryContent(InputAction.CallbackContext ctx)
    {
        if(ctx.ReadValue<float>() == 0)
        {
            return;
        }

        Debug.Log("----------Dictionary Contents----------");
        foreach(KeyValuePair<Vector3Int, TileData> tile in tileData)
        {
            Debug.Log("Tile Position: " + tile.Key + " | Data:" + tile.Value.seedsPlanted);
        }
        Debug.Log("---------------------------------------");
    }
}

[System.Serializable]
public class TileData
{
    public List<bool> seedsPlanted = new List<bool> { false, false };

    //public bool seed1Planted = false; the first bool will be seed 1 
    //public bool seed2Planted = false; 2nd one will be seed 2 

    public void TriggerSpecificBool(int index)
    {
        if(index >= 0 && index < seedsPlanted.Count)
        {
            seedsPlanted[index] = !seedsPlanted[index];
        }
    }    
}
