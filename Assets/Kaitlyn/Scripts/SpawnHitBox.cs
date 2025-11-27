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
        if (ctx.ReadValue<float>() == 0) return;

        Physics.SphereCast(transform.position, attackRadius, dh.playerDirection * .25f, out RaycastHit hitInfo, attackRange, attackLayer);

        if (hitInfo.collider)
        {
            Debug.Log("Hit " + hitInfo.collider.gameObject.name);
            
            if (hitInfo.collider.TryGetComponent(out Stats enemy) && TryGetComponent(out Stats player))
            {
                float calculatedDamage = player.Attack_Power - enemy.Defense;
                enemy.Health_Current -= calculatedDamage;
            }
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
