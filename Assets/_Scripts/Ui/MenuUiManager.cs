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
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _playerInputField.onEndEdit.AddListener(TryChangePlayerName);
        if (_gameManager.PlayerData.PlayerName != null && _gameManager.PlayerData.PlayerName != "")
        {
            _playerInputField.text = _gameManager.PlayerData.PlayerName;
        }
    }

    private void TryChangePlayerName(string name)
    {
        _gameManager.PlayerData.SetPlayerName(name, () =>
        {
            _playerInputField.Select();
            _playerInputField.text = "";
        });
    }

    public void OnHostButtonClick()
    {
        _gameManager.LoadGameplayScene(ConnectionType.Host);
    }

    public void OnClientButtonClick()
    {
        _gameManager.LoadGameplayScene(ConnectionType.Client);
    }

}
