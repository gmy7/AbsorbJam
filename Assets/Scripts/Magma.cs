using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma : MonoBehaviour
{
    private CircleCollider2D col2D;
    private void Awake()
    {
        col2D = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if(col2D.radius > 1.38) { return; }
        col2D.radius += 2 * Time.deltaTime;
    }
    public void FinishMagma()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("EnemyProjectile"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.TakeHit(true, Crystal.CrystalType.Red);
            }
        }
        if (gameObject.CompareTag("PlayerProjectile"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeHit();
            }
        }
    }

}
