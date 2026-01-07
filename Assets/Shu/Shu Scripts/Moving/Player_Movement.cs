//using UnityEditor;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    public static Player_Movement instance;
    private string mainScene;

    public Rigidbody myRigidbody;
    public float movespeed = 10f;
    public Animator myAnimator;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        mainScene = "updataed map";

        //mainScene = SaveData.instance.mainScene;
    }

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.A) == true)
        {
            Vector3 newVelocity = myRigidbody.linearVelocity;
            newVelocity.x = -movespeed;

            myRigidbody.linearVelocity = newVelocity;
        }
        if (Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true)
        {
            Vector3 newVelocity = myRigidbody.linearVelocity;
            newVelocity.x = movespeed;

            myRigidbody.linearVelocity = newVelocity;
        }
        if (Input.GetKey(KeyCode.DownArrow) == true || Input.GetKey(KeyCode.S) == true)
        {
            Vector3 newVelocity = myRigidbody.linearVelocity;
            newVelocity.z = -movespeed;

            myRigidbody.linearVelocity = newVelocity;
        }
        if (Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.W) == true)
        {
            Vector3 newVelocity = myRigidbody.linearVelocity;
            newVelocity.z = movespeed;

            myRigidbody.linearVelocity = newVelocity;
        }

        if (myAnimator != null)
        {
            if ((Input.GetKey(KeyCode.LeftArrow) == true || Input.GetKey(KeyCode.A) == true) ||
                (Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.D) == true)||
                (Input.GetKey(KeyCode.DownArrow) == true || Input.GetKey(KeyCode.S) == true)||
                (Input.GetKey(KeyCode.UpArrow) == true || Input.GetKey(KeyCode.W) == true))
            { 
                myAnimator.SetFloat("Xmv", myRigidbody.linearVelocity.normalized.x);
                myAnimator.SetFloat("Zmv", myRigidbody.linearVelocity.normalized.z);
            }

            myAnimator.SetBool("Ismoving", myRigidbody.linearVelocity.magnitude > 0);
        }
    }
}
