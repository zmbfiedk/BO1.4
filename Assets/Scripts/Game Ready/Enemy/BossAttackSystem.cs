using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSystem : MonoBehaviour
{
    public static event Action ThrowProjectile;

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
    [SerializeField]private bool isAttacking; // Prevent attack spamming

    void Start()
    {
        PlayerPosition = GameObject.FindWithTag("Player").transform;
        BossPostion = GameObject.FindWithTag("Boss").transform;
        isOnTheMove = true;
        rangedTimer = RangedCooldown;
    }

    void Update()
    {
        if (PlayerPosition == null || BossPostion == null) return;

        rangedTimer += Time.deltaTime;

        Vector3 direction = PlayerPosition.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        DistanceToPlayer = Vector3.Distance(PlayerPosition.position, BossPostion.position);

        if (DistanceToPlayer >= 25f)
        {
            isOnTheMove = true;
        }
        if (DistanceToPlayer <= 3.5f && isOnTheMove && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
        }
        else if (DistanceToPlayer <= 20f && !isOnTheMove)
        {
            BossRanged();
        }
        else
        {
            WalkToPlayer();
        }
    }

    private void ResetAllAnimStates()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isAttacking2", false);
        anim.SetBool("isThrowing", false);
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        ResetAllAnimStates();
        isOnTheMove = false;

        int attackType = UnityEngine.Random.Range(1, 3);

        if (attackType == 1)
        {
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking2", true);
        }

        foreach (var col in meleeColliders)
            col.enabled = true;

        yield return new WaitForSeconds(1.5f); 

        foreach (var col in meleeColliders)
            col.enabled = false;

        yield return new WaitForSeconds(1f); 

        isAttacking = false;
    }

    private void BossRanged()
    {
        ResetAllAnimStates();

        if (DistanceToPlayer >= 10f && DistanceToPlayer < 15f)
        {
            if (PlayerPosition != null)
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
        }

        if (rangedTimer >= RangedCooldown)
        {
            ThrowProjectile?.Invoke();
            anim.SetBool("isThrowing", true);
            rangedTimer = 0f;
        }
    }

    private void WalkToPlayer()
    {
        ResetAllAnimStates();

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

            anim.SetBool("isWalking", true);
            isOnTheMove = true;
        }
    }
}
