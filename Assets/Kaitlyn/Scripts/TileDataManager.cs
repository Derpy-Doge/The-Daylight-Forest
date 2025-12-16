using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        foreach (var position in interactableTilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableTilemap.GetTile(interactableTilemap.WorldToCell(position));

            if (tile != null)
            {
                tileData[position] = new TileData(new List<bool>() { false, false });
            }
        }
    }

    public TileData GetData(Vector3 position)
    {
        if (!tileData.ContainsKey(position))
        {
            Debug.Log(tileData[position]);
            return tileData[position];
        }

        return null;
    }


    public void TriggerBool(Vector3 tilePosition, int boolIndex)
    {
        if(tilePosition == null)
        {
            Debug.Log("yo do smth");
        }
        else
        {
            if (tileData.ContainsKey(tilePosition))
            {
                if (boolIndex >= 0 && boolIndex < tileData[tilePosition].seedsPlanted.Count)
                {
                    tileData[tilePosition].seedsPlanted[boolIndex] = !tileData[tilePosition].seedsPlanted[boolIndex];
                    Debug.Log("Bool at index {boolIndex} for tile {tilePosition} is now: {tileData[tilePosition].seedsPlanted[boolIndex]}");
                }
            }
        }
        
    }
}

public class TileData
{
    public List<bool> seedsPlanted;

    //public bool seed1Planted = false; the first bool will be seed 1 
    //public bool seed2Planted = false; 2nd one will be seed 2 

    public TileData(List<bool> initialBools)
    {
        seedsPlanted = initialBools ?? new List<bool>();
    }
}
