using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class MonsterHandler : MonoBehaviour
    {
        public GameObject player;
        private GameManager gameManager;
        [SerializeField] private GameObject golem;
        [SerializeField] private List<GameObject> spawnsGO = new();
        private List<Spawner> spawns = new();
        [SerializeField] private int startingSpawns = 2;

        private float timeBetweenSpawns = 3;
        public int monsterCount;
        [SerializeField] private int monsterCap = 2;
        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.gameEnded = new GameManager.GameEnded(GameOver);
            foreach(GameObject spawnGO in spawnsGO)
            {
                spawns.Add(spawnGO.GetComponent<Spawner>());
            }
        }
        private void Start()
        {
            SpawnMonster();
            StartCoroutine(StartDelay());
        }
        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(3);
            for (int i = 0; i < startingSpawns; i++)
            {
                SpawnMonster();
            }
            StartCoroutine(LevelIncrease());
        }
        private IEnumerator LevelIncrease()
        {
            bool firstSpawn = true;
            StartCoroutine(Spawner());

            while (true)
            {
                if (firstSpawn)
                    yield return new WaitForSeconds(15);
                firstSpawn = false;
                monsterCap++;
                yield return new WaitForSeconds(15);
            }
        }
        private IEnumerator Spawner()
        {
            while (true)
            {
                SpawnMonster();
                if (timeBetweenSpawns >= 0.25f)
                    timeBetweenSpawns -= .25f;
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
        private void SpawnMonster()
        {
            if (monsterCap <= monsterCount) { return; }
            int randSpawn = Random.Range(0, spawns.Count);

            for (int i = 0; i < spawns.Count; i++)
            {
                if (!spawns[randSpawn].SpawnerOccupied)
                {
                    spawns[randSpawn].SpawnMonster(golem, gameObject);
                    monsterCount++;
                    break;
                }
                else
                {
                    randSpawn++;
                    //Prevent index overflow
                    if (randSpawn >= spawns.Count)
                        randSpawn = 0;
                }
            }
        }
        private void GameOver()
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Enemy>().Die();
            }
            StopAllCoroutines();
        }
    }
}

