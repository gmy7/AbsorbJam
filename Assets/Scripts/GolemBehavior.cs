using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehavior : MonoBehaviour
{
    private Mover mover;
    private ProjectileShooter shooter;
    private GameObject player;
    private enum BehaviorState { Walking, Shooting }
    [SerializeField] private BehaviorState behaviorState;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<ProjectileShooter>();
        player = transform.root.GetComponent<MonsterHandler>().player;
    }
    private void Update()
    {
        if(behaviorState == BehaviorState.Walking)
        {
            Vector3 movementVector = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            mover.Move(movementVector, true);
        }
    }
}
