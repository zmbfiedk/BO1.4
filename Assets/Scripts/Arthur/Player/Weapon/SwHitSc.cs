using System.Collections;
using UnityEngine;

public class SwHitSc : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxC2D;
    [SerializeField] private Attack Attack;

    void Start()
    {
        boxC2D = GetComponent<BoxCollider2D>();
        boxC2D.enabled = false;
        Attack.OnSwordAttack += TurnHitBox;
    }

    private void TurnHitBox()
    {
        StartCoroutine(hitboxon());
        Debug.Log("SwHitBox: attack triggered");
    }

    IEnumerator hitboxon()
    {
        yield return new WaitForFixedUpdate();
        boxC2D.enabled = true;
        Debug.Log("SwHitBox: on");
        yield return new WaitForSeconds(0.1f); // Adjust this value as needed
        boxC2D.enabled = false;
        Debug.Log("SwHitBox: off");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TryDamage(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (boxC2D.enabled && other.CompareTag("Enemy"))
        {
            TryDamage(other);
        }
    }

    private void TryDamage(Collider2D other)
    {
        Takedamage enemy = other.GetComponent<Takedamage>();
        if (enemy != null)
        {
            enemy.TakeHit(1);
        }
    }

    private void OnDrawGizmos()
    {
        if (boxC2D != null && boxC2D.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxC2D.bounds.center, boxC2D.bounds.size);
        }
    }
}
