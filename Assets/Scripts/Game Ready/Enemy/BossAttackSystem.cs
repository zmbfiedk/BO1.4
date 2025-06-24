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
    [SerializeField] private float followDistance = 2.5f;

    private float rangedTimer;
    [SerializeField] private bool isAttacking;

    private void Start()
    {
        PlayerPosition = GameObject.FindWithTag("Player").transform;
        BossPostion = GameObject.FindWithTag("Boss").transform;
        isOnTheMove = true;
        rangedTimer = RangedCooldown;
    }

    private void Update()
    {
        if (PlayerPosition == null || BossPostion == null) return;

        rangedTimer += Time.deltaTime;

        Vector3 direction = PlayerPosition.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        DistanceToPlayer = Vector3.Distance(PlayerPosition.position, BossPostion.position);

        if (DistanceToPlayer >= 20f)
        {
            isOnTheMove = true;
        }

        if (DistanceToPlayer <= 3.5f && isOnTheMove && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
        }
        else if (DistanceToPlayer <= 15f && !isOnTheMove)
        {
            BossRanged();
        }
        else
        {
            WalkToPlayer();
        }
    }

    private void SetAttackType(int type)
    {
        anim.SetInteger("AttackType", type);
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        isOnTheMove = false;

        int attackType = UnityEngine.Random.Range(1, 3); // 1 or 2
        Debug.Log($"Melee Attack Triggered. AttackType: {attackType}");

        SetAttackType(attackType);

        foreach (var col in meleeColliders)
            col.enabled = true;

        yield return new WaitForSeconds(1.5f); // match animation length

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
        }
    }
}
