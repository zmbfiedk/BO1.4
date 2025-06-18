using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private BoxCollider2D BC2D;

    // Movement variables
    private float currentSpeed;
    private Vector2 moveDirection;
    private bool isDodging = false;

    [Header("Base move")]
    [SerializeField] private float moveSpeed = 10f;

    [Header("Advanced movement")]
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float dashSpeed = 50f;

    [Header("Stamina")]
    [SerializeField] private float stamina = 100;
    [SerializeField] private float staminaDrain = 1f;
    [SerializeField] private float dodgeStaminaCost = 20f;
    [SerializeField] private float staminaRegenRate = 5f;

    [Header("HP")]
    [SerializeField] private float health_points = 100;

    // Public property for health points
    public float hp
    {
        get { return health_points; }
        set { health_points = value; }
    }

    // Public property for stamina, with lower bound check
    public float Stamina
    {
        get { return stamina; }
        set
        {
            if (value > 0)
            {
                stamina = value;
            }
        }
    }

    void Start()
    {
        // Cache component references
        rb = GetComponent<Rigidbody2D>();
        BC2D = GetComponent<BoxCollider2D>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        HandleMovement();
        RegenerateStamina();
        Dodge();
        playerkill();
    }

    // Handle player movement input and stamina consumption
    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontal, vertical).normalized;

        // Sprinting consumes stamina if not dodging
        if (!isDodging)
        {
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                currentSpeed = sprintSpeed;
                stamina -= staminaDrain * Time.deltaTime;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }
        stamina = Mathf.Clamp(stamina, 0f, 100f);
        rb.velocity = moveDirection * currentSpeed;
    }

    // Regenerate stamina when not sprinting or dodging
    void RegenerateStamina()
    {
        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        if (!sprinting && !isDodging)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
    }

    // Handle dodge input and stamina cost
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDodging && stamina >= dodgeStaminaCost)
        {
            stamina -= dodgeStaminaCost;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
            StartCoroutine(DodgeRoutine());
        }
    }

    // Coroutine that disables collider and boosts speed briefly for dodge effect
    IEnumerator DodgeRoutine()
    {
        isDodging = true;
        BC2D.enabled = false;
        currentSpeed = dashSpeed;

        yield return new WaitForSeconds(0.05f);

        currentSpeed = moveSpeed;
        BC2D.enabled = true;
        isDodging = false;
    }

    // Public method to consume stamina for external use (returns success/fail)
    public bool ConsumeStamina(float amount)
    {
        if (stamina >= amount)
        {
            stamina -= amount;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
            return true;
        }
        return false;
    }

    // Destroy the player GameObject if health reaches zero
    void playerkill()
    {
        if (health_points <= 0)
        {
            Object.Destroy(gameObject);
        }
    }

    void setHP()
    {
        if(hp > 100)
        {
            hp = 100;
        }
    }
}
