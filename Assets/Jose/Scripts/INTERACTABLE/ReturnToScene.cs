using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToScene : MonoBehaviour
{
    public string sceneToReturn = "updated map";

    public void GoBack()
    {
        
        Time.timeScale = 1f;

       

        Debug.Log("Returning safely to updated map");

        SceneManager.LoadScene(sceneToReturn);
    }
}
