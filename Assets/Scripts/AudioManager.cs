using UnityEngine;

public enum SoundEffect
{
    BulletFire,
    ButtonClick,
    EnemyDeath,
    EnemyHit,
    MeleeAttack,
    PlayerDeath,
    PlayerHit,
    PlayerLowHealth,
    PromotePiece,
    QueenLaser,
    textprint
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private float volume = 1f;
    [SerializeField] private float pitchVariation = 0.05f;
    [SerializeField] private float spatialBlend = 0f;
    [SerializeField] private int poolSize = 16;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] clips;
    
    private AudioSource[] pool;
    private int nextSource;

#if UNITY_EDITOR
    [ContextMenu("Load Clips")]
    private void LoadClips()
    {
        // 1. Force Unity to record this change so the array doesn't clear on Play
        UnityEditor.Undo.RecordObject(this, "Load Audio Clips");

        string[] names = System.Enum.GetNames(typeof(SoundEffect));
        clips = new AudioClip[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets(names[i] + " t:AudioClip", new[] { "Assets/Sound" });
            if (guids.Length == 0)
            {
                //Debug.LogWarning($"AudioManager: no file found for {names[i]}");
                continue;
            }

            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            clips[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        }

        // 2. Ensure changes are saved even if this script is on a Prefab
        UnityEditor.EditorUtility.SetDirty(this);
        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(this))
        {
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }
        
        //Debug.Log("AudioManager: Clips loaded successfully! Don't forget to save your scene/prefab.");
    }
#endif

    private void Awake()
    {
        // 3. Singleton Safety: Destroy duplicates if a scene reloads
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        pool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            // 4. Positional Audio Fix: Create independent child objects for each AudioSource
            GameObject sourceObj = new GameObject($"AudioSource_{i}");
            sourceObj.transform.SetParent(this.transform);
            
            pool[i] = sourceObj.AddComponent<AudioSource>();
            pool[i].playOnAwake = false;
        }
    }

    public static void Play(SoundEffect effect, Vector3? position = null)
    {
        if (Instance == null)
        {
            //Debug.LogWarning("AudioManager: no instance in scene.");
            return;
        }

        Instance.PlayInternal(effect, position);
    }

    private void PlayInternal(SoundEffect effect, Vector3? position)
    {
        if (clips == null || clips.Length == 0)
        {
            //Debug.LogWarning("AudioManager: clips array is empty. Right-click AudioManager and run 'Load Clips'.");
            return;
        }

        int index = (int)effect;
        if (index >= clips.Length || clips[index] == null)
        {
            //Debug.LogWarning($"AudioManager: slot for {effect} is None.");
            return;
        }

        AudioClip clip = clips[index];
        AudioSource source = pool[nextSource];
        nextSource = (nextSource + 1) % pool.Length;

        source.clip = clip;
        source.volume = volume;
        source.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);

        if (position.HasValue)
        {
            source.transform.position = position.Value;
            source.spatialBlend = spatialBlend;
        }
        else
        {
            source.spatialBlend = 0f;
        }

        source.Play();
    }
}