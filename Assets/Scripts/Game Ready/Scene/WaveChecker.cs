using System;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    public static event Action OnMaxEnemySpawn;
    public static event Action OnWaveOver;

    [SerializeField] private int enemyAmmount = 0;
    [SerializeField] private int maxEnemyAmount = 1;

    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int enemiesKilledThisWave = 0;
    [SerializeField] private int enemiesToKillThisWave = 1;
    [SerializeField] private int enemiesSpawnedThisWave = 0;
    [SerializeField] private Move Move;

    private bool waveActive = true;


    public int ETPW
    {
        get { return enemiesKilledThisWave; }
        set { enemiesKilledThisWave = value; }
    }

    public int WAVE
    {
        get { return waveNumber; }
        set { waveNumber = value; }
    }

    void Start()
    {
        EnemySpawner.OnEnemySpawn += CountEnemy;
        Takedamage.onDeath += OnEnemyDeath;
    }

    void Update()
    {
        if (enemyAmmount >= maxEnemyAmount)
        {
            OnMaxEnemySpawn?.Invoke();
        }
        maxEnemyAmount = enemiesToKillThisWave;
    }

    private void CountEnemy()
    {
        enemyAmmount++;
        enemiesSpawnedThisWave++;
        Debug.Log($"Enemy Spawned. Current enemy count: {enemyAmmount}");
    }

    private void OnEnemyDeath()
    {
        if (!waveActive) return;

        enemyAmmount = Mathf.Max(0, enemyAmmount - 1);
        enemiesKilledThisWave++;
        Debug.Log($"Enemy Died. Current enemy count: {enemyAmmount}");

        if (enemiesKilledThisWave >= enemiesToKillThisWave)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        waveActive = false;
        Debug.Log("Wave " + waveNumber + " ended!");
        OnWaveOver?.Invoke();
        Move.hp += 50;
        Invoke(nameof(StartNextWave), 5f);
    }

    private void StartNextWave()
    {
        waveNumber+= 10;
        enemiesKilledThisWave = 0;
        enemiesSpawnedThisWave = 0;
        enemyAmmount = 0;
        enemiesToKillThisWave += 10;
        waveActive = true;
        Debug.Log("Wave " + waveNumber + " started! (Level " + GetLevel() + ")");
    }

    public int GetWaveNumber() => waveNumber;
    public bool IsWaveActive() => waveActive;
    public int GetEnemiesToKillThisWave() => enemiesToKillThisWave;
    public int GetEnemiesSpawnedThisWave() => enemiesSpawnedThisWave;

    public int GetLevel() => Mathf.Clamp(1 + ((waveNumber - 1) / 10), 1, 3); // New: Level based on wave
}
