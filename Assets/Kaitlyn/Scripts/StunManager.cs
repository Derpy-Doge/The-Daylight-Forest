using System.Collections;
using UnityEngine;

public class StunManager : MonoBehaviour
{
    public SpawnHitBox shb;
    public Rigidbody rb;

    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    public IEnumerator Stun()
    {
        savedVelocity = rb.linearVelocity;
        savedAngularVelocity = rb.angularVelocity;

        rb.isKinematic = true;

        yield return new WaitForSeconds(shb.stunDuration);

        if (rb == null) yield break;

        rb.isKinematic = false;
        rb.linearVelocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;
    }
}
