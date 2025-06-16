using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 2;
    [SerializeField] private GameObject projectilePrefab;
    private EnemyAi enemyAI;
    private GameObject player;
    LayerMask layermask;
    // Start is called before the first frame update
    void Start()
    {
        int layerToAdd = LayerMask.NameToLayer("environmental objects");
        layermask |= 1 << layerToAdd;
        player = GameObject.FindWithTag("Player");
        StartCoroutine(cooldown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator cooldown()
    {
        while (true)
        {
            Debug.Log("running");
            Vector2 raycastreference = player.transform.position - gameObject.transform.position;
            int distance = ((int)raycastreference.magnitude);
            raycastreference.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, raycastreference, distance, layermask);
            if (hit.collider != null)
            {
                Debug.Log("player behind cover");
                yield return new WaitForSeconds(attackSpeed);
            }
            if (hit.collider == null)
            {
                Debug.Log("should work");
                InstantiateProjectile();
                yield return new WaitForSeconds(attackSpeed);
            }
            
        }
    }
    private void InstantiateProjectile()
    {
        Instantiate(projectilePrefab, transform);
    }
}
