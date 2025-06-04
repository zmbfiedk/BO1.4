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
        EnemySee();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Hitdetection();
    }

    public void Hitdetection()
    {
        if (CompareTag("enemy"))
        {
            Destroy(gameObject);
        }  
    }

    private void EnemySee()
    {
        if (EF.follow == true)
        {
            sp.color = Color.red;
        }
        else 
        {
            sp .color = Color.green;
        }
    }
}
