using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Zenject.Asteroids;

[RequireComponent(typeof(HumanPlayerInput))]
[RequireComponent(typeof(HumanAnimator))]
[RequireComponent(typeof(OnGroundCheker))]

public class HumanPlayerController : DamagebleObject
{
    #region MovingProperties
    [Header("Moving")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumpForce = 1.5f;
    //[SerializeField] private float _fireAngle = -5;
    //[SerializeField] private float _fireColldown = 1;
    [SerializeField] private float _jumpMoveMultiplier = 0.3f;
    #endregion

    [SerializeField] private FloatingHud _playerHud;
    private Rigidbody _rigidbody;
    private HumanAnimator _humanAnimator;
    private OnGroundCheker _onGroundCheker;
    private float _lastVerInput; //Необходимо для определения направления во время движения в воздухе


    private NetworkVariable<FixedString32Bytes> _name =
        new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    //*********************** УДАЛИТЬ!
    [SerializeField] private Transform _cameraTarget;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _humanAnimator = GetComponent<HumanAnimator>();
        _onGroundCheker = GetComponent<OnGroundCheker>();
    }

    public override void OnNetworkSpawn()
    {
        _name.OnValueChanged += (FixedString32Bytes prevValue, FixedString32Bytes newValue) 
            => { _playerHud.UpdateName(newValue.ToString()); };
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            FollowCamera cam = Camera.main.GetComponent<FollowCamera>();
            cam.transform.position = transform.position;
            cam.SetTarget(_cameraTarget);
            _name.Value = GameManager.Instance.PlayerData.PlayerName;
        }
        InitPlayerHud();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        _name.OnValueChanged -= (FixedString32Bytes prevValue, FixedString32Bytes newValue)
            => { _playerHud.UpdateName(newValue.ToString()); };
    }

    private void InitPlayerHud()
    {
        if (_name.Value != "")
            _playerHud.UpdateName(_name.Value.ToString());
        else
            _playerHud.UpdateName("Player " + OwnerClientId);


        // Здесь необходимо получить значения из БД и потом устанавливаем их на HUD и в самом классе
        _playerHud.UpdateHpBar(100, 100);
    }

    private void OnEnable()
    {
        _onGroundCheker.OnFallEnd.AddListener(GetFallDamage);
        OnHpChangedEvent.AddListener(_playerHud.UpdateHpBar);
    }

    private void OnDisable()
    {
        _onGroundCheker.OnFallEnd.RemoveListener(GetFallDamage);
        OnHpChangedEvent.RemoveListener(_playerHud.UpdateHpBar);
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

    public void Fire()
    {

    }

    protected override void Death()
    {

    }
}
