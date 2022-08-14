using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject defaultProjectile;
    [SerializeField] private GameObject lightning;
    [SerializeField] private Transform firePoint;
    public void FireProjectile(GameObject _projectile, Vector3 fireVector, bool isEnemy)
    {
        GameObject projectile;
        if (_projectile == null) 
            projectile = defaultProjectile; 
        else
            projectile = _projectile;

        GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        if (isEnemy)
            newProjectile.tag = "EnemyProjectile";

        newProjectile.GetComponent<Projectile>().FireDirection = fireVector;
    }
    public void FireLightning(Vector3 mousePosition, bool isEnemy)
    {
        GameObject newProjectile = Instantiate(lightning, mousePosition, transform.rotation);
        if (isEnemy)
            newProjectile.tag = "EnemyProjectile";
    }
}
