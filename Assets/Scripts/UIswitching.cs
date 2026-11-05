using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIswitching : MonoBehaviour
{
    [SerializeField] GameObject GameUIPanel;
    [SerializeField] GameObject pauseUIPanel;
    public Image[] pieceEmpty;
    public Image[] pieceFilled;
    public float[] energyLevels = { 3f, 5f, 10f, 20f }; //queen is placeholder 15 for now


    public void Awake() {
        Color c = new Color(0f, 0f, 0f, 0.7f);  
        foreach (Image img in pieceEmpty) {
            img.color = c;
        }
    }   

    public void updatePieceFills(float energy) {
        for (int i = 0; i < pieceFilled.Length && i < energyLevels.Length; i++) {
            pieceFilled[i].fillAmount = Mathf.Clamp01(energy / energyLevels[i]);
        }
    }
    

    public void resetPieceFills() {
        Color c = new Color(0f, 0f, 0f, 0.7f);  
        foreach (Image img in pieceEmpty) {
            img.color = c;
        }
    }
    // needs to then update to the correct level of energy, so 10 energy then resetting down to ig 5
    // means the reset would need to be the correct level? im not sure i cant get it to work

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ReturntoGame()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().OnPlay();
    }
}