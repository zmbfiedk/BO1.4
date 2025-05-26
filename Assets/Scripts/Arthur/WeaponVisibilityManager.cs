using UnityEngine;

public class WeaponVisibilityManager : MonoBehaviour
{
    [Header("Player Weapon Parts in Hierarchy")]
    public GameObject sword;
    public GameObject trident;
    public GameObject bow;
    public GameObject arrow;
    public GameObject stringPart;

    public void ShowOnly(string weaponName)
    {
        sword.SetActive(false);
        trident.SetActive(false);
        bow.SetActive(false);
        arrow.SetActive(false);
        stringPart.SetActive(false);

        switch (weaponName.ToLower())
        {
            case "sword":
                sword.SetActive(true);
                break;
            case "trident":
                trident.SetActive(true);
                break;
            case "bow":
                bow.SetActive(true);
                arrow.SetActive(true);
                stringPart.SetActive(true);
                break;
        }
    }
}
