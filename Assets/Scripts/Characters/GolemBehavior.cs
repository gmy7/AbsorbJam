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
    private bool movingOutOfSpawn;
    public enum BehaviorState { Walking, Idle, Shooting, Dying, Spawning, Swiping }
    public BehaviorState behaviorState;
    public Crystal.CrystalType behaviourType;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<ProjectileShooter>();
        player = transform.root.GetComponent<MonsterHandler>().player;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        randomStartDelay = Random.Range(1.5f, 4f);
        randomShootDelay = Random.Range(4f, 5f);
    }
    public void FinishedSpawning()
    {
        StartWalking();
        StartCoroutine(WakeUpShootDelay());
    }
    private void Update()
    {
        if(behaviorState == BehaviorState.Idle) { return; }
        if (movingOutOfSpawn)
        {
            MoveTowardsPlayer(true);
            return;
        }
        if (behaviorState == BehaviorState.Walking)
        {
            if (behaviourType == Crystal.CrystalType.Blue)
                MoveTowardsPlayer(true);
            if (behaviourType == Crystal.CrystalType.Yellow)
                MoveTowardsPlayer(false);
        }
    }
    private void MoveTowardsPlayer(bool towards)
    {
        Vector3 movementVector = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        if (towards)
            mover.Move(movementVector, true);
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 8f)
                mover.Move(-movementVector, true);
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
        if (behaviourType == Crystal.CrystalType.Blue)
            shooter.FireProjectile(null, firingVector, true);
        if (behaviourType == Crystal.CrystalType.Yellow)
            shooter.FireLightning(player.transform.position, true);
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
        StartCoroutine(TimeToMoveOutOfSpawn());
        movingOutOfSpawn = true;
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
    private IEnumerator TimeToMoveOutOfSpawn()
    {
        yield return new WaitForSeconds(3);
        movingOutOfSpawn = false;
    }
}
