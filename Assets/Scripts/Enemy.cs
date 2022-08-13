using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private GolemBehavior golem;
    [SerializeField] private int health = 1;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        golem = GetComponent<GolemBehavior>();
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
        ScoreHandler.Score += 20;
        animator.SetBool("Dying", true);
        golem.behaviorState = GolemBehavior.BehaviorState.Dying;
        transform.root.GetComponent<MonsterHandler>().monsterCount--;
    }
    public void FinishDeath()
    {
        //Called from animator
        Destroy(gameObject);
    }
}
