using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Search;
public class UiWave : MonoBehaviour
{
    [SerializeField] private WaveChecker waveChecker;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private int waveNumber;
    private bool michealIsBeaten;

    void Start()
    {
        waveNumber = waveChecker.WAVE;
    }

    void Update()
    {
        if (waveChecker.WAVE != waveNumber)
        {
            StartCoroutine(WaveCoroutine());
            waveNumber = waveChecker.WAVE;
        }
    }

    IEnumerator WaveCoroutine()
    {
        textMeshPro.text = "WAVE = " + waveChecker.WAVE.ToString("F0");
        yield return new WaitForSeconds(9f);
        textMeshPro.text = "";
    }
}
