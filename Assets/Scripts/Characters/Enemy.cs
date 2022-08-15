using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private GolemBehavior golem;
    private Collider2D col2D;
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject core;
    [SerializeField] private GameObject coreLight;
    [SerializeField] private GameObject swipeBox;
    [SerializeField] private GameObject swingRangeFinder;
    [SerializeField] private AudioClip death;
    private readonly CooldownGuard damageFlash = new();
    private bool rampingColor;
    [SerializeField] private float flashSpeed = 1;
    private AudioSource src;
    private SpriteRenderer sr;
    private Crystal.CrystalType golemType;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        golem = GetComponent<GolemBehavior>();
        col2D = GetComponent<Collider2D>();
        src = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        damageFlash.durationEnded = new CooldownGuard.DurationEnded(FinishFlash);

    }
    private void Start()
    {
        golem.behaviorState = GolemBehavior.BehaviorState.Spawning;
        golemType = golem.behaviourType;
        animator.SetBool("Spawning", true);
        col2D.enabled = false;
    }
    private void Update()
    {
        if (!damageFlash.DurationOver)
            FlashDamaged();

    }
    //Called from animator
    public void FinishSpawning()
    {
        golem.FinishedSpawning();
        animator.SetBool("Spawning", false);
        col2D.enabled = true;

    }
    public void TakeHit()
    {
        health--;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            damageFlash.DurationOver = false;
            StartCoroutine(Duration(damageFlash, 0.75f));

        }
    }
    public void Die()
    {
        coreLight.SetActive(false);
        col2D.enabled = false;
        //This is to only give score if they are killed by the player as opposed to being killed by the end game screen
        if (health == 0)
        {
            if (golemType == Crystal.CrystalType.Blue)
                ScoreHandler.Score += 10;
            else if (golemType == Crystal.CrystalType.Yellow)
                ScoreHandler.Score += 25;
            else if (golemType == Crystal.CrystalType.Red)
                ScoreHandler.Score += 40;
        }
        animator.SetBool("Dying", true);
        animator.SetBool("Swiping", false);
        animator.SetBool("Shooting", false);
        golem.behaviorState = GolemBehavior.BehaviorState.Dying;
        transform.root.GetComponent<MonsterHandler>().monsterCount--;
        int rand = Random.Range(0, 5);
        if (rand == 0)
        {
            GameObject droppedCore = Instantiate(core, transform.position, transform.rotation);
            droppedCore.GetComponent<Core>().coreType = golem.behaviourType;
        }
        src.PlayOneShot(death, SoundSettings.effectsSound);
    }
    public void FinishDeath()
    {
        //Called from animator
        Destroy(gameObject);
    }
    public void StartSwing()
    {
        animator.SetBool("Swiping", true);
    }
    public void StrikeFrame()
    {
        swipeBox.SetActive(true);
    }
    public void FinishSwing()
    {
        swipeBox.SetActive(false);
        animator.SetBool("Swiping", false);
    }
    private void FlashDamaged()
    {
        Color newColor;
        newColor = sr.color;
        if (!rampingColor)
        {
            newColor.r -= 0.03f * flashSpeed;
        }
        else
        {
            newColor.r += 0.03f * flashSpeed;
        }
        newColor.g = newColor.r;
        newColor.b = newColor.r;

        if (newColor.r < 0.1)
            rampingColor = true;
        if (newColor.r > 1)
            rampingColor = false;
        sr.color = newColor;
    }
    private void FinishFlash()
    {
        sr.color = Color.white;
    }

    private IEnumerator Duration(CooldownGuard guard, float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        guard.DurationOver = true;
    }
}
