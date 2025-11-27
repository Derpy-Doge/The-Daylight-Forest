using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class SpawnHitBox : MonoBehaviour
{
    public float attackRadius = .12f;
    public float attackRange = .3f;
    public LayerMask attackLayer;

    public DirectionHandler dh;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        Physics.SphereCast(transform.position, attackRadius, dh.playerDirection * .25f, out RaycastHit hitInfo, attackRange, attackLayer);

        if (hitInfo.collider)
        {
            Debug.Log("Hit " + hitInfo.collider.gameObject.name);
            Destroy(hitInfo.collider.gameObject);
        }
        else
        {
            Debug.Log("didnt hit nothin");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + dh.playerDirection * .25f, attackRadius);
    }
}
