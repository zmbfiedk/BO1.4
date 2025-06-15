using System.Collections;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Attack attackScript;

    [Header("Arrow Settings")]
    [SerializeField] private float arrowSpeed = 30f;

    private void OnEnable()
    {
        Attack.OnBowRelease += ShootArrow;
    }

    private void OnDisable()
    {
        Attack.OnBowRelease -= ShootArrow;
    }

    private void ShootArrow()
    {
        if (arrowPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Missing arrowPrefab or firePoint!");
            return;
        }

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = firePoint.up * arrowSpeed;
        }
        else
        {
            Debug.LogWarning("Arrow prefab has no Rigidbody2D component.");
        }

        if (attackScript != null)
            attackScript.IsAttacking = false;


        Debug.Log("Arrow shot!");
    }
}

