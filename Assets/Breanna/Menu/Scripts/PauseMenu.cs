using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;


    private void Awake()
    {
        container = GameObject.FindGameObjectWithTag("PauseMenu");
    }

    private void Start()
    {
        container.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(container.activeSelf)
            {
                container.SetActive(false);
                Time.timeScale = 1;
                return;
            }
            else
            {
                container.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void SkillsTreeButton()
    {
        SceneManager.LoadScene("Skill_Tree", LoadSceneMode.Additive);
    }
}
