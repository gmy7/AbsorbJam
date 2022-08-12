using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private GameObject spriteGO;
    [SerializeField] private float moveSpeed;
    private void Awake()
    {
        if(spriteGO == null) { spriteGO = gameObject; }
    }
    public void Move(Vector3 direction, bool moverHandleRotation)
    {
        if(moverHandleRotation)
            HandleRotation(direction);

        float arcTan = Mathf.Atan(direction.y / direction.x);
        transform.position = new Vector3(transform.position.x + Mathf.Sign(direction.x) * Mathf.Cos(arcTan) * moveSpeed * Time.deltaTime,
                                         transform.position.y + Mathf.Sign(direction.x) * Mathf.Sin(arcTan) * moveSpeed * Time.deltaTime);
    }
    private void HandleRotation(Vector3 direction)
    {
        if (direction.x < 0)
            spriteGO.transform.eulerAngles = new Vector3(0, 180);
        else if (direction.x > 0)
            spriteGO.transform.eulerAngles = new Vector3(0, 0);
    }
}
