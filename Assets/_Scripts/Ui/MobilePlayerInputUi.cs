using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobilePlayerInputUi : MonoBehaviour
{
    public static MobilePlayerInputUi Instance;

    [SerializeField] private GameObject _root;
    [SerializeField] private Button _jumpButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        _root.SetActive(false);
#else
        _root.SetActive(true);
#endif
    }

    public void AddEventToJumpButton(Action action)
    {
        AddEventToButton(_jumpButton, action, EventTriggerType.PointerClick);
    }

    private void AddEventToButton(Button button, Action action, EventTriggerType triggerType)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener((data) => action());
        trigger.triggers.Add(entry);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
