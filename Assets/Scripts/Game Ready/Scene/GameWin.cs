using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
     WaveCheckerF waveCheckerF;
     WaveCheckerN waveCheckerN;
     private Transform playerTF;
    void Start()
    {
        if (CompareTag("WaveManager"))

            waveCheckerN = FindObjectOfType<WaveCheckerN>();

        GameObject waveManagerObject = GameObject.FindGameObjectWithTag("WaveManager");
        if (waveManagerObject != null)
        {
            waveCheckerN = waveManagerObject.GetComponent<WaveCheckerN>();
        }
        else
        {
            Debug.LogWarning("WaveManager not found in the scene!");
        }

        waveCheckerF = FindObjectOfType<WaveCheckerF>();

        GameObject waveManagerObjectF = GameObject.FindGameObjectWithTag("WaveManager");
        if (waveManagerObject != null)
        {
            waveCheckerF = waveManagerObject.GetComponent<WaveCheckerF>();
        }
        else
        {
            Debug.LogWarning("WaveManager not found in the scene!");
        }

        playerTF = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        float? levelF = waveCheckerF != null ? waveCheckerF.WAVE : (float?)null;
        float? levelN = waveCheckerN != null ? waveCheckerN.WAVE : (float?)null;

        if (levelF == null && levelN == null) return;

        float level = Mathf.Max(levelF ?? float.MinValue, levelN ?? float.MinValue);

        if (level > 40)
        {
            SceneManager.LoadScene(6);
        }
    }
}
