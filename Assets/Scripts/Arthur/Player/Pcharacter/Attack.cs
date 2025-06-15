using UnityEngine;
using System;
using System.Collections;

public class Attack : MonoBehaviour
{
    public static event Action OnTridentAttack;
    public static event Action OnSwordAttack;
    public static event Action OnBowRelease;

    [Header("Attack Stats")]
    [SerializeField] private float attackStaminaCost = 20f;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float chargeResetDelay = 0.5f;

    [Header("Dependencies")]
    [SerializeField] private SwitchWeapon switchWeapon;
    [SerializeField] private Move playerMovement;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool canAttack = true;
    private string currentWeapon = "trident";
    private bool isAttacking = false;

    public float Cooldown
    {
        get => attackCooldown;
        set => attackCooldown = value;
    }

    public float StaminaCost
    {
        get => attackStaminaCost;
        set => attackStaminaCost = value;
    }

    public bool IsAttacking
    {
        get => isAttacking;
        set
        {
            isAttacking = value;
            Debug.Log($"IsAttacking set to: {value}");
        }
    }

    public string CurrentWeapon => currentWeapon;

    private void Start()
    {
        playerMovement = GetComponent<Move>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RotatePlayerToMouse();
        HandleAttack();
        SetIdleAnimation();
        TryAttackEvent();
    }

    public void SetCurrentWeapon(string weaponName)
    {
        currentWeapon = weaponName.ToLower();
    }

    private void RotatePlayerToMouse()
    {
        if (Camera.main == null) return;

        Vector2 mouseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButton(1)) // Charging
        {
            StartCharging();

            if (Input.GetMouseButtonDown(0) && canAttack && playerMovement.ConsumeStamina(attackStaminaCost))
            {
                ReleaseAttack();
            }
        }
        else
        {
            ResetCharging();
        }
    }

    private void StartCharging()
    {
        spriteRenderer.color = Color.yellow;
        SetAnimState("charging", true);
        SetAnimState("ready", true);
        SetAnimState("idle", false);
    }

    private void ReleaseAttack()
    {
        SetAnimState("release", true);
        SetAnimState("ready", false);
        IsAttacking = true;
        spriteRenderer.color = Color.red;

        StartCoroutine(AttackCooldownRoutine());
        StartCoroutine(ResetChargeAfter(chargeResetDelay));
        StartCoroutine(ResetReleaseAfter(chargeResetDelay));
    }

    private void ResetCharging()
    {
        SetAnimState("charging", false);
        SetAnimState("ready", false);
        SetAnimState("idle", true);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator AttackCooldownRoutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator ResetChargeAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetAnimState("charging", false);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator ResetReleaseAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetAnimState("release", false);
        SetAnimState("idle", true);
        IsAttacking = false;
    }

    private void SetIdleAnimation()
    {
        SetAnimState("idle", true);
    }

    private void SetAnimState(string action, bool value)
    {
        animator.SetBool($"{currentWeapon}_{action}", value);
    }

    private void TryAttackEvent()
    {
        if (!IsAttacking || playerMovement.Stamina >= attackStaminaCost)
            return;

        switch (currentWeapon)
        {
            case "trident":
                OnTridentAttack?.Invoke();
                Debug.Log("Trident Attack");
                break;
            case "sword":
                OnSwordAttack?.Invoke();
                Debug.Log("Sword Attack");
                break;
            case "bow":
                OnBowRelease?.Invoke();
                Debug.Log("Bow Release");
                break;
        }
    }
}
