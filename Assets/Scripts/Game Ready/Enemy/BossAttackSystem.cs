using System;
using System.Collections;
using UnityEngine;

public class BossAttackSystem : MonoBehaviour
{
    public event Action ThrowProjectile;

    [SerializeField] private bool isOnTheMove;
    [SerializeField] private float movespeed = 3f;
    [SerializeField] private Transform PlayerPosition;
    [SerializeField] private Transform BossPostion;
    [SerializeField] private Collider2D[] meleeColliders;
    [SerializeField] private Animator anim;
    [SerializeField] private float DistanceToPlayer;
    [SerializeField] private float RangedCooldown = 2f;
    [SerializeField] private float followDistance = 8f;

    [SerializeField] private float outOfRangeTimeThreshold = 3f;
    private float outOfRangeTimer;

    private float rangedTimer;
    [SerializeField] private bool isAttacking;

    private void Start()
    {
        PlayerPosition = GameObject.FindWithTag("Player").transform;
        BossPostion = GameObject.FindWithTag("Boss").transform;
        isOnTheMove = true;
        rangedTimer = RangedCooldown;
        outOfRangeTimer = 0f;
    }

    private void Update()
    {
        if (PlayerPosition == null || BossPostion == null) return;

        rangedTimer += Time.deltaTime;

        Vector3 direction = PlayerPosition.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        DistanceToPlayer = Vector3.Distance(PlayerPosition.position, BossPostion.position);

        // Track time player stays out of range
        if (DistanceToPlayer > 15f)
        {
            outOfRangeTimer += Time.deltaTime;
        }
        else
        {
            outOfRangeTimer = 0f;
        }

        // Force melee if player runs away for too long
        if (outOfRangeTimer >= outOfRangeTimeThreshold && !isAttacking)
        {
            Debug.Log("Player ran too far — forcing melee attack!");
            StartCoroutine(MeleeAttack());
            outOfRangeTimer = 0f;
            return;
        }

        // Normal attack logic
        if (DistanceToPlayer <= 7.5f && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
        }
        else if (DistanceToPlayer > 7.5f && DistanceToPlayer <= 15f && !isAttacking)
        {
            isOnTheMove = false;
            BossRanged();
        }
        else if (DistanceToPlayer > followDistance)
        {
            WalkToPlayer();
        }
        else
        {
            SetAttackType(0); // Idle
            isOnTheMove = false;
        }
    }

    private void SetAttackType(int type)
    {
        anim.SetInteger("SetAttackType", type);
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        isOnTheMove = false;

        int attackType = UnityEngine.Random.Range(1, 3); // 1 or 2
        Debug.Log($"Melee Attack Triggered. AttackType: {attackType}");

        SetAttackType(attackType);

        yield return new WaitForSeconds(1.7f); // adjust this to match your animation wind-up

        foreach (var col in meleeColliders)
            col.enabled = true;

        yield return new WaitForSeconds(.7f); // active hitbox duration, adjust as needed

        foreach (var col in meleeColliders)
            col.enabled = false;

        yield return new WaitForSeconds(1f); // cooldown buffer

        SetAttackType(0); // reset to idle
        isAttacking = false;
    }

    private void BossRanged()
    {
        if (DistanceToPlayer >= 10f && DistanceToPlayer < 15f)
        {
            Vector3 scale = transform.localScale;
            scale.x = (PlayerPosition.position.x < transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;

            Vector2 direction = (transform.position - PlayerPosition.position).normalized;
            Vector2 targetPosition = (Vector2)transform.position + direction;

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                movespeed * Time.deltaTime
            );
        }

        if (rangedTimer >= RangedCooldown)
        {
            ThrowProjectile?.Invoke();
            SetAttackType(3); // Throw animation
            rangedTimer = 0f;
        }
    }

    private void WalkToPlayer()
    {
        if (PlayerPosition != null && DistanceToPlayer > followDistance)
        {
            Vector3 scale = transform.localScale;
            scale.x = (PlayerPosition.position.x < transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;

            Vector2 direction = (PlayerPosition.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(
                transform.position,
                PlayerPosition.position,
                movespeed * Time.deltaTime
            );

            SetAttackType(4); // Walking animation
            isOnTheMove = true;
        }
        else
        {
            SetAttackType(0); // Idle
            isOnTheMove = false;
        }
    }
}
