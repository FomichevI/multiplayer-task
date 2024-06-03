using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class MedicineChest : NetworkBehaviour
{
    [HideInInspector] public UnityEvent OnChastUsed = new UnityEvent();
    [SerializeField] private int _healingHp = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<DamagebleObject>().Heal(_healingHp);
            OnChastUsed?.Invoke();
            gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }

    public override void OnDestroy()
    {
        OnChastUsed.RemoveAllListeners();
        base.OnDestroy();
    }
}
