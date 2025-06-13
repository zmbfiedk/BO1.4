using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    [SerializeField] private Attack attackScript;
    [SerializeField] private WeaponVisibilityManager weaponVisibilityManager;

    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject tridentPrefab;
    [SerializeField] private GameObject bowPrefab;
    [SerializeField] private GameObject swordPrefab;

    [Header("Weapon Hold Point")]
    [SerializeField] private Transform weaponHolder;

    private GameObject currentWeapon;

    void Start()
    {
        if (attackScript == null)
            attackScript = GetComponent<Attack>();

        if (weaponVisibilityManager == null)
            Debug.LogWarning("WeaponVisibilityManager reference is missing!");

        SwitchToWeapon(tridentPrefab, 0.8f, "trident", 70f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchToWeapon(tridentPrefab, 0.8f, "trident", 70f);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchToWeapon(bowPrefab, 1f, "bow", 20f);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchToWeapon(swordPrefab, 0.5f, "sword", 40f);
    }

    private void SwitchToWeapon(GameObject weaponPrefab, float cooldown, string weaponName, float staminaDrain)
    {
        ResetCurrentWeaponAnimations();

        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        if (attackScript != null)
        {
            attackScript.ACD = cooldown;
            attackScript.SetCurrentWeapon(weaponName);
            attackScript.staminaCost = staminaDrain;
        }
        else
        {
            Debug.LogWarning("Attack script reference missing.");
        }

        if (weaponVisibilityManager != null)
        {
            weaponVisibilityManager.ShowOnly(weaponName);
        }
    }

    private void ResetCurrentWeaponAnimations()
    {
        Animator animator = attackScript?.GetComponent<Animator>();
        if (animator == null) return;

        string[] states = { "_charging", "_ready", "_release", "_idle" };

        foreach (string state in states)
        {
            // Fixed concatenation: e.g. "trident_idle"
            animator.SetBool($"{attackScript.CurrentWeapon}{state}", false);
        }
    }
}
