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
    private void Awake()
    {
        animator = GetComponent<Animator>();
        golem = GetComponent<GolemBehavior>();
        col2D = GetComponent<Collider2D>();
    }
    private void Start()
    {
        golem.behaviorState = GolemBehavior.BehaviorState.Spawning;
        animator.SetBool("Spawning", true);
        col2D.enabled = false;
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
    }
    public void Die()
    {
        coreLight.SetActive(false);
        col2D.enabled = false;
        //This is to only give score if they are killed by the player as opposed to being killed by the end game screen
        if (health == 0)
            ScoreHandler.Score += 10;
        animator.SetBool("Dying", true);
        animator.SetBool("Swiping", false);
        animator.SetBool("Shooting", false);
        golem.behaviorState = GolemBehavior.BehaviorState.Dying;
        transform.root.GetComponent<MonsterHandler>().monsterCount--;
        int rand = Random.Range(0, 5);
        if (rand == 0)
            Instantiate(core, transform.position, transform.rotation);
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
}
