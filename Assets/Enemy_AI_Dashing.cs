using UnityEngine;

public class Enemy_AI_Dashing : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public GameObject player;
    public float movespeed = 1f;
    public float xfollow;
    public float zfollow;
    public float dashTimer;
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

            newVelocity.x *= movespeed;
            newVelocity.z *= movespeed;
            myRigidbody.linearVelocity = newVelocity;
        }
        //Dash-Timer
        dashTimer = dashTimer + Time.deltaTime;
        if (dashTimer >= 5f)
        {
            movespeed = 1;
            dashTimer = 0f;
        }
        if (dashTimer >= 4.8f)
        {
            movespeed = 30;
        }
    }
    void FixedUpdate()
    {
        xfollow = (player.transform.position.x - transform.position.x);
        zfollow = (player.transform.position.z - transform.position.z);
    }
}
