using Inventory.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile; 

    [SerializeField] private Tile PlowedTile;
    [SerializeField] private Tile WateredTile;
    [SerializeField] private Tile Seed1Tile;
    [SerializeField] private Tile Seed2Tile;

    [SerializeField] private HotbarController hbc;
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
        if(hbc != null)
        {
            if (hbc.usingPlow)
            {
                interactableMap.SetTile(interactableMap.WorldToCell(position), PlowedTile);
            }
            else if (hbc.usingWateringCan)
            {
                interactableMap.SetTile(interactableMap.WorldToCell(position), WateredTile);
            }
            else if (hbc.usingSeed1)
            {
                interactableMap.SetTile(interactableMap.WorldToCell(position), Seed1Tile);
            }
            else if (hbc.usingSeed2)
            {
                interactableMap.SetTile(interactableMap.WorldToCell(position), Seed2Tile);
            }
            else
            {
                return;
            }
        }

        return;
    }

}
