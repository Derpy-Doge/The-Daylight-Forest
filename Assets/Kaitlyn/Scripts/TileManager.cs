using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile; 

    [SerializeField] private Tile interactedTile;
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
            if(tile.name == "Interactable")
            {
                return true;
            }
        }
        return false;
    }

    public void SetInteracted(Vector3 position)
    {
        interactableMap.SetTile(interactableMap.WorldToCell(position), interactedTile);
    }

}
