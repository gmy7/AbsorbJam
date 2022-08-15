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
    private MonsterHandler monsterHandler;
    [SerializeField] private AudioClip spawnSound;
    [HideInInspector]
    private AudioSource src;
    public enum BehaviorState { Walking, Idle, Shooting, Dying, Spawning, Swiping }
    public BehaviorState behaviorState;
    public Crystal.CrystalType behaviourType;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<ProjectileShooter>();
        monsterHandler = transform.root.GetComponent<MonsterHandler>();
        player = monsterHandler.player;
        animator = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }
    private void Start()
    {
        src.PlayOneShot(spawnSound, SoundSettings.effectsSound);
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
            if (behaviourType == Crystal.CrystalType.Blue || behaviourType == Crystal.CrystalType.Red)
                MoveTowardsPlayer(true);
            if (behaviourType == Crystal.CrystalType.Yellow)
                MoveTowardsPlayer(false);
        }
    }
    private void MoveTowardsPlayer(bool towards)
    {
        Vector3 movementVector = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

        if (towards)
            mover.Move(movementVector, true, true);
        else
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < 1.5f)
            {
                if (Mathf.Abs(transform.position.x) >= 6.6 || Mathf.Abs(transform.position.y) >= 3.3)
                {
                    return;
                }
                else
                {
                    mover.Move(-movementVector, true, true);
                    return;
                }
            }
            if (distance < 12f && distance > 9f)
            {
                mover.Move(movementVector, true, true);
            }
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
            shooter.FireLightning(new Vector3(player.transform.position.x + Random.Range(-.33f, 0.33f), player.transform.position.y + Random.Range(-.33f, 0.33f)), true);
        if (behaviourType == Crystal.CrystalType.Red) 
            shooter.FireMagma(transform.position, true);
    }
    #endregion
    public void StartShooting()
    {
        if(Vector3.Distance(player.transform.position, transform.position) > 2.5f) { return; }
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
        yield return new WaitForSeconds(0.7f);
        movingOutOfSpawn = false;
    }
}
