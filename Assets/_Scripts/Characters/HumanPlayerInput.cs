using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(HumanPlayerController))]

public class HumanPlayerInput : NetworkBehaviour
{
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    [SerializeField] private Button _fireButton;
    [SerializeField] private Button _jumpButton;
    private float _inputHorizontal;
    private float _inputVertical;
    private HumanPlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<HumanPlayerController>();
        //AddEventToButton(_fireButton, Fire, EventTriggerType.PointerClick);

        //AddEventToButton(_jumpButton, Jump, EventTriggerType.PointerClick);
    }

    void Update()
    {
        if (!IsOwner) return;

        _inputHorizontal = SimpleInput.GetAxis(_horizontalAxis);
        _inputVertical = SimpleInput.GetAxis(_verticalAxis);
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Fire();
    }

    private void Jump()
    {
        _playerController.Jump(); 
    }

    private void Fire()
    {
        _playerController.Fire(); 
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        _playerController.Move(_inputHorizontal, _inputVertical);
    }

    private void AddEventToButton(Button button, Action action, EventTriggerType triggerType) //метод для будущего улучшения скрипта
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener((data) => action());
        trigger.triggers.Add(entry);
    }
}
