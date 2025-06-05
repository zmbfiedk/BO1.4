using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StaminaUI : MonoBehaviour
{
    [SerializeField]private Move moveScript;
    private TMP_Text textfield;

    void Start()
    {
        textfield = GetComponent<TMP_Text>();
    }

    void Update()
    {
        textfield.text = "Stamina = " + moveScript.Stamina.ToString("F0");
    }
}
