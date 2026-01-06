using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableScene : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string sceneToLoad = "Skill_Tree";      
    public string sceneToReturn = "updated map";   
    public float interactDistance = 3f;            
    public KeyCode interactKey = KeyCode.E;        
    public Transform player;                        

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if (player == null) return;

        // Check if player is close enough
        if (Vector3.Distance(player.position, transform.position) <= interactDistance)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Optional: draw interaction radius in editor
    private void OnDrawGizmos()
    {
        if (player == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
