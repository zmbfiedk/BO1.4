using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwHitSc : MonoBehaviour
{
    private BoxCollider2D boxC2D;
    [SerializeField] Attack Attack;

    void Start()
    {
        boxC2D = GetComponent<BoxCollider2D>();
        boxC2D.enabled = false;
        Attack.OnAttackSw += TurnHitBox;
    }



    void Update()
    {
    }

    private void TurnHitBox()
    {
        StartCoroutine(hitboxon());
        Debug.Log("attack");
        Attack.isAtacking = false;
    }

    IEnumerator hitboxon()
    {
        boxC2D.enabled = true;
        Debug.Log("on");
        yield return new WaitForSeconds(1);
        boxC2D.enabled = false;
        Debug.Log("off");
    }
}
