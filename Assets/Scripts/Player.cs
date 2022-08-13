using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public int ammo;
    private Animator animator;
    private SpriteRenderer sr;
    private Collider2D col2D;
    private CooldownGuard damageFlash = new CooldownGuard();
    private bool rampingColor;

    [SerializeField] private float counterTime;
    [SerializeField] private float flashSpeed = 1;
    public enum PlayerState { Idle, Moving, Countering }
    public PlayerState playerState;

    private bool counterActive;
    public bool CounterActive
    {
        get
        {
            return counterActive;
        }
        set
        {
            counterActive = value;
            HandleShieldPlayerState(value);
            animator.SetBool("Shielding", value);
            if (value)
            {
                StartCoroutine(ShieldCollapse());
            }
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (!damageFlash.DurationOver)
            FlashDamaged();
        else
            sr.color = Color.white;
    }
    private void HandleShieldPlayerState(bool value)
    {
        if (value == false)
        {
            //Sets the palyer state to idle if the shield value changes to false
            playerState = PlayerState.Idle;
        }
        else
            playerState = PlayerState.Countering;
    }


    public void TakeHit()
    {
        if (counterActive)
        {
            ammo++;
            return;
        }
        health--;

        if(health <= 0)
        {
            Die();
        }
        else
        {
            damageFlash.DurationOver = false;
            StartCoroutine(Duration(damageFlash, 2));
        }
    }
    public void Die()
    {

    }
    //Called from inputs - input handles call to Projectile Shooter. This handles animations. Its not perfect, but neither are you
    public void TakingShot()
    {
        //Starts the animation
        animator.SetBool("Shooting", true);
        ammo--;
    }
    public void FinishShot()
    {
        //Resets bool so that after the exit time its ready to take new input
        animator.SetBool("Shooting", false);
    }

    private IEnumerator ShieldCollapse()
    {
        yield return new WaitForSeconds(counterTime);
        StopCountering();
    }
    public void StopCountering()
    {
        animator.SetBool("Shielding", false);
        CounterActive = false;
    }
    private void FlashDamaged()
    {
        Color newColor;
        newColor = sr.color;
        if(!rampingColor)
        {
            newColor.r -= 0.01f * flashSpeed;
        }
        else
        {
            newColor.r += 0.01f * flashSpeed;
        }
        newColor.g = newColor.r;
        newColor.b = newColor.r;

        if (newColor.r < 0.5)
            rampingColor = true;
        if (newColor.r > 1)
            rampingColor = false;
        sr.color = newColor;
    }
    IEnumerator Duration(CooldownGuard guard, float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        guard.DurationOver = true;
    }
}
