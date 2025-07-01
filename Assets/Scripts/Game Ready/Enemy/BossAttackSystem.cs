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

    [Header("Sounds")]
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip pokeSound;  // attack type 1
    [SerializeField] private AudioClip slashSound; // attack type 2

    private AudioSource audioSource;

    private void Start()
    {
        PlayerPosition = GameObject.FindWithTag("Player").transform;
        BossPostion = GameObject.FindWithTag("Boss").transform;
        isOnTheMove = true;
        rangedTimer = RangedCooldown;
        outOfRangeTimer = 0f;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (PlayerPosition == null || BossPostion == null) return;

        rangedTimer += Time.deltaTime;

        Vector3 direction = PlayerPosition.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        DistanceToPlayer = Vector3.Distance(PlayerPosition.position, BossPostion.position);

        if (DistanceToPlayer > 15f)
        {
            outOfRangeTimer += Time.deltaTime;
        }
        else
        {
            outOfRangeTimer = 0f;
        }

        if (outOfRangeTimer >= outOfRangeTimeThreshold && !isAttacking)
        {
            Debug.Log("Player ran too far — forcing melee attack!");
            StartCoroutine(MeleeAttack());
            outOfRangeTimer = 0f;
            return;
        }

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

        // Play correct attack sound
        if (attackType == 1 && pokeSound != null)
        {
            audioSource.PlayOneShot(pokeSound);
        }
        else if (attackType == 2 && slashSound != null)
        {
            audioSource.PlayOneShot(slashSound);
        }

        yield return new WaitForSeconds(1f);

        foreach (var col in meleeColliders)
            col.enabled = true;

        yield return new WaitForSeconds(.4f);

        foreach (var col in meleeColliders)
            col.enabled = false;

        yield return new WaitForSeconds(1f);

        SetAttackType(0);
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
            SetAttackType(3);
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

            SetAttackType(4);
            isOnTheMove = true;

            if (walkingSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(walkingSound);
            }
        }
        else
        {
            SetAttackType(0);
            isOnTheMove = false;
        }
    }
}
