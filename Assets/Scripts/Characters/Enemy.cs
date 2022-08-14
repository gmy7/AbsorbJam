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
    private void Awake()
    {
        animator = GetComponent<Animator>();
        golem = GetComponent<GolemBehavior>();
        col2D = GetComponent<Collider2D>();
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
        col2D.enabled = false;
        //This is to only give score if they are killed by the player as opposed to being killed by the end game screen
        if (health == 0)
            ScoreHandler.Score += 10;
        animator.SetBool("Dying", true);
        golem.behaviorState = GolemBehavior.BehaviorState.Dying;
        transform.root.GetComponent<MonsterHandler>().monsterCount--;
        int rand = Random.Range(0, 5);
        if (rand == 0)
            Instantiate(core);
    }
    public void FinishDeath()
    {
        //Called from animator
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeHit();
        }
    }
}
