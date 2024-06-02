using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MedicineChest : NetworkBehaviour
{
    [SerializeField] private int _healingHp = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<DamagebleObject>().Heal(_healingHp);
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }
}
