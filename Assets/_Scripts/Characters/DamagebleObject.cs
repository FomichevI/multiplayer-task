using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class DamagebleObject : NetworkBehaviour
{
    [HideInInspector] protected UnityEvent<int,int> OnHpChangedEvent = new UnityEvent<int, int>();

    [Header("Hit points")]
    [SerializeField] private NetworkVariable<int> _maxHp = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private NetworkVariable<int> _currentHp = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public int CurrentHp { get { return _currentHp.Value; } }
    [SerializeField] private float _minFallHeightToGetDamage = 2f;
    [SerializeField] private float _fallDamageMultyplier = 1.5f; // �������������� ����� �� ������� ����������� ���� �� ������� �� ������ ������� ������ 
    // ����� ����������� ������, ��� ������� � ������� ������ �������� ����

    private bool _isAlive = true;

    public override void OnNetworkSpawn()
    {
        _currentHp.OnValueChanged += (int prevValue, int newValue) => { OnHpChangedEvent?.Invoke(_currentHp.Value, _maxHp.Value); };
        _maxHp.OnValueChanged += (int prevValue, int newValue) => { OnHpChangedEvent?.Invoke(_currentHp.Value, _maxHp.Value); };
    }

    public void GetDamage(int damage)
    {
        if (!IsOwner) return;
        if (_isAlive)
        {
            _currentHp.Value -= damage;
            if (_currentHp.Value <= 0)
            {
                _currentHp.Value = 0;
                _isAlive = false;
                Death();
            }
        }
    }

    protected void GetFallDamage(float fallHeight)
    {
        if (!IsOwner) return;
        if (fallHeight > _minFallHeightToGetDamage)
            GetDamage(CalculateFallDamage(fallHeight));
    }

    private int CalculateFallDamage(float fallHeight)
    {
        int defaulDamage = _maxHp.Value / 20;
        int damage = (int)(defaulDamage * (1 + (fallHeight - _minFallHeightToGetDamage) * _fallDamageMultyplier));
        Debug.Log("������: " + fallHeight + "\n����: " + damage);
        return damage;
    }

    public void Heal(int hitPoints)
    {
        if (!IsOwner) return;
        if (_isAlive)
        {
            if (_currentHp.Value + hitPoints > _maxHp.Value)
                _currentHp.Value = _maxHp.Value;
            else
                _currentHp.Value += hitPoints;
        }
    }

    protected virtual void Death()
    {

    }
}
