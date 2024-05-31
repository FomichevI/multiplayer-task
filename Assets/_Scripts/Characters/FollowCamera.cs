using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerTrans;
    [SerializeField] private float _followSpeed;
    [SerializeField] private bool _isRotated = false;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = _playerTrans.InverseTransformPoint(transform.position);
    }

    private void FixedUpdate()
    {
        if (_isRotated)
        {
            Vector3 newPos = _playerTrans.TransformPoint(_startPosition);
            transform.position = Vector3.Lerp(transform.position, newPos, _followSpeed * Time.fixedDeltaTime);
            transform.LookAt(_playerTrans);
        }
        else
        {
            Vector3 newPos = _playerTrans.TransformPoint(_startPosition);
            transform.position = Vector3.Lerp(transform.position, newPos, _followSpeed * Time.fixedDeltaTime);
        }
    }
}
