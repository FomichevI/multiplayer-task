using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class OnGroundCheker : MonoBehaviour
{
    /// <summary>
    /// Ивент окончания падения с указанием высоты, с которой упал объект
    /// </summary>
    [HideInInspector] public UnityEvent<float> OnFallEnd = new UnityEvent<float>();

    [SerializeField] private LayerMask _groundLm;
    [SerializeField] private float _checkDistance = 0.05f;
    [SerializeField] private float _checkRadius = 0.5f;
    [SerializeField] private Transform _lowestPointTrans;

    private bool _onTheGround = true; public bool OnTheGround { get { return _onTheGround; } }
    private RaycastHit _hit;
    private float _maxHeight;

    private void Start()
    {
        _maxHeight = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (Physics.BoxCast(_lowestPointTrans.position, new Vector3(_checkRadius, 0.01f, _checkRadius),Vector3.down, Quaternion.identity, _checkDistance, _groundLm))
            _onTheGround = true;
        else
            _onTheGround = false;

        if (!_onTheGround)
        {
            CalculateMaxHeithFall();
        }
        else
        {
            if (_maxHeight - transform.position.y > 0.5f)
                OnFallEnd?.Invoke(_maxHeight - transform.position.y);
            _maxHeight = transform.position.y;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Physics.BoxCast(_lowestPointTrans.position, new Vector3(_checkRadius, 0.01f, _checkRadius), Vector3.down, Quaternion.identity, _checkDistance, _groundLm))
            Gizmos.DrawWireCube(_lowestPointTrans.position, new Vector3(_checkRadius, 0.01f, _checkRadius));
    }

    private void CalculateMaxHeithFall()
    {
        if (transform.position.y > _maxHeight)
            _maxHeight = transform.position.y;
    }
}
