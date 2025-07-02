using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bossHealthUI;
    [SerializeField] private GameObject[] healthPieces; // 6 pieces

    private Takedamage bossTakeDamage; // Reference to boss health script
    [SerializeField]private bool bossIsSpawned = false;

    private BossSpawner SpawnerN;
    private BossSpawnerF SpawnerF;

    private float pieceHealth;

    public static event Action OnDeath;

    void Start()
    {
        GameObject waveManagerObjectN = GameObject.FindGameObjectWithTag("BossSpawner");
        if (waveManagerObjectN != null)
        {
            SpawnerN = waveManagerObjectN.GetComponent<BossSpawner>();
        }
        else
        {
            Debug.LogWarning("BossSpawner not found in the scene!");
        }

        // Zoek spawnerF
        GameObject waveManagerObjectF = GameObject.FindGameObjectWithTag("BossSpawner");
        if (waveManagerObjectF != null)
        {
            SpawnerF = waveManagerObjectF.GetComponent<BossSpawnerF>();
        }
        else
        {
            Debug.LogWarning("BossSpawnerF not found in the scene!");
        }

        bossHealthUI.SetActive(false);
    }

    void Update()
    {
        bool? bossSpawnedN = SpawnerN != null ? (bool?)SpawnerN.Spawned : null;
        bool? bossSpawnedF = SpawnerF != null ? (bool?)SpawnerF.Spawned : null;

        if (bossSpawnedN == null && bossSpawnedF == null) return;

        bool bossIsSpawned = (bossSpawnedN ?? false) || (bossSpawnedF ?? false);

        if (bossIsSpawned)
        {
            // Boss is spawned - enable UI, update health, etc.
            if (bossTakeDamage == null)
            {
                GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
                if (bossObj != null)
                {
                    bossTakeDamage = bossObj.GetComponent<Takedamage>();
                    if (bossTakeDamage != null)
                    {
                        pieceHealth = bossTakeDamage.CurrentHealth / healthPieces.Length;
                        bossHealthUI.SetActive(true);
                    }
                }
            }
            else
            {
                UpdateHealthUI();
            }
        }
        else
        {
            // Boss not spawned
            bossHealthUI.SetActive(false);
            bossTakeDamage = null;
        }

    }

    private void UpdateHealthUI()
    {
        if (bossTakeDamage == null) return;

        float currentHealth = bossTakeDamage.CurrentHealth;
        int piecesToShow = Mathf.CeilToInt(currentHealth / pieceHealth);

        for (int i = 0; i < healthPieces.Length; i++)
        {
            healthPieces[i].SetActive(i < piecesToShow);
        }

        if(currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
