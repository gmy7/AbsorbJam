using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public int ammo;
    [SerializeField] private float shieldLifetime;
    [SerializeField] private GameObject shield;

    private bool shieldActive;
    public bool ShieldActive
    {
        get
        {
            return shieldActive;
        }
        set
        {
            shieldActive = value;
            shield.SetActive(value);
            if (value) {
                StartCoroutine(ShieldCollapse());
            }
        }
    }
    public void TakeHit()
    {
        if (shieldActive)
        {
            ammo++;
            ShieldActive = false;
            return;
        }
        health--;
        if(health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {

    }
    private IEnumerator ShieldCollapse()
    {
        yield return new WaitForSeconds(shieldLifetime);
        shield.SetActive(false);
    }
}
