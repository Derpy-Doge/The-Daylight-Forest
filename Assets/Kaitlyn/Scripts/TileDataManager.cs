using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDataManager : MonoBehaviour
{
    public static TileDataManager Instance;
    public Tilemap interactableTilemap;

    private Dictionary<Vector3, TileData> tileData = new Dictionary<Vector3, TileData>();

    private void Awake()
    {
        if (Instance == null)
        {
          Instance = this;
        }
    }

    public void SetTileData(Vector3 position, TileData data)
    {
        TileBase tile = interactableTilemap.GetTile(interactableTilemap.WorldToCell(position));

        if (tile != null)
        {
            tileData[position] = data;   
        }
    }

    public TileData GetData(Vector3 position)
    {
        if (!tileData.ContainsKey(position))
        {
            return tileData[position];
        }

        return null;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class TileData
{
    public bool seed1Planted = false;
    public bool seed2Planted = false;
}
