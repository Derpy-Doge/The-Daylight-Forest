using UnityEngine;

public class EnemyDirectionHandler : MonoBehaviour
{
    public Rigidbody enemyRb;
    [HideInInspector] public Vector3 enemyDirection;

    void Start()
    {
        
    }

    
    void Update()
    {
        Debug.DrawRay(transform.position, enemyDirection * .3f, Color.white);

        if (enemyRb.linearVelocity.magnitude > 0.001f)
        {
            enemyDirection = enemyRb.linearVelocity.normalized;
        }
    }
}
