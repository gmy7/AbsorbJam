using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private Collider2D col2D;
    private Animator animator;
    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
    }
    private void Start()
    {
        StartCoroutine(LightningStrike());
    }
    private IEnumerator LightningStrike()
    {
        yield return new WaitForSeconds(2.5f);
        col2D.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("EnemyProjectile"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.TakeHit(true, Crystal.CrystalType.Yellow);
                if (player.CounterActive)
                {
                    //animator.SetBool("Absorbing", true);
                }
                else
                {
                    //animator.SetBool("Colliding", true);
                }
            }
        }
        if (gameObject.CompareTag("PlayerProjectile"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeHit();
                //animator.SetBool("Colliding", true);
            }
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            //animator.SetBool("Colliding", true);
        }
    }
    public void FinishLightning()
    {
        Destroy(gameObject);
    }
}

