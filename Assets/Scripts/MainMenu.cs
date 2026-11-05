using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGameBoss() {
        GlobalInfoScript.fromBoss = true;
        SceneManager.LoadScene(1);
    }

    public void LoadGameBossFromMainMenu() {
        GlobalInfoScript.fromBoss = true;
        GlobalInfoScript.fromMainMenu = true;
        SceneManager.LoadScene(1);
    }

    public void LoadGameSpeedrun() {
        GlobalInfoScript.speedrun = true;
        SceneManager.LoadScene(1);
    }

    public void LoadMain()
    {
        GlobalInfoScript.speedrun = false;
        GlobalInfoScript.fromBoss = false;
        GlobalInfoScript.fromMainMenu = false;
        SceneManager.LoadScene(0);
    }

    public void LoadDead()
    {
        SceneManager.LoadScene(2);
    }
}
