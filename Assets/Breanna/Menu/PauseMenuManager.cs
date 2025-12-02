using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    private bool isPaused = false;

    private void Start()
    {
        PauseMenuPanel.SetActive(false);
    }

    private void Update()
    {

    }   

    public void PauseGame(InputAction.CallbackContext ctx)
    {

        if (!isPaused)
        {
            isPaused = true;
            PauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        else
        {
            isPaused = false;
            PauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
        
    }

    public void ResumeGame()
    {
        
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }
}
