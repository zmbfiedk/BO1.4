using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Minimap2D : MonoBehaviour
{
    public RectTransform minimapRect;      // Minimap UI panel
    public GameObject enemyIconPrefab;     // Enemy icon prefab
    public GameObject playerIconPrefab;    // Player icon prefab

    private Transform player;
    private GameObject playerIcon;
    private List<GameObject> enemyIcons = new List<GameObject>();

    public float arenaSize = 95f;          // Arena is 95x95 units

    void Start()
    {
        // Find player automatically by tag
        player = GameObject.FindWithTag("Player").transform;

        // Create player icon once
        playerIcon = Instantiate(playerIconPrefab, minimapRect);
    }

    void Update()
    {
        if (player == null) return;

        // Update player icon position
        playerIcon.GetComponent<RectTransform>().anchoredPosition = WorldToMinimapPos(player.position);

        // Remove old enemy icons
        foreach (var icon in enemyIcons)
        {
            Destroy(icon);
        }
        enemyIcons.Clear();

        // Find enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            GameObject icon = Instantiate(enemyIconPrefab, minimapRect);
            icon.GetComponent<RectTransform>().anchoredPosition = WorldToMinimapPos(enemy.transform.position);
            enemyIcons.Add(icon);
        }
    }

    Vector2 WorldToMinimapPos(Vector3 worldPos)
    {
        float mapWidth = minimapRect.rect.width;
        float mapHeight = minimapRect.rect.height;

        float normalizedX = (worldPos.x / arenaSize) + 0.5f;
        float normalizedY = (worldPos.y / arenaSize) + 0.5f;

        float x = (normalizedX * mapWidth) - (mapWidth / 2f);
        float y = (normalizedY * mapHeight) - (mapHeight / 2f);

        return new Vector2(x, y);
    }
}
