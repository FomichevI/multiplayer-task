using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;
    [SerializeField] private Transform[] _spawnPoints;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public Transform GetRandomSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
