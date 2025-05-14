
using UnityEditor;
using UnityEngine;
public class Move : MonoBehaviour
{
    private BoxCollider2D BC;
    [SerializeField] private GameObject Attackzone;
    private SpriteRenderer sp;
    [SerializeField] private float moveSpeed = 10f;
    private float currentSpeed;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float staminaDrain = 1f;
    [SerializeField] private float lifeTime = 0.05f;
    [SerializeField] private float dashspeed = 50f;
    [SerializeField] private float ADStaminadrain = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        RotatePlayerToMouse();
        Attack();
        Dodge();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;
        //sprint
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0) 
        {
            currentSpeed = sprintSpeed;
            stamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        stamina = Mathf.Clamp(stamina, 0f, 100f);

        rb.velocity = moveDirection * currentSpeed;
    }

    Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radianAngle = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radianAngle);
        float sin = Mathf.Sin(radianAngle);
        float x = vector.x * cos - vector.y * sin;
        float y = vector.x * sin + vector.y * cos;
        return new Vector2(x, y);
    }

    void RotatePlayerToMouse()
    {
        Vector2 directionToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Attack()
    {
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Right mouse clicked");
            sp.color = Color.yellow;

            if (Input.GetMouseButton(0) && stamina < 0)
            {
                sp.color = Color.red;
                Debug.Log("Attack");
                stamina -= ADStaminadrain;

                GameObject attackInstance = Instantiate(Attackzone, transform.position, Quaternion.identity);
                Destroy(attackInstance, lifeTime); 
            }
        }
    }

    void Dodge()
    {
        if (Input.GetKey(KeyCode.LeftControl) && stamina < 0)
        {
            BC.enabled = false;
            moveSpeed = dashspeed;
            stamina -= ADStaminadrain;

        }
        else
        {
            BC.enabled=true;
            moveSpeed = moveSpeed;
        }
    }
}
