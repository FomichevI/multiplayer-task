using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class GameplayUiManager : NetworkBehaviour
{
    public void OnDisconnectClick()
    {
        GameManager.Instance.DisconnectPlayer();
    }
}
