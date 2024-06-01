using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class DamagebleObject : NetworkBehaviour
{
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _currentHp = 100; public int CurrentHp { get { return _currentHp; } }
    private bool _isAlive = true;

    public void GetDamage(int damage)
    {
        if (_isAlive)
        {
            _currentHp -= damage;
            if (_currentHp <= 0)
            {
                _currentHp = 0;
                _isAlive = false;
                Death();
            }
        }
    }

    public void Heal(int hitPoints)
    {
        if (_isAlive)
        {
            _currentHp += hitPoints;
            if (_currentHp > _maxHp)
                _currentHp = _maxHp;
            Debug.Log("Player " + OwnerClientId + " healed");
        }
    }

    protected virtual void Death()
    {

    }
}
