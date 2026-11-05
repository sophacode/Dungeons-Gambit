using UnityEngine;

public class GlobalInfoScript : MonoBehaviour
{
    public static GlobalInfoScript Instance { get; private set; }

    public static bool speedrun = false;
    public static bool fromBoss = false;
    public static bool fromMainMenu = false;
    public static float finalTime = 0f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
