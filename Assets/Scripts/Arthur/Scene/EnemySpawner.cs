using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static event Action OnEnemySpawn;

    [Header("Level 1")]
    [SerializeField] private GameObject Lmeleeenemyprefab1;
    [SerializeField] private GameObject Hmeleeenemyprefab1;
    [SerializeField] private GameObject Rangeenemyprefab1;

    [Header("Level 2")]
    [SerializeField] private GameObject Lmeleeenemyprefab2;
    [SerializeField] private GameObject Hmeleeenemyprefab2;
    [SerializeField] private GameObject Rangeenemyprefab2;

    [Header("Level 3")]
    [SerializeField] private GameObject Lmeleeenemyprefab3;
    [SerializeField] private GameObject Hmeleeenemyprefab3;
    [SerializeField] private GameObject Rangeenemyprefab3;

    [SerializeField] private float minimumspawntime;
    [SerializeField] private float maximumspawntime;

    private float timeTillSpawn;
    private WaveChecker waveChecker;
    [SerializeField] private bool canSpawm = true;
    [SerializeField] private int level;
    void Awake()
    {
        maximumspawntime = UnityEngine.Random.Range(0, 10);
        minimumspawntime = UnityEngine.Random.Range(10, 15);
        waveChecker = FindObjectOfType<WaveChecker>();
        SetTimeUntilSpawn();
    }

    private void Start()
    {
        WaveChecker.OnMaxEnemySpawn += StopSpawn;
        WaveChecker.OnWaveOver += StopSpawn;
        WaveChecker.OnWaveOver += () => Invoke(nameof(AllowSpawning), 9f); 
    }

    void Update()
    {
        if (!waveChecker.IsWaveActive()) return;
        timeTillSpawn -= Time.deltaTime;

        if (timeTillSpawn <= 0 && canSpawm)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeTillSpawn = UnityEngine.Random.Range(minimumspawntime, maximumspawntime);
    }

    private void SpawnEnemy()
    {
        int currentWave = waveChecker.GetWaveNumber();
        int level = 1 + (currentWave / 10);
        level = Mathf.Clamp(level, 1, 3);
        GameObject enemyToSpawn = null;

        int enemyType = UnityEngine.Random.Range(0, 3);
        OnEnemySpawn?.Invoke();

        switch (level)
        {
            case 1:
                enemyToSpawn = enemyType == 0 ? Lmeleeenemyprefab1 : enemyType == 1 ? Hmeleeenemyprefab1 : Rangeenemyprefab1;
                break;
            case 2:
                enemyToSpawn = enemyType == 0 ? Lmeleeenemyprefab2 : enemyType == 1 ? Hmeleeenemyprefab2 : Rangeenemyprefab2;
                break;
            case 3:
                enemyToSpawn = enemyType == 0 ? Lmeleeenemyprefab3 : enemyType == 1 ? Hmeleeenemyprefab3 : Rangeenemyprefab3;
                break;
        }

        if (enemyToSpawn != null)
        {
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }

    private void StopSpawn()
    {
        canSpawm = false;
    }

    private void AllowSpawning()
    {
        canSpawm = true;
    }
}
