using UnityEngine;
using System;
using System.Collections;

public class Attack : MonoBehaviour
{
    // Events to notify other scripts when specific attacks happen
    public static event Action OnAtckTri;
    public static event Action OnAttackSw;
    public static event Action OnBowRelease;

    [Header("Attack Stats")]
    [SerializeField] private float attackStaminaCost = 20f;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float chargeResetDelay = 0.5f;

    [Header("Needed scripts")]
    [SerializeField] private SwitchWeapon SW;
    [SerializeField] private Move playerMovement;

    // Components references
    private SpriteRenderer sp;
    private Animator animator;

    // State variables
    private bool canAttack = true;
    private string currentWeapon = "trident";
    public string CurrentWeapon => currentWeapon; // Public getter for current weapon
    private bool isAttacking = false;

    // Properties to access private fields
    public float ACD
    {
        get { return attackCooldown; }
        set { attackCooldown = value; }
    }

    public float staminaCost
    {
        get { return attackStaminaCost; }
        set { attackStaminaCost = value; }
    }

    public bool isAtacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    void Start()
    {
        // Get required component references at start
        playerMovement = GetComponent<Move>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RotatePlayerToMouse();
        HandleAttack();

        // Set idle animation based on current weapon by default every frame
        animator.SetBool($"{currentWeapon}_idle", true);

        // Check for specific weapon attack triggers
        TridentAttack();
        SwordAttack();
        BowRelease();
    }

    // Set the current weapon by name, converted to lowercase for consistency
    public void SetCurrentWeapon(string weaponName)
    {
        currentWeapon = weaponName.ToLower();
    }

    // Rotate player to face the mouse pointer position
    void RotatePlayerToMouse()
    {
        Vector2 directionToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    // Handle attack input and animations
    void HandleAttack()
    {
        if (Input.GetMouseButton(1)) // Right mouse button held down (charging attack)
        {
            sp.color = Color.yellow;

            // Set charging and ready animations on
            animator.SetBool($"{currentWeapon}_charging", true);
            animator.SetBool($"{currentWeapon}_ready", true);
            animator.SetBool($"{currentWeapon}_idle", false);

            // On left click (mouse button down) while charging and if can attack
            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                // Try consuming stamina; only proceed if successful
                if (playerMovement.ConsumeStamina(attackStaminaCost))
                {
                    animator.SetBool($"{currentWeapon}_release", true);
                    animator.SetBool($"{currentWeapon}_ready", false);
                    isAttacking = true;
                    sp.color = Color.red;

                    // Start cooldown and reset coroutines
                    StartCoroutine(AttackCooldown());
                    StartCoroutine(ResetChargeAfter(chargeResetDelay));
                    StartCoroutine(ResetReleaseAfter(chargeResetDelay));
                }
            }
        }
        else // Right mouse button not held - reset animations and color
        {
            animator.SetBool($"{currentWeapon}_charging", false);
            animator.SetBool($"{currentWeapon}_ready", false);
            animator.SetBool($"{currentWeapon}_idle", true);
            sp.color = Color.white;
        }
    }

    // Coroutine to enforce attack cooldown
    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Coroutine to reset charging animation and color after a delay
    IEnumerator ResetChargeAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool($"{currentWeapon}_charging", false);
        sp.color = Color.white;
    }

    // Coroutine to reset release animation and set idle after a delay
    IEnumerator ResetReleaseAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool($"{currentWeapon}_release", false);
        animator.SetBool($"{currentWeapon}_idle", true);
    }

    // Invoke Trident attack event if conditions are met
    private void TridentAttack()
    {
        if (currentWeapon == "trident" && playerMovement.Stamina <= staminaCost && isAttacking)
        {
            OnAtckTri?.Invoke();
        }
    }

    // Invoke Sword attack event if conditions are met
    private void SwordAttack()
    {
        if (currentWeapon == "sword" && playerMovement.Stamina <= staminaCost && isAttacking)
        {
            OnAttackSw?.Invoke();
        }
    }

    // Invoke Bow release event if conditions are met
    private void BowRelease()
    {
        if (currentWeapon == "bow" && playerMovement.Stamina >= staminaCost && isAttacking)
        {
            OnBowRelease?.Invoke();
            Debug.Log("invoked");
        }
    }
}
