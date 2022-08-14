using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameSystem
{
    public class InputHandler : MonoBehaviour
    {
        private Mover mover;
        private ProjectileShooter shooter;
        private Player player;
        public bool gamePaused;

        private readonly CooldownGuard counter = new();
        public readonly CooldownGuard inputReady = new();
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
            if (gamePaused) { return; }
            InputMovement();
            InputShoot();
            InputCounter();
        }

        private void InputMovement()
        {
            Vector3 moveVector = Vector3.zero;
            bool isTeleporting = false;
            if (Input.GetKeyDown(KeyCode.LeftShift) && player.Ammo > 0)
            {
                player.StartBlink();
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

            if (moveVector != Vector3.zero)
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
            if (player.Ammo <= 0)
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

                player.StartShoot();
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
                StartCoroutine(CoolDown(counter, player.counterCooldown));
                StartCoroutine(CoolDown(inputReady, 0.4f));
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!inputReady.actionReady) { return; }

            if (Input.GetKey(KeyCode.Q))
            {
                if (collision.gameObject.CompareTag("Core"))
                {
                    player.StartAbsorbingCore(collision.gameObject);
                    player.StopCountering();
                }
            }
        }


        IEnumerator CoolDown(CooldownGuard guard, float coolDown)
        {
            yield return new WaitForSeconds(coolDown);
            guard.actionReady = true;
        }
    }
}


