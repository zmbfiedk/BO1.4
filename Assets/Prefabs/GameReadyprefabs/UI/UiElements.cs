using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Move playerMove;
    [SerializeField] private Attack attack;

    [Header("Weapon UI")]
    [SerializeField] private GameObject tridentUI;
    [SerializeField] private GameObject swordUI;
    [SerializeField] private GameObject bowUI;

    [Header("Heart (First 50 HP)")]
    [SerializeField] private GameObject[] hp2Full;
    [SerializeField] private GameObject[] hp2_3_4;
    [SerializeField] private GameObject[] hp2Half;
    [SerializeField] private GameObject[] hp2Kwart;
    [SerializeField] private GameObject[] hp2Empty;

    [Header("Heart_1 (Second 50 HP)")]
    [SerializeField] private GameObject[] hp1Full;
    [SerializeField] private GameObject[] hp1_3_4;
    [SerializeField] private GameObject[] hp1Half;
    [SerializeField] private GameObject[] hp1Kwart;
    [SerializeField] private GameObject[] hp1Empty;

    [Header("Stamina UI")]
    [SerializeField] private GameObject[] staminaFull;
    [SerializeField] private GameObject[] stamina_3_4;
    [SerializeField] private GameObject[] staminaHalf;
    [SerializeField] private GameObject[] staminaKwart;
    [SerializeField] private GameObject[] staminaEmpty;

    void Update()
    {
        if (playerMove == null || attack == null) return;

        UpdateWeaponUI(attack.CurrentWeapon);
        UpdateHealthUI(playerMove.hp);
        UpdateDualUI(playerMove.Stamina, staminaFull, stamina_3_4, staminaHalf, staminaKwart, staminaEmpty);
    }

    void UpdateWeaponUI(string weapon)
    {
        tridentUI?.SetActive(weapon == "trident");
        swordUI?.SetActive(weapon == "sword");
        bowUI?.SetActive(weapon == "bow");
    }

    void UpdateHealthUI(float hp)
    {
        // REVERSED: Heart (hp2) drains first, then Heart_1 (hp1)
        float firstHeart = Mathf.Clamp(hp - 50, 0, 50);   // Heart_1
        float secondHeart = Mathf.Clamp(hp, 0, 50);       // Heart

        UpdateDualUI(firstHeart, hp1Full, hp1_3_4, hp1Half, hp1Kwart, hp1Empty);
        UpdateDualUI(secondHeart, hp2Full, hp2_3_4, hp2Half, hp2Kwart, hp2Empty);
    }

    void UpdateDualUI(float value, GameObject[] full, GameObject[] q3, GameObject[] half, GameObject[] kwart, GameObject[] empty)
    {
        SetGroupActive(full, value > 37.5f);
        SetGroupActive(q3, value <= 37.5f && value > 25f);
        SetGroupActive(half, value <= 25f && value > 12.5f);
        SetGroupActive(kwart, value <= 12.5f && value > 0f);
        SetGroupActive(empty, value <= 0f);
    }

    void SetGroupActive(GameObject[] group, bool isActive)
    {
        foreach (var go in group)
        {
            if (go != null) go.SetActive(isActive);
        }
    }
}
