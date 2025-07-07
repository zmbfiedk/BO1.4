using System;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    public static event Action onDeath;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private float currentHealth;

    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;

    private bool isDead = false;
    private bool isFlashing = false;

    private WaveCheckerN waveCheckerN;
    private WaveCheckerF waveCheckerF;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;

    private AudioSource audioSource;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    void Start()
    {
        currentHealth = maxHealth;

        // Get all SpriteRenderers in this object and its children
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (spriteRenderers.Length == 0)
        {
            Debug.LogWarning($"No SpriteRenderers found on {gameObject.name} or its children!");
        }

        // Store original colors
        originalColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }

        audioSource = GetComponent<AudioSource>();

        // Find WaveCheckerN
        GameObject waveManagerObject = GameObject.FindGameObjectWithTag("WaveManager");
        if (waveManagerObject != null)
        {
            waveCheckerN = waveManagerObject.GetComponent<WaveCheckerN>();
        }
        else
        {
            Debug.LogWarning("WaveManager not found in the scene!");
        }

        // Find WaveCheckerF
        GameObject waveManagerObjectF = GameObject.FindGameObjectWithTag("WaveManager");
        if (waveManagerObjectF != null)
        {
            waveCheckerF = waveManagerObjectF.GetComponent<WaveCheckerF>();
        }
        else
        {
            Debug.LogWarning("WaveManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        float? levelF = waveCheckerF != null ? waveCheckerF.GetLevel() : (float?)null;
        float? levelN = waveCheckerN != null ? waveCheckerN.GetLevel() : (float?)null;

        if (levelF == null && levelN == null) return;

        float level = Mathf.Max(levelF ?? float.MinValue, levelN ?? float.MinValue);

        if (level < 1f)
        {
            if (other.CompareTag("Arrow")) TakeHit(4f);
            if (other.CompareTag("Trident")) TakeHit(9f);
            if (other.CompareTag("Sword")) TakeHit(6f);
        }
        else if (level < 2f)
        {
            if (other.CompareTag("Arrow")) TakeHit(5f);
            if (other.CompareTag("Trident")) TakeHit(10f);
            if (other.CompareTag("Sword")) TakeHit(8f);
        }
        else if (level < 3f)
        {
            if (other.CompareTag("Arrow")) TakeHit(6f);
            if (other.CompareTag("Trident")) TakeHit(15f);
            if (other.CompareTag("Sword")) TakeHit(10f);
        }
        else if (level < 4f)
        {
            if (other.CompareTag("Arrow")) TakeHit(10f);
            if (other.CompareTag("Trident")) TakeHit(25f);
            if (other.CompareTag("Sword")) TakeHit(15f);
        }
        else // level >= 4f
        {
            if (other.CompareTag("Arrow")) TakeHit(15f);
            if (other.CompareTag("Trident")) TakeHit(35f);
            if (other.CompareTag("Sword")) TakeHit(20f);
        }
    }

    public void TakeHit(float damage)
    {

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health now: {currentHealth}");

        // Only flash if still alive after taking damage
        if (currentHealth > 0)
        {
            if (spriteRenderers != null && spriteRenderers.Length > 0)
                StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashRed()
    {
        isFlashing = true;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }

        isFlashing = false;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Set isdead animator parameter
        if (anim != null)
        {
            anim.SetInteger("isdead", 1);
        }

        onDeath?.Invoke();

        // Optional: destroy object after delay to allow animation & sound to finish
        Destroy(gameObject, 0.5f);
    }
}
