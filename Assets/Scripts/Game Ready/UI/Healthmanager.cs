using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Healthmanager : MonoBehaviour
{
    [SerializeField] Move move_player;
    private TMP_Text textfield;

    void Start()
    {
       textfield = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textfield.text = "HP = " + move_player.hp.ToString("F0");
    }
}
