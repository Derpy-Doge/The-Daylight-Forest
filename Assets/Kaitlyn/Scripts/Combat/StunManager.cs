using System.Collections;
using UnityEngine;

public class StunManager : MonoBehaviour
{
    private SpawnHitBox shb; // player spawn hitbox
    public Hazard h;
    public Rigidbody rb;

    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    private void Awake()
    {
        shb = FindFirstObjectByType<SpawnHitBox>();
    }

    public IEnumerator Stun()
    {
        savedVelocity = rb.linearVelocity;
        savedAngularVelocity = rb.angularVelocity;

        rb.isKinematic = true;
        h.enabled = false;

        yield return new WaitForSeconds(shb.stunDuration);

        if (rb == null) yield break;

        rb.isKinematic = false;
        h.enabled = true;
        rb.linearVelocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;
    }
}
