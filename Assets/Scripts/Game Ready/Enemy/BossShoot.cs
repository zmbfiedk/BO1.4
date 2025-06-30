using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour
{
    [SerializeField] private BossAttackSystem bossAttackSystem;
    [SerializeField] private GameObject ArrowPrefab;
    [SerializeField] private Transform ShootPosition;

    private void Start()
    {
        if (bossAttackSystem != null)
            bossAttackSystem.ThrowProjectile += BossShootProj;
        else
            Debug.LogError("BossAttackSystem reference missing!");
    }

    private void BossShootProj()
    {
        Debug.Log("BossShootProjectile: Shooting arrow");
        Instantiate(ArrowPrefab, ShootPosition.position, ShootPosition.rotation);
    }

    private void OnDestroy()
    {
        if (bossAttackSystem != null)
            bossAttackSystem.ThrowProjectile -= BossShootProj;
    }
}
