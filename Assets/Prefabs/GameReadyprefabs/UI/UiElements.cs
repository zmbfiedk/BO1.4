using UnityEngine;
using UnityEngine.UI;  // <-- Added for UI Image
using System.Collections;
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

    private Coroutine flashRoutine;

    private void Start()
    {
        Attack.OnStaminaUsed += FlashStaminaUI;
    }

    private void OnDestroy()
    {
        Attack.OnStaminaUsed -= FlashStaminaUI;
    }

    void Update()
    {
        if (playerMove == null || attack == null) return;

        UpdateWeaponUI(attack.CurrentWeapon);
        UpdateHealthUI(playerMove.hp);
        UpdateStaminaUI(playerMove.Stamina);
    }

    void UpdateWeaponUI(string weapon)
    {
        tridentUI?.SetActive(weapon == "trident");
        swordUI?.SetActive(weapon == "sword");
        bowUI?.SetActive(weapon == "bow");
    }

    void UpdateHealthUI(float hp)
    {
        float firstHeart = Mathf.Clamp(hp - 50, 0, 50); // hp1
        float secondHeart = Mathf.Clamp(hp, 0, 50);    // hp2

        UpdateSingleHeart(firstHeart, hp1Full, hp1_3_4, hp1Half, hp1Kwart, hp1Empty);
        UpdateSingleHeart(secondHeart, hp2Full, hp2_3_4, hp2Half, hp2Kwart, hp2Empty);
    }

    void UpdateStaminaUI(float stamina)
    {
        // Stamina thresholds: 100, 75, 50, 25, 0
        SetGroupActive(staminaFull, stamina > 75f);
        SetGroupActive(stamina_3_4, stamina <= 75f && stamina > 50f);
        SetGroupActive(staminaHalf, stamina <= 50f && stamina > 25f);
        SetGroupActive(staminaKwart, stamina <= 25f && stamina > 0f);
        SetGroupActive(staminaEmpty, stamina <= 0f);
    }

    void UpdateSingleHeart(float value, GameObject[] full, GameObject[] q3, GameObject[] half, GameObject[] kwart, GameObject[] empty)
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

    // FLASHING LOGIC:

    private void FlashStaminaUI(float amountUsed)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        // Intensity based on how much stamina was used, capped 0..1
        float intensity = Mathf.Clamp01(amountUsed / 100f);
        flashRoutine = StartCoroutine(FlashStaminaRoutine(intensity));
    }

    private IEnumerator FlashStaminaRoutine(float intensity)
    {
        float flashDuration = 0.1f;
        int flashes = Mathf.CeilToInt(3 + 3 * intensity);

        for (int i = 0; i < flashes; i++)
        {
            SetStaminaGroupColor(Color.yellow);
            yield return new WaitForSeconds(flashDuration);

            SetStaminaGroupColor(Color.white);
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void SetStaminaGroupColor(Color color)
    {
        SetColorForGroup(staminaFull, color);
        SetColorForGroup(stamina_3_4, color);
        SetColorForGroup(staminaHalf, color);
        SetColorForGroup(staminaKwart, color);
        SetColorForGroup(staminaEmpty, color);
    }

    private void SetColorForGroup(GameObject[] group, Color color)
    {
        foreach (var go in group)
        {
            if (go != null)
            {
                var image = go.GetComponent<Image>();
                if (image != null)
                {
                    image.color = color;
                }
            }
        }
    }
}
