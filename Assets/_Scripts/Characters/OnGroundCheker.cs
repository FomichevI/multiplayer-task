using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform _lowestPointTrans;

    private bool _onTheGround = true; public bool OnTheGround { get { return _onTheGround; } }
    private Ray _ray;
    private RaycastHit _hit;
    private float _maxHeight;

    private void Start()
    {
        _maxHeight = transform.position.y;
    }

    private void FixedUpdate()
    {
        _ray = new Ray(_lowestPointTrans.position, Vector3.down);
        if (Physics.Raycast(_ray, out _hit, _checkDistance, _groundLm))
            _onTheGround = true;
        else
            _onTheGround = false;
        //Debug.Log(_onTheGround);

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

    private void CalculateMaxHeithFall()
    {
        if (transform.position.y > _maxHeight)
            _maxHeight = transform.position.y;
    }
}
