using System;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    public static event Action onDeath;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private float currentHealth;

    private SpriteRenderer sp;
    private Enemyfollow EF;
    private bool isDead = false;
    WaveCheckerN waveCheckerN;
    WaveCheckerF waveCheckerF;

    void Start()
    {
        currentHealth = maxHealth;
        sp = GetComponent<SpriteRenderer>();
        EF = GetComponent<Enemyfollow>();
        if (CompareTag("WaveManager"))
       
        waveCheckerN = FindObjectOfType<WaveCheckerN>();

        GameObject waveManagerObject = GameObject.FindGameObjectWithTag("WaveManager");
        if (waveManagerObject != null)
        {
            waveCheckerN = waveManagerObject.GetComponent<WaveCheckerN>();
        }
        else
        {
            Debug.LogWarning("WaveManager not found in the scene!");
        }
        
        waveCheckerF = FindObjectOfType<WaveCheckerF>();

        GameObject waveManagerObjectF = GameObject.FindGameObjectWithTag("WaveManager");
        if (waveManagerObject != null)
        {
            waveCheckerF = waveManagerObject.GetComponent<WaveCheckerF>();
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
        // Get levels if available
        float? levelF = waveCheckerF != null ? waveCheckerF.GetLevel() : (float?)null;
        float? levelN = waveCheckerN != null ? waveCheckerN.GetLevel() : (float?)null;

        // Exit if both are null
        if (levelF == null && levelN == null) return;

        // Use whichever exists (or average them, or pick the higher/lower — here we choose max)
        float level = Mathf.Max(levelF ?? float.MinValue, levelN ?? float.MinValue);

        // Apply damage based on range
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
            if (other.CompareTag("Arrow")) TakeHit(8f);
            if (other.CompareTag("Trident")) TakeHit(20f);
            if (other.CompareTag("Sword")) TakeHit(11f);
        }
    }


    public void TakeHit(float damage)
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
