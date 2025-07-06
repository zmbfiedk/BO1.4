using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    GameObject player;
    private Vector2 movedirection;
    [SerializeField] private float speed;

    private int lightEnemyHits = 0;
    private int rangedEnemyHits = 0;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        getdirection();
    }

    void Update()
    {
        transform.position += (Vector3)(movedirection * speed * Time.deltaTime);
    }

    private void getdirection()
    {
        movedirection = player.transform.position - gameObject.transform.position;
        movedirection = movedirection.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("heavyEnemy"))
        {
            // Destroy projectile immediately if it hits a heavy enemy
            Destroy(gameObject);
        }
        else if (collision.CompareTag("lightEnemy"))
        {
            lightEnemyHits++;
            if (lightEnemyHits >= 2)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("projEnemy"))
        {
            rangedEnemyHits++;
            if (rangedEnemyHits >= 3)
            {
                Destroy(gameObject);
            }
        }
    }
}
