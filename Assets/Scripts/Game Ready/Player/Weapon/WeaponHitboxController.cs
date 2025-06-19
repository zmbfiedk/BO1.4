using System.Collections;
using UnityEngine;

public class WeaponHitboxController : MonoBehaviour
{
    [Header("Trident Hitboxes")]
    [SerializeField] private Collider2D[] tridentHitboxes;
    [SerializeField] private float tridentActiveDuration = 1f;

    [Header("Sword Hitboxes")]
    [SerializeField] private Collider2D[] swordHitboxes;
    [SerializeField] private float swordActiveDuration = 1f;

    private void OnEnable()
    {
        Attack.OnTridentAttack += ActivateTridentHitboxes;
        Attack.OnSwordAttack += ActivateSwordHitboxes;
    }

    private void OnDisable()
    {
        Attack.OnTridentAttack -= ActivateTridentHitboxes;
        Attack.OnSwordAttack -= ActivateSwordHitboxes;
    }

    private void Start()
    {
        DisableAllHitboxes();
    }

    private void DisableAllHitboxes()
    {
        SetCollidersEnabled(tridentHitboxes, false);
        SetCollidersEnabled(swordHitboxes, false);
    }

    private void ActivateTridentHitboxes()
    {
        Debug.Log("Activating Trident Hitboxes");
        StartCoroutine(ActivateHitboxesRoutine(tridentHitboxes, tridentActiveDuration));
    }

    private void ActivateSwordHitboxes()
    {
        Debug.Log("Activating Sword Hitboxes");
        StartCoroutine(ActivateHitboxesRoutine(swordHitboxes, swordActiveDuration));
    }

    private IEnumerator ActivateHitboxesRoutine(Collider2D[] colliders, float duration)
    {
        SetCollidersEnabled(colliders, true);
        yield return new WaitForSeconds(duration);
        SetCollidersEnabled(colliders, false);
    }

    private void SetCollidersEnabled(Collider2D[] colliders, bool enabled)
    {
        foreach (var col in colliders)
        {
            if (col != null)
                col.enabled = enabled;
        }
    }
}
