using System.Collections;
using UnityEngine;

public class TridentHitboxController : MonoBehaviour
{
    [SerializeField] private Collider2D[] hitboxColliders;  // Use Collider[] for 3D or Collider2D[] for 2D
    [SerializeField] private float activeDuration = 1f;

    private void OnEnable()
    {
        Attack.OnTridentAttack += ActivateHitboxes;
    }

    private void OnDisable()
    {
        Attack.OnTridentAttack -= ActivateHitboxes;
    }

    private void ActivateHitboxes()
    {
        StartCoroutine(HitboxRoutine());
    }

    private IEnumerator HitboxRoutine()
    {
        foreach (var col in hitboxColliders)
            col.enabled = true;

        yield return new WaitForSeconds(activeDuration);

        foreach (var col in hitboxColliders)
            col.enabled = false;
    }

    private void Start()
    {
        // Disable all colliders at start
        foreach (var col in hitboxColliders)
            col.enabled = false;
    }
}
