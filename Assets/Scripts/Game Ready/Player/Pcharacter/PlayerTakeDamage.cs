using System.Collections;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] private Move Move;

    private bool isInvulnerable = false;
    [SerializeField] private float invulnerableDuration = 1.5f; 
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvulnerable) return;  

        if (other.CompareTag("lightEnemy"))
        {
            TakeDamage(5);
        }
        else if (other.CompareTag("heavyEnemy"))
        {
            TakeDamage(15);
        }
        else if (other.CompareTag("projEnemy"))
        {
            TakeDamage(3);
        }
        else if (other.CompareTag("BossAttack"))
        {
            TakeDamage(15);
        }
        else if (other.CompareTag("BossRanged"))
        {
            TakeDamage(5);
        }
    }

    private void TakeDamage(int damage)
    {
        Move.hp -= damage;
        StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        float flashInterval = 0.1f;

        while (elapsed < invulnerableDuration)
        {
            // Toggle visibility
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        spriteRenderer.enabled = true;

        isInvulnerable = false;
    }
}
