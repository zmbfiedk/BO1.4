using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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

    void Awake()
    {
        waveChecker = FindObjectOfType<WaveChecker>();
        SetTimeUntilSpawn();
    }

    void Update()
    {
        if (!waveChecker.IsWaveActive()) return; 
        timeTillSpawn -= Time.deltaTime;

        if (timeTillSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn();
        }
    }


    private void SetTimeUntilSpawn()
    {
        timeTillSpawn = Random.Range(minimumspawntime, maximumspawntime);
    }

    private void SpawnEnemy()
    {
        int currentWave = waveChecker.GetWaveNumber();

        int level = 1 + (currentWave / 10); 
        level = Mathf.Clamp(level, 1, 3);   
        GameObject enemyToSpawn = null;

        int enemyType = Random.Range(0, 3);

        switch (level)
        {
            case 1:
                if (enemyType == 0) enemyToSpawn = Lmeleeenemyprefab1;
                else if (enemyType == 1) enemyToSpawn = Hmeleeenemyprefab1;
                else enemyToSpawn = Rangeenemyprefab1;
                break;
            case 2:
                if (enemyType == 0) enemyToSpawn = Lmeleeenemyprefab2;
                else if (enemyType == 1) enemyToSpawn = Hmeleeenemyprefab2;
                else enemyToSpawn = Rangeenemyprefab2;
                break;
            case 3:
                if (enemyType == 0) enemyToSpawn = Lmeleeenemyprefab3;
                else if (enemyType == 1) enemyToSpawn = Hmeleeenemyprefab3;
                else enemyToSpawn = Rangeenemyprefab3;
                break;
        }

        if (enemyToSpawn != null)
        {
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }
}
