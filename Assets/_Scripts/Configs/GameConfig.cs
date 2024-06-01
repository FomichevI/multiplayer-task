using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/GameConfigs", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Scenes settings")]
    [SerializeField] private string _menuSceneName; public string MenuSceneName {  get { return _menuSceneName; } }
    [SerializeField] private string _gameplaySceneName; public string GameplaySceneName { get { return _gameplaySceneName; } }

    private string _playerName;

    public void SetPlayerName(string name)
    {
        _playerName = name;
    }

}
