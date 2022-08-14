using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;
public class GolemBehavior : MonoBehaviour
{
    private Mover mover;
    private ProjectileShooter shooter;
    private GameObject player;
    private Animator animator;
    private float randomStartDelay;
    private float randomShootDelay;
    public enum BehaviorState { Walking, Idle, Shooting, Dying, Spawning, Swiping }
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
        randomStartDelay = Random.Range(0.1f, 1f);
        randomShootDelay = Random.Range(2f, 4f);
    }
    public void FinishedSpawning()
    {
        StartCoroutine(WakeUpShootDelay());
    }
    private void Update()
    {
        if(behaviorState == BehaviorState.Idle) { return; }
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
    public void StartSwiping()
    {
        behaviorState = BehaviorState.Swiping;
    }
    private IEnumerator WakeUpShootDelay()
    {
        yield return new WaitForSeconds(randomStartDelay);
        StartCoroutine(ShootCoroutine());
    }
    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            StartShooting();
            yield return new WaitForSeconds(randomShootDelay);
        }
    }
}
