using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyfollow : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 3f;    
    public float followDistance = 0.1f; 
    void Start()
    {
        
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > followDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }
    }
}
