using System;
using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    public static event Action OnMaxEnemySpawn;
    public static event Action OnWaveOver;

    [SerializeField] private int enemyAmmount = 0;
    private int maxEnemyAmount = 15;
    private int enemiesKilled = 0;

    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int enemiesKilledThisWave = 0;
    private int enemiesToKillThisWave = 15;

    public int ETPW
    {
        get { return enemiesKilledThisWave; }
        set { enemiesKilledThisWave = value; }
    }

    private bool waveActive = true;

    void Start()
    {
        EnemySpawner.OnEnemySpawn += CountEnemy;
        Takedamage.onDeath += AddEnemiesKilled;
        Takedamage.onDeath += RemoveEnemiesKilled;
    }

    void Update()
    {
        if (enemyAmmount > maxEnemyAmount)
        {
            OnMaxEnemySpawn?.Invoke();
        }
    }

    private void CountEnemy()
    {
        enemyAmmount++;
    }

    private void AddEnemiesKilled()
    {
        enemiesKilled++;
        if (enemiesKilled == maxEnemyAmount)
        {
            OnWaveOver?.Invoke();
        }
    }

    private void RemoveEnemiesKilled()
    {
        enemiesKilledThisWave++;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public bool IsWaveActive()
    {
        return waveActive;
    }

    public int GetEnemiesToKillThisWave()
    {
        return enemiesToKillThisWave;
    }

    public void EnemyKilled()
    {
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

        StartNextWave();
    }

    private void StartNextWave()
    {
        waveNumber++;
        enemiesKilledThisWave = 0;
        enemiesToKillThisWave++; 
        waveActive = true;
        Debug.Log("Wave " + waveNumber + " started!");
    }
}
