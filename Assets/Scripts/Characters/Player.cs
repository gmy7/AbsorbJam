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
    [SerializeField] private bool invincible;
    public float counterCooldown = 2;
    public float teleportCooldown = 1;

    [SerializeField] private float counterTime;
    [SerializeField] private float flashSpeed = 1;
    [SerializeField] private GameObject cooldownBar;
    [SerializeField] private GameObject teleportFill;
    [SerializeField] private List<GameObject> counterFills = new();
    [SerializeField] private List<GameObject> ammoSlotsGO = new();
    public List<AmmoSlot> ammoSlots = new();
    private ProjectileShooter shooter;
    private Coroutine counterCooldownCR;

    [SerializeField] private List<GameObject> healthCrystals = new();
    [SerializeField] private Sprite brokenHealthCrystal;
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
                foreach(GameObject counterFill in counterFills)
                {
                    counterFill.transform.localScale = new Vector3(0, counterFill.transform.localScale.y);
                }
                counterCooldownCR = StartCoroutine(ShieldCollapse());
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
        shooter = GetComponent<ProjectileShooter>();

        foreach (GameObject ammoSlotGO in ammoSlotsGO)
        {
            ammoSlots.Add(ammoSlotGO.GetComponent<AmmoSlot>());
        }
    }
    private void Update()
    {
        if (!damageFlash.DurationOver)
            FlashDamaged();

        FillCooldownBars();
    }

    private void FillCooldownBars()
    {
        foreach (GameObject counterFill in counterFills)
        {
            if (counterFill.transform.localScale.x >= 1)
            {
                counterFill.transform.localScale = new Vector3(1, counterFill.transform.localScale.y);
                continue;
            }
            float fillIncrement = 1 / counterCooldown;
            counterFill.transform.localScale = new Vector3(counterFill.transform.localScale.x + fillIncrement * Time.deltaTime, counterFill.transform.localScale.y);
        }
        if (teleportFill.transform.localScale.x >= 1)
        {
            teleportFill.transform.localScale = new Vector3(1, teleportFill.transform.localScale.y);
            return;
        }
        float increment = 1 / teleportCooldown;
        teleportFill.transform.localScale = new Vector3(teleportFill.transform.localScale.x + increment * Time.deltaTime, teleportFill.transform.localScale.y);
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
    public void TakeHit(bool counterable, Crystal.CrystalType projectileType)
    {
        if (counterActive && counterable)
        {
            if(ammo > 3) { return; }
            ammoSlots[ammo].ammoType = projectileType;
            Ammo++;
            return;
        }
        if (invincible)
            return;
        health--;
        if(health >= 0)
            healthCrystals[health].GetComponent<SpriteRenderer>().sprite = brokenHealthCrystal;
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
    public void StartShoot(Vector3 firingVector)
    {
        //Starts the animation
        animator.SetBool("Shooting", true);
        animator.SetBool("Absorbing", false);
        Ammo--;
        if (ammoSlots[ammo].ammoType == Crystal.CrystalType.Blue)
        {
            shooter.FireProjectile(null, firingVector, false);
        }
        else if (ammoSlots[ammo].ammoType == Crystal.CrystalType.Yellow)
        {
            Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            shooter.FireLightning(mousePos, false);
        }
        else if (ammoSlots[ammo].ammoType == Crystal.CrystalType.Red)
        {
            shooter.FireMagma(transform.position, false);
        }
    }
    public void StartBlink()
    {
        animator.SetBool("Shielding", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Absorbing", false);
        animator.SetBool("Blinking", true);

        teleportFill.transform.localScale = new Vector3(0, teleportFill.transform.localScale.y);
    }
    public void StartAbsorbingCore(GameObject coreGO)
    {
        Core core = coreGO.GetComponent<Core>();
        if (core.isDrained) { return; }
        ScoreHandler.Score += 5;
        for (int i = ammo; i < 4; i++)
        {
            ammoSlots[i].ammoType = core.coreType;
            Ammo++;
        }
        core.isDrained = true;
        core.destination = gameObject.transform;
        animator.SetBool("Absorbing", true);
        animator.SetBool("Shooting", false);
        animator.SetBool("Blinking", false);

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
        if(counterCooldownCR != null)
            StopCoroutine(counterCooldownCR);
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

    private IEnumerator Duration(CooldownGuard guard, float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        guard.DurationOver = true;
    }
}
