using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Zenject.Asteroids;
using static SimpleInput;

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
    [SerializeField] private float _jumpCollDown = 0.5f;
    //[SerializeField] private float _fireAngle = -5;
    //[SerializeField] private float _fireColldown = 1;
    [SerializeField] private float _jumpMoveMultiplier = 0.3f;
    private float _lastJumpTime;
    #endregion

    [SerializeField] private FloatingHud _playerHud;
    private Rigidbody _rigidbody;
    private HumanAnimator _humanAnimator;
    private OnGroundCheker _onGroundCheker;
    private float _lastVerInput; //Ќеобходимо дл€ определени€ направлени€ во врем€ движени€ в воздухе

    // !!! »зменить зависимость !!!  остыль !!!
    [SerializeField] private Transform _cameraTarget;
    private FollowCamera _playerCamera;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _humanAnimator = GetComponent<HumanAnimator>();
        _onGroundCheker = GetComponent<OnGroundCheker>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InitProperties();
        InitPlayerHud();
    }

    private void InitProperties()
    {
        if (IsOwner)
        {
            _playerCamera = Camera.main.GetComponent<FollowCamera>(); // !!!  остыль !!! Ќужно добавить зависимость через zenject, но, как и со всем, что св€зано
            // с сетевым кодом, это сделать можно только через новые костыли
            _playerCamera.transform.position = transform.position;
            _playerCamera.SetTarget(_cameraTarget);
            LocalPlayerData data = GameManager.Instance.PlayerData;
            _name.Value = data.PlayerName;
            _maxHp.Value = data.MaxHp;
            _currentHp.Value = data.MaxHp;
            // спавним в рандомной позиции
            SpawnOnRandomPoint();

            _lastJumpTime = Time.time - _jumpCollDown;
        }
    }

    private void InitPlayerHud()
    {
        if (_name.Value != "")
            _playerHud.UpdateName(_name.Value.ToString());
        else
            _playerHud.UpdateName("Player " + OwnerClientId);

        _playerHud.UpdateHpBar(_currentHp.Value, _maxHp.Value);
    }

    private void OnEnable()
    {
        _onGroundCheker.OnFallEnd.AddListener(GetFallDamage);
        OnHpChangedEvent.AddListener(_playerHud.UpdateHpBar);
        OnNameChangedEvent.AddListener(_playerHud.UpdateName);
    }

    private void OnDisable()
    {
        _onGroundCheker.OnFallEnd.RemoveListener(GetFallDamage);
        OnHpChangedEvent.RemoveListener(_playerHud.UpdateHpBar);
        OnNameChangedEvent.RemoveListener(_playerHud.UpdateName);
    }

    public void Move(float horInput, float verInput)
    {
        if (!_isAlive) return;
        // ¬ полете уменьшаем скорость персонажа. ≈сли персонаж прыгнул с места, то его скорость перемещени€ уменьшаетс€ еще сильнее
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
        if (!_isAlive) return;
        if (Time.time - _lastJumpTime <= _jumpCollDown) return;
        if (_onGroundCheker.OnTheGround && _lastVerInput >= 0)
        {
            _rigidbody.AddForce(Vector3.up * 10000 * _jumpForce);
            _lastJumpTime = Time.time;
        }
    }

    //public void Fire()
    //{

    //}

    protected override void Death()
    {
        _humanAnimator.SetAnimationJump(true); // ¬ будущем заменить на анимацию смерти
        _humanAnimator.Move(0);
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
        IEnumerator RespawnCo()
        {
            yield return new WaitForSeconds(3);
            SpawnOnRandomPoint();
            ResetHp();
        }
    }

    private void SpawnOnRandomPoint()
    {
        Transform newTrans = PlayerSpawner.Instance.GetRandomSpawnPoint();
        transform.position = newTrans.position;
        transform.rotation = newTrans.rotation;
        _playerCamera.SetPositionImmediatly();
    }
}
