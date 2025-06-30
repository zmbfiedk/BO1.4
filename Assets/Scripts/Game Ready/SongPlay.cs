using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlay : MonoBehaviour
{
    [SerializeField] AudioSource[] soundtrack; // Zorg dat er 2 audiosources zijn
    [SerializeField] BossSpawner spawnerN;
    [SerializeField] BossSpawnerF spawnerF;

    private int currentSongIndex = -1; // Huidige actieve song (-1 = geen)

    void Start()
    {
        // Zoek spawnerN
        spawnerN = FindObjectOfType<BossSpawner>();
        if (spawnerN == null)
        {
            Debug.LogWarning("BossSpawner not found in the scene!");
        }

        // Zoek spawnerF
        GameObject waveManagerObjectF = GameObject.FindGameObjectWithTag("BossSpawner");
        if (waveManagerObjectF != null)
        {
            spawnerF = waveManagerObjectF.GetComponent<BossSpawnerF>();
        }
        else
        {
            Debug.LogWarning("BossSpawnerF not found in the scene!");
        }
    }

    void Update()
    {
        bool bossspawnedF = spawnerF != null && spawnerF.Spawned;
        bool bossspawnedN = spawnerN != null && spawnerN.Spawned;

        int desiredSongIndex = (bossspawnedF || bossspawnedN) ? 1 : 0;

        if (desiredSongIndex != currentSongIndex)
        {
            SwitchSong(desiredSongIndex);
        }
    }

    void SwitchSong(int index)
    {
        for (int i = 0; i < soundtrack.Length; i++)
        {
            if (soundtrack[i].isPlaying)
                soundtrack[i].Stop();
        }

        if (!soundtrack[index].isPlaying)
        {
            soundtrack[index].Play();
        }

        currentSongIndex = index;
    }
}
