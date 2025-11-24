using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnHitBox : MonoBehaviour
{
    public float attackRadius = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        Physics.SphereCast(transform.position, attackRadius, transform.forward, out RaycastHit hitInfo, 1f);

        if (hitInfo.collider)
        {
            Debug.Log("Hit " + hitInfo.collider.name);
        }
    }
}
