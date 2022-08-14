using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject spawnLight;
    private bool spawnerOccupied;
    public bool SpawnerOccupied
    {
        get
        {
            return spawnerOccupied;
        }
        set
        {
            spawnLight.SetActive(value);
            spawnerOccupied = value;
        }
    }

    public void SpawnMonster(GameObject monster, GameObject monsterHandler)
    {
        Instantiate(monster, spawnPoint.transform.position, spawnPoint.transform.rotation, monsterHandler.transform);
        StartCoroutine(OccupyTimer());
    }
    private IEnumerator OccupyTimer()
    {
        SpawnerOccupied = true;
        yield return new WaitForSeconds(2.25f);
        SpawnerOccupied = false;
    }
}
