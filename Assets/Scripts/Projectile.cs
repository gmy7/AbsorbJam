using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 FireDirection { get; set; }
    private Mover mover;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        Destroy(gameObject, 5);

    }
    void Update()
    {
        mover.Move(FireDirection, false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("EnemyProjectile"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player>().TakeHit();
                Destroy(gameObject);
            }
        }
        if (gameObject.CompareTag("PlayerProjectile"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeHit();
                Destroy(gameObject);
            }
        }

    }
}
