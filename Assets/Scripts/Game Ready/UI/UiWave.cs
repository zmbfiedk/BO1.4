using System.Collections;
using UnityEngine;
using TMPro;

public class UiWave : MonoBehaviour
{
    [SerializeField] private WaveCheckerN waveCheckerN;       // Can assign in Inspector, or it will find
    [SerializeField] private WaveCheckerF waveCheckerF;       // Can assign in Inspector, or it will find
    [SerializeField] private TextMeshProUGUI textMeshPro;    // Assign in Inspector!

    private int waveNumber;

    void Start()
    {
        // Try to find WaveCheckerN if not assigned
        if (waveCheckerN == null)
        {
            GameObject waveManagerObject = GameObject.FindGameObjectWithTag("WaveManager");
            if (waveManagerObject != null)
            {
                waveCheckerN = waveManagerObject.GetComponent<WaveCheckerN>();
            }
        }

        // Try to find WaveCheckerF if not assigned
        if (waveCheckerF == null)
        {
            GameObject waveManagerObject = GameObject.FindGameObjectWithTag("WaveManager");
            if (waveManagerObject != null)
            {
                waveCheckerF = waveManagerObject.GetComponent<WaveCheckerF>();
            }
        }

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI is NOT assigned in Inspector!");
        }

        // Initialize waveNumber from either checker if available
        if (waveCheckerN != null)
        {
            waveNumber = waveCheckerN.WAVE;
        }
        else if (waveCheckerF != null)
        {
            waveNumber = waveCheckerF.WAVE;
        }
        else
        {
            Debug.LogWarning("No WaveChecker found!");
        }
    }

    void Update()
    {
        if (textMeshPro == null) return;

        int currentWave = 0;
        float level = 0;

        // Check which checker is available and get data
        if (waveCheckerN != null)
        {
            currentWave = waveCheckerN.WAVE;
            level = waveCheckerN.GetLevel();
        }
        else if (waveCheckerF != null)
        {
            currentWave = waveCheckerF.WAVE;
            level = waveCheckerF.GetLevel();
        }
        else
        {
            return;
        }

        if (currentWave != waveNumber)
        {
            StartCoroutine(WaveCoroutine(currentWave, level));
            waveNumber = currentWave;
        }

        if (currentWave == 0)
        {
            textMeshPro.text = "kill an enemy to progress to the next wave";
        }
    }

    IEnumerator WaveCoroutine(int wave, float level)
    {
        textMeshPro.text = "Wave " + wave + " started! (Level " + level + ")";
        yield return new WaitForSeconds(2f);
        textMeshPro.text = "";
    }
}
