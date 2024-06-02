using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class DamagebleObject : NetworkBehaviour
{
    [HideInInspector] protected UnityEvent<int,int> OnHpChangedEvent = new UnityEvent<int, int>();

    [Header("Hit points")]
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _currentHp = 100; public int CurrentHp { get { return _currentHp; } }
    [SerializeField] private float _minFallHeightToGetDamage = 2f;
    [SerializeField] private float _fallDamageMultyplier = 1.5f; // мультипликатор урона от падения увеличивает урон от падения за каждую единицу высоты 
    // сверх минимальной высоты, при падении с которой объект получает урон

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
            OnHpChangedEvent?.Invoke(_currentHp, _maxHp);
        }
    }

    protected void GetFallDamage(float fallHeight)
    {
        if (fallHeight > _minFallHeightToGetDamage)
            GetDamage(CalculateFallDamage(fallHeight));
    }

    private int CalculateFallDamage(float fallHeight)
    {
        int defaulDamage = _maxHp / 20;
        int damage = (int)(defaulDamage * (1 + (fallHeight - _minFallHeightToGetDamage) * _fallDamageMultyplier));
        Debug.Log("Высота: " + fallHeight + "\nУрон: " + damage);
        return damage;
    }

    public void Heal(int hitPoints)
    {
        if (_isAlive)
        {
            _currentHp += hitPoints;
            if (_currentHp > _maxHp)
                _currentHp = _maxHp;
            OnHpChangedEvent?.Invoke(_currentHp, _maxHp);
        }
    }

    protected virtual void Death()
    {

    }
}
