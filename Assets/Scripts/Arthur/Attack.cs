using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackZone;
    [SerializeField] private float attackStaminaCost = 20f;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float lifeTime = 0.05f;

    private Move playerMovement;
    private SpriteRenderer sp;
    private bool canAttack = true;

    void Start()
    {
        playerMovement = GetComponent<Move>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        RotatePlayerToMouse();
        HandleAttack();
    }

    void RotatePlayerToMouse()
    {
        Vector2 directionToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void HandleAttack()
    {
        if (Input.GetMouseButton(1))
        {
            sp.color = Color.yellow;

            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                if (playerMovement.ConsumeStamina(attackStaminaCost))
                {
                    sp.color = Color.red;
                    GameObject attackInstance = Instantiate(attackZone, transform.position, Quaternion.identity);
                    Destroy(attackInstance, lifeTime);
                    StartCoroutine(AttackCooldown());
                }
            }
        }
        else
        {
            sp.color = Color.white;
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
