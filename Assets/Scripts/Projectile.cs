using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 FireDirection { get; set; }
    private Mover mover;
    private bool isMoving = true;
    private Animator animator;
    public Crystal.CrystalType projectileType;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        Destroy(gameObject, 5);
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(isMoving)
            mover.Move(FireDirection, false, false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("EnemyProjectile"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.TakeHit(true, Crystal.CrystalType.Blue);
                if (player.CounterActive)
                {
                    animator.SetBool("Absorbing", true);
                }
                else
                {
                    animator.SetBool("Colliding", true);
                }
                isMoving = false;
            }
        }
        if (gameObject.CompareTag("PlayerProjectile"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeHit();
                animator.SetBool("Colliding", true);
                isMoving = false;
            }
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            animator.SetBool("Colliding", true);
            isMoving = false;
        }
    }
    public void FinishCollisionAnimation()
    {
        Destroy(gameObject);

    }
}
