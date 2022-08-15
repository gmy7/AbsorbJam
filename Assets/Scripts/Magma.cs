using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma : MonoBehaviour
{
    private Collider2D col2D;
    private Animator animator;
    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }
}
