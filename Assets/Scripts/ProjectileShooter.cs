using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject defaultProjectile;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject magma;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip projectileFire;
    [SerializeField] private AudioClip lightningFire;
    [SerializeField] private AudioClip magmaFire;
    private AudioSource src;
    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

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
        src.PlayOneShot(projectileFire, SoundSettings.effectsSound);
    }
    public void FireLightning(Vector3 mousePosition, bool isEnemy)
    {
        GameObject newProjectile = Instantiate(lightning, mousePosition, transform.rotation);
        if (isEnemy)
            newProjectile.tag = "EnemyProjectile";

        src.PlayOneShot(lightningFire, SoundSettings.effectsSound);
    }
    public void FireMagma(Vector3 position, bool isEnemy)
    {
        GameObject newProjectile = Instantiate(magma, position, transform.rotation);
        if (isEnemy)
            newProjectile.tag = "EnemyProjectile";

        src.PlayOneShot(magmaFire, SoundSettings.effectsSound);


    }
}
