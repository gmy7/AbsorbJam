using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Mover mover;
    private ProjectileShooter shooter;
    private Player player;

    private CooldownGuard counter = new CooldownGuard();
    private CooldownGuard inputReady = new CooldownGuard();
    private void Awake()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<ProjectileShooter>();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        //player.playerState = Player.PlayerState.Idle;
        if (!inputReady.actionReady) { return; }
        InputMovement();
        InputShoot();
        InputCounter();
    }
    private void InputMovement()
    {
        Vector3 moveVector = Vector3.zero;
        bool isTeleporting = false;
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.ammo > 0)
        {
            player.ammo--;
            isTeleporting = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
                moveVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
                moveVector.y += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
                moveVector.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
                moveVector.x += -1;
        }

        if(moveVector != Vector3.zero)
        {
            player.playerState = Player.PlayerState.Moving;
            if (isTeleporting)
            {
                mover.Teleport(moveVector);
            }
            else
                mover.Move(moveVector, false);
            player.StopCountering();
            return;
        }
    }
    private void InputShoot()
    {
        if(player.ammo <= 0)
        {
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 playerPos = transform.position;
            Vector3 firingVector = new Vector3(mousePos.x - playerPos.x, mousePos.y - playerPos.y);

            shooter.FireProjectile(null, firingVector, false);
            player.StopCountering();

            player.TakingShot();
        }
    }
    private void InputCounter()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!counter.actionReady) { return; }
            player.CounterActive = true;
            counter.actionReady = false;
            inputReady.actionReady = false;
            StartCoroutine(CoolDown(counter, 2f));
            StartCoroutine(CoolDown(inputReady, 0.25f));
        }
    }
    IEnumerator CoolDown(CooldownGuard guard, float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        guard.actionReady = true;
    }
}

