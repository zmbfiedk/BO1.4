using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] //turn trigger on for collider, of course. make a tag for the player named "Player" if you want this script to work
public class EnemyHitbox : MonoBehaviour
{
    PlayerDamage playerdamage;
    void Start()
    {
        GetComponent<BoxCollider2D>();
        playerdamage = FindObjectOfType<PlayerDamage>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerDamage player = collision.GetComponent<PlayerDamage>();
            if (player.isCoroutineOn == false)
            {
                StartCoroutine(player.DamageEffect());
            }
        }
    }
}
