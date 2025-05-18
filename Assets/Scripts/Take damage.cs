using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    public SpriteRenderer sp;
    public Enemyfollow EF;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //sp.color = new Color(Random.value, Random.value, Random.value); this was a test for hit detection
        Hitdetection();
        EnemySee();
    }

    public void Hitdetection()
    {
        if (CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }  
    }

    private void EnemySee()
    {
        if (EF.Isfollowing == true)
        {
            sp.color = Color.red;
        }
        else 
        {
            sp .color = Color.green;
        }
    }
}
