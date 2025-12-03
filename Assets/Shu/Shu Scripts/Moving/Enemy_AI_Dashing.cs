using UnityEngine;

public class Enemy_AI_Dashing : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public GameObject player;
    public float Emovespeed = 1f;
    public float xfollow;
    public float zfollow;
    public float dashTimer;
    public float dashTimerLength = 5f;
    public float dashLength = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Sqrt((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x) + (player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z)) <= 10)
        {
            Vector3 newVelocity = new(xfollow, 0, zfollow);
            newVelocity.Normalize();

            newVelocity.x *= Emovespeed;
            newVelocity.z *= Emovespeed;
            myRigidbody.linearVelocity = newVelocity;
        }
        //Dash-Timer
        dashTimer = dashTimer + Time.deltaTime;
        if (dashTimer >= dashTimerLength)
        {
            Emovespeed = 1;
            dashTimer = 0f;
        }
        if (dashTimer >= (dashTimerLength - (dashLength*2/50)))
        {
            Emovespeed = 30;
        }
    }
    void FixedUpdate()
    {
        xfollow = (player.transform.position.x - transform.position.x);
        zfollow = (player.transform.position.z - transform.position.z);
    }
}
