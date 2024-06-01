using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MedChestSpawner : NetworkBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private MedicineChest _medChestPrefab;

    private void Start()
    {
        SpawnNewChest();
    }

    private void SpawnNewChest()
    {
        if (IsHost)
        {
            int rand = Random.Range(0, _spawnPoints.Length);
            MedicineChest chest = Instantiate(_medChestPrefab, _spawnPoints[rand].position, _spawnPoints[rand].rotation, transform);
            chest.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SpawnNewChest();

    }
}
