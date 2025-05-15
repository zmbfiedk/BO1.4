using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    [SerializeField] private Attack atackScript;
    [SerializeField] private GameObject Tridentprefab;
    [SerializeField] private GameObject bowprefab;
    [SerializeField] private GameObject swordprefab;
    void Start()
    {
        GetComponent<Attack>();
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            trident(true);
            bow(false);
            sword(false);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            trident(false);
            bow(true);
            sword(false);
        }
        if (Input.GetKey(KeyCode.Alpha3)) 
        {
            trident(false);
            bow(false);
            sword(true);
        }
    }

    private void trident(bool activate)
    {
           Tridentprefab.gameObject.SetActive(activate);
           atackScript.ACD = 0.3f;

    }

    private void bow(bool activate)
    {
            bowprefab.gameObject.SetActive(activate);
            atackScript.ACD = 0.1f;
    }

    private void sword(bool activate)
    {
            swordprefab.gameObject.SetActive(activate);
            atackScript.ACD = 0.22f;
    }
}
