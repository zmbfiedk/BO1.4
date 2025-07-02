using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private BoxCollider2D BC2D;
    private AudioSource audioSource;

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

    [Header("Sounds")]
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip deathSound;

    // Public property for health points
    public float hp
    {
        get { return health_points; }
        set { health_points = value; }
    }

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
        rb = GetComponent<Rigidbody2D>();
        BC2D = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        currentSpeed = moveSpeed;

        // Enable interpolation for smoother physics movement
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        HandleInput();
        RegenerateStamina();
        Dodge();
        playerkill();
        setHP();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;
    }

    void HandleMovement()
    {
        if (!isDodging)
        {
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                currentSpeed = sprintSpeed;
                stamina -= staminaDrain * Time.fixedDeltaTime;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }
        stamina = Mathf.Clamp(stamina, 0f, 100f);

        if (moveDirection != Vector2.zero)
        {
            Vector2 newPosition = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void RegenerateStamina()
    {
        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        if (!sprinting && !isDodging)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDodging && stamina >= dodgeStaminaCost)
        {
            stamina -= dodgeStaminaCost;
            stamina = Mathf.Clamp(stamina, 0f, 100f);

            if (dashSound != null && audioSource != null)
                audioSource.PlayOneShot(dashSound);

            StartCoroutine(DodgeRoutine());
        }
    }

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

    void playerkill()
    {
        if (health_points <= 0)
        {
            if (deathSound != null && audioSource != null)
                audioSource.PlayOneShot(deathSound);

            Destroy(gameObject);
        }
    }

    void setHP()
    {
        if (hp > 100)
        {
            hp = 100;
        }
    }
}
