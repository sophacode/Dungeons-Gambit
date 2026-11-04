using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMain()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadDead()
    {
        SceneManager.LoadScene(2);
    }
}
