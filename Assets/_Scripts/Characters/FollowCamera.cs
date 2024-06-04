using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerTrans;
    [SerializeField] private float _followSpeed = 7;
    [SerializeField] private bool _isRotated = true;
    private Vector3 _startPosition;
    private Vector3 _fixStartPosition = new Vector3(0,4,-7);

    public void SetTarget(Transform targetTrans)
    {
        _playerTrans = targetTrans;
        Vector3 inversedPos = _playerTrans.InverseTransformPoint(transform.position);
        _startPosition = new Vector3(inversedPos.x + _fixStartPosition.x, inversedPos.y + _fixStartPosition.y, inversedPos.z + _fixStartPosition.z) ;
    }

    private void FixedUpdate()
    {
        if (!_playerTrans) return;         
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
