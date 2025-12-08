using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class SpawnHitBox : MonoBehaviour
{
    public LayerMask attackLayer;

    public float slashRadius = .12f;
    public float slashRange = .3f;
    private bool isSlashing = false;
    private bool canSlash = true;
    public float slashCooldown = .5f;

    public float stunRadius = 1.5f;
    public float stunDuration = 2f;
    private bool isStunning = false; // :skull:
    public bool canStun = true;
    public float stunCooldown = 5f;

    public float knockbackRadius = 1f;
    public float knockbackForce = 100f;
    private bool isKnockingBack = false;
    private bool canKnockback = true;
    public float knckbackCooldown = 3f;


    public DirectionHandler dh;

    public void Slash(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0) return;

        if (canSlash)
        {
            StartCoroutine(Slash());
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

        if (!isKnockingBack  && !isSlashing && canStun)
        {
        StartCoroutine(Stun());
        }
        else
        {
            Debug.Log("you cant stun yet");
        }
    }

    public void Knockback(InputAction.CallbackContext ctx) 
    {
        if (ctx.ReadValue<float>() == 0) return;

        Physics.OverlapSphere(transform.position, knockbackRadius, attackLayer, QueryTriggerInteraction.Ignore);

        if (!isStunning && !isSlashing && canKnockback) //knockback uses physics so it literally doesnt work when stun pauses physics :skull:
        {         
            StartCoroutine(Knockback());
        }
        else
        {
            Debug.Log("you cant knockback yet");
        }
    }

    IEnumerator Slash()
    {
        canSlash = false;
        isSlashing = true;

        Physics.SphereCast(transform.position, slashRadius, dh.playerDirection * .3f, out RaycastHit hitInfo, slashRange, attackLayer);

        if (hitInfo.collider)
        {
            

            if (hitInfo.collider.TryGetComponent(out Stats enemy) && TryGetComponent(out Stats player))
            {
                float calculatedDamage = player.Attack_Power - enemy.Defense;
                enemy.Health_Current -= calculatedDamage;

                Debug.Log("Hit " + hitInfo.collider.gameObject.name + " took " + calculatedDamage + " damage");
            }
        }
        isSlashing = false;

        yield return new WaitForSeconds(slashCooldown);

        canSlash = true;
    }

    IEnumerator Stun() // uhhh so like thi sdidnt work :sob: 
    //"setting linear velocity of a kenematic rigidbody is not supported" ... ill figure it our later
    // its just cause enemy movement sets velocity even when kinematic is on so ill just ignore it :man_juggling:
    {
        canStun = false;
        isStunning = true;

        foreach (Collider hitCollider in Physics.OverlapSphere(transform.position, stunRadius, attackLayer, QueryTriggerInteraction.Ignore))
        {
            if (hitCollider.TryGetComponent(out StunManager sm))
            {

                StartCoroutine(sm.Stun());
            }
        }

        isStunning = false;

        yield return new WaitForSeconds(stunCooldown);

        canStun = true;
    }

    IEnumerator Knockback()
    {
        canKnockback = false;
        isKnockingBack = true;

        foreach (Collider hitCollider in Physics.OverlapSphere(transform.position, knockbackRadius, attackLayer, QueryTriggerInteraction.Ignore))
        {
            if (hitCollider.TryGetComponent(out Rigidbody rb))
            {
                Vector3 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }

        isKnockingBack = false;

        yield return new WaitForSeconds(knckbackCooldown);

        canKnockback = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + dh.playerDirection * .25f, slashRadius);

        Gizmos.DrawWireSphere(transform.position, stunRadius); 

        Gizmos.DrawWireSphere(transform.position, knockbackRadius);
    }
}
