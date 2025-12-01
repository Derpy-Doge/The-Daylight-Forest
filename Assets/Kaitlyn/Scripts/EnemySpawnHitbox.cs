using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpawnHitbox : MonoBehaviour
{
    public LayerMask attackLayer;

    public float attackRadius = .5f;
    public float attackRange = .3f;
    private bool isAttacking = false; // so uhhh why doesnt this work ?
    private bool canAttack = true;
    public float attackCooldown = 1f;


    public EnemyDirectionHandler dh;
    public Stats enemy;


    void Update()
    {
        Physics.SphereCast(transform.position, attackRadius, dh.enemyDirection * .4f, out RaycastHit hitInfo, attackRange, attackLayer);

        if (hitInfo.collider != null)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
            else
            {
                Debug.Log("Enemy attack on cooldown");
            }
        }
        else
        {
            Debug.Log("player out of range");
        }
    }

    public IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true; // i literally use it right here...

        if(Physics.SphereCast(transform.position, attackRadius, dh.enemyDirection * .4f, out RaycastHit hitInfo, attackRange, attackLayer))
        {
            if(hitInfo.collider.TryGetComponent(out Stats player))
            {
                float calculatedDamage = enemy.Attack_Power - player.Defense;
                player.Health_Current -= calculatedDamage;
            }
            else
            {
                Debug.Log("player has no stats");
            }
        }
        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + dh.enemyDirection * attackRange, attackRadius);
    }
}

