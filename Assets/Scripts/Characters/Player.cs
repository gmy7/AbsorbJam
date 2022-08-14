using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;
using System;

public class Player : MonoBehaviour
{
    public int health = 3;

    private Animator animator;
    private InputHandler inputHandler;
    private SpriteRenderer sr;
    private readonly CooldownGuard damageFlash = new();
    private GameManager gameManager;
    private bool rampingColor;
    private bool invincible;
    public float counterCooldown = 2;
    [SerializeField] private float counterTime;
    [SerializeField] private float teleportCooldown;
    [SerializeField] private float flashSpeed = 1;
    [SerializeField] private GameObject cooldownBar;
    [SerializeField] private GameObject counterFill;
    [SerializeField] private List<GameObject> ammoSlotsGO = new();
    public List<AmmoSlot> ammoSlots = new();
    public enum PlayerState { Idle, Moving, Countering }
    public PlayerState playerState;

    private int ammo;
    public int Ammo 
    {
        get
        {
            return ammo;
        }
        set
        {
            if(value > 4)
            {
                return;
            }
            if(ammo < value)
                ammoSlots[value - 1].IsFull = true;
            else
                ammoSlots[value].IsFull = false;

            ammo = value;
        }
    }
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
                animator.SetBool("Shooting", false);
                animator.SetBool("Absorbing", false);
                counterFill.transform.localScale = new Vector3(0, counterFill.transform.localScale.y);
                StartCoroutine(ShieldCollapse());
            }
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        inputHandler = GetComponent<InputHandler>();
        damageFlash.durationEnded = new CooldownGuard.DurationEnded(FinishInvulnerability);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        foreach(GameObject ammoSlotGO in ammoSlotsGO)
        {
            ammoSlots.Add(ammoSlotGO.GetComponent<AmmoSlot>());
        }
    }
    private void Update()
    {
        if (!damageFlash.DurationOver)
            FlashDamaged();

        FillCounterCooldownBar();
    }

    private void FillCounterCooldownBar()
    {
        if(counterFill.transform.localScale.x >= 1)
        {
            counterFill.transform.localScale = new Vector3(1, counterFill.transform.localScale.y);
            return;
        }
        float fillIncrement = 1 / counterCooldown;
        counterFill.transform.localScale = new Vector3(counterFill.transform.localScale.x + fillIncrement * Time.deltaTime, counterFill.transform.localScale.y);
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
    public void TakeHit(bool counterable)
    {
        if (counterActive && counterable)
        {
            Ammo++;
            return;
        }
        if (invincible)
            return;
        health--;
        //if melee hit, this will trigger
        StopCountering();
        if (health <= 0)
        {
            Die();
        }
        else
        {
            damageFlash.DurationOver = false;
            StartCoroutine(Duration(damageFlash, 2));
            invincible = true;
        }
    }
    public void Die()
    {
        foreach (GameObject ammoSlotGO in ammoSlotsGO)
        {
            ammoSlotGO.SetActive(false);
        }
        animator.SetBool("Dying", true);
        inputHandler.inputReady.actionReady = false;
        cooldownBar.SetActive(false);
    }
    #region CalledFromAnimator
    public void FinishDeath()
    {
        //Called from Animator
        sr.enabled = false;
        gameManager.GameOver();
    }
    //Resets bools so that after the exit time its ready to take new input
    public void FinishShot()
    {
        animator.SetBool("Shooting", false);
    }
    public void FinishBlink()
    {
        animator.SetBool("Blinking", false);
    }
    public void FinishAbsorb()
    {
        animator.SetBool("Absorbing", false);
    }
    #endregion
    #region Inputs
    //Called from inputs - input handles call to Projectile Shooter. This handles animations. Its not perfect, but neither are you
    public void StartShoot()
    {
        //Starts the animation
        animator.SetBool("Shooting", true);
        animator.SetBool("Absorbing", false);
        Ammo--;
    }
    public void StartBlink()
    {
        animator.SetBool("Shielding", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Absorbing", false);
        animator.SetBool("Blinking", true);
    }
    public void StartAbsorbingCore(GameObject coreGO)
    {
        Core core = coreGO.GetComponent<Core>();
        if (core.isDrained) { return; }

        for (int i = ammo; i < 4; i++)
        {
            Ammo++;
        }
        core.isDrained = true;
        core.destination = gameObject.transform;
        animator.SetBool("Absorbing", true);
        animator.SetBool("Shooting", false);
    }
    #endregion

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
    public void FinishInvulnerability()
    {
        sr.color = Color.white;
        invincible = false;
    }

    IEnumerator Duration(CooldownGuard guard, float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        guard.DurationOver = true;
    }
}
