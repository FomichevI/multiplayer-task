using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private SceneLoader _sceneLoader; public SceneLoader SceneLoader { get { return _sceneLoader; } }
    [SerializeField] private GameConfig _gameConfig;

    private ConnectionType _connectType = ConnectionType.Host; public ConnectionType ConnectType { get { return _connectType;} }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadMenuScene();
    }

    public void LoadMenuScene()
    {
        _sceneLoader.LoadScene(_gameConfig.MenuSceneName);
    }

    public void LoadGameplayScene(ConnectionType type)
    {
        _connectType = type;
        _sceneLoader.LoadScene(_gameConfig.GameplaySceneName);
    }
}

public enum ConnectionType { Client, Server, Host}