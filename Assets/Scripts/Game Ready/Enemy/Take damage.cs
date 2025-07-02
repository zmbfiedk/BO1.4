using System;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    public static event Action onDeath;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private float currentHealth;

    [SerializeField] private SpriteRenderer sp;
    private Color originalColor;

    private Enemyfollow EF;
    private bool isDead = false;
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
        sp = GetComponent<SpriteRenderer>();
        if (sp == null)
        {
            Debug.LogWarning($"SpriteRenderer not found on {gameObject.name}");
        }
        else
        {
            originalColor = sp.color; // store original color for flashing
        }

        EF = GetComponent<Enemyfollow>();

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

    void Update()
    {
        EnemySee();
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
        else // level >= 3f
        {
            if (other.CompareTag("Arrow")) TakeHit(10f);
            if (other.CompareTag("Trident")) TakeHit(25f);
            if (other.CompareTag("Sword")) TakeHit(15f);
        }
    }

    public void TakeHit(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health now: {currentHealth}");

        // Start flash coroutine
        if (sp != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashRed()
    {
        sp.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sp.color = originalColor;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} died.");

        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Set isDeath animator integer parameter to 1
        if (anim != null)
        {
            anim.SetInteger("isdead", 1);
        }

        onDeath?.Invoke();

        // Optional: destroy object after delay to allow animation & sound to finish
        Destroy(gameObject, 0.5f);
    }

    private void EnemySee()
    {
        if (EF == null) return;
        sp.color = EF.follow ? Color.red : Color.green;
    }
}
