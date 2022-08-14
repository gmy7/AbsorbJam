using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeBox : MonoBehaviour
{
    [SerializeField] private bool damageBox;
    private Enemy enemy;
    private bool playerInBox;
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    public bool PlayerInBox
    {
        get
        {
            return playerInBox;
        }
        set
        {
            if (!damageBox)
                enemy.StartSwing();

            playerInBox = value;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInBox = true;
            if(damageBox)
                collision.gameObject.GetComponent<Player>().TakeHit(false, Crystal.CrystalType.Blue);
        }
    }
}
