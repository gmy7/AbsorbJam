using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float teleportDistance = 5;
    public void Move(Vector3 direction, bool moverHandleRotation)
    {
        if(moverHandleRotation)
            HandleRotation(direction);

        float arcTan = Mathf.Atan(direction.y / direction.x);
        transform.position = new Vector3(transform.position.x + Mathf.Sign(direction.x) * Mathf.Cos(arcTan) * moveSpeed * Time.deltaTime,
                                         transform.position.y + Mathf.Sign(direction.x) * Mathf.Sin(arcTan) * moveSpeed * Time.deltaTime);
    }
    public void Teleport(Vector3 direction)
    {
        float arcTan = Mathf.Atan(direction.y / direction.x);
        transform.position = new Vector3(transform.position.x + Mathf.Sign(direction.x) * Mathf.Cos(arcTan) * teleportDistance,
                                         transform.position.y + Mathf.Sign(direction.x) * Mathf.Sin(arcTan) * teleportDistance);
    }
    private void HandleRotation(Vector3 direction)
    {
        if (direction.x < 0)
            transform.eulerAngles = new Vector3(0, 180);
        else if (direction.x > 0)
            transform.eulerAngles = new Vector3(0, 0);
    }
}
