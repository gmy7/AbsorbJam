using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSlot : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite empty;
    [Serializable]
    public struct Ammo
    {
        public Sprite AmmoSprite;
        public Crystal.CrystalType AmmoType;
    }
    public Ammo[] ammoTypes;
    private bool isFull;
    public Crystal.CrystalType ammoType;
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
        {
            foreach(Ammo _ammo in ammoTypes)
            {
                if(_ammo.AmmoType == ammoType)
                    sr.sprite = _ammo.AmmoSprite;
            }
        }
        else
            sr.sprite = empty;
    }
}
