using System;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    public static event Action onDeath;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    private SpriteRenderer sp;
    private Enemyfollow EF;
    private bool isDead = false;  

    void Start()
    {
        currentHealth = maxHealth;
        sp = GetComponent<SpriteRenderer>();
        EF = GetComponent<Enemyfollow>();
    }

    void Update()
    {
        EnemySee();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arrow"))
        {
            TakeHit(4);
        }
        if (other.CompareTag("Trident"))
        {
            TakeHit(9);
        }
        if (other.CompareTag("Sword"))
        {
            TakeHit(6);
        }

    }

    public void TakeHit(int damage)
    {
        if (isDead) return;  

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health now: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; 

        isDead = true;
        Debug.Log($"{gameObject.name} died.");
        onDeath?.Invoke();

        Destroy(gameObject);
    }

    private void EnemySee()
    {
        if (EF == null) return;
        sp.color = EF.follow ? Color.red : Color.green;
    }
}
