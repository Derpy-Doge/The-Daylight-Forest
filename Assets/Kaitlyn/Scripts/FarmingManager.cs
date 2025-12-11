using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingManager : MonoBehaviour
{
    private TileManager tileManager;
    private GameObject player;

    void Awake()
    {
        tileManager = FindFirstObjectByType<TileManager>();
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void CanInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0) return;

        Vector3Int pos = new Vector3Int((int)player.transform.position.x, 0, (int)player.transform.position.z);

        if (tileManager.IsInteractable(pos))
        {
            Debug.Log("tile is interactable");
            tileManager.SetInteracted(pos);
        }
        else
        {
            Debug.Log("nah twn");
        }
    }
}
