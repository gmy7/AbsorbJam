using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehavior : MonoBehaviour
{
    private Mover mover;
    private ProjectileShooter shooter;
    private GameObject player;
    private Animator animator;
    private float randomWakeUpDelay;
    public enum BehaviorState { Walking, Shooting, Dying }
    public BehaviorState behaviorState;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<ProjectileShooter>();
        player = transform.root.GetComponent<MonsterHandler>().player;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        randomWakeUpDelay = Random.Range(1.5f, 3.5f);
        StartCoroutine(WakeUpShootDelay());
    }
    private void Update()
    {
        if(behaviorState == BehaviorState.Walking)
        {
            Vector3 movementVector = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            mover.Move(movementVector, true);
        }
    }
    #region AnimationEvents
    public void StartWalking()
    {
        //Called by animation event
        behaviorState = BehaviorState.Walking;
        animator.SetBool("Shooting", false);
    }
    public void Shoot()
    {
        //Called by animation event
        Vector3 firingVector = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        shooter.FireProjectile(null, firingVector, true);
    }
    #endregion
    public void StartShooting()
    {
        //Start animation shooting
        animator.SetBool("Shooting", true);
        behaviorState = BehaviorState.Shooting;
    }
    private IEnumerator WakeUpShootDelay()
    {
        yield return new WaitForSeconds(randomWakeUpDelay);
        StartCoroutine(ShootCoroutine());
    }
    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            StartShooting();
            yield return new WaitForSeconds(5f);
        }
    }
}
