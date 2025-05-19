using System.Runtime.InteropServices.WindowsRuntime;
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

        SwitchToWeapon(swordPrefab, 0.22f, "sword");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchToWeapon(tridentPrefab, 0.3f, "trident");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchToWeapon(bowPrefab, 0.1f, "bow");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchToWeapon(swordPrefab, 0.22f, "sword");
    }

    private void SwitchToWeapon(GameObject weaponPrefab, float cooldown, string weaponName)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        attackScript.ACD = cooldown;
        attackScript.SetCurrentWeapon(weaponName);
        weaponVisibilityManager.ShowOnly(weaponName);
    }
}
