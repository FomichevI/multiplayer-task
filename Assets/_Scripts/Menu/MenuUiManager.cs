using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MenuUiManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerInputField;

    private void Awake()
    {
        _playerInputField.onEndEdit.AddListener(TryChangePlayerName);
    }

    private void TryChangePlayerName(string name)
    {
        GameManager.Instance.PlayerData.SetPlayerName(name, () =>
        {
            _playerInputField.Select();
            _playerInputField.text = "";
        });
    }

    public void OnHostButtonClick()
    {
        GameManager.Instance.LoadGameplayScene(ConnectionType.Host);
    }

    public void OnClientButtonClick()
    {
        GameManager.Instance.LoadGameplayScene(ConnectionType.Client);
    }

}
