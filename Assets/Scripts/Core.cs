using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public bool isDrained;
    [SerializeField] private float shrink = 0.5f;
    private void Update()
    {
        if (isDrained)
        {
            transform.localScale = new Vector3(transform.localScale.x - shrink * Time.deltaTime, transform.localScale.y - shrink * Time.deltaTime);
        }
        if(transform.localScale.x < 0.05)
        {
            Destroy(gameObject);
        }
    }
}
