using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHandler : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private List<GameObject> spawns = new List<GameObject>();

    private void Awake()
    {
        
    }
}
