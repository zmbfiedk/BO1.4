using UnityEngine;
using System;
using System.Collections;

public class Attack : MonoBehaviour
{
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

    private SpriteRenderer sp;
    private Animator animator;
    private bool canAttack = true;
    private string currentWeapon = "trident";
    public string CurrentWeapon => currentWeapon;
    private bool isAttacking = false;

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
        set {  isAttacking = value; }
    }

    void Start()
    {
        playerMovement = GetComponent<Move>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RotatePlayerToMouse();
        HandleAttack();
        animator.SetBool($"{currentWeapon}_idle", true);
        TridentAttack();
        SwordAttack();
        BowRelease();
    }

    public void SetCurrentWeapon(string weaponName)
    {
        currentWeapon = weaponName.ToLower();
    }

    void RotatePlayerToMouse()
    {
        Vector2 directionToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    void HandleAttack()
    {
        if (Input.GetMouseButton(1))
        {
            sp.color = Color.yellow;

            animator.SetBool($"{currentWeapon}_charging", true);
            animator.SetBool($"{currentWeapon}_ready", true);
            animator.SetBool($"{currentWeapon}_idle", false);

            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                if (playerMovement.ConsumeStamina(attackStaminaCost))
                {
                    animator.SetBool($"{currentWeapon}_release", true);
                    animator.SetBool($"{currentWeapon}_ready", false);
                    isAttacking = true;
                    sp.color = Color.red;
                    StartCoroutine(AttackCooldown());
                    StartCoroutine(ResetChargeAfter(chargeResetDelay));
                    StartCoroutine(ResetReleaseAfter(chargeResetDelay));
                }
            }
        } 
        else
        {
            animator.SetBool($"{currentWeapon}_charging", false);
            animator.SetBool($"{currentWeapon}_ready", false);
            animator.SetBool($"{currentWeapon}_idle", true);
            sp.color = Color.white;
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ResetChargeAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool($"{currentWeapon}_charging", false);
        sp.color = Color.white;
    }

    IEnumerator ResetReleaseAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool($"{currentWeapon}_release", false);
        animator.SetBool($"{currentWeapon}_idle", true);
    }

    private void TridentAttack()
    {
        if (currentWeapon == "trident" && playerMovement.Stamina <= staminaCost && isAttacking)
        {
            OnAtckTri?.Invoke();
        }
    }

    private void SwordAttack()
    {
        if (currentWeapon == "sword" && playerMovement.Stamina <= staminaCost && isAttacking)
        {
            OnAttackSw?.Invoke();
        }
    }

    private void BowRelease()
    {
        if (currentWeapon == "bow" && playerMovement.Stamina >= staminaCost && isAttacking)
        {
            OnBowRelease?.Invoke();
            Debug.Log("invoked");
        }
    }
}
