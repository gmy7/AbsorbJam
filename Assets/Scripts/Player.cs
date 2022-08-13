using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public int ammo;

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
        }
    }
    public void TakeHit()
    {
        if (shieldActive)
        {
            ammo++;
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
}
