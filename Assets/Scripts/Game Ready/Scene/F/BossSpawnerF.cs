using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnerF : MonoBehaviour
{
    [SerializeField] private WaveCheckerF waveChecker;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint; // Optional: where to spawn the boss
    public static event Action bossSpawned;

    private bool BossSpawned = false;

    void Update()
    {
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        if (waveChecker.WAVE >= 40 && !BossSpawned)
        {
            Instantiate(bossPrefab, spawnPoint != null ? spawnPoint.position : transform.position, Quaternion.identity);
            BossSpawned = true;
            bossSpawned?.Invoke();
        }
    }
}

