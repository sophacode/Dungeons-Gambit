using TMPro;
using UnityEngine;

public class FinalTimeScript : MonoBehaviour
{
    float time;
    private void Awake()
    {
        if (GlobalInfoScript.speedrun) {
            gameObject.SetActive(true);
        }
        time = GlobalInfoScript.finalTime;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        GetComponent<TextMeshProUGUI>().text =  "Final time: " + string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
