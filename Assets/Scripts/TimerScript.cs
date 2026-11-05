using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    private TextMeshProUGUI text;
    public float time;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        time = 0f;
    }

    private void Update()
    {
        time += Time.deltaTime;
        text.text = FormatTime(time);
    }

    private string FormatTime(float t) {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int milliseconds = Mathf.FloorToInt((t * 100f) % 100f);

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);

    }
}
