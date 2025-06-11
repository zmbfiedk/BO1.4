using System.Collections;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Attack attackScript;
    [SerializeField] private float arrowSpeed = 30f;

    void Start()
    {
        Attack.OnBowRelease += ShootArrow;
    }

    void Update()
    {
    }

    private void ShootArrow()
    {
        FireArrow();
        Debug.Log("Bow fired!");
        attackScript.isAtacking = false;
    }

    private void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.up * arrowSpeed;
        }
        Debug.Log("arrowShot");
    }
}
