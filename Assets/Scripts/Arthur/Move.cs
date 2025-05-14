using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D BC2D;
    private float currentSpeed;
    private Vector2 moveDirection;
    private bool isDodging = false;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float staminaDrain = 1f;
    [SerializeField] private float dodgeStaminaCost = 20f;
    [SerializeField] private float staminaRegenRate = 5f;

    public float Stamina => stamina;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BC2D = GetComponent<BoxCollider2D>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        HandleMovement();
        RegenerateStamina();
        Dodge();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;

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
            StartCoroutine(DodgeRoutine());
        }
    }

    IEnumerator DodgeRoutine()
    {
        isDodging = true;
        BC2D.enabled = false;
        currentSpeed = dashSpeed;

        yield return new WaitForSeconds(0.5f);

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
}