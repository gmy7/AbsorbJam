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
        [SerializeField] private List<GameObject> spawns = new List<GameObject>();
        [SerializeField] private int startingSpawns = 2;

        private int previousSpawnIndex = 0;
        public int monsterCount;
        [SerializeField] private int monsterCap = 3;
        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.gameEnded = new GameManager.GameEnded(GameOver);
        }
        private void Start()
        {
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
            while (true)
            {
                if (firstSpawn)
                    yield return new WaitForSeconds(15);
                firstSpawn = false;
                StartCoroutine(Spawner(4));
                monsterCap++;
                yield return new WaitForSeconds(15);
            }
        }
        private IEnumerator Spawner(float timer)
        {
            while (true)
            {
                SpawnMonster();
                yield return new WaitForSeconds(timer);
            }
        }
        private void SpawnMonster()
        {
            if (monsterCap <= monsterCount) { return; }
            int spawnIndex = 0;
            while (spawnIndex == previousSpawnIndex)
                spawnIndex = GetRandomSpawnIndex();

            previousSpawnIndex = spawnIndex;
            Instantiate(golem, spawns[spawnIndex].transform.position, transform.rotation, transform);
            monsterCount++;
        }
        private int GetRandomSpawnIndex()
        {
            int randSpawnIndex = Random.Range(0, spawns.Count);
            return randSpawnIndex;
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

