using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject defaultProjectile;
    [SerializeField] private Transform firePoint;
    public void FireProjectile(GameObject _projectile, Vector3 fireVector)
    {
        GameObject projectile;
        if (_projectile == null) 
            projectile = defaultProjectile; 
        else
            projectile = _projectile;

        GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        newProjectile.GetComponent<Projectile>().FireDirection = fireVector;
    }
}
