using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Search;
public class UiWave : MonoBehaviour
{
    [SerializeField] WaveChecker waveChecker;
    TMPro.TextMeshPro textMeshPro;
    [SerializeField] private int wavenmr;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        wavenmr = waveChecker.WAVE;
    }


    void Update()
    {
        if (waveChecker.WAVE != wavenmr)
        {
            StartCoroutine(waveCouritine());
            wavenmr = waveChecker.WAVE;
        }
    }

    IEnumerator waveCouritine()
    {
        textMeshPro.text = "WAVE = " + waveChecker.WAVE.ToString("F1");
        yield return new WaitForSeconds(9f);
        textMeshPro.text = "";
    }
}
