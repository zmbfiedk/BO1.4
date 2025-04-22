using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    public SpriteRenderer sp;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        sp.color = new Color(Random.value, Random.value, Random.value);
    }
}
