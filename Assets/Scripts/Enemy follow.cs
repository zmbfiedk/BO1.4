
using UnityEngine;

public class Enemyfollow : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 3f;    
    public float followDistance = 0.1f;
    public bool Isfollowing;
    void Start()
    {
        
    }

    void Update()
    {
        Followplayer();
    }

    public void Followplayer()
    {
        if (player != null)
        {
            Isfollowing = true;
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
