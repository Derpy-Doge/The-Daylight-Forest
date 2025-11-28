using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void Continue(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void Main_Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
