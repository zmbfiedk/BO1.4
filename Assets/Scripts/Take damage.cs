using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //sp.color = new Color(Random.value, Random.value, Random.value); this was a test for hit detection
        Hitdetection();
    }

    public void Hitdetection()
    {
        if (CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }  
    }
}
