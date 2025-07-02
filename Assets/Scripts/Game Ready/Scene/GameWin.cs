using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{

    void Start()
    {
        BossManager.OnDeath += gamewin;
    }


    private void gamewin()
    {
        SceneManager.LoadScene(6);
    }
}
