using UnityEngine;

public class EnemyAvoid : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float avoidDistance = 0.1f;
    [SerializeField] private bool isAvoiding;
    public float rotationSpeed = 5f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public bool Avoiding
    {
        get { return isAvoiding; }
        set { isAvoiding = value; }
    }

    void Update()
    {
        AvoidPlayer();
        Vector3 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Keep looking at the player
    }
    public void AvoidPlayer()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance < avoidDistance)
            {
                isAvoiding = true;

                // Flip visually
                Vector3 scale = transform.localScale;
                scale.x = (player.position.x < transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                transform.localScale = scale;
                Vector2 direction = (transform.position - player.position).normalized;
                Vector2 targetPosition = (Vector2)transform.position + direction * avoidDistance;

                transform.position = Vector2.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );
            }
            else
            {
                isAvoiding = false;
            }
        }
    }

}

