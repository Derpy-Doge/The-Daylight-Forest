using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile; 

    [SerializeField] private Tile PlowedTile;
    [SerializeField] private Tile WateredTile;
    [SerializeField] private Tile Seed1Tile;
    [SerializeField] private Tile Seed2Tile;

    [SerializeField] private TileDataManager tdm;
    public HotbarController hbc;
    private FarmingManager fm;

    public GameObject farmSprite;
    [SerializeField] private Transform spriteParent;

    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private List<InventoryItem> Plants; // 0 for seed1, 1 for seed2

    private void Awake()
    {
       GameObject hotbar = GameObject.FindGameObjectWithTag("Hotbar");
        hbc = hotbar.GetComponent<HotbarController>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        fm = player.GetComponent<FarmingManager>();
    }

    void Start()
    {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
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
        TileBase tile = interactableMap.GetTile(interactableMap.WorldToCell(position));
        Vector3Int intPos = new Vector3Int((int)position.x, (int)position.y, (int)position.z);

        if (tile != null)
        {
            if(tile == hiddenInteractableTile && hbc.usingPlow)
            {
                return true;
            }
            else if(tile == PlowedTile && hbc.usingSeed1 || tile == PlowedTile && hbc.usingSeed1)
            {
                return true;
            }
            else if(tdm.tileData[intPos].seedsPlanted[0] && hbc.usingWateringCan || tdm.tileData[intPos].seedsPlanted[1] && hbc.usingWateringCan)
            {
                return true;
            }
            else if(tdm.tileData[intPos].seedsPlanted[2] && hbc.handsEmpty) 
            {
                return true;
            }
        }

        Debug.Log("you either need a tool or youre using the wrong tool... idiot");
        return false;
    }

    public void SetInteracted(Vector3 position)
    {
        Vector3Int intPos = new Vector3Int((int)position.x, (int)position.y, (int)position.z);

        if (hbc != null)
        {
            if (hbc.usingPlow)
            {
               tdm.SetData(intPos, new TileData());
               interactableMap.SetTile(interactableMap.WorldToCell(position), PlowedTile);
            }
            else if (hbc.usingWateringCan)
            {
                interactableMap.SetTile(interactableMap.WorldToCell(position), WateredTile);

                tdm.GetData(intPos);
                if (tdm.tileData[intPos].seedsPlanted[0])
                {
                    StartCoroutine(fm.GrowSeed1());
                    Debug.Log("Gurt: Yo");
                }
                else if (tdm.tileData[intPos].seedsPlanted[1])
                {
                    StartCoroutine(fm.GrowSeed2());
                    Debug.Log("Yo: Gurt");
                }
            }
            else if (hbc.usingSeed1)
            {
                tdm.TriggerBool(intPos, 0);
                PutSpriteOnTile(position);

                interactableMap.SetTile(interactableMap.WorldToCell(position), Seed1Tile);
            }
            else if (hbc.usingSeed2)
            {
                tdm.TriggerBool(intPos, 1);
                PutSpriteOnTile(position);

                interactableMap.SetTile(interactableMap.WorldToCell(position), Seed2Tile);
                tdm.GetData(intPos);
            }
            else
            {
                return;
            }
        }
        if (tdm.tileData[intPos].seedsPlanted[2]) // if a fully grown plant is there
        {
            interactableMap.SetTile(interactableMap.WorldToCell(position), hiddenInteractableTile);

            if (tdm.tileData[intPos].seedsPlanted[0])
            {
                inventoryData.AddItem(Plants[0]);
                tdm.TriggerBool(intPos, 0);


            }

            if (tdm.tileData[intPos].seedsPlanted[1])
            {
                inventoryData.AddItem(Plants[1]);
                tdm.TriggerBool(intPos, 1);
            }

            tdm.TriggerBool(intPos, 2);
        }

        return;
    }

    public void PutSpriteOnTile(Vector3 pos)
    {
        if (interactableMap == null || farmSprite == null)
            return;

        TileBase tile = interactableMap.GetTile(interactableMap.WorldToCell(pos));
        if (tile == null)
            return;

        Vector3Int intPos = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);

        if (spriteParent != null)
        {
            foreach (Transform child in spriteParent)
            {
                if (Vector3.Distance(child.position, intPos) < 0.01f)
                {
                    return; //if theres already have a sprite at this tile
                }
            }
        }

        GameObject instance = Instantiate(farmSprite, intPos, Quaternion.identity);

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

                if(spriteParent != null)
                {
                    instance.transform.parent = spriteParent;
                }
            }
        }
    }

}
