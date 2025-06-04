using UnityEngine;

public class Enemyfollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float followDistance = 0.1f;
    [SerializeField] private bool Isfollowing;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public bool follow
    {
        get { return Isfollowing; }
        set { Isfollowing = value; }
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

            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > followDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }
    }
}
