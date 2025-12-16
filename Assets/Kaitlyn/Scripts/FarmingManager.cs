using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingManager : MonoBehaviour
{
    private GameObject player;

    void Awake()
    {
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

        if (TimeManager.instance.tileManager.IsInteractable(player.transform.position))
        {
            TimeManager.instance.tileManager.SetInteracted(player.transform.position);
        }
        else
        {
            Debug.Log("nah twn");
        }
    }
}
