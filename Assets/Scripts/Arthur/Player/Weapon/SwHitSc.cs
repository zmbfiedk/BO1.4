using System.Collections;
using UnityEngine;

public class SwHitSc : MonoBehaviour
{
    private BoxCollider2D boxC2D;
    [SerializeField] private Attack Attack;

    void Start()
    {
        boxC2D = GetComponent<BoxCollider2D>();
        boxC2D.enabled = false;
        Attack.OnAttackSw += TurnHitBox;
    }

    private void TurnHitBox()
    {
        StartCoroutine(hitboxon());
        Debug.Log("SwHitBox: attack triggered");
    }

    IEnumerator hitboxon()
    {
        boxC2D.enabled = true;
        Debug.Log("SwHitBox: on");
        yield return new WaitForSeconds(1);
        boxC2D.enabled = false;
        Debug.Log("SwHitBox: off");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            Takedamage enemy = other.GetComponent<Takedamage>();
            if (enemy != null)
            {
                enemy.TakeHit(1); // You can change this value as needed
            }
        }
    }
}
