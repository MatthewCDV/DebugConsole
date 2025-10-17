using UnityEngine;

public class SpawnObjectsScript : MonoBehaviour
{
    // Singleton
    public static SpawnObjectsScript Instance { get; private set; }

    [Header("Prefab")]
    [SerializeField] private GameObject prefab;

    [Header("Spawn target/limit")]
    [SerializeField] private Transform parent;
    [SerializeField] private int maxCount = 100;

    [Header("Spawn behaviour")]
    [SerializeField] private bool spawnOnStart = false;
    [SerializeField] private bool repeatSpawn = false;
    [SerializeField] private float spawnInterval = 1f; // sekundy, u¿ywane gdy repeatSpawn == true
    [SerializeField] private KeyCode spawnKey = KeyCode.Space; // spawn przy nacisniêciu

    [Header("Spawn area (center = this.transform.position)")]
    [SerializeField] private Vector3 areaSize = new Vector3(1f, 1f, 1f);

    private float timer;
    private int spawnedCount;

    private void Awake()
    {
        // Prosty singleton: jeœli ju¿ istnieje instancja, usuñ duplikat.
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Destroying duplicate SpawnObjectsScript on '{gameObject.name}'. Use SpawnObjectsScript.Instance to access the spawner.");
            Destroy(this);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        timer = 0f;
        spawnedCount = 0;

        if (spawnOnStart)
            TrySpawn();
    }

    void Update()
    {
        if (prefab == null) return;

        if (repeatSpawn && spawnedCount < maxCount)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                TrySpawn();
            }
        }

        //if (Input.GetKeyDown(spawnKey))
        //    TrySpawn();
    }
    
    public void TrySpawn()
    {
        if (prefab == null) return;
        if (spawnedCount >= maxCount) return;

        Vector3 pos = GetRandomPositionInArea();
        GameObject go = Instantiate(prefab, pos, Quaternion.identity, parent);
        spawnedCount++;
    }

    public void SpawnMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (spawnedCount >= maxCount) break;
            TrySpawn();
        }
    }

    public void ClearSpawned()
    {
        if (parent == null) return;

        // usuwa tylko bezpoœrednie dzieci parenta
        for (int i = parent.childCount - 1; i >= 0; i--)
            DestroyImmediate(parent.GetChild(i).gameObject);

        spawnedCount = 0;
    }

    private Vector3 GetRandomPositionInArea()
    {
        Vector3 half = areaSize * 0.5f;
        float x = Random.Range(-half.x, half.x);
        float y = Random.Range(-half.y, half.y);
        float z = Random.Range(-half.z, half.z);
        return transform.position + new Vector3(x, y, z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawCube(transform.position, areaSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
