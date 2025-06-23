using UnityEngine;

public class Enemyfollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float followDistance = 0.1f;
    [SerializeField] private bool Isfollowing;
    public float rotationSpeed = 5f;

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
        Vector3 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void Followplayer()
    {
        if (player != null)
        {
            Isfollowing = true;

            Vector3 scale = transform.localScale;
            if (player.position.x < transform.position.x)
            {
                scale.x = -Mathf.Abs(scale.x); 
            }
            else
            {
                scale.x = Mathf.Abs(scale.x); 
            }
            transform.localScale = scale;

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
