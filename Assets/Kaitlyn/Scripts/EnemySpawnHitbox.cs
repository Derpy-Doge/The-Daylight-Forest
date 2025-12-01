using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpawnHitbox : MonoBehaviour
{
    public LayerMask attackLayer;

    public float attackRadius = .5f;
    public float attackRange = .3f;
    public bool canAttack = true;
    public float attackCooldown = 1f;


    public EnemyDirectionHandler dh;
    public Stats enemy;

    public GameObject player;


    void Update()
    {
        Physics.SphereCast(transform.position, attackRadius, dh.enemyDirection * .4f, out RaycastHit hitInfo, attackRange, attackLayer);
        Vector3.Distance(transform.position, player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRadius && canAttack)
        {
                StartCoroutine(Attack());
            Debug.Log("work?");
        }
        else
        {
            Debug.Log("player out of range or enemy cant attack");
        }
    }

    public IEnumerator Attack()
    {
        Debug.Log("it work");
        canAttack = false;

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

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + dh.enemyDirection * attackRange, attackRadius);
    }
}

