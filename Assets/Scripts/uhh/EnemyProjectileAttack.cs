using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 2;
    [SerializeField] private GameObject projectilePrefab;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
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
            InstantiateProjectile();
            yield return new WaitForSeconds(attackSpeed);
        }
    }
    private void InstantiateProjectile()
    {
        Instantiate(projectilePrefab, transform);
    }
}
