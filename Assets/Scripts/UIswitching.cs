using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject GameUIPanel;
    [SerializeField] GameObject pauseUIPanel;

    public void LoadMainMenu()
    {   
        Debug.Log("Main menu Button");
        SceneManager.LoadScene(0);
    }

    public void returntoGame()
    {   
        Debug.Log("game Button");
        pauseUIPanel.SetActive(false);
    }
}