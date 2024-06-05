using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(HumanPlayerController))]

public class HumanPlayerInput : NetworkBehaviour
{
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    private float _inputHorizontal;
    private float _inputVertical;
    private HumanPlayerController _playerController;


    private void Awake()
    {
        _playerController = GetComponent<HumanPlayerController>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        MobilePlayerInputUi.Instance.AddEventToJumpButton(Jump);
    }

    void Update()
    {
        if (!IsOwner) return;

        _inputHorizontal = SimpleInput.GetAxis(_horizontalAxis);
        _inputVertical = SimpleInput.GetAxis(_verticalAxis);
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //    Fire();
    }

    private void Jump()
    {
        _playerController.Jump(); 
    }

    //private void Fire()
    //{
    //    _playerController.Fire(); 
    //}

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        _playerController.Move(_inputHorizontal, _inputVertical);
    }

}
