using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class SpawnHitBox : MonoBehaviour
{
    public LayerMask attackLayer;

    public float slashRadius = .12f;
    public float slashRange = .3f;

    public float stunRadius = 1.5f;
    public float stunDuration = 2f;
    public bool isStunning = false; // :skull:
    public bool canStun = true;
    public float stunCooldown = 5f;
    private Vector3 savedEnemyVelocity;
    private Vector3 savedEnemyAngularVelocity;

    public float knockbackRadius = 1f;


    public DirectionHandler dh;

    public void Slash(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0) return;

        Physics.SphereCast(transform.position, slashRadius, dh.playerDirection * .25f, out RaycastHit hitInfo, slashRange, attackLayer);

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

    public void Stun(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0) return;

        Physics.OverlapSphere(transform.position, stunRadius, attackLayer, QueryTriggerInteraction.Ignore);

        StartCoroutine(Stun());
    }

    IEnumerator Stun() // uhhh so like thi sdidnt work :sob: 
    //"setting linear velocity of a kenematic rigidbody is not supported" ... ill figure it our later
    {
        canStun = false;
        isStunning = true;

        foreach (Collider hitCollider in Physics.OverlapSphere(transform.position, stunRadius, attackLayer, QueryTriggerInteraction.Ignore))
        {
            if (hitCollider.TryGetComponent(out Rigidbody rb))
            {

                savedEnemyVelocity = rb.linearVelocity;
                savedEnemyAngularVelocity = rb.angularVelocity;

                rb.isKinematic = true;

                yield return new WaitForSeconds(stunDuration);

                rb.isKinematic = false;

                rb.linearVelocity = savedEnemyVelocity;
                rb.angularVelocity = savedEnemyAngularVelocity;
            }
        }

        isStunning = false;

        yield return new WaitForSeconds(stunCooldown);

        canStun = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + dh.playerDirection * .25f, slashRadius);

        Gizmos.DrawWireSphere(transform.position, stunRadius); 
    }
}
