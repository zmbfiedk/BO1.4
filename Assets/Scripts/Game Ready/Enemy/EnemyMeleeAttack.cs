using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float rangeX = 1f;
    public float rangeY = 1f;
    public Transform player;
    // Start is called before the first frame update
    [SerializeField] private Collider2D[] WeaponHitboxes;
    [SerializeField] private float AttackActiveDuration = 1f;
    private Animator anim;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        DisableAllHitboxes();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        IsplayerInRange();
        if (IsplayerInRange())
        {
            StartAttack();
        }
    }
    bool IsplayerInRange()
    {
        Vector3 delta = player.position - transform.position;

        if(Mathf.Abs(delta.x) <= rangeX && Mathf.Abs(delta.y) <= rangeY)
        {
            return true;
        }
        return false;
        
    }
    private void StartAttack()
    {
        ActivateEnemyHitboxes();

    }

    private void DisableAllHitboxes()
    {
        SetCollidersEnabled(WeaponHitboxes, false);
    }

    private void ActivateEnemyHitboxes()
    {
        StartCoroutine(ActivateHitboxesRoutine(WeaponHitboxes, AttackActiveDuration));
    }

    private IEnumerator ActivateHitboxesRoutine(Collider2D[] colliders, float duration)
    {
        SetCollidersEnabled(colliders, true);
        anim.SetBool("", true);
        yield return new WaitForSeconds(duration);
        anim.SetBool("", false);
        SetCollidersEnabled(colliders, false);
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
