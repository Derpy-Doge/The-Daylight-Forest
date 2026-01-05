using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
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

        spriteParent = GameObject.FindGameObjectWithTag("FarmSpriteParent").transform;
        farmSprite = GameObject.FindGameObjectWithTag("FarmSprite");

        tm = FindFirstObjectByType<TimeManager>();
    }

    void Start()
    {
        interactableMap = GameObject.FindGameObjectWithTag("InteractableMap").GetComponent<Tilemap>();

        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible")
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }

    }

    public bool IsInteractable(Vector3 position)
    {
        Vector3Int cellPos = interactableMap.WorldToCell(position);
        TileBase tile = interactableMap.GetTile(cellPos);
        Vector3Int intPos = new Vector3Int((int)position.x, (int)position.y, (int)position.z);

        if (tile != null)
        {
            if(tile == hiddenInteractableTile && hbc.usingPlow)
            {
                return true;
            }
            else if(tile == PlowedTile && hbc.usingSeed1 || tile == PlowedTile && hbc.usingSeed2 && plant2Unlocked)
            {
                return true;
            }
            else if(tdm.tileData[cellPos].seedsPlanted[0] && hbc.usingWateringCan || tdm.tileData[cellPos].seedsPlanted[1] && hbc.usingWateringCan)
            {
                return true;
            }
            else if(tdm.tileData[cellPos].seedsPlanted[2] && hbc.handsEmpty) 
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
        Vector3Int intPos = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
        Vector3Int cellPos = interactableMap.WorldToCell(position);

        if (hbc != null)
        {
            if (hbc.usingPlow)
            {
                tm.StartCoroutine(tm.WaitForAnimationEnd("Plow", "isPlowing")); // (name, bool) ..also uhh great bool name i guess :skull:
               tdm.SetData(cellPos, new TileData());
               interactableMap.SetTile(interactableMap.WorldToCell(position), PlowedTile);
            }
            else if (hbc.usingWateringCan)
            {
                tm.StartCoroutine(tm.WaitForAnimationEnd("Water", "isWatering"));
                interactableMap.SetTile(interactableMap.WorldToCell(position), WateredTile);

                tdm.GetData(cellPos);
                if (tdm.tileData[cellPos].seedsPlanted[2])
                {
                    return;
                }
                else
                {
                    if(tdm.tileData[cellPos].seedsPlanted[0])
                    {
                        StartCoroutine(fm.GrowSeed1());
                        Debug.Log("Gurt: Yo");
                    }
                    else if (tdm.tileData[cellPos].seedsPlanted[1])
                    {
                        StartCoroutine(fm.GrowSeed2());
                        Debug.Log("Yo: Gurt");
                    }
                }
               
            }
            else if (hbc.usingSeed1 && hbc.inventory.GetItemAt(hbc.SelectedIndex).quantity > 0 && !tdm.tileData[cellPos].seedsPlanted[0])
            {
                tm.StartCoroutine(tm.WaitForAnimationEnd("Plant Seed", "isPlanting"));
                PutSpriteOnTile(position);
                tdm.TriggerBool(cellPos, 0);
                hbc.UseItem();
            }
            else if (hbc.usingSeed2 && hbc.inventory.GetItemAt(hbc.SelectedIndex).quantity > 0 && !tdm.tileData[cellPos].seedsPlanted[1])
            {
                tm.StartCoroutine(tm.WaitForAnimationEnd("Plant Seed", "isPlanting"));
                PutSpriteOnTile(position);
                tdm.TriggerBool(cellPos, 1);
                hbc.UseItem();
            }
            else if (hbc.handsEmpty)
            {
                farmTileSprites.TryGetValue(cellPos, out GameObject farmSpriteObj);

                if (farmSpriteObj != null)
                {
                    Destroy(farmSpriteObj);
                    farmTileSprites.Remove(cellPos);
                }

                interactableMap.SetTile(interactableMap.WorldToCell(position), hiddenInteractableTile);

                if (tdm.tileData[cellPos].seedsPlanted[0])
                {
                    inventoryData.AddItem(Plants[0]);
                    tdm.TriggerBool(cellPos, 0);


                }
                else if (tdm.tileData[cellPos].seedsPlanted[1])
                {
                    inventoryData.AddItem(Plants[1]);
                    tdm.TriggerBool(cellPos, 1);
                }

                tdm.TriggerBool(cellPos, 2);
            }
            else
            {
                return;
            }
        }       
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

    public void PutSpritesOnFarmTiles()
    {
        foreach(var pos in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(pos);

            if (tile != null )
            {
                Vector3 worldPos = interactableMap.GetCellCenterWorld(pos);

                GameObject instance = Instantiate(farmSprite, worldPos, Quaternion.identity);

                if (spriteParent != null)
                {
                    instance.transform.parent = spriteParent;
                }
            }
        }
    }

}
