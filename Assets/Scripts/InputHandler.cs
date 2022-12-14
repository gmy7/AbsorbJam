using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameSystem
{
    public class InputHandler : MonoBehaviour
    {
        private Mover mover;
        private Player player;
        public bool gamePaused;

        private readonly CooldownGuard counter = new();
        private readonly CooldownGuard teleport = new();
        public readonly CooldownGuard inputReady = new();
        private void Awake()
        {
            mover = GetComponent<Mover>();
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
            if (Input.GetKeyDown(KeyCode.LeftShift) && teleport.actionReady)
            {
                isTeleporting = true;
                teleport.actionReady = false;
                StartCoroutine(CoolDown(teleport, player.teleportCooldown));
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
                player.StartBlink();
                    mover.Teleport(moveVector);
                }
                else
                    mover.Move(moveVector, false, true);
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

                player.StopCountering();

                player.StartShoot(firingVector);
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
            if (collision.gameObject.CompareTag("Core"))
            {
                player.StartAbsorbingCore(collision.gameObject);
                player.StopCountering();
            }

        }


        IEnumerator CoolDown(CooldownGuard guard, float coolDown)
        {
            yield return new WaitForSeconds(coolDown);
            guard.actionReady = true;
        }
    }
}


