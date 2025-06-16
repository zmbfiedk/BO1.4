using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] //turn trigger on for collider, of course. make a tag for the player named "Player" if you want this script to work
public class EnemyHitbox : MonoBehaviour
{
    DamageEffect DamageEffect;
    void Start()
    {
        GetComponent<BoxCollider2D>();
        DamageEffect = FindObjectOfType<DamageEffect>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DamageEffect player = collision.GetComponent<DamageEffect>();
            if (player != null && player.isCoroutineOn == false)
            {
                StartCoroutine(player.Effect());
            }
        }
    }
}
