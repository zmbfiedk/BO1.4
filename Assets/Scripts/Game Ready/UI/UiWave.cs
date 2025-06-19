using System.Collections;
using UnityEngine;
using TMPro;

public class UiWave : MonoBehaviour
{
    [SerializeField] private WaveCheckerN waveChecker;          // Assign in Inspector!
    [SerializeField] private TextMeshProUGUI textMeshPro;      // Assign in Inspector!

    private int waveNumber;

    void Start()
    {
        if (waveChecker == null)
        {
            Debug.LogError("WaveChecker is NOT assigned in Inspector!");
        }

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI is NOT assigned in Inspector!");
        }

        waveNumber = waveChecker != null ? waveChecker.WAVE : 0;
    }

    void Update()
    {
        if (waveChecker == null || textMeshPro == null)
            return;

        if (waveChecker.WAVE != waveNumber)
        {
            StartCoroutine(WaveCoroutine());
            waveNumber = waveChecker.WAVE;
        }

        if(waveChecker.WAVE == 0)
        {
            textMeshPro.text = "kill an enemy to progress to the next wave";
        }
    }

    IEnumerator WaveCoroutine()
    {
        textMeshPro.text = "Wave " + waveChecker.WAVE + " started! (Level " + waveChecker.GetLevel() + ")";
        yield return new WaitForSeconds(2f);
        textMeshPro.text = "";
    }
}
