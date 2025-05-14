
using UnityEditor;
using UnityEngine;
public class Move : MonoBehaviour
{
    public GameObject Attackzone;
    private SpriteRenderer sp;
    public float moveSpeed = 10f;
    private float currentSpeed;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    public float stamina = 100f;
    public float sprintSpeed = 15f;
    public float staminaDrain = 1f;
    public float lifeTime = 0.05f;

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
        dodge();
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

            if (Input.GetMouseButton(0))
            {
                sp.color = Color.red;
                Debug.Log("Attack");

                GameObject attackInstance = Instantiate(Attackzone, transform.position, Quaternion.identity);
                Destroy(attackInstance, lifeTime); 
            }
        }
    }

    void dodge()
    {

    }
}
