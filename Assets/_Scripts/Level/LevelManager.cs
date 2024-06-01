using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager.ConnectType == ConnectionType.Client)
            NetworkManager.Singleton.StartClient();
        else if (gameManager.ConnectType == ConnectionType.Host)
            NetworkManager.Singleton.StartHost();
    }
}
