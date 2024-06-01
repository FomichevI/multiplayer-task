using UnityEngine;
using Zenject.Asteroids;

[RequireComponent(typeof(HumanPlayerInput))]
[RequireComponent(typeof(HumanAnimator))]
[RequireComponent(typeof(OnGroundCheker))]

public class HumanPlayerController : DamagebleObject
{
    #region MovingProperties
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumpForce = 1.5f;
    [SerializeField] private float _fireAngle = -5;
    [SerializeField] private float _fireColldown = 1;
    [SerializeField] private float _jumpMoveMultiplier = 0.3f;
    #endregion

    [SerializeField] private float _minFallHeightToGetDamage = 2f;
    private Rigidbody _rigidbody;
    private HumanAnimator _humanAnimator;
    private OnGroundCheker _onGroundCheker;
    private float _lastVerInput; //Необходимо для определения направления во время движения в воздухе


    //*********************** УДАЛИТЬ!
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private FollowCamera _cameraPrefab;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _humanAnimator = GetComponent<HumanAnimator>();
        _onGroundCheker = GetComponent<OnGroundCheker>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //FollowCamera cam = Instantiate(_cameraPrefab, transform.position, Quaternion.identity);
            FollowCamera cam = Camera.main.GetComponent<FollowCamera>();
            cam.transform.position = transform.position;
            cam.SetTarget(_cameraTarget);
        }
    }

    private void OnEnable()
    {
        _onGroundCheker.OnFallEnd.AddListener(GetFallDamage);
    }

    private void OnDisable()
    {
        _onGroundCheker.OnFallEnd.RemoveListener(GetFallDamage);
    }

    public void Move(float horInput, float verInput)
    {
        // В полете уменьшаем скорость персонажа. Если персонаж прыгнул с места, то его скорость перемещения уменьшается еще сильнее
        float speedMulti = _onGroundCheker.OnTheGround ? 1 : _jumpMoveMultiplier * (_lastVerInput <= 0.2f ? 0.5f : 1f);

        if (verInput > 0)
            transform.Translate(Vector3.forward * _moveSpeed * Time.fixedDeltaTime * speedMulti);
        else if (verInput < 0)
            transform.Translate(-Vector3.forward * _moveSpeed * Time.fixedDeltaTime * speedMulti);
        if (Mathf.Abs(horInput) > 0.2)
            transform.Rotate(Vector3.up * horInput, _rotationSpeed * Time.fixedDeltaTime);

        if (_onGroundCheker.OnTheGround)
        {
            _humanAnimator.Move(verInput);
            _lastVerInput = verInput;
        }

        _humanAnimator.SetAnimationJump(_onGroundCheker.OnTheGround);
    }

    public void Jump()
    {
        if (_onGroundCheker.OnTheGround && _lastVerInput >= 0)
        {
            _rigidbody.AddForce(Vector3.up * 10000 * _jumpForce);
            //_humanAnimator.Jump();
        }
    }

    private void GetFallDamage(float fallHeight)
    {
        Debug.Log(fallHeight);
        if (fallHeight > _minFallHeightToGetDamage)
            GetDamage(10);
    }

    public void Fire()
    {

    }

    protected override void Death()
    {

    }
}
