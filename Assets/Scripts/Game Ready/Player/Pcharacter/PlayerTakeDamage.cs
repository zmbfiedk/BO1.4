using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{

    [SerializeField]private Move Move;

    void Start()
    {
        
    }



    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("lightEnemy"))
        {
            Move.hp -= 5;
        }
        if (other.CompareTag("heavyEnemy"))
        {
            Move.hp -= 15;
        }
        if (other.CompareTag("projEnemy"))
        {
            Move.hp -= 3;
        }
    }
}
