using System.Collections;
using UnityEngine;

public class TriHitSc : MonoBehaviour
{
    private BoxCollider2D boxC2D;
    [SerializeField] private Attack Attack;

    void Start()
    {
        boxC2D = GetComponent<BoxCollider2D>();
        boxC2D.enabled = false;

        if (Attack != null)
            Attack.OnAtckTri += TurnHitBox;
        else
            Debug.LogWarning("TriHitSc: Attack reference missing!");
    }

    void OnDestroy()
    {
        if (Attack != null)
            Attack.OnAtckTri -= TurnHitBox;
    }

    private void TurnHitBox()
    {
        StartCoroutine(hitboxon());
        Debug.Log("TriHitBox: attack triggered");
    }

    IEnumerator hitboxon()
    {
        boxC2D.enabled = true;
        Debug.Log("TriHitBox: on");
        yield return new WaitForSeconds(1f);
        boxC2D.enabled = false;
        Debug.Log("TriHitBox: off");
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
