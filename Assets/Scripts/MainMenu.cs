using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GlobalInfoScript.speedrun = false;
        GlobalInfoScript.fromBoss = false;
        SceneManager.LoadScene(1);
    }

    public void LoadGameBoss() {
        GlobalInfoScript.fromBoss = true;
        GlobalInfoScript.speedrun = false;
        SceneManager.LoadScene(1);
    }

    public void LoadGameBossFromMainMenu() {
        GlobalInfoScript.speedrun = false;
        GlobalInfoScript.fromBoss = true;
        GlobalInfoScript.fromMainMenu = true;
        SceneManager.LoadScene(1);
    }

    public void LoadGameSpeedrun() {
        GlobalInfoScript.speedrun = true;
        GlobalInfoScript.fromBoss = false;
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

    public void Restart() {
        if (GlobalInfoScript.speedrun) {
            LoadGameSpeedrun();
        }
        else {
            LoadGame();
        }
    }

    public void Quit() {
        Application.Quit();
    }
}
