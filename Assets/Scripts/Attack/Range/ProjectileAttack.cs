﻿using UnityEngine;

public class ProjectileAttack : AttackComponent
{

    [Header("Projectile Setup")]
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] float projectileSpeed = 50f;

    public override void Attack(Transform dirTransform, DamageInfo info)
    {
        GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        go.name = gameObject.name + " Projectile Attack";
        go.GetComponent<IProjectile>().Launch(dirTransform.position, projectileSpeed, info.amount, info.origin);
    }

    public override void Attack(Vector3 dir, DamageInfo info)
    {
        GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        go.name = gameObject.name + " Projectile Attack";
        go.GetComponent<IProjectile>().Launch(dir, projectileSpeed, info.amount, info.origin);
    }
}