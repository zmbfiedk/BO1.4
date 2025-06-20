using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    private enum BossState { Idle, Walk, Attack, Throw }

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject meleeHitbox;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;  // Your spear/fireball prefab
    [SerializeField] private Transform projectileSpawnPoint; // Throw spawn position

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f;

    // Areas defined as width x height in units
    [SerializeField] private Vector2 meleeAreaSize = new Vector2(5f, 5f);
    [SerializeField] private Vector2 throwAreaSize = new Vector2(10f, 10f);
    [SerializeField] private Vector2 walkAreaSize = new Vector2(20f, 20f);

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float throwSpeed = 15f;

    private BossState currentState = BossState.Idle;
    private bool lastAttackWasMelee = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        meleeHitbox.SetActive(false);
    }

    void Update()
    {
        Vector2 bossPos = transform.position;

        bool inMeleeArea = IsPlayerInArea(bossPos, meleeAreaSize);
        bool inThrowArea = IsPlayerInArea(bossPos, throwAreaSize);
        bool inWalkArea = IsPlayerInArea(bossPos, walkAreaSize);

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        switch (currentState)
        {
            case BossState.Idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isThrowing", false);

                if (inMeleeArea)
                {
                    currentState = BossState.Attack;
                }
                else if (!inMeleeArea && inThrowArea)
                {
                    if (lastAttackWasMelee)
                        currentState = BossState.Throw;
                    else
                        currentState = BossState.Attack;
                }
                else if (!inThrowArea && inWalkArea)
                {
                    currentState = BossState.Walk;
                }
                break;

            case BossState.Walk:
                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isThrowing", false);

                MoveTowardTarget();

                if (inMeleeArea)
                    currentState = BossState.Attack;
                else if (!inMeleeArea && inThrowArea)
                {
                    if (lastAttackWasMelee)
                        currentState = BossState.Throw;
                    else
                        currentState = BossState.Attack;
                }
                break;

            case BossState.Throw:
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isThrowing", true);

                // If player leaves throw area (enters melee), force melee attack next
                if (inMeleeArea)
                {
                    currentState = BossState.Attack;
                    cooldownTimer = 0f;
                }
                else
                {
                    ThrowProjectile();
                    cooldownTimer = attackCooldown;
                    lastAttackWasMelee = false;
                    currentState = BossState.Idle;
                }
                break;

            case BossState.Attack:
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);
                animator.SetBool("isThrowing", false);

                cooldownTimer = attackCooldown;
                lastAttackWasMelee = true;
                currentState = BossState.Idle;
                break;
        }
    }

    bool IsPlayerInArea(Vector2 center, Vector2 size)
    {
        Vector2 playerPos = target.position;
        Vector2 halfSize = size / 2f;

        return (playerPos.x >= center.x - halfSize.x && playerPos.x <= center.x + halfSize.x) &&
               (playerPos.y >= center.y - halfSize.y && playerPos.y <= center.y + halfSize.y);
    }

    public void EnableHitbox()
    {
        meleeHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        meleeHitbox.SetActive(false);
    }

    private void MoveTowardTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void ThrowProjectile()
    {
        if (projectilePrefab == null || projectileSpawnPoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (target.position - projectileSpawnPoint.position).normalized;
            rb.velocity = direction * throwSpeed;
        }
    }
}
