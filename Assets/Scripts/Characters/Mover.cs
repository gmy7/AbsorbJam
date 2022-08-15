using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float teleportDistance = 5;
    public void Move(Vector3 direction, bool moverHandleRotation, bool isPlayer)
    {

        if(moverHandleRotation)
            HandleRotation(direction);

        float arcTan = Mathf.Atan(direction.y / direction.x);
        Vector3 movePos = new Vector3(transform.position.x + Mathf.Sign(direction.x) * Mathf.Cos(arcTan) * moveSpeed * Time.deltaTime,
                                         transform.position.y + Mathf.Sign(direction.x) * Mathf.Sin(arcTan) * moveSpeed * Time.deltaTime);
        if (!isPlayer)
        {
            transform.position = movePos;
            return;
        }
        if(Mathf.Abs(transform.position.x) >= 6.85 || Mathf.Abs(transform.position.y) >= 3.45)
        {
            Debug.Log("OutOfBounds");
            transform.position = movePos;
            return;
        }
        if (Mathf.Abs(movePos.x) > 6.8 || Mathf.Abs(movePos.y) > 3.4)
        {
            Debug.Log("MovingOutOfBounds");
            return;
        }

        else
            transform.position = movePos;
    }
    public void Teleport(Vector3 direction)
    {
        float arcTan = Mathf.Atan(direction.y / direction.x);
        Vector3 teleportationVector = new Vector3(transform.position.x + Mathf.Sign(direction.x) * Mathf.Cos(arcTan) * teleportDistance,
                                               transform.position.y + Mathf.Sign(direction.x) * Mathf.Sin(arcTan) * teleportDistance);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, teleportDistance);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Wall"))
            {
                transform.position = hit.point;
                return;
            }
        }
        transform.position = teleportationVector;

;
    }
    private void HandleRotation(Vector3 direction)
    {
        if (direction.x > 0)
            transform.eulerAngles = new Vector3(0, 180);
        else if (direction.x < 0)
            transform.eulerAngles = new Vector3(0, 0);
    }
}
