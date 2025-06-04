using UnityEngine;

public class WaveChecker : MonoBehaviour
{
    [SerializeField] private int waveNumber = 0;
    [SerializeField] private int enemiesKilledThisWave = 0;
    [SerializeField] private int enemiesToKillPerWave = 15;

    private bool waveActive = true;

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public bool IsWaveActive()
    {
        return waveActive;
    }

    public void EnemyKilled()
    {
        enemiesKilledThisWave++;

        if (enemiesKilledThisWave >= enemiesToKillPerWave)
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
        waveActive = true;
        Debug.Log("Wave " + waveNumber + " started!");
    }
}
