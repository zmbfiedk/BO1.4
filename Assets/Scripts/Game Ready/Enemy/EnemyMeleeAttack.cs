using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float rangeX = 1f;
    public float rangeY = 1f;
    public Transform player;

    [SerializeField] private Collider2D[] WeaponHitboxes;
    [SerializeField] private float AttackActiveDuration = 1f;
    [SerializeField] private float attackCooldown = 1f;

    private Animator anim;
    private bool isOnCooldown = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        DisableAllHitboxes();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isOnCooldown && IsplayerInRange())
        {
            StartCoroutine(AttackRoutine());
        }
    }

    bool IsplayerInRange()
    {
        Vector3 delta = player.position - transform.position;

        return Mathf.Abs(delta.x) <= rangeX && Mathf.Abs(delta.y) <= rangeY;
    }

    private IEnumerator AttackRoutine()
    {
        isOnCooldown = true;

        ActivateEnemyHitboxes();
        anim.SetBool("Attack", true);

        yield return new WaitForSeconds(AttackActiveDuration);

        anim.SetBool("Attack", false);
        DisableAllHitboxes();

        yield return new WaitForSeconds(attackCooldown);

        isOnCooldown = false;
    }

    private void DisableAllHitboxes()
    {
        SetCollidersEnabled(WeaponHitboxes, false);
    }

    private void ActivateEnemyHitboxes()
    {
        SetCollidersEnabled(WeaponHitboxes, true);
    }

    private void SetCollidersEnabled(Collider2D[] colliders, bool enabled)
    {
        foreach (var col in colliders)
        {
            if (col != null)
                col.enabled = enabled;
        }
    }
}
