using Inventory.UI;
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

    [SerializeField] private HotbarController hbc;
    [SerializeField] private TileDataManager tdm;

    [SerializeField] private GameObject farmSprite;
    [SerializeField] private Transform spriteParent;
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
       
        if(tile != null)
        {
            if(tile == hiddenInteractableTile && hbc.usingPlow)
            {
                return true;
            }
            else if(tile == PlowedTile && hbc.usingWateringCan)
            {
                return true;
            }
            else if(tile == WateredTile && hbc.usingSeed1 || tile == WateredTile && hbc.usingSeed2)
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
            }
            else if (hbc.usingSeed1)
            {
                tdm.TriggerBool(intPos, 0);
                PutSpritesOnFarmTiles();

                interactableMap.SetTile(interactableMap.WorldToCell(position), Seed1Tile);

                if (tdm.tileData[intPos].seedsPlanted[0])
                {
                    Debug.Log("Gurt: Yo");
                }

                tdm.GetData(intPos);
            }
            else if (hbc.usingSeed2)
            {
                tdm.TriggerBool(intPos, 1);
                PutSpritesOnFarmTiles();

                interactableMap.SetTile(interactableMap.WorldToCell(position), Seed2Tile);
                tdm.GetData(intPos);
            }
            else
            {
                return;
            }
        }

        return;
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
