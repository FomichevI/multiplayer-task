using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public LocalPlayerData PlayerData;
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
#if !UNITY_EDITOR
        LoadMenuScene();
#endif
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

    public void DisconnectPlayer()
    {
        if (IsHost)
        {
            DisconnectPlayerClientRpc();
        }
        else
        {
            NetworkManager.Singleton.Shutdown();
            LoadMenuScene();
        }
    }

    [ClientRpc]
    private void DisconnectPlayerClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        LoadMenuScene();
    }

}

public enum ConnectionType { Client, Server, Host}