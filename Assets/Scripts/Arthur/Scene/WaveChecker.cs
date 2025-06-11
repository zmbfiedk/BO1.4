using System;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    public static event Action OnMaxEnemySpawn;
    public static event Action OnWaveOver;

    [SerializeField] private int enemyAmmount = 0;
    [SerializeField] private int maxEnemyAmount = 15;

    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int enemiesKilledThisWave = 0;
    [SerializeField] private int enemiesToKillThisWave = 15;
    [SerializeField] private int enemiesSpawnedThisWave = 0;

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
    }

    private void OnEnemyDeath()
    {
        enemyAmmount--;
        enemiesKilledThisWave++;

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

        Invoke(nameof(StartNextWave), 5f);
    }

    private void StartNextWave()
    {
        waveNumber++;
        enemiesKilledThisWave = 0;
        enemiesSpawnedThisWave = 0;
        enemiesToKillThisWave += 5;
        waveActive = true;
        Debug.Log("Wave " + waveNumber + " started! (Level " + GetLevel() + ")");
    }

    public int GetWaveNumber() => waveNumber;
    public bool IsWaveActive() => waveActive;
    public int GetEnemiesToKillThisWave() => enemiesToKillThisWave;
    public int GetEnemiesSpawnedThisWave() => enemiesSpawnedThisWave;

    public int GetLevel() => Mathf.Clamp(1 + ((waveNumber - 1) / 10), 1, 3); // New: Level based on wave
}
