using UnityEngine;

public class GlobalInfoScript : MonoBehaviour
{
    public static GlobalInfoScript Instance { get; private set; }

    public static bool speedrun = false;
    public static bool fromBoss = false;


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
