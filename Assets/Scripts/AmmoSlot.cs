using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSlot : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite full;
    private bool isFull;
    public bool IsFull
    {
        get
        {
            return isFull;
        }
        set
        {
            FillBar(value);
            isFull = value;
        }
    }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void FillBar(bool shouldFill)
    {
        if (shouldFill)
            sr.sprite = full;
        else
            sr.sprite = empty;
    }
}
