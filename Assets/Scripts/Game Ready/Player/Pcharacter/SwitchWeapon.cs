using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    [SerializeField] private Attack attackScript;
    [SerializeField] private WeaponVisibilityManager weaponVisibilityManager;

    void Start()
    {
        if (attackScript == null)
            attackScript = GetComponent<Attack>();

        if (weaponVisibilityManager == null)
            Debug.LogWarning("WeaponVisibilityManager reference is missing!");

        SwitchToWeapon("trident", 0.2f, 45f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchToWeapon("trident", 0.2f, 45f);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchToWeapon("bow", .2f, 10f);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchToWeapon("sword", 0.2f, 20f);
    }

    private void SwitchToWeapon(string weaponName, float cooldown, float staminaDrain)
    {
        ResetCurrentWeaponAnimations();

        if (attackScript != null)
        {
            attackScript.Cooldown = cooldown;
            attackScript.SetCurrentWeapon(weaponName);
            attackScript.StaminaCost = staminaDrain;
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
            animator.SetBool($"{attackScript.CurrentWeapon}{state}", false);
        }
    }
}
