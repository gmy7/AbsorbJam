using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Core : MonoBehaviour
{
    public bool isDrained;
    [SerializeField] private float shrink = 0.5f;
    [SerializeField] private GameObject coreLightGO;
    [SerializeField] private float flashSpeed;
    private bool isFlashing;
    private bool becomingTransparent = true;
    private Light2D coreLight;
    private SpriteRenderer sr;
    public Transform destination;
    public Crystal.CrystalType coreType;


    private void Awake()
    {
        coreLight = coreLightGO.GetComponent<Light2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(DestroyAfterTime());
    }
    private void Update()
    {
        if (isFlashing && !isDrained)
        {
            FlashDamaged();
        }
        if (destination == null) { return; }

        if (isDrained)
        {
            transform.localScale = new Vector3(transform.localScale.x - shrink * Time.deltaTime, transform.localScale.y - shrink * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, destination.position, 2.5f * Time.deltaTime);
            coreLight.intensity -= shrink * Time.deltaTime;
        }
        if(transform.localScale.x < 0.05 || Vector3.Distance(transform.position,destination.position) < 0.01f )
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(6);
        isFlashing = true;
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void FlashDamaged()
    {
        Color newColor;
        newColor = sr.color;
        if (becomingTransparent)
        {
            newColor.a -= 0.01f * flashSpeed;
        }
        else
        {
            newColor.a += 0.01f * flashSpeed;
        }
        if (newColor.a < 0.25f)
            becomingTransparent = false;
        if (newColor.a > 1)
            becomingTransparent = true;
        coreLight.intensity = newColor.a;
        sr.color = newColor;
    }
}
