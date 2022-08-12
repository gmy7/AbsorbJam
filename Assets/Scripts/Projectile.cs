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
}
