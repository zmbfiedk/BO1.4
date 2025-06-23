using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSystem : MonoBehaviour
{

    public static event Action ThrowProjectile;

    [SerializeField] private bool isOnTheMove;
    [SerializeField] private float movespeed = 3;
    [SerializeField] private Transform PlayerPosition;
    [SerializeField] private Transform BossPostion;
    [SerializeField] private Collider2D[] meleeColliders;
    [SerializeField] private Animator anim;
    [SerializeField] private float DistanceToPlayer;
    [SerializeField] private float RangedCooldown;
    [SerializeField] private float followDistance = 2.5f;

    void Start()
    {
        PlayerPosition = GameObject.FindWithTag("Player").transform;
        BossPostion = GameObject.FindWithTag("Boss").transform;
        isOnTheMove = true;
    }


    void Update()
    {
        Vector3 direction = PlayerPosition.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle += 90);

        DistanceToPlayer = Vector3.Distance(PlayerPosition.transform.position, BossPostion.transform.position);

        if ( isOnTheMove && DistanceToPlayer <= 5)
        {
            BossMelee();
        }
        else if ( DistanceToPlayer <= 15)
        {
            BossRanged();
        }
        else if (isOnTheMove)
        {
            WalkToPlayer();
        }
    }

    private void BossMelee()
    {
        
        anim.SetBool("isAttacking" , true);
        isOnTheMove = false;
    }

    private void BossRanged()
    {
        if (DistanceToPlayer >= 7.5 && DistanceToPlayer < 10) 
        {
            if (PlayerPosition != null)
            {

                // Flip visually
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
        ThrowProjectile?.Invoke();
        anim.SetBool("isThrowing", true);
        isOnTheMove = false;
    }

    private void WalkToPlayer()
    {
        if (PlayerPosition != null)
        {

            Vector3 scale = transform.localScale;
            if (PlayerPosition.position.x < transform.position.x)
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            else
            {
                scale.x = Mathf.Abs(scale.x);
            }
            transform.localScale = scale;

            float distance = Vector2.Distance(transform.position, PlayerPosition.position);
            if (distance > followDistance)
            {
                Vector2 direction = (PlayerPosition.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    PlayerPosition.position,
                    movespeed * Time.deltaTime
                );
                isOnTheMove = false ;
            }
        }

        anim.SetBool("isWalking" , true);
        isOnTheMove = true;
    }

    
}
