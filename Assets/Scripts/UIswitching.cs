using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIswitching : MonoBehaviour
{
    [SerializeField] GameObject GameUIPanel;
    [SerializeField] GameObject pauseUIPanel;
    public Image[] pieceEmpty;
    public Image[] pieceFilled;
    public float[] energyLevels = { 3f, 5f, 10f, 15f }; //queen is placeholder 15 for now


    public void Awake() {
        Color c = new Color(0f, 0f, 0f, 0.7f);  
        foreach (Image img in pieceEmpty) {
            img.color = c;
        }
    }   

    public void updatePieceFills(float energy) {
        Debug.Log("Increase UI energy");
        for (int i = 0; i < pieceFilled.Length && i < energyLevels.Length; i++) {
            pieceFilled[i].fillAmount = Mathf.Clamp01(energy / energyLevels[i]);
        }
    }
    

    public void resetPieceFills() {
        Debug.Log("Reset UI energy");
        Color c = new Color(0f, 0f, 0f, 0.7f);  
        foreach (Image img in pieceEmpty) {
            img.color = c;
        }
    }
    // needs to then update to the correct level of energy, so 10 energy then resetting down to ig 5
    // means the reset would need to be the correct level? im not sure i cant get it to work

    public void LoadMainMenu()
    {
        Debug.Log("Main menu Button");
        SceneManager.LoadScene(0);
    }

    public void ReturntoGame()
    {
        Debug.Log("game Button");
        pauseUIPanel.SetActive(false);
    }
}