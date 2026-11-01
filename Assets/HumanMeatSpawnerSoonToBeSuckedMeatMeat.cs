using UnityEngine;
using Sirenix.OdinInspector;

public class HumanMeatSpawnerSoonToBeSuckedMeatMeat : MonoBehaviour
{
    [SerializeField] private HumanData[] humanPrefabs;

    [Header("Fresh meat")]
    [SerializeField] int minHumans = 8;
    [SerializeField] int maxHumans = 25;
    [SerializeField] bool countExistingSceneHumans = true;

    [Header("Make Meat")]
    [SerializeField] float minSpawnTime = 0.3f;
    [SerializeField] float maxSpawnTime = 1.5f;
    [SerializeField] float spawnRateTweenDuration = 0.5f;

    [SerializeField] float minSpawnRadius = 11f;
    [SerializeField] float maxSpawnRadius = 16f;

    [Header("Debug")]
    [SerializeField, ReadOnly] int aliveCount;
    [SerializeField, ReadOnly] float currentSpawnRate;
    [SerializeField, ReadOnly] float targetSpawnRate;

    float spawnTimer;

    void Start()
    {
        for (int i = 0; i < minHumans; i++)
        {
            Spawn();
        }

        targetSpawnRate = CalculateTargetSpawnRate();
        currentSpawnRate = targetSpawnRate;
        spawnTimer = currentSpawnRate;
    }

    void Update()
    {
        if (aliveCount >= maxHumans) return;

        targetSpawnRate = CalculateTargetSpawnRate();
        currentSpawnRate = targetSpawnRate;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            Spawn();
            spawnTimer = currentSpawnRate;
        }
    }

    private float CalculateTargetSpawnRate()
    {
        float t = Mathf.InverseLerp(minHumans, maxHumans, aliveCount);
        return Mathf.Lerp(minSpawnTime, maxSpawnTime, t);
    }

    public void MeatSucked()
    {
        aliveCount = Mathf.Max(0, aliveCount - 1);
    }

    private void Spawn()
    {
        if (humanPrefabs == null || humanPrefabs.Length == 0) return;

        HumanData data = humanPrefabs[Random.Range(0, humanPrefabs.Length)];
        if (data == null || data.Prefab == null) return;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;

        HumanHealth human = Instantiate(data.Prefab, transform.position + offset, Quaternion.identity);
        human.SetHumanData(data);
        human.SetSpawner(this);
        aliveCount++;
    }
}
