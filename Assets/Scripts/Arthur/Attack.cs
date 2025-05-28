using UnityEngine;
using System.Collections;
using System.Diagnostics.Tracing;

public class Attack : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject attackZoneS;
    [SerializeField] private GameObject attackZoneT;
    [SerializeField] private GameObject attackZoneB;

    [Header("Attack Stats")]
    [SerializeField] private float attackStaminaCost = 20f;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float lifeTime = 0.05f;
    [SerializeField] private float chargeResetDelay = 0.5f;

    [Header("Needed scripts")]
    [SerializeField] private SwitchWeapon SW;
    [SerializeField] private Move playerMovement;

    private SpriteRenderer sp;
    private Animator animator;
    private bool canAttack = true;
    private string currentWeapon = "trident";
    public string CurrentWeapon => currentWeapon;


    public float ACD
    {
        get { return attackCooldown; }
        set { attackCooldown = value; }
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
                    

                    sp.color = Color.red;
                    SpawnAttack();
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

    void SpawnAttack()
    {
        GameObject attackPrefab = null;

        switch (currentWeapon)
        {
            case "sword":
                attackPrefab = attackZoneS;
                break;
            case "trident":
                attackPrefab = attackZoneT;
                break;
            case "bow":
                attackPrefab = attackZoneB;
                break;
        }

        if (attackPrefab != null)
        {
            float attackZ = transform.eulerAngles.z - 90;
            Quaternion attackRotation = Quaternion.Euler(0, 0, attackZ);

            Vector3 forwardOffset = transform.up * 1f;

            GameObject attackInstance = Instantiate(
                attackPrefab,
                transform.position + forwardOffset,
                attackRotation
            );

            Destroy(attackInstance, lifeTime);
            StartCoroutine(AttackCooldown());
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
}
