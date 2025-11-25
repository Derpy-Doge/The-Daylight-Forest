using UnityEngine;

public class DirectionHandler : MonoBehaviour
{
    private Vector3 playerDirection;

    Vector2 currentInput;
    Vector2 lastInput;

    void Start()
    {

    }

    void Update()
    {
        Debug.DrawRay(transform.position, playerDirection * .5f, Color.red);
        LastDirection();

        currentInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerDirection =  new Vector3 (lastInput.x, 0f, lastInput.y);

        if(playerDirection.magnitude > .01f)
        {
           playerDirection.Normalize();
        }
    }

    public void LastDirection()
    {
        if(currentInput != Vector2.zero)
        {
            lastInput = currentInput;
        }
    }
}
