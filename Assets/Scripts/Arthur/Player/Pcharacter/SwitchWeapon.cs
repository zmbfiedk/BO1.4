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

        SwitchToWeapon(tridentPrefab, 0.8f, "trident",70);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchToWeapon(tridentPrefab, 0.8f, "trident",70);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchToWeapon(bowPrefab, 1f, "bow",20);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchToWeapon(swordPrefab, 0.5f, "sword",40);
    }

    private void SwitchToWeapon(GameObject weaponPrefab, float cooldown, string weaponName, float staminadrain)
    {
        ResetCurrentWeaponAnimations();

        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        attackScript.ACD = cooldown;
        attackScript.SetCurrentWeapon(weaponName);
        weaponVisibilityManager.ShowOnly(weaponName);

        attackScript.staminaCost = staminadrain;
    }

    private void ResetCurrentWeaponAnimations()
    {
        string[] states = { "_charging", "_ready", "_release", "_idle" };
        Animator animator = attackScript.GetComponent<Animator>();

        foreach (string state in states)
        {
            animator.SetBool(attackScript.CurrentWeapon + state, false);
        }
    }
}
