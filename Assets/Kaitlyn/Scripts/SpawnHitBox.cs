using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class SpawnHitBox : MonoBehaviour
{
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.back * 2, Color.red);
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        Physics.SphereCast(transform.position, attackRadius, transform.forward, out RaycastHit hitInfo, 1.5f, attackLayer);

        if (hitInfo.collider)
        {
            Debug.Log("Hit " + hitInfo.collider.gameObject.name);
        }
        else
        {
            Debug.Log("didnt hit nothin");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
