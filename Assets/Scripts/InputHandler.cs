using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Mover mover;
    private ProjectileShooter shooter;
    private Player player;

    private CoolDownGuard shieldCooldown = new CoolDownGuard();
    private enum InputState { Idle, Moving }
    [SerializeField] private InputState inputState;
    private void Awake()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<ProjectileShooter>();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        inputState = InputState.Idle;
        InputMovement();
        InputShoot();
        InputShield();
    }
    private void InputMovement()
    {
        Vector3 moveVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveVector.y += 1;
            inputState = InputState.Moving;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector.y += -1;
            inputState = InputState.Moving;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector.x += 1;
            inputState = InputState.Moving;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x += -1;
            inputState = InputState.Moving;
        }
        if (inputState == InputState.Moving)
        {
            if(moveVector == Vector3.zero)
            {
                inputState = InputState.Idle;
                return;
            }
            mover.Move(moveVector, false);
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
        }
    }
    private void InputShield()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!shieldCooldown.actionReady) { return; }
            player.ShieldActive = true;
            shieldCooldown.actionReady = false;
            StartCoroutine(CoolDown(shieldCooldown, 2f));
        }
    }
    IEnumerator CoolDown(CoolDownGuard guard, float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        guard.actionReady = true;
    }
}

public class CoolDownGuard
{
    public bool actionReady = true;
}
